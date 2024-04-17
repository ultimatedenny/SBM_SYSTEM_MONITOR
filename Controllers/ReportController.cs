using System.IO;
using System.Security.Cryptography;
using System;
using System.Web.Mvc;
using SBM_SYSTEM_MONITOR.Classes;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Management;
using System.Collections.Generic;

namespace SBM_SYSTEM_MONITOR.Controllers
{

    public class ReportController : Controller
    {
        readonly JwtTokenGenerator JWT = new JwtTokenGenerator();
        readonly MasterDataController MD = new MasterDataController();

        [JwtAuthentication]
        public ActionResult QA()
        {
            try
            {
                GetCookies();
                COOKIES.PostCookies("SERVER", "QA");
                COOKIES.PostCookies("IP_DESTINATION", "172.18.100.172");
                COOKIES.PostCookies("IP_HOST", "");
                COOKIES.PostCookies("IP_STATUS", "");
                COOKIES.PostCookies("IP_SOURCE", "");
                COOKIES.PostCookies("APP_NAME", "");
                
                if(COOKIES.GetCookies("SERVER") != "QA")
                {
                    return RedirectToAction("QA", "Report");
                }
                else
                {
                    ViewBag.Title = COOKIES.GetCookies("SERVER");
                    ViewBag.Subtitle = COOKIES.GetCookies("IP_DESTINATION");
                    return View();
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        [JwtAuthentication]
        public ActionResult PRD()
        {
            try
            {
                GetCookies();
                COOKIES.PostCookies("SERVER", "PRD");
                COOKIES.PostCookies("IP_DESTINATION", "172.18.100.84");
                COOKIES.PostCookies("IP_HOST", "");
                COOKIES.PostCookies("IP_STATUS", "");
                COOKIES.PostCookies("IP_SOURCE", "");
                COOKIES.PostCookies("APP_NAME", "");

                if (COOKIES.GetCookies("SERVER") != "PRD")
                {
                    return RedirectToAction("PRD", "Report");
                }
                else
                {
                    ViewBag.Title = COOKIES.GetCookies("SERVER");
                    ViewBag.Subtitle = COOKIES.GetCookies("IP_DESTINATION");
                    return View();
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        [JwtAuthentication]
        public ActionResult Detail_Ip(string IP)
        {
            try
            {
                if(!string.IsNullOrEmpty(IP))
                {
                    GetCookies();
                    var IP_HOST = "";
                    var IP_STATUS = "";
                    using (Ping ping = new Ping())
                    {
                        PingReply reply = ping.Send(IP);
                        IPHostEntry hostEntrys = Dns.GetHostEntry(IP);
                        IP_STATUS = reply.Status.ToString() ?? "UNKNOWN";
                        IP_HOST = hostEntrys.HostName.ToString();

                        COOKIES.PostCookies("IP_HOST", IP_HOST);
                        COOKIES.PostCookies("IP_STATUS", IP_STATUS);
                        COOKIES.PostCookies("IP_SOURCE", IP);
                        ViewBag.Title = COOKIES.GetCookies("SERVER");
                        ViewBag.Subtitle = COOKIES.GetCookies("IP_DESTINATION");
                        ViewBag.Ip = COOKIES.GetCookies("IP_SOURCE");
                        ViewBag.IpHost = COOKIES.GetCookies("IP_HOST");
                        ViewBag.IpStatus = COOKIES.GetCookies("IP_STATUS");
                        //string[] activeUsers = GetActiveUsers(IP, "", "");
                        //Console.WriteLine("Active Users:");
                        //foreach (string user in activeUsers)
                        //{
                        //    Console.WriteLine(user);
                        //}
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Error");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        [JwtAuthentication]
        public ActionResult Detail_Ap(string APP)
        {
            try
            {
                if (!string.IsNullOrEmpty(APP))
                {
                    GetCookies();
                    COOKIES.PostCookies("APP_NAME", APP);
                    ViewBag.Title = COOKIES.GetCookies("SERVER");
                    ViewBag.Subtitle = COOKIES.GetCookies("IP_DESTINATION");
                    ViewBag.AppName = COOKIES.GetCookies("APP_NAME");
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Error");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }


















        public static string[] GetActiveUsers(string remoteMachineIP, string username, string password)
        {
            try
            {
                List<string> activeUsers = new List<string>();
                ConnectionOptions options = new ConnectionOptions
                {
                    Username = username,
                    Password = password
                };
                ManagementScope scope = new ManagementScope($@"\\{remoteMachineIP}\root\cimv2", options);
                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                {
                    ManagementObjectCollection queryCollection = searcher.Get();
                    foreach (ManagementObject m in queryCollection.Cast<ManagementObject>())
                    {
                        activeUsers.Add(m["UserName"].ToString());
                    }
                }
                return activeUsers.ToArray();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public void GetCookies()
        {
            ViewBag.USERID = COOKIES.GetCookies("USERID") ?? "";
            ViewBag.WINDOWSID = COOKIES.GetCookies("WINDOWSID") ?? "";
            ViewBag.NAME = COOKIES.GetCookies("NAME") ?? "";
            ViewBag.EMAIL = COOKIES.GetCookies("EMAIL") ?? "";
            ViewBag.DEPARTMENT = COOKIES.GetCookies("DEPARTMENT") ?? "";
            ViewBag.GROUPS_MDM = COOKIES.GetCookies("GROUPS_MDM") ?? "";
            ViewBag.GROUPS_PBI = COOKIES.GetCookies("GROUPS_PBI") ?? "";
            ViewBag.INITIALS = COOKIES.GetCookies("INITIALS") ?? "";
        }
        private string GetUserIpAddress()
        {
            string ipAddress = HttpContext.Request.UserHostAddress;
            if (string.IsNullOrEmpty(ipAddress) || ipAddress.ToLower() == "unknown")
            {
                string forwardedFor = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(forwardedFor))
                {
                    string[] forwardedIps = forwardedFor.Split(',');
                    if (forwardedIps.Length > 0)
                    {
                        ipAddress = forwardedIps[0].Trim();
                    }
                }
            }
            if (string.IsNullOrEmpty(ipAddress) || ipAddress.ToLower() == "unknown")
            {
                ipAddress = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ipAddress;
        }
        public static string Encrypt(string plainText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(key);
                aesAlg.GenerateIV();
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    byte[] encryptedDataWithIV = aesAlg.IV.Concat(msEncrypt.ToArray()).ToArray();
                    return Convert.ToBase64String(encryptedDataWithIV);
                }
            }
        }
        public static string Decrypt(string cipherText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(key);
                byte[] iv = new byte[aesAlg.BlockSize / 8];
                byte[] encryptedData = Convert.FromBase64String(cipherText);
                Array.Copy(encryptedData, iv, iv.Length);
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedData, iv.Length, encryptedData.Length - iv.Length))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}