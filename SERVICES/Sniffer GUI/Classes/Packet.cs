using System;
using System.Net;
using System.Text;

namespace SnifferGUI
{
    public class Packet
    {
        private const int LineCount = 30;
        private enum ProtocolType
        {
            GGP = 3,
            ICMP = 1,
            IDP = 22,
            IGMP = 2,
            IP = 4,
            ND = 77,
            PUP = 12,
            TCP = 6,
            UDP = 17,
            OTHERS = -1
        }
        private byte[] raw_Packet;
        private DateTime dateTime;
        private ProtocolType protocolType;
        private IPAddress src_IPAddress;
        private IPAddress des_IPAddress;
        private int src_Port;
        private int des_Port;
        private int totalLength;
        private int headLength;

        public Packet(byte[] raw)
        {
            if (raw == null)
            {
                throw new ArgumentNullException();
            }
            if (raw.Length < 20)
            {
                throw new ArgumentException();
            }
            raw_Packet = raw;
            dateTime = DateTime.Now;
            headLength = (raw[0] & 0x0F) * 4;
            if ((raw[0] & 0x0F) < 5)
            {
                throw new ArgumentException();
            }
            if ((raw[2] * 256 + raw[3]) != raw.Length)
            {
                throw new ArgumentException();
            }
            if (Enum.IsDefined(typeof(ProtocolType), (int)raw[9]))
            {
                protocolType = (ProtocolType)raw[9];
            }
            else
            {
                protocolType = ProtocolType.OTHERS;
            }
            src_IPAddress = new IPAddress(BitConverter.ToUInt32(raw, 12));
            des_IPAddress = new IPAddress(BitConverter.ToUInt32(raw, 16));
            totalLength = raw[2] * 256 + raw[3];
            if (protocolType == ProtocolType.TCP || protocolType == ProtocolType.UDP)
            {
                src_Port = raw[headLength] * 256 + raw[headLength + 1];
                des_Port = raw[headLength + 2] * 256 + raw[headLength + 3];
                if (protocolType == ProtocolType.TCP)
                {
                    headLength += 20;
                }
                else if (protocolType == ProtocolType.UDP)
                {
                    headLength += 8;
                }
            }
            else
            {
                src_Port = -1;
                des_Port = -1;
            }
        }

        public string SourceIp
        {
            get
            {
                return src_IPAddress.ToString();
            }
        }

        public string SourcePort
        {
            get
            {
                if (src_Port != -1)
                    return src_Port.ToString();
                else
                    return "";
            }
        }

        public string DestinationIP
        {
            get
            {
                return des_IPAddress.ToString();
            }
        }

        public string DestinationPort
        {
            get
            {
                if (des_Port != -1)
                    return des_Port.ToString();
                else
                    return "";
            }
        }

        public string Protocol
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                for (int i = headLength; i < TotalLength; i += LineCount)
                {
                    for (int j = i; j < TotalLength && j < i + LineCount; j++)
                    {
                        if (raw_Packet[j] > 31 && raw_Packet[j] < 128)
                        {
                            sb.Append((char)raw_Packet[j]);
                        }
                        else
                        {
                            sb.Append(".");
                        }
                    }
                    sb.Append("\n");
                }

