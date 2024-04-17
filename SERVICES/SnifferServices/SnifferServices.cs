using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.ServiceProcess;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;

namespace SnifferServices
{
    public partial class SnifferServices : ServiceBase
    {
        DataTable dataTable = new DataTable();
        private NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        private Monitor currentMonitor;
        private List<Monitor> monitorList = new List<Monitor>();
        public SnifferServices()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            SetupEventLog();
            LogToDatabase(1, $"Service started");
            InitializeDataTable(dataTable);
            LogNetworkInterfaces();
            StartReceiving();
        }
        protected override void OnStop()
        {
            foreach (Monitor monitor in monitorList)
            {
                monitor.Stop();
            }
            LogToDatabase(1, $"Service stopped");
        }
        static void SetupEventLog()
        {
            EventLog eventLog1 = new EventLog();
            if (!EventLog.SourceExists("SnifferLogSource"))
            {
                EventLog.CreateEventSource("SnifferLogSource", "SnifferLog");
            }
            eventLog1.Source = "SnifferLogSource";
            eventLog1.Log = "SnifferLog";
        }
        static void LogToLocal(int type, string message)
        {
            EventLog eventLog1 = new EventLog();
            SetupEventLog();
            if (type == 0)
            {
                eventLog1.WriteEntry(message, EventLogEntryType.Error);
            }
            else if (type == 1)
            {
                eventLog1.WriteEntry(message, EventLogEntryType.Information);
            }
            else
            {
                eventLog1.WriteEntry(message, EventLogEntryType.Error);
            }
        }
        static void LogToDatabase(int type, string message)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["DBConn"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO [DBSYSTEMMONITOR].DBO.TABLE_SNIFFER_LOG (ID, MACHINE, TIME, TYPE, MESSAGE) 
                                     VALUES (@ID, @Machine, @Time, @Type, @Message)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", Guid.NewGuid().ToString());
                        command.Parameters.AddWithValue("@Machine", Environment.MachineName);
                        command.Parameters.AddWithValue("@Time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@Type", (type == 1) ? "Information" : "Error");
                        command.Parameters.AddWithValue("@Message", message);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogToLocal(0, ex.Message.ToString());
            }
        }
        public void LogNetworkInterfaces()
        {
            try
            {
                for (var i = 0; i <= networkInterfaces.Length - 1; i++)
                {
                    LogToDatabase(1, $"Network Interface: {networkInterfaces[i].Name}");
                }
            }
            catch (Exception ex)
            {
                LogToDatabase(0, $"Error: {ex.Message}");
            }
        }
        public void InitializeDataTable(DataTable dataTable)
        {
            try
            {
                LogToDatabase(1, "Initialize DataTable");
                dataTable.Columns.Add("source_ip", typeof(string));
                dataTable.Columns.Add("source_port", typeof(string));
                dataTable.Columns.Add("destination_ip", typeof(string));
                dataTable.Columns.Add("destination_port", typeof(string));
                dataTable.Columns.Add("protocol", typeof(string));
                dataTable.Columns.Add("time", typeof(string));
                dataTable.Columns.Add("length", typeof(int));
                dataTable.Columns.Add("methode", typeof(string));
                dataTable.Columns.Add("data", typeof(string));
                dataTable.Columns.Add("hex", typeof(string));
                dataTable.Columns.Add("raw", typeof(byte[]));
            }
            catch (Exception ex)
            {
                LogToDatabase(0, ex.Message.ToString());
            }
        }
        public bool StartReceiving()
        {
            monitorList.Clear();
            IPAddress[] hosts = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            foreach (IPAddress host in hosts)
            {
                Monitor monitor = new Monitor(host);
                monitor.PacketEventHandler += OnNewPacket;
                monitorList.Add(monitor);
            }
            foreach (Monitor monitor in monitorList)
            {
                monitor.Start();
            }
            return true;
        }
        private void StopReceiving()
        {
            foreach (Monitor monitor in monitorList)
            {
                monitor.Stop();
            }
            //currentMonitor.Stop();
        }
        private void OnNewPacket(Monitor monitor, Packet p)
        {
            OnRefresh(p);
        }
        public void OnRefresh(Packet p)
        {
            try
            {
                if ((p.Protocol == "TCP" || p.Protocol == "UDP" || p.Protocol == "SQL") && p.Methode != "-")
                {
                    dataTable.Rows.Add(new object[]
                    {
                        p.SourceIp, p.SourcePort, p.DestinationIP, p.DestinationPort, p.Protocol,
                        p.Time, p.TotalLength, p.Methode, p.CharString, p.HexString, p.Bytes
                    });
                    LogToDatabase(1, "Total Data: " + Convert.ToString(dataTable.Rows.Count));
                    if (dataTable.Rows.Count >= 20)
                    {
                        StopReceiving();
                        string connectionString = ConfigurationManager.AppSettings["DBConn"];
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            using (SqlTransaction transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    foreach (DataRow row in dataTable.Rows)
                                    {
                                        using (SqlCommand cmd = new SqlCommand(
                                            "INSERT INTO TABLE_SNIFFER_NEW (source_ip, source_port, destination_ip, destination_port, " +
                                            "protocol, time, length, methode, data, hex, raw) " +
                                            "VALUES (@source_ip, @source_port, @destination_ip, @destination_port, @protocol, " +
                                            "@time, @length, @methode, @data, @hex, @raw)",
                                            connection, transaction))
                                        {
                                            cmd.Parameters.AddWithValue("@source_ip", row["source_ip"]);
                                            cmd.Parameters.AddWithValue("@source_port", row["source_port"]);
                                            cmd.Parameters.AddWithValue("@destination_ip", row["destination_ip"]);
                                            cmd.Parameters.AddWithValue("@destination_port", row["destination_port"]);
                                            cmd.Parameters.AddWithValue("@protocol", row["protocol"]);
                                            cmd.Parameters.AddWithValue("@time", row["time"]);
                                            cmd.Parameters.AddWithValue("@length", row["length"]);
                                            cmd.Parameters.AddWithValue("@methode", row["methode"]);
                                            cmd.Parameters.AddWithValue("@data", row["data"]);
                                            cmd.Parameters.AddWithValue("@hex", row["hex"]);
                                            cmd.Parameters.AddWithValue("@raw", row["raw"]);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    transaction.Commit();
                                    dataTable.Rows.Clear();
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    LogToDatabase(1, "Error during row-by-row insertion: " + ex.Message);
                                    connection.Close();
                                    dataTable.Rows.Clear();
                                }
                            }
                        }
                        dataTable.Rows.Clear();
                        StartReceiving();
                    }
                    DeleteUnWantedSQL();
                }
            }
            catch (Exception ex)
            {
                LogToDatabase(0, ex.Message.ToString());
            }
        }
        static void DeleteUnWantedSQL()
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["DBConn"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"EXEC [SNIFFER_DELETE]";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogToLocal(0, ex.Message.ToString());
            }
        }
    }
}
