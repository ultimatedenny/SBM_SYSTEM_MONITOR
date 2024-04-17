using SBM_SYSTEM_MONITOR.Classes;
using SBM_SYSTEM_MONITOR.Models;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SBM_SYSTEM_MONITOR.Controllers
{
    [JwtAuthentication]
    public class MasterDataController : Controller
    {
        readonly MASTERDATA MD = new MASTERDATA();
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
        public JsonResult GETHEADER(SERVER MODEL)
        {
            try
            {
                return Json(new { data = MD.GETHEADER(MODEL) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = false,
                    StatusCode = ((HttpException)ex).GetHttpCode(),
                    Message = ex.Message.ToString(),
                    Data = "",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_DATA_CHART_1(SERVER MODEL)
        {
            try
            {
                return Json(new { data = MD.GET_DATA_CHART_1(MODEL) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = false,
                    StatusCode = ((HttpException)ex).GetHttpCode(),
                    Message = ex.Message.ToString(),
                    Data = "",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_DATA_CHART_2(SERVER MODEL)
        {
            try
            {
                return Json(new { data = MD.GET_DATA_CHART_2(MODEL) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = false,
                    StatusCode = ((HttpException)ex).GetHttpCode(),
                    Message = ex.Message.ToString(),
                    Data = "",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_DATA_CHART_3(SERVER MODEL)
        {
            try
            {
                return Json(new { data = MD.GET_DATA_CHART_3(MODEL) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = false,
                    StatusCode = ((HttpException)ex).GetHttpCode(),
                    Message = ex.Message.ToString(),
                    Data = "",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_DATA_CHART_4(SERVER MODEL)
        {
            try
            {
                return Json(new { data = MD.GET_DATA_CHART_4(MODEL) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = false,
                    StatusCode = ((HttpException)ex).GetHttpCode(),
                    Message = ex.Message.ToString(),
                    Data = "",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_DATA_CHART_5(SERVER MODEL)
        {
            try
            {
                return Json(new { data = MD.GET_DATA_CHART_5(MODEL) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = false,
                    StatusCode = ((HttpException)ex).GetHttpCode(),
                    Message = ex.Message.ToString(),
                    Data = "",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_DATA_CHART_6(SERVER MODEL)
        {
            try
            {
                return Json(new { data = MD.GET_DATA_CHART_6(MODEL) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = false,
                    StatusCode = ((HttpException)ex).GetHttpCode(),
                    Message = ex.Message.ToString(),
                    Data = "",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_DATA_CHART(SERVER MODEL)
        {
            try
            {
                return Json(new { data = MD.GET_DATA_CHART(MODEL) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = false,
                    StatusCode = ((HttpException)ex).GetHttpCode(),
                    Message = ex.Message.ToString(),
                    Data = "",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_DATA_CHART_DETAILIP(SERVER MODEL)
        {
            try
            {
                return Json(new { data = MD.GET_DATA_CHART_DETAILIP(MODEL) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = false,
                    StatusCode = ((HttpException)ex).GetHttpCode(),
                    Message = ex.Message.ToString(),
                    Data = "",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_DATA_TABLE_DETAILIP(SERVER MODEL)
        {
            try
            {
                return Json(new { data = MD.GET_DATA_TABLE_DETAILIP(MODEL) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = false,
                    StatusCode = ((HttpException)ex).GetHttpCode(),
                    Message = ex.Message.ToString(),
                    Data = "",
                }, JsonRequestBehavior.AllowGet);
            }
        }






        public JsonResult GET_URL(PBI_SHARE_URL MODEL)
        {
            try
            {
                var URL_PASS  = GenerateAesKey(256);
                //var URL_TXT   = Encrypt(MODEL.REPORT_ID, URL_PASS);
                var URL_TXT   = HttpUtility.UrlEncode(Encrypt(MODEL.REPORT_ID, URL_PASS));
                var URL_TYPE  = MODEL.URL_TYPE;
                var URL_EXPI  = MODEL.URL_EXPI ?? "3999-12-31";
                var REPORT_ID = MODEL.REPORT_ID;
                var res       = MD.INSERT_PBI_SHARE_URL(REPORT_ID, URL_PASS, URL_TYPE, URL_TXT, URL_EXPI, "");
               
                return Json(new { res }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = false,
                    StatusCode = ((HttpException)ex).GetHttpCode(),
                    Message = ex.Message.ToString(),
                    Data = "",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_PERMISSION()
        {
            try
            {
                var res = MD.GET_PERMISSION();
                return Json(new { res }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = false,
                    StatusCode = ((HttpException)ex).GetHttpCode(),
                    Message = ex.Message.ToString(),
                    Data = "",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public static string GenerateStrongKey(params string[] values)
        {
            var concatenatedValues = string.Join("|", values);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(concatenatedValues));
                return Convert.ToBase64String(hashBytes);
            }
        }
        public static string Random128()
        {
            const int byteLength = 256;
            byte[] randomBytes = new byte[byteLength];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }
            return BitConverter.ToString(randomBytes).Replace("-", "").ToLower();
        }
        public static string GenerateAesKey(int keySizeInBits)
        {
            if (keySizeInBits != 128 && keySizeInBits != 192 && keySizeInBits != 256)
            {
                throw new ArgumentException("Invalid key size. Valid sizes are 128, 192, or 256 bits.");
            }
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] keyBytes = new byte[keySizeInBits / 8];
                rng.GetBytes(keyBytes);
                return Convert.ToBase64String(keyBytes);
            }
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