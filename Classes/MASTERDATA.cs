using SBM_SYSTEM_MONITOR;
using SBM_SYSTEM_MONITOR.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

namespace SBM_SYSTEM_MONITOR.Classes
{
    public class MASTERDATA
    {
        public string CONNECTION = ConfigurationManager.ConnectionStrings["CONNECTION"].ToString();


        public List<PBI_USER_DC> GET_USER_DC()
        {
            DataTable DT1 = new DataTable();
            string query = @"EXEC [GET_USER_DC]";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }
            var DATA = (from row in DT1.AsEnumerable()
                        select new PBI_USER_DC()
                        {
                            samaccountname = row["samaccountname"].ToString(),
                            givenname = row["givenname"].ToString(),
                            surname = row["surname"].ToString(),
                            mail = row["mail"].ToString(),
                            telephonenumber = row["telephonenumber"].ToString(),
                            department = row["department"].ToString(),
                            pbi_group = row["pbi_group"].ToString(),
                            company = row["company"].ToString(),
                            country = row["country"].ToString(),
                            userprincipalname = row["userprincipalname"].ToString(),
                            lastlogontimestamp = row["lastlogontimestamp"].ToString(),
                            whencreated = row["whencreated"].ToString(),
                            whenchanged = row["whenchanged"].ToString(),
                        }).ToList();
            return DATA;
        }
        public List<PBI_USER> DETAIL_USER_DC(PBI_USER_DC MODEL)
        {
            DataTable DT1 = new DataTable();
            string query = @"EXEC [DETAIL_USER_DC] @ID";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@ID", MODEL.samaccountname);
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }
            var DATA = (from row in DT1.AsEnumerable()
                        select new PBI_USER()
                        {
                            WINDOWSID  = row["WINDOWSID"].ToString(),
                            GROUPS_PBI = row["GROUPID"].ToString(),
                            
                        }).ToList();
            return DATA;
        }
        public PP<RESPONSE> UPDATE_USER_GROUP(PBI_USER MODEL)
        {
            DataTable DT1 = new DataTable();
            string query = @"EXEC [UPDATE_USER_GROUP] @WINDOWSID, @GROUPID, @USER";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@WINDOWSID", MODEL.WINDOWSID);
                command.Parameters.AddWithValue("@GROUPID", MODEL.GROUPS_PBI);
                command.Parameters.AddWithValue("@USER", COOKIES.GetCookies("NAME"));
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }


        public List<PBI_USER> GET_USER_INFO(string USERID)
        {
            DataSet DT1 = new DataSet();
            string query = @"EXEC [GET_USER_INFO] @ID";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@ID", USERID);
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }

            var DATA1 = (from row in DT1.Tables[1].AsEnumerable()
                        select new PBI_USER()
                        {
                            PLANT = row["PLANT"].ToString(),
                            USERID = row["USERID"].ToString(),
                            WINDOWSID = row["WINDOWSID"].ToString(),
                            FULL_NAME = row["FULL_NAME"].ToString(),
                            DEPT = row["DEPT"].ToString(),
                            EMAIL = row["EMAIL"].ToString(),
                            GROUPS_MDM = row["GROUPS_MDM"].ToString(),
                            GROUPS_PBI = row["GROUPS_PBI"].ToString(),
                        }).ToList();

            return DATA1;
        }
        public PP<RESPONSE> UPDATE_TOTAL_VIEW(PBI_REPORT MODEL)
        {
            DataTable DT1 = new DataTable();
            string query = @"EXEC [UPDATE_TOTAL_VIEW] @ID";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@ID", MODEL.ID);
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }
        
        public List<PBI_REPORT> UPDATE_TOTAL_VIEW_NEW(PBI_REPORT MODEL)
        {
            DataSet DT1 = new DataSet();
            string query = @"EXEC [UPDATE_TOTAL_VIEW_NEW] @ID";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@ID", MODEL.ID);
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }
            var DATA = (from row in DT1.Tables[1].AsEnumerable()
                        select new PBI_REPORT()
                        {
                            REPORT_URL = row["REPORT_URL"].ToString(),
                        }).ToList();
            return DATA;
        }

        public PP<RESPONSE> LOG_URL(string REPORT_ID, string IP, string MACHINE)
        {
            DataTable DT1 = new DataTable();
            string query = @"EXEC [LOG_URL] @REPORT_ID, @IP, @MACHINE";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@REPORT_ID", REPORT_ID);
                command.Parameters.AddWithValue("@IP", IP);
                command.Parameters.AddWithValue("@MACHINE", MACHINE);
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }
        public List<PBI_REPORT> GET_RECENT()
        {
            DataTable DT1 = new DataTable();
            string query = @"EXEC [GET_RECENT] @USERID";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@USERID", COOKIES.GetCookies("USERID"));
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }
            var DATA = (from row in DT1.AsEnumerable()
                        select new PBI_REPORT()
                        {
                            ID = row["ID"].ToString(),
                            REPORT_NAME = row["REPORT_NAME"].ToString(),
                            REPORT_DESC = row["REPORT_DESC"].ToString(),
                            REPORT_URL = row["REPORT_URL"].ToString(),
                            REPORT_DEPT = row["REPORT_DEPT"].ToString(),
                            REPORT_CATEGORY = row["REPORT_CATEGORY"].ToString(),
                            REPORT_VISIBLE = row["REPORT_VISIBLE"].ToString(),
                            TOTAL_VIEW = row["TOTAL_VIEW"].ToString(),
                            CREATE_USER = row["CREATE_USER"].ToString(),
                            CREATE_DATE = row["CREATE_DATE"].ToString(),
                            UPDATE_USER = row["UPDATE_USER"].ToString(),
                            UPDATE_DATE = row["UPDATE_DATE"].ToString(),
                            TAG_DEPT = row["TAG_DEPT"].ToString(),
                        }).ToList();
            return DATA;
        }
        public PP<RESPONSE> SYNC_DEPARTMENT()
        {
            DataTable DT1 = new DataTable();
            string query = @"EXEC [SYNC_DEPARTMENT] ";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }


        public List<PBI_SHARE_URL> INSERT_PBI_SHARE_URL(string REPORT_NAME, string REPORT_ID, string URL_TYPE, string URL_TXT,string URL_EXPI, string URL_PASS)
        {
            DataSet DT1 = new DataSet();
            string query = @"EXEC [INSERT_PBI_SHARE_URL] @REPORT_NAME, @REPORT_ID, @URL_TYPE,@URL_EXPI, @URL_TXT, @URL_PASS, @USER";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@REPORT_NAME", REPORT_NAME);
                command.Parameters.AddWithValue("@REPORT_ID", REPORT_ID);
                command.Parameters.AddWithValue("@URL_TYPE", URL_TYPE);
                command.Parameters.AddWithValue("@URL_EXPI", URL_EXPI);
                command.Parameters.AddWithValue("@URL_TXT" , URL_TXT);
                command.Parameters.AddWithValue("@URL_PASS", URL_PASS);
                command.Parameters.AddWithValue("@USER", COOKIES.GetCookies("USERID"));
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }
            var DATA = (from row in DT1.Tables[0].AsEnumerable()
                        select new PBI_SHARE_URL()
                        {
                            ID = row["ID"].ToString(),
                            REPORT_ID = row["REPORT_ID"].ToString(),
                            URL_TYPE = row["URL_TYPE"].ToString(),
                            URL_TEXT = row["URL_TEXT"].ToString(),
                            URL_PASS = row["URL_PASS"].ToString()
                        }).ToList();
            return DATA;
        }

        public List<PBI_SHARE_URL> GET_PBI_SHARE_URL(string URL_TYPE, string URL_TXT, string URL_PASS)
        {
            DataSet DT1 = new DataSet();
            string query = @"EXEC [GET_PBI_SHARE_URL] @URL_TYPE, @URL_TXT, @URL_PASS";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@URL_TYPE", URL_TYPE);
                command.Parameters.AddWithValue("@URL_TXT", URL_TXT);
                command.Parameters.AddWithValue("@URL_PASS", URL_PASS);
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }
            var DATA = (from row in DT1.Tables[0].AsEnumerable()
                        select new PBI_SHARE_URL()
                        {
                            ID = row["ID"].ToString(),
                            REPORT_ID = row["REPORT_ID"].ToString(),
                            URL_TYPE = row["URL_TYPE"].ToString(),
                            URL_TEXT = row["URL_TEXT"].ToString(),
                        }).ToList();
            return DATA;
        }


        public List<PBI_SHARE_URL> GET_MY_LINK()
        {
            DataTable DT1 = new DataTable();
            string query = @"EXEC [GET_MY_LINK] @USER";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@USER", COOKIES.GetCookies("USERID"));
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }
            var DATA = (from row in DT1.AsEnumerable()
                        select new PBI_SHARE_URL()
                        {
                            ID = row["ID"].ToString(),
                            REPORT_NAME = row["REPORT_NAME"].ToString(),
                            URL_TEXT = row["URL_TEXT"].ToString(),
                            URL_EXPI = row["URL_EXPI"].ToString(),
                            URL_TYPE = row["URL_TYPE"].ToString(),
                        }).ToList();
            return DATA;
        }
        public List<PBI_SHARE_URL> GET_MY_LOG_LINK()
        {
            DataTable DT1 = new DataTable();
            string query = @"EXEC [GET_LOG_URL] @USER";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@USER", COOKIES.GetCookies("USERID"));
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }
            var DATA = (from row in DT1.AsEnumerable()
                        select new PBI_SHARE_URL()
                        {
                            ID = row["ID"].ToString(),
                            REPORT_NAME = row["REPORT_NAME"].ToString(),
                            URL_TEXT = row["URL_TEXT"].ToString(),
                            URL_EXPI = row["URL_EXPI"].ToString(),
                            URL_TYPE = row["URL_TYPE"].ToString(),
                        }).ToList();
            return DATA;
        }

        public bool RETEIVE_USER_DC()
        {
            try
            {
                var A = SYNC_USER_DC();
                if(A.Result == true)
                {
                    DataTable tblPBIUserDC = new DataTable("TBL_PBI_USER_DC");
                    tblPBIUserDC.Columns.AddRange(new[]
                    {
                        new DataColumn("samaccountname", typeof(string)),
                        new DataColumn("givenname", typeof(string)),
                        new DataColumn("surname", typeof(string)),
                        new DataColumn("mail", typeof(string)),
                        new DataColumn("telephonenumber", typeof(string)),
                        new DataColumn("department", typeof(string)),
                        new DataColumn("company", typeof(string)),
                        new DataColumn("country", typeof(string)),
                        new DataColumn("userprincipalname", typeof(string)),
                        new DataColumn("lastlogontimestamp", typeof(DateTime)),
                        new DataColumn("whencreated", typeof(DateTime)),
                        new DataColumn("whenchanged", typeof(DateTime))
                    });
                    using (DirectoryEntry entry = new DirectoryEntry("LDAP://" + "SHIMANOACE", "sbm_deni", "JuliaErfani04"))
                    {
                        using (DirectorySearcher searcher = new DirectorySearcher(entry))
                        {
                            searcher.Filter = "(objectClass=user)";
                            searcher.PropertiesToLoad.AddRange(new string[] {
                            "sAMAccountName", "givenName", "sn", "mail", "telephoneNumber",
                            "department", "company", "co", "userPrincipalName",
                            "lastLogonTimestamp", "whenCreated", "whenChanged"
                        });
                            searcher.PageSize = 1000;
                            searcher.SizeLimit = 0;
                            int totalCount = 0;
                            SearchResultCollection results;
                            do
                            {
                                results = searcher.FindAll();

                                foreach (SearchResult result in results)
                                {
                                    var row = tblPBIUserDC.NewRow();
                                    row["sAMAccountName"] = GetStringProperty(result, "sAMAccountName");
                                    row["givenName"] = GetStringProperty(result, "givenName");
                                    row["surname"] = GetStringProperty(result, "sn");
                                    row["mail"] = GetStringProperty(result, "mail");
                                    row["telephoneNumber"] = GetStringProperty(result, "telephoneNumber");
                                    row["department"] = GetStringProperty(result, "department");
                                    row["company"] = GetStringProperty(result, "company");
                                    row["country"] = GetStringProperty(result, "co");
                                    row["userPrincipalName"] = GetStringProperty(result, "userPrincipalName");
                                    row["lastLogonTimestamp"] = GetDateTimeProperty(result, "lastLogonTimestamp");
                                    row["whenCreated"] = GetStringProperty(result, "whenCreated");
                                    row["whenChanged"] = GetStringProperty(result, "whenChanged");

                                    tblPBIUserDC.Rows.Add(row);
                                    totalCount++;
                                }
                                searcher.FindAll().Dispose();
                            } while (results.Count == searcher.PageSize);
                        }
                    }
                    string tableName = "PBI_USER_DC";
                    string connectionString = ConfigurationManager.ConnectionStrings["CONNECTION"].ToString();
                    BulkInsert(tblPBIUserDC, connectionString, tableName);
                    return true;
                }
                return A.Result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public PP<RESPONSE> SYNC_USER_DC()
        {
            DataTable DT1 = new DataTable();
            string query = @"EXEC [SYNC_USER_DC]";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }

        public List<PBI_GROUP> GET_PERMISSION()
        {
            DataSet DT1 = new DataSet();
            string query = @"EXEC [GET_PERMISSION] @USERID";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@USERID", (COOKIES.GetCookies("USERID") ?? "") );
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }
            var DATA = (from row in DT1.Tables[0].AsEnumerable()
                        select new PBI_GROUP()
                        {
                            ALLOW_CREATE = row["ALLOW_CREATE"].ToString(),
                            ALLOW_EXPORT = row["ALLOW_EXPORT"].ToString(),
                            ALLOW_SHARE = row["ALLOW_SHARE"].ToString(),
                            ALLOW_VIEW = row["ALLOW_VIEW"].ToString(),
                        }).ToList();
            return DATA;
        }

        static void BulkInsert(DataTable dataTable, string connectionString, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = tableName;
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                    bulkCopy.BulkCopyTimeout = 60;
                    bulkCopy.BatchSize = 1000;
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }
        private string GetStringProperty(SearchResult result, string propertyName)
        {
            if (result.Properties.Contains(propertyName) && result.Properties[propertyName].Count > 0)
            {
                return result.Properties[propertyName][0].ToString();
            }
            return string.Empty;
        }
        private object GetDateTimeProperty(SearchResult result, string propertyName)
        {
            if (result.Properties.Contains(propertyName) && result.Properties[propertyName].Count > 0)
            {
                string dateString = result.Properties[propertyName][0].ToString();
                if (DateTime.TryParseExact(dateString, "yyyyMMddHHmmss.0Z", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime parsedDateTime))
                {
                    return parsedDateTime;
                }
                else
                {
                    string fileTimeString = dateString;
                    long fileTime;
                    if (long.TryParse(fileTimeString, out fileTime))
                    {
                        DateTime utcDateTime = DateTime.FromFileTime(fileTime);
                        DateTime localDateTime = utcDateTime.ToLocalTime();
                        return localDateTime;
                    }
                }
            }
            return DBNull.Value;
        }

        public PP<List<GET_HEADER>> GETHEADER(SERVER SVR)
        {
            DataSet DT1 = new DataSet();
            string query = @"EXEC [SNIFFER_GETHEADER] @SERVER, @FILTER_PERIOD";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@SERVER", SVR.NAME);
                command.Parameters.AddWithValue("@FILTER_PERIOD", SVR.FILTER_PERIOD);
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }
            var DATA = (from row in DT1.Tables[1].AsEnumerable()
                        select new GET_HEADER()
                        {
                            TR_LM = row["TR_LM"].ToString(),
                            IS_LM = row["IS_LM"].ToString(),
                            ID_LM = row["ID_LM"].ToString(),
                            TR_TM = row["TR_TM"].ToString(),
                            IS_TM = row["IS_TM"].ToString(),
                            ID_TM = row["ID_TM"].ToString(),
                            TR_PER = row["TR_PER"].ToString() ?? "0%",
                            IS_PER = row["IS_PER"].ToString() ?? "0%",
                            ID_PER = row["ID_PER"].ToString() ?? "0%",
                        }).ToList();

            return new PP<List<GET_HEADER>>
            {
                Result = Convert.ToBoolean(DT1.Tables[0].Rows[0]["RESULT"]),
                Message = DT1.Tables[0].Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = DATA
            };
        }
        public PP<List<GET_DATA_CHART_1>> GET_DATA_CHART_1(SERVER SVR)
        {
            DataSet DT1 = new DataSet();
            string query = @"EXEC [SNIFFER_GETDATA] @SERVER";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@SERVER", SVR.NAME);
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }

            var DATA = (from row in DT1.Tables[1].AsEnumerable()
                        select new GET_DATA_CHART_1()
                        {
                            SOURCE_IP = row["SOURCE_IP"].ToString(),
                            REQUESTCOUNT = row["REQUESTCOUNT"].ToString(),
                            MINUTE = row["MINUTE"].ToString(),
                            AVGLENGTH = row["AVGLENGTH"].ToString()
                        }).ToList();

            return new PP<List<GET_DATA_CHART_1>>
            {
                Result = Convert.ToBoolean(DT1.Tables[0].Rows[0]["RESULT"]),
                Message = DT1.Tables[0].Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = DATA
            };
        }
        public PP<List<GET_DATA_CHART_2>> GET_DATA_CHART_2(SERVER SVR)
        {
            DataSet DT1 = new DataSet();
            string query = @"EXEC [SNIFFER_GETDATA] @SERVER";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@SERVER", SVR.NAME);
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }

            var DATA = (from row in DT1.Tables[2].AsEnumerable()
                        select new GET_DATA_CHART_2()
                        {
                            SOURCE_IP = row["SOURCE_IP"].ToString(),
                            METHODE = row["METHODE"].ToString(),
                            ACTIVITY_COUNT = row["ACTIVITY_COUNT"].ToString()
                        }).ToList();

            return new PP<List<GET_DATA_CHART_2>>
            {
                Result = Convert.ToBoolean(DT1.Tables[0].Rows[0]["RESULT"]),
                Message = DT1.Tables[0].Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = DATA
            };
        }
        public PP<List<GET_DATA_CHART_3>> GET_DATA_CHART_3(SERVER SVR)
        {
            DataSet DT1 = new DataSet();
            string query = @"EXEC [SNIFFER_GETDATA] @SERVER";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@SERVER", SVR.NAME);
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }

            var DATA = (from row in DT1.Tables[3].AsEnumerable()
                        select new GET_DATA_CHART_3()
                        {
                            DATE = row["DATE"].ToString(),
                            TOTALREQUEST = row["TOTALREQUEST"].ToString()
                        }).ToList();

            return new PP<List<GET_DATA_CHART_3>>
            {
                Result = Convert.ToBoolean(DT1.Tables[0].Rows[0]["RESULT"]),
                Message = DT1.Tables[0].Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = DATA
            };
        }
        public PP<List<GET_DATA_CHART_4>> GET_DATA_CHART_4(SERVER SVR)
        {
            DataSet DT1 = new DataSet();
            string query = @"EXEC [SNIFFER_GETDATA] @SERVER";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@SERVER", SVR.NAME);
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }

            var DATA = (from row in DT1.Tables[4].AsEnumerable()
                        select new GET_DATA_CHART_4()
                        {
                            Hours = row["hours"].ToString(),
                            TimeStart = row["time_start"].ToString(),
                            TimeEnd = row["time_end"].ToString(),
                            ActivityCount = row["activity_count"].ToString(),
                            ActivityGet = row["activity_get"].ToString(),
                            ActivityPost = row["activity_post"].ToString(),
                            ActivityOther = row["activity_other"].ToString()
                        }).ToList();

            return new PP<List<GET_DATA_CHART_4>>
            {
                Result = Convert.ToBoolean(DT1.Tables[0].Rows[0]["RESULT"]),
                Message = DT1.Tables[0].Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = DATA
            };
        }
        public PP<List<GET_DATA_CHART_5>> GET_DATA_CHART_5(SERVER SVR)
        {
            DataSet DT1 = new DataSet();
            string query = @"EXEC [SNIFFER_GETDATA] @SERVER";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@SERVER", SVR.NAME);
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }

            var DATA = (from row in DT1.Tables[5].AsEnumerable()
                        select new GET_DATA_CHART_5()
                        {
                            TOTAL_REQUEST = row["TOTAL_REQUEST"].ToString(),
                            APP_NAME = row["APP_NAME"].ToString(),
                        }).ToList();

            return new PP<List<GET_DATA_CHART_5>>
            {
                Result = Convert.ToBoolean(DT1.Tables[0].Rows[0]["RESULT"]),
                Message = DT1.Tables[0].Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = DATA
            };
        }
        public PP<List<GET_DATA_CHART_6>> GET_DATA_CHART_6(SERVER SVR)
        {
            DataSet DT1 = new DataSet();
            string query = @"EXEC [SNIFFER_GETDATA] @SERVER";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@SERVER", SVR.NAME);
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }

            var DATA = (from row in DT1.Tables[6].AsEnumerable()
                        select new GET_DATA_CHART_6()
                        {
                            TOTAL_REQUEST = row["TOTAL_REQUEST"].ToString(),
                            USERAGENT = row["USERAGENT"].ToString(),
                        }).ToList();

            return new PP<List<GET_DATA_CHART_6>>
            {
                Result = Convert.ToBoolean(DT1.Tables[0].Rows[0]["RESULT"]),
                Message = DT1.Tables[0].Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = DATA
            };
        }



        public PP<List<GETDATA>> GET_DATA_CHART(SERVER SVR)
        {
            DataSet DT1 = new DataSet();
            string query = @"EXEC [SNIFFER_GETDATA] @SERVER, @IP_S, @FILTER_PERIOD";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                var IP_S = (COOKIES.GetCookies("IP_SOURCE") ?? "");
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@SERVER", SVR.NAME);
                command.Parameters.AddWithValue("@IP_S", IP_S);
                command.Parameters.AddWithValue("@FILTER_PERIOD", SVR.FILTER_PERIOD);
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }

            var DATA1 = (from row in DT1.Tables[1].AsEnumerable()
                         select new GET_DATA_CHART_1()
                         {
                             SOURCE_IP = row["SOURCE_IP"].ToString(),
                             REQUESTCOUNT = row["REQUESTCOUNT"].ToString(),
                             MINUTE = row["MINUTE"].ToString(),
                             AVGLENGTH = row["AVGLENGTH"].ToString()
                         }).ToList();

            var DATA2 = (from row in DT1.Tables[2].AsEnumerable()
                         select new GET_DATA_CHART_2()
                         {
                             SOURCE_IP = row["SOURCE_IP"].ToString(),
                             METHODE = row["METHODE"].ToString(),
                             ACTIVITY_COUNT = row["ACTIVITY_COUNT"].ToString()
                         }).ToList();

            var DATA3 = (from row in DT1.Tables[3].AsEnumerable()
                        select new GET_DATA_CHART_3()
                        {
                            DATE = row["DATE"].ToString(),
                            TOTALREQUEST = row["TOTALREQUEST"].ToString()
                        }).ToList();

            var DATA4 = (from row in DT1.Tables[4].AsEnumerable()
                        select new GET_DATA_CHART_4()
                        {
                            Hours = row["hours"].ToString(),
                            TimeStart = row["time_start"].ToString(),
                            TimeEnd = row["time_end"].ToString(),
                            ActivityCount = row["activity_count"].ToString(),
                            ActivityGet = row["activity_get"].ToString(),
                            ActivityPost = row["activity_post"].ToString(),
                            ActivityOther = row["activity_other"].ToString()
                        }).ToList();

            var DATA5 = (from row in DT1.Tables[5].AsEnumerable()
                        select new GET_DATA_CHART_5()
                        {
                            TOTAL_REQUEST = row["TOTAL_REQUEST"].ToString(),
                            APP_NAME = row["APP_NAME"].ToString(),
                        }).ToList();

            var DATA6 = (from row in DT1.Tables[6].AsEnumerable()
                        select new GET_DATA_CHART_6()
                        {
                            TOTAL_REQUEST = row["TOTAL_REQUEST"].ToString(),
                            USERAGENT = row["USERAGENT"].ToString(),
                        }).ToList();

            var DATAS = new GETDATA
            {
                CHART_1 = DATA1,
                CHART_2 = DATA2,
                CHART_3 = DATA3,
                CHART_4 = DATA4,
                CHART_5 = DATA5,
                CHART_6 = DATA6,
            };

            return new PP<List<GETDATA>>
            {
                Result = Convert.ToBoolean(DT1.Tables[0].Rows[0]["RESULT"]),
                Message = DT1.Tables[0].Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = new List<GETDATA> { DATAS }
            };
        }

        public PP<List<GETDATA>> GET_DATA_CHART_DETAILIP(SERVER SVR)
        {
            DataSet DT1 = new DataSet();
            string query = @"EXEC [SNIFFER_GETDATA_DETAILIP] @SERVER, @IP_S";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                var IP_S = (COOKIES.GetCookies("IP_SOURCE") ?? "");
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@SERVER", SVR.NAME);
                command.Parameters.AddWithValue("@IP_S", IP_S);
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }

            var DATA1 = (from row in DT1.Tables[1].AsEnumerable()
                         select new GET_DATA_CHART_1()
                         {
                             SOURCE_IP = row["SOURCE_IP"].ToString(),
                             REQUESTCOUNT = row["REQUESTCOUNT"].ToString(),
                             MINUTE = row["MINUTE"].ToString(),
                             AVGLENGTH = row["AVGLENGTH"].ToString()
                         }).ToList();

            var DATA2 = (from row in DT1.Tables[2].AsEnumerable()
                         select new GET_DATA_CHART_2()
                         {
                             SOURCE_IP = row["SOURCE_IP"].ToString(),
                             METHODE = row["METHODE"].ToString(),
                             ACTIVITY_COUNT = row["ACTIVITY_COUNT"].ToString()
                         }).ToList();

            var DATA3 = (from row in DT1.Tables[3].AsEnumerable()
                         select new GET_DATA_CHART_3()
                         {
                             DATE = row["DATE"].ToString(),
                             TOTALREQUEST = row["TOTALREQUEST"].ToString()
                         }).ToList();

            var DATA4 = (from row in DT1.Tables[4].AsEnumerable()
                         select new GET_DATA_CHART_4()
                         {
                             Hours = row["hours"].ToString(),
                             TimeStart = row["time_start"].ToString(),
                             TimeEnd = row["time_end"].ToString(),
                             ActivityCount = row["activity_count"].ToString(),
                             ActivityGet = row["activity_get"].ToString(),
                             ActivityPost = row["activity_post"].ToString(),
                             ActivityOther = row["activity_other"].ToString()
                         }).ToList();

            var DATA5 = (from row in DT1.Tables[5].AsEnumerable()
                         select new GET_DATA_CHART_5()
                         {
                             TOTAL_REQUEST = row["TOTAL_REQUEST"].ToString(),
                             APP_NAME = row["APP_NAME"].ToString(),
                         }).ToList();

            var DATA6 = (from row in DT1.Tables[6].AsEnumerable()
                         select new GET_DATA_CHART_6()
                         {
                             TOTAL_REQUEST = row["TOTAL_REQUEST"].ToString(),
                             USERAGENT = row["USERAGENT"].ToString(),
                         }).ToList();

            var DATAS = new GETDATA
            {
                CHART_1 = DATA1,
                CHART_2 = DATA2,
                CHART_3 = DATA3,
                CHART_4 = DATA4,
                CHART_5 = DATA5,
                CHART_6 = DATA6,
            };

            return new PP<List<GETDATA>>
            {
                Result = Convert.ToBoolean(DT1.Tables[0].Rows[0]["RESULT"]),
                Message = DT1.Tables[0].Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = new List<GETDATA> { DATAS }
            };
        }

        public List<GET_DATA_TABLE_DETAILIP> GET_DATA_TABLE_DETAILIP(SERVER SVR)
        {
            DataSet DT1 = new DataSet();
            string query = @"EXEC [SNIFFER_GETDATA_DETAILIP_TABLE] @SERVER, @IP_S";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                var IP_S = (COOKIES.GetCookies("IP_SOURCE") ?? "");
                var NAME = (COOKIES.GetCookies("SERVER") ?? "");
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@SERVER", NAME);
                command.Parameters.AddWithValue("@IP_S", IP_S);
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = 1000;
                command.Connection.Close();
            }
            var DATA = (from row in DT1.Tables[0].AsEnumerable()
                        select new GET_DATA_TABLE_DETAILIP()
                        {
                            TOTAL = row["TOTAL"].ToString(),
                            QUERY = row["QUERY"].ToString(),
                            HEX = row["HEX"].ToString()
                        }).ToList();
            return DATA;
        }

        public class RESPONSE
        {
            public bool RESULT { get; set; }
            public string MESSAGE { get; set; }
            public int STATUSCODE { get; set; }
        }
        public class PP<T>
        {
            public T Data { get; set; }
            public bool Result { get; set; }
            public string Message { get; set; }
            public int StatusCode { get; set; }
        }
    }
}