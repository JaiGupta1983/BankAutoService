using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using EgDAL;
using System.Web;
using DL;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;

namespace EgBL
{
    public class EgErrorHandller
    {
        GenralFunction gf = new GenralFunction();
        
        #region Class Method

        public string SetError(string error)
        {
            string ErrorName = "";

            if (HttpContext.Current.Error.InnerException != null)
            {
               ErrorName = HttpContext.Current.Error.InnerException.Message.ToString();
               // ErrorName = HttpContext.Current.Error.InnerException.StackTrace;
            }
            else if (HttpContext.Current.Server.GetLastError().Message != null)
            {
                //ErrorName = HttpContext.Current.Server.GetLastError().StackTrace;
               ErrorName = HttpContext.Current.Server.GetLastError().Message.ToString();
            }
            else
            {
                ErrorName = error.ToString();
            }

            if (ErrorName.Trim() == "")
            {
                var serverError = HttpContext.Current.Server.GetLastError() as HttpException;
                int errorCode = serverError.GetHttpCode();

                var ex = HttpContext.Current.Server.GetLastError();
                var httpException = ex as HttpException;
                ErrorName = httpException.GetHttpCode().ToString();
            }
            

            string Page_Name = System.Web.HttpContext.Current.Request.Url.ToString();

            SqlParameter[] PM = new SqlParameter[2];
            PM[0] = new SqlParameter("@ErrorName", SqlDbType.VarChar,-1) { Value = ErrorName };
            PM[1] = new SqlParameter("@PageName", SqlDbType.VarChar, 100) { Value = Page_Name };
            gf.UpdateData(PM, "GetErrorLog");

            //ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();

            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ////string responseString = SMSservice.GetSMSDetails(cipherText);
            //HttpWebRequest myReq =
            //    (HttpWebRequest)WebRequest.Create("https://smsgw.sms.gov.in/failsafe/HttpLink?username=" + "egras.auth" + "&pin=" + "Jh*$23et" + "&message=" + "Error:" + ErrorName + "&mnumber=" + "919462223691" + "&signature=" + "EGRASJ");

            //HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
            //System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
            //string responseString = respStreamReader.ReadToEnd();// "Message Accepted for Request ID=12313946898506681731361~code=API000 & info=Platform accepted & Time =2014/03/13/11/20";//
            //respStreamReader.Close();
            //myResp.Close();

            return ErrorName.ToString();
        }

        public int InsertError(string error)
        {
            string Page_Name = System.Web.HttpContext.Current.Request.Url.ToString();

            SqlParameter[] PM = new SqlParameter[2];
            PM[0] = new SqlParameter("@ErrorName", SqlDbType.VarChar, 500) { Value = error };
            PM[1] = new SqlParameter("@PageName", SqlDbType.VarChar, 100) { Value = Page_Name };
            return gf.UpdateData(PM, "GetErrorLog");
        }
        public void InsertStampLog(int MerchantCode, string EncData, string AUIN, Int64 GRN, string LogType)
        {
            GenralFunction gf1 = new GenralFunction();
            SqlParameter[] PM = new SqlParameter[7];
            PM[0] = new SqlParameter("@MerchantCode", SqlDbType.Int) { Value = MerchantCode };
            PM[1] = new SqlParameter("@EncData", SqlDbType.NVarChar, -1) { Value = EncData };
            PM[2] = new SqlParameter("@AUIN", SqlDbType.VarChar, 50) { Value = AUIN };
            PM[3] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRN };
            PM[4] = new SqlParameter("@URL", SqlDbType.NVarChar, 400) { Value = Convert.ToString(System.Web.HttpContext.Current.Request.UrlReferrer.AbsoluteUri) };
            PM[5] = new SqlParameter("@IpAddress", SqlDbType.VarChar, 20) { Value = HttpContext.Current.Request.ServerVariables[ConfigurationManager.AppSettings["IPAddressHTTP"]].ToString() };// Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]) };
            PM[6] = new SqlParameter("@Type", SqlDbType.Char, 2) { Value = LogType };
            gf1.UpdateData(PM, "InsertStampServiceLog");
        }

        public class TrustAllCertificatePolicy : System.Net.ICertificatePolicy
        {
            public TrustAllCertificatePolicy() { }
            public bool CheckValidationResult(ServicePoint sp,
                X509Certificate cert,
                WebRequest req,
                int problem)
            {
                return true;
            }
        }

        #endregion
    }
}