                if (
                    sb.ToString().ToUpper().Contains("S.E.L.E.C.T") || sb.ToString().ToUpper().Contains("I.N.S.E.R.T") || sb.ToString().ToUpper().Contains("U.P.D.A.T.E") || sb.ToString().ToUpper().Contains("D.E.L.E.T.E") ||
                    sb.ToString().ToUpper().Contains("C.R.E.A.T.E") || sb.ToString().ToUpper().Contains("A.L.T.E.R") || sb.ToString().ToUpper().Contains("D.R.O.P") ||
                    sb.ToString().ToUpper().Contains("E.X.E.C.U.T.E") || sb.ToString().ToUpper().Contains("D.E.C.L.A.R.E") || sb.ToString().ToUpper().Contains("T.R.U.N.C.A.T.E") ||
                    sb.ToString().ToUpper().Contains("M.E.R.G.E") || sb.ToString().ToUpper().Contains("W.I.T.H") || sb.ToString().ToUpper().Contains("I.N.D.E.X") ||
                    sb.ToString().ToUpper().Contains("B.E.G.I.N") || sb.ToString().ToUpper().Contains("C.O.M.M.I.T") || sb.ToString().ToUpper().Contains("R.O.L.L.B.A.C.K") ||
                    sb.ToString().ToUpper().Contains("G.R.A.N.T") || sb.ToString().ToUpper().Contains("R.E.V.O.K.E") || sb.ToString().ToUpper().Contains("D.E.N.Y") ||
                    sb.ToString().ToUpper().Contains("U.N.I.O.N") || sb.ToString().ToUpper().Contains("I.N.T.E.R.S.E.C.T") || sb.ToString().ToUpper().Contains("E.X.C.E.P.T") ||
                    sb.ToString().ToUpper().Contains("H.A.V.I.N.G") || sb.ToString().ToUpper().Contains("G.R.O.U.P B.Y") || sb.ToString().ToUpper().Contains("O.R.D.E.R B.Y") ||
                    sb.ToString().ToUpper().Contains("J.O.I.N") || sb.ToString().ToUpper().Contains("I.N.N.E.R J.O.I.N") || sb.ToString().ToUpper().Contains("O.U.T.E.R J.O.I.N") ||
                    sb.ToString().ToUpper().Contains("L.E.F.T J.O.I.N") || sb.ToString().ToUpper().Contains("R.I.G.H.T J.O.I.N") || sb.ToString().ToUpper().Contains("F.U.L.L J.O.I.N") ||
                    sb.ToString().ToUpper().Contains("W.H.E.R.E") || sb.ToString().ToUpper().Contains("F.R.O.M") || sb.ToString().ToUpper().Contains("I.N.T.O") ||
                    sb.ToString().ToUpper().Contains("V.A.L.U.E.S") || sb.ToString().ToUpper().Contains("S.E.T") || sb.ToString().ToUpper().Contains("A.S") ||
                    sb.ToString().ToUpper().Contains("C.A.S.E") || sb.ToString().ToUpper().Contains("W.H.E.N") || sb.ToString().ToUpper().Contains("E.L.S.E") ||
                    sb.ToString().ToUpper().Contains("T.H.E.N") || sb.ToString().ToUpper().Contains("E.N.D") || sb.ToString().ToUpper().Contains("I.F") ||
                    sb.ToString().ToUpper().Contains("E.X.E.C") || sb.ToString().ToUpper().Contains("C.A.L.L") || sb.ToString().ToUpper().Contains("P.R.O.C.E.D.U.R.E") ||
                    sb.ToString().ToUpper().Contains("F.U.N.C.T.I.O.N") || sb.ToString().ToUpper().Contains("T.R.I.G.G.E.R") || sb.ToString().ToUpper().Contains("V.I.E.W") ||
                    sb.ToString().ToUpper().Contains("U.S.I.N.G") || sb.ToString().ToUpper().Contains("W.H.I.L.E") || sb.ToString().ToUpper().Contains("L.O.O.P") ||
                    sb.ToString().ToUpper().Contains("D.E.C.L.A.R.E") || sb.ToString().ToUpper().Contains("C.U.R.S.O.R") || sb.ToString().ToUpper().Contains("F.E.T.C.H") ||
                    sb.ToString().ToUpper().Contains("O.P.E.N") || sb.ToString().ToUpper().Contains("C.L.O.S.E") || sb.ToString().ToUpper().Contains("R.E.T.U.R.N")
                )
                {
                    return "SQL";
                }
                else
                {
                    return protocolType.ToString();
                }
            }
        }

        public int TotalLength
        {
            get
            {
                return totalLength;
            }
        }

        public string Time
        {
            get
            {
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss tt");
            }
        }

        public string HexString
        {
            get
            {
                StringBuilder sb = new StringBuilder(raw_Packet.Length);
                for (int i = headLength; i < TotalLength; i += LineCount)
                {
                    for (int j = i; j < TotalLength && j < i + LineCount; j++)
                    {
                        sb.Append(raw_Packet[j].ToString("X2") + " ");
                    }
                    sb.Append("\n");
                }
                return sb.ToString();
            }
        }

        public string Methode
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                for (int i = headLength; i < TotalLength; i += LineCount)
                {
                    for (int j = i; j < TotalLength && j < i + LineCount; j++)
                    {
                        if (raw_Packet[j] > 31 && raw_Packet[j] < 128)
                            sb.Append((char)raw_Packet[j]);
                        else
                            sb.Append(".");
                    }
                    sb.Append("\n");
                }
                if (sb.ToString().Contains("GET"))
                    return "GET";
                else if (sb.ToString().Contains("POST"))
                    return "POST";
                else if (sb.ToString().Contains("PUT"))
                    return "PUT";
                else if (sb.ToString().Contains("DELETE"))
                    return "DELETE";
                else if (sb.ToString().Contains("PATCH"))
                    return "PATCH";
                else if (sb.ToString().Contains("HEAD"))
                    return "HEAD";
                else if (sb.ToString().Contains("OPTIONS"))
                    return "OPTIONS";
                else if (sb.ToString().Contains("TRACE"))
                    return "TRACE";
                else if (sb.ToString().Contains("CONNECT"))
                    return "CONNECT";
                else
                    return "-"; 
            }
        }

        public string CharString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int i = headLength; i < TotalLength; i += LineCount)
                {
                    for (int j = i; j < TotalLength && j < i + LineCount; j++)
                    {
                        if (raw_Packet[j] > 31 && raw_Packet[j] < 128)
                        {
                            sb.Append((char)raw_Packet[j]);
                        }
                        else
                        {
                            sb.Append("\n");
                        }
                    }
                    sb.Append("");
                }
                return sb.ToString();
            }
        }

        public byte[] Bytes
        {
            get
            {
                return raw_Packet;
            }
        }

    }
}