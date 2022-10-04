using System;
using EgBL;
using System.Configuration;
using System.Net;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for SBI
/// </summary>
public class SBI : Banks
{
    public SBI(string paymentmode)
    {
        KeyName = "RAJASTHAN_EGRASS";
        KeyName = paymentmode == "M" ? "RAJASTHAN_EGRASS_Manual" : "RAJASTHAN_EGRASS";
        Version = paymentmode == "M" ? "1.0" : "2.0";
    }

    public override string CallManualDataPushService(string cipherText)
    {
        try
        {
            using (SBIManualChallanServ.IserSTGTV2Receipt_INBClient Ojpost = new SBIManualChallanServ.IserSTGTV2Receipt_INBClient())
            {
               // IpAddress = Dns.GetHostByName(ConfigurationManager.AppSettings["SBIManualServ.RAJASTHANWS"].Split('/').GetValue(2).ToString().Replace("www.", "")).AddressList[0].ToString();
                return Ojpost.ProcessRajasthanData(cipherText);// PUSH Service SBI Manual Challan anywhere
            }
        }
        catch (Exception ex)
        {
            InsertErrorLog(ex.Message, "0006326", ConfigurationManager.AppSettings["SBIManualServ.RAJASTHANWS"].Split('/').GetValue(2).ToString().Replace("www.", ""), 1);
            return null;
        }
    }

    public override string CallVerifyManualService(string cipherText)
    {
       
        try
        {
            using (SBIManualChallanServ.IserSTGTV2Receipt_INBClient Ojpost = new SBIManualChallanServ.IserSTGTV2Receipt_INBClient())
            {
                //IpAddress = Dns.GetHostByName(ConfigurationManager.AppSettings["SBIManualServ.RAJASTHANWS"].Split('/').GetValue(2).ToString().Replace("www.", "")).AddressList[0].ToString();
                // return obj.challanenquiry(cipherText);
               return Ojpost.OnlineEnquiry_Rajasthan(cipherText);
            }
        }
        catch (Exception ex)
        {
            InsertErrorLog(ex.Message, "0006326", "https://gbss.sbi.co.in:8443/GBSS_Service/STGTV2/Receipt/serSTGTV2Receipt_INB.svc", 1);
            return null;
        }
    }

    public override string CallVerifyService(string cipherText)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        try
        {


            using (SBIWebServ.DoubleVerifyMerchantService SbiService = new SBIWebServ.DoubleVerifyMerchantService())
            {

                IpAddress = Dns.GetHostByName(ConfigurationManager.AppSettings["SBIWebServ.DoubleVerifyMerchantService"].Split('/').GetValue(2).ToString().Replace("uatmerchant.", "")).AddressList[0].ToString();
                cipherText = cipherText + "^" + "RAJASTHAN_EGRASS";
                return SbiService.DoubleVerification(cipherText);
            }
        }
        catch (Exception ex)
        {
            InsertErrorLog(ex.Message, "0006326", ConfigurationManager.AppSettings["SBIWebServ.DoubleVerifyMerchantService"].Split('/').GetValue(2).ToString().Replace("merchant.", ""), 1);
            return null;
        }
    }

    //protected override string DecryptResponseString(string cipherText)
    //{
    //    SbiEncryptionDecryption objEncry = new SbiEncryptionDecryption();
    //    return objEncry.Decrypt(cipherText, System.Web.HttpContext.Current.Server.MapPath("../WebPages/Key/" + KeyName + ".key"));
    //}

    public override Dictionary<string, string> GetBankForwardString()
    {
        string plainText = string.Format("HEAD_OF_ACC9={17}|HEAD_OF_ACC8={15}|HEAD_OF_ACC7={13}|HEAD_OF_ACC6={11}|HEAD_OF_ACC5={9}|HEAD_OF_ACC4={7}|HEAD_OF_ACC3={5}|HEAD_OF_ACC2={3}|HEAD_OF_ACC1={1}|TOTALAMOUNT={20}|Filler={24}|LocationCode={23}|GRN={0}|PayMode={21}|AMT9={18}|AMT8={16}|AMT7={14}|AMT6={12}|AMT5={10}|AMT4={8}|AMT3={6}|REMITTER_NAME={19}|AMT2={4}|AMT1={2}|REG-TIN-NO={22}", GRN.ToString(), Head_Name[0], Head_Amount[0], Head_Name[1], Head_Amount[1], Head_Name[2], Head_Amount[2], Head_Name[3], Head_Amount[3], Head_Name[4], Head_Amount[4], Head_Name[5], Head_Amount[5], Head_Name[6], Head_Amount[6], Head_Name[7], Head_Amount[7], Head_Name[8], Head_Amount[8], RemitterName, TotalAmount, PaymentMode, TIN, LocationCode, filler);
        EncryptDecryptionBL objEncryption = new EncryptDecryptionBL();
        checkSum = objEncryption.GetSHA256(plainText);
        plainText = plainText + "|checkSum=" + checkSum;
        string EncData = BanksEncryptionDecryption.GetEncryptedString(plainText, KeyName,Version);
        if (Convert.ToInt64(GRN.ToString()) != Convert.ToInt64(HttpContext.Current.Session["GrnNumber"]) || TotalAmount != Convert.ToDouble(HttpContext.Current.Session["NetAmount"]))
        {
            EgErrorHandller obj = new EgErrorHandller();
            obj.InsertError("ErrorAgencyBank" + '-' + Convert.ToString(GRN) + '-' + HttpContext.Current.Session["GrnNumber"].ToString());
            HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
        }
        return GetforwardDictionary(EncData);
    }

    public override string GetRequestString(string plainText)
    {
        return BanksEncryptionDecryption.GetEncryptedString(plainText, KeyName, Version);
    }
}