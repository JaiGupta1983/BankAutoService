using EgBL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for ICICI
/// </summary>
public class ICICI : Banks
{  /// <summary>
   /// version 2.0 Payment , Verification, corporateService, Scroll SHA256
   /// </summary>
    private string BankCode;
    public ICICI()
    {
        KeyName = "ICICI";
        BankCode = "6390013";
        Version = "2.0";
    }
    public override string CallManualDataPushService(string cipherText)
    {
        throw new NotImplementedException();
    }

    public override string CallVerifyManualService(string cipherText)
    {
        throw new NotImplementedException();
    }

    public override string CallVerifyService(string cipherText)
    {
         var response = string.Empty;
       
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        string URI = System.Web.Configuration.WebConfigurationManager.AppSettings["ICICI"];
        string myParameters = "encdata=" + cipherText + "";
        string beforesend = myParameters;
        using (WebClient wc = new WebClient())
        {
            wc.Headers.Add("apikey", "HImIO9hOkw3cLAwj3oBbtPi0maWHP0b6");
            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            response = wc.UploadString(URI, "POST", myParameters);
        }
        return response;
    }
    

    public override Dictionary<string, string> GetBankForwardString()
    {
        string plainText = string.Format("GRN={0}|HEAD_OF_ACC={1}|AMT={2}|REMITTER_NAME={3}|TOTALAMOUNT={4}|PayMode={5}|REG-TIN-NO={6}|LocationCode={7}|Filler={8}", GRN.ToString(), Head_Name[0].Substring(0, 4), Head_Amount[0], RemitterName, TotalAmount.ToString(), PaymentMode, TIN, LocationCode, filler);
        EncryptDecryptionBL objEncryption = new EncryptDecryptionBL();
        checkSum = objEncryption.GetSHA256(plainText);
        plainText = plainText + "|checksum=" + checkSum;
        string EncData = BanksEncryptionDecryption.GetEncryptedString(plainText, KeyName, Version);
        return GetforwardDictionary(EncData);
    }

    public override string GetRequestString(string plainText)
    {
        return BanksEncryptionDecryption.GetEncryptedString(plainText + "|checkSum=" + checkSum, KeyName, Version);
    }
}