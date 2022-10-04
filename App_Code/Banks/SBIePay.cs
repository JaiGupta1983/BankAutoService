using EgBL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
/// <summary>
/// Summary description for SBIePay
/// </summary>
public class SBIePay : Banks
{
    private string BankCode;
    public SBIePay()
    {
        KeyName = "BwmHPemeQsQhpwEGWmyQtQ==";
        BankCode = "1000132";
    }

    public override string CallManualDataPushService(string cipherText)
    {
        throw new NotImplementedException();
    }

    public override string CallVerifyService(string cipherText)
    {
        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            byte[] byteArray = Encoding.UTF8.GetBytes(cipherText);
            WebRequest request = WebRequest.Create("https://www.sbiepay.sbi/payagg/orderStatusQuery/getOrderStatusQuery");
            request.Method = "POST";
            request.ContentLength = byteArray.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            IpAddress = Dns.GetHostByName("sbiepay.sbi").AddressList[0].ToString();
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader readStream = new StreamReader(data, encode);
            Char[] read = new Char[256];
            int count = readStream.Read(read, 0, 256);
            string returnData = "";
            while (count > 0)
            {
                String str = new String(read, 0, count);
                returnData = returnData + "" + str;
                count = readStream.Read(read, 0, 256);
            }
            response.Close();
            return returnData;
        }
        catch (Exception ex)
        {
            InsertErrorLog(ex.Message, "1000132", "https://www.sbiepay.sbi/payagg/orderStatusQuery/getOrderStatusQuery", 1);
            return null;
        }
    }

    //protected override string DecryptResponseString(string cipherText)
    //{
    //    ePayEncryptionDecryptionBL objEpay = new ePayEncryptionDecryptionBL();
    //    return objEpay.Decrypt(cipherText, KeyName, 128);
    //}

    public override Dictionary<string, string> GetBankForwardString()
    {
        string plainText = string.Format("{0}^{1}^{2}^{3}^{4}^{5}^{6}^{7}^{8}^{9}^{10}^{11}^{12}^{13}^{14}^{15}^{16}^{17}^{18}^{19}^{20}^{21}^{22}^{23}^{24}", GRN.ToString(), Head_Name[0], Head_Amount[0], Head_Name[1], Head_Amount[1], Head_Name[2], Head_Amount[2], Head_Name[3], Head_Amount[3], Head_Name[4], Head_Amount[4], Head_Name[5], Head_Amount[5], Head_Name[6], Head_Amount[6], Head_Name[7], Head_Amount[7], Head_Name[8], Head_Amount[8], RemitterName, TotalAmount, PaymentMode, TIN, LocationCode, filler);
        plainText = "1000132|DOM|IN|INR|" + TotalAmount + "|" + plainText + "|" + ConfigurationManager.AppSettings["SuccessURL"].ToString() + "|" + ConfigurationManager.AppSettings["FailURL"].ToString() + "|SBIEPAY|" + GRN.ToString() + "|1000132|IMPS|ONLINE|ONLINE";
        //plainText = plainText + "|checkSum=" + checkSum;
        string EncData = BanksEncryptionDecryption.GetEncryptedString(plainText, KeyName);

        if (Convert.ToInt64(GRN.ToString()) != Convert.ToInt64(HttpContext.Current.Session["GrnNumber"]) || TotalAmount != Convert.ToDouble(HttpContext.Current.Session["NetAmount"]))
        {
            EgErrorHandller obj = new EgErrorHandller();
            obj.InsertError("ErrorAgencyBank" + '-' + Convert.ToString(GRN) + '-' + HttpContext.Current.Session["GrnNumber"].ToString());
            HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
        }
        return GetforwardDictionary(EncData);
    }
    //protected override string EncryptString(string PlainText)
    //{
    //    ePayEncryptionDecryptionBL objEncry = new ePayEncryptionDecryptionBL();
    //    return objEncry.Encrypt(PlainText, KeyName, 128);
    //}
    //protected override string UpdateStatus(string plainText, string Grn, string amt)
    //{
    //    string strmsg = "";
    //    try
    //    {
    //        EgEChallanBankBL objEgEChallanBankBL = new EgEChallanBankBL();
    //        //List<string> lstPlainText = new List<string>();
    //        string[] lstPlainText = plainText.Split('|');

    //        if (lstPlainText[0].ToString().Trim() == Grn.ToString().Trim() && Convert.ToDouble(lstPlainText[3].ToString()) == Convert.ToDouble(amt.ToString()))
    //        {

    //            objEgEChallanBankBL.GRN = Convert.ToInt32(Grn);
    //            objEgEChallanBankBL.BankCode = lstPlainText[13].ToString();
    //            if (lstPlainText[12].ToString() == "NA")
    //            {
    //                objEgEChallanBankBL.CIN = "0";
    //            }
    //            else
    //            {
    //                objEgEChallanBankBL.CIN = lstPlainText[12].ToString();
    //            }
    //            objEgEChallanBankBL.Ref = lstPlainText[1].ToString();
    //            objEgEChallanBankBL.Amount = Convert.ToDouble(amt);
    //            objEgEChallanBankBL.Status = lstPlainText[2].ToString().Substring(0, 1);
    //            if (lstPlainText[10].ToString() == "NA")
    //            {
    //                strmsg = "Due to some error, Unable to process.";
    //                return strmsg;
    //            }
    //            objEgEChallanBankBL.timeStamp = Convert.ToDateTime(lstPlainText[10].ToString());
    //            int result = objEgEChallanBankBL.UpdateSuccessStatus();

    //            if (result != 1)
    //            {
    //                //ScriptManager.RegisterStartupScript(this, GetType(), "Message", "alert('Unable to process.');", true);
    //                strmsg = "Unable to process.";
    //            }
    //            else
    //            {
    //                //ScriptManager.RegisterStartupScript(this, GetType(), "Message", "alert('Status Updated.');", true);
    //                strmsg = "Status Updated.";
    //            }
    //        }
    //        else
    //        {
    //            objEgEChallanBankBL.BankCode = lstPlainText[13].ToString();
    //            objEgEChallanBankBL.Ref = lstPlainText[1].ToString();
    //            if (lstPlainText[12].ToString() == "NA")
    //            {
    //                objEgEChallanBankBL.CIN = "0";
    //            }
    //            else
    //            {
    //                objEgEChallanBankBL.CIN = lstPlainText[12].ToString();
    //            }
    //            objEgEChallanBankBL.Amount = Convert.ToDouble(amt);
    //            objEgEChallanBankBL.Status = "F";
    //            objEgEChallanBankBL.GRN = Convert.ToInt32(Grn);
    //            if (lstPlainText[10].ToString() == "NA")
    //            {
    //                strmsg = "Due to some error, Unable to process.";
    //                return strmsg;
    //            }
    //            objEgEChallanBankBL.timeStamp = Convert.ToDateTime(lstPlainText[10].ToString());
    //            objEgEChallanBankBL.UpdateSuccessStatus();

    //            //ScriptManager.RegisterStartupScript(this, GetType(), "Message", "alert('Verified Unsuccessful.');", true);
    //            strmsg = "Verified Unsuccessful.";
    //        }
    //        //BindGrid();
    //    }
    //    catch (Exception ex)
    //    {
    //        EgErrorHandller obj = new EgErrorHandller();
    //        int result = obj.InsertError(ex.Message.ToString());
    //        //ScriptManager.RegisterStartupScript(this, GetType(), "Message", "alert('Due to some error, Unable to process.');", true);
    //        strmsg = "Due to some error, Unable to process.";
    //    }
    //    return strmsg;
    //}
    public override string GetRequestString(string plainText)
    {
        plainText = "" + "|" + BankCode + "|" + plainText.Split('|').GetValue(0).ToString().Split('=').GetValue(1).ToString();
        return "queryRequest=" + plainText + "&aggregatorId=SBIEPAY&merchantId=" + "1000132";
    }

    public override string CallVerifyManualService(string cipherText)
    {
        throw new NotImplementedException();
    }
}