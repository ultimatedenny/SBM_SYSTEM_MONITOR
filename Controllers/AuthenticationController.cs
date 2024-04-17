using System;
using System.Web.Mvc;
using System.DirectoryServices;
using SBM_SYSTEM_MONITOR.Models;
using SBM_SYSTEM_MONITOR.Classes;
using System.Web;
using SBM_SYSTEM_MONITOR;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using BotDetect.Web.Mvc;


namespace SBM_SYSTEM_MONITOR.Controllers
{

    public class AuthenticationController : Controller
    {
        readonly MASTERDATA MD = new MASTERDATA();
        readonly JwtTokenGenerator JWT = new JwtTokenGenerator();

        [AllowAnonymous]
        public ActionResult Login()
        {   
            string userIP = Request.UserHostAddress;
            if (JWT.IsAuth())
            {
                return RedirectToAction("QA", "Report");
            }
            else
            {
                HttpCookieCollection cookies = Request.Cookies;
                foreach (string cookieName in cookies.AllKeys)
                {
                    if (!cookieName.StartsWith("__RequestVerificationToken") && !cookieName.Contains("ASP.NET_SessionId"))
                    {
                        HttpCookie cookie = new HttpCookie(cookieName)
                        {
                            Expires = DateTime.Now.AddDays(-1)
                        };
                        Response.Cookies.Add(cookie);
                    }
                }
                return View();
            }
        }

        public ActionResult Logout()
        {
            HttpCookieCollection cookies = Request.Cookies;
            foreach (string cookieName in cookies.AllKeys)
            {
                HttpCookie cookie = new HttpCookie(cookieName)
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(cookie);
            }
            return RedirectToAction("Login", "Authentication");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [CaptchaValidationActionFilter("CaptchaCode", "ExampleCaptcha", "Incorrect!")]
        public ActionResult Login(LoginModel model)
        {
            try
            {
                MvcCaptcha.ResetCaptcha("ExampleCaptcha");
                if (ModelState.IsValid)
                {
                    var Result = MD.GET_USER_INFO(model.WINDOWS_ID);
                    if (Result.Count != 0)
                    {
                        UserInfo userInfo = GetUserInfo(model.WINDOWS_ID, model.PASSWORD, "SHIMANOACE");
                        if (!string.IsNullOrEmpty(userInfo.Email.ToLower()))
                        {
                            CookieContainer cookieContainer = new CookieContainer();
                            bool isCookieAvailable = IsCookieAvailable(cookieContainer, "app.powerbi.com", "SignInStateCookie");
                            string INITIALS = GetFirstAndLastInitials(Result[0].FULL_NAME.ToUpper());

                            COOKIES.PostCookies("USERID", Result[0].USERID.ToLower());
                            COOKIES.PostCookies("WINDOWSID", Result[0].WINDOWSID.ToLower());
                            COOKIES.PostCookies("NAME", Result[0].FULL_NAME.ToUpper());
                            COOKIES.PostCookies("EMAIL", Result[0].EMAIL.ToUpper());
                            COOKIES.PostCookies("DEPARTMENT", Result[0].DEPT.ToUpper());
                            COOKIES.PostCookies("GROUPS_MDM", Result[0].GROUPS_MDM.ToUpper());
                            COOKIES.PostCookies("GROUPS_PBI", Result[0].GROUPS_PBI.ToUpper());
                            COOKIES.PostCookies("INITIALS", INITIALS);

                            COOKIES.SetExpire("USERID", 1);
                            COOKIES.SetExpire("WINDOWSID", 1);
                            COOKIES.SetExpire("NAME", 1);
                            COOKIES.SetExpire("EMAIL", 1);
                            COOKIES.SetExpire("DEPARTMENT", 1);
                            COOKIES.SetExpire("GROUPS_MDM", 1);
                            COOKIES.SetExpire("GROUPS_PBI", 1);
                            COOKIES.SetExpire("INITIALS", 1);

                            JwtTokenGenerator jwtTokenGenerator = new JwtTokenGenerator();

                            var USERID = COOKIES.GetCookies("USERID").ToString();
                            var WINDOWSID = COOKIES.GetCookies("WINDOWSID").ToString();
                            var NAME = COOKIES.GetCookies("NAME").ToString();
                            var EMAIL = COOKIES.GetCookies("EMAIL").ToString();
                            var DEPARTMENT = COOKIES.GetCookies("DEPARTMENT").ToString();
                            var GROUPS_MDM = COOKIES.GetCookies("GROUPS_MDM").ToString();
                            var GROUPS_PBI = COOKIES.GetCookies("GROUPS_PBI").ToString();

                            var KEY = GenerateStrongKey(USERID, WINDOWSID, NAME, EMAIL, DEPARTMENT, GROUPS_MDM, GROUPS_PBI);
                            GROUPS_PBI = (KEY + GROUPS_PBI + KEY);
                            DEPARTMENT = (KEY + DEPARTMENT + KEY);

                            var TOKEN = jwtTokenGenerator.GenerateToken(USERID, DEPARTMENT, GROUPS_PBI);

                            COOKIES.PostCookies("TOKEN", TOKEN);

                            //TempData["RedirectUrl"] = "https://app.powerbi.com/signin#code=0.AVQA224vNSkDaU2yenJvvsssXw8BHIdhXrFPg6yYYQp-kRBUADc.AgABAAIAAAAmoFfGtYxvRrNriQdPKIZ-AgDs_wUA9P-DqZonaaYdquvmB9ySpHz6zIcKZZGAWNO_Cy-oBWZnIoJeoZaXQ9p0xB0-0RiBNxkbdZ2wvKFTUDbVawMBp8ppBU-CYAeDMs-ZQ-i3r8vdXogfpJQjNsFqgIoxGjL_RsCb-RYV2xAZveEdZ8qCzX16nX8duTwUb77tnBIrEmxO6svp0ZT8Jvp107rRziDt8tbNzRMJUfEY2iNDqljdAh8LOs1qJFNnR_M00G9knxoSxWRgYApmzhf-fu51LLH4_8VGOwXlVl31kkcJktYf4ogxR5-wabPeq96nN-Ne_lDz-V5IpuCIK_lgNoAguDBhS-rkR_iS-bq-pKNJUxpnn9EXf4g9f2ooHRAEoFbuIcuK_LgmgsrWcUaqwb3Qg2lrUycAchIWbZoCJxinrqkOeL20uNYHY-9deCHkZymh5lmNgP4b5_YTtozMrKivXDkqliz_SRTPx_La-7M0GGBeKjIq6lJINtZz47PS7nhYMvpDYo1yNaf5Tk3eSTdZC4Zt85q2zNDN1TRHDXTsTOc6Vw7MEwDIye3Cfps_aCcgux63ErE8IxFp4YZtcIhtUbn7qU9fp1uw_Pc0Q35F7ZjzFpg4H9gNc7yOEECJlFL9zNVuewNHZf87Wqd14e9TBrraFwtenji5FPIt4QkAhtn2CMe0vNclQnXwuWQNkt0XSfrfhZ3Gnl25XXBskZBkcfykFEJvCZvEdoJNJmk-cxYYkNLBnEoqJxqg3H1J8q5NpfDmT6h5SjZTDaL9SXoMMv2TgiRy04CIbzR5ZDWWkKVrfSxrzlek76jVD6of8B_NR2HsKOH34DJdjgZZhzcd5-8tU3NEYKlHzzW5deuvtVi6buu85veBBEShn9AfQ0kT8c30a6wXeMgmStXXdJeFuVoI1yHa3gSWqNLgdXlWswKY9kwmRxBXZt2jkRjBBbGtFVO2jr17k3ELns90mM5VQwtdGmM87cT67zSMmtih_lKxmWgeZ0KKQh0u7Yc&client_info=eyJ1aWQiOiJmMzlkMjczMC0xMjQ0LTQ4MmQtOTEwYy05NmZiMmE5YmFiZjQiLCJ1dGlkIjoiMzUyZjZlZGItMDMyOS00ZDY5LWIyN2EtNzI2ZmJlY2IyYzVmIn0&state=eyJpZCI6ImU3OTkzNjdmLTQ4NzgtNGEyNi04MWQwLTVjODlkMTQ2NGI1MiIsIm1ldGEiOnsiaW50ZXJhY3Rpb25UeXBlIjoicG9wdXAifX0%3d%7c1702972330441.2002%3b1702972330441.9001%3b1702972310782.6&session_state=3b2c971e-6ed2-465d-aed1-8b84987c5bee&correlation_id=1ff7befd-79ba-440d-a9d6-00abbc16c2db";
                            return RedirectToAction("QA", "Report");
                        }
                        else
                        {
                            throw new Exception("Not Registered on Directory, please call IT for this.");
                        }
                    }
                    else
                    {
                        throw new Exception("Not Registered on MDM, please call IT for this.");
                    }
                }
                else
                {
                    throw new Exception("Credentials Error");
                }
            }
            catch (Exception ex)
            {
                TempData["FAILED"] = ex.Message.Replace("\r\n", "");
                return View();
            }
        }

        static string GenerateStrongKey(params string[] values)
        {
            var concatenatedValues = string.Join("|", values);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(concatenatedValues));
                return Convert.ToBase64String(hashBytes);
            }
        }

        public UserInfo GetUserInfo(string userName, string password, string domain)
        {
            UserInfo userInfo = new UserInfo();
            using (DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain, userName, password))
            {
                DirectorySearcher searcher = new DirectorySearcher(entry);
                searcher.Filter = "(&(objectClass=user)(sAMAccountName=" + userName + "))";
                searcher.PropertiesToLoad.Add("displayName");
                searcher.PropertiesToLoad.Add("mail");
                searcher.PropertiesToLoad.Add("department");
                searcher.PropertiesToLoad.Add("title");
                searcher.PropertiesToLoad.Add("telephoneNumber");
                searcher.PropertiesToLoad.Add("company");
                searcher.PropertiesToLoad.Add("co");
                //searcher.PropertiesToLoad.Add("manager"); // User's manager
                //searcher.PropertiesToLoad.Add("employeeID"); // User's employee ID
                searcher.PropertiesToLoad.Add("physicalDeliveryOfficeName");
                SearchResult result = searcher.FindOne();
                if (result != null)
                {
                    userInfo.Name = result.Properties["displayName"][0].ToString() ?? "";
                    userInfo.Email = result.Properties["mail"][0].ToString() ?? "";
                    userInfo.Department = result.Properties["department"][0].ToString() ?? "";
                    userInfo.Title = "";
                    userInfo.PhoneNumber = "";
                    userInfo.Company = result.Properties["company"][0].ToString() ?? "";
                    userInfo.Country = result.Properties["co"][0].ToString() ?? "";
                    //userInfo.Manager = result.Properties["manager"][0].ToString() ?? "";
                    //userInfo.EmployeeID = result.Properties["employeeID"][0].ToString() ?? "";
                    userInfo.Office = "";
                }
            }
            return userInfo;
        }

        static bool IsCookieAvailable(CookieContainer cookieContainer, string domain, string cookieName)
        {
            CookieCollection cookies = cookieContainer.GetCookies(new Uri($"https://{domain}"));

            foreach (System.Net.Cookie cookie in cookies)
            {
                if (cookie.Name == cookieName)
                {
                    return true;
                }
            }

            return false;
        }

        static string GetFirstAndLastInitials(string fullName)
        {
            string[] words = fullName.Split(' ');
            StringBuilder initialsBuilder = new System.Text.StringBuilder();
            if (words.Length > 0 && !string.IsNullOrEmpty(words[0]))
            {
                initialsBuilder.Append(words[0][0]);
            }
            if (words.Length > 1 && !string.IsNullOrEmpty(words[words.Length - 1]))
            {
                initialsBuilder.Append(words[words.Length - 1][0]);
            }
            string initials = initialsBuilder.ToString().ToUpper();
            return initials;
        }
    }
}