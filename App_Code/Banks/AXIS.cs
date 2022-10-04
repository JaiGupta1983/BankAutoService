using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EgBL;
using System.Net;

/// <summary>
/// Summary description for AXIS
/// </summary>
public class AXIS : Banks
{
    private string BankCode;
    public AXIS()
    {
        KeyName = "AXIS";
        BankCode = "6360010";
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

        string URI = "https://uat-etendering.axisbank.co.in/easypay2.0/frontend/api/egrasenquiry";
       
        string myParameters = "encdata=" + cipherText + "&merchant_code=6918";
       
      
        string beforesend = myParameters;
        using (WebClient wc = new WebClient())
        { 
            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            // wc.Headers.Add("Content-Type", "application/multipart/form-data");
            //error=423a&message=Required+parameters+are+missing.

            response = wc.UploadString(URI, "POST", myParameters);
            //if (response.Split('=')[0] == "error")
            //{
            //    throw new Exception("No Response From AXIS Bank Service");
            //}
        }
        return response;
    }

    public override Dictionary<string, string> GetBankForwardString()
    {
        //string plainText = string.Format("GRN={0}|HEAD_OF_ACC={1}|AMT={2}|REMITTER_NAME={3}|UDF1={4}|Filler={5}", GRN.ToString(), Head_Name[0], Head_Amount[0], RemitterName, BsrCode.ToString(), filler);
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