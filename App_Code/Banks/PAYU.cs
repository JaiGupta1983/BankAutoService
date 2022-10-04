using EgBL;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for PAYU
/// </summary>
public class PAYU : Banks
{
    private string BankCode;
    private string ProductInfo;
    private string EmailId;
    private string HashText;
    public string RESTServiceURL { get; private set; }
    public string ResultString { get; private set; }
    public string URITemplateMethodName { get; private set; }


    public PAYU()
    {
        ProductInfo = "XYZ";
        EmailId = "xyz@mail.com";
        BankCode = "9910001";
        KeyName = "PayU";
    }
    public override string CallManualDataPushService(string cipherText)
    {
        throw new NotImplementedException();
    }

    public override Dictionary<string, string> GetforwardDictionary(string EncData)
    {
        string AmountForm = Convert.ToDecimal(TotalAmount).ToString("g29");// eliminating trailing zeros
        Dictionary<string, string> GetforwardDictionary = new Dictionary<string, string>()
        {
            { "hash", EncData },
            { "txnid", GRN.ToString()},
            { "key", ConfigurationManager.AppSettings["PayUKey"].ToString() },
            { "amount", TotalAmount.ToString()},
            {"firstname", RemitterName.ToString() },
            {"email", EmailId},
            { "phone", "2222222222"},
            {"productinfo", ProductInfo},
            {"surl", ConfigurationManager.AppSettings["SuccessURL"].ToString() },
            { "furl", ConfigurationManager.AppSettings["FailURL"].ToString() },
            { "lastname", "xyz"},
            { "udf1", BankCode},
            { "udf2", Head_Name[0].ToString()},
            { "url",URL}
        };
        return GetforwardDictionary;

    }


    public override string CallVerifyService(string cipherText)
    {
        try
        {
            var responseString = "";
            using (var client = new WebClient())
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var values = new NameValueCollection();
                values["key"] = cipherText.Split('|').GetValue(0).ToString();
                values["var1"] = cipherText.Split('|').GetValue(1).ToString();
                values["command"] = cipherText.Split('|').GetValue(2).ToString();
                values["hash"] = cipherText.Split('|').GetValue(3).ToString();
                var response = client.UploadValues(ConfigurationManager.AppSettings["PayuVerifyURL"].ToString(), values);
                responseString = Encoding.Default.GetString(response);
                IpAddress = Dns.GetHostByName(ConfigurationManager.AppSettings["PayuVerifyURL"].Split('/').GetValue(2).ToString().Replace("www.", "")).AddressList[0].ToString();
                UploadData(BankCode, responseString, Convert.ToInt64(cipherText.Split('|').GetValue(1).ToString()), IpAddress, "N");
            }
            if (responseString.Split('&').GetValue(0).ToString().Split('=').GetValue(1).ToString() == "InvalidRequest" || responseString.Split('&').GetValue(0).ToString().Split('=').GetValue(1).ToString() == "hashIncorrect")
            {
                return null;
            }
            else
            {
                string Responsehash = responseString.Split('&').GetValue(2).ToString().Split('=').GetValue(1).ToString();

                ///Success & pending - SHA512(salt|status|amount|udf1|txnid|key)
                ///TransactionIdNotFound - SHA512(txnid)
                ///status=success&txnid=7119573&hash=27df99aac0e71d3e042e26163f8ca9a0fe7b96cb76be76eb3996545d6d463e34d5f0055b4fde59bfc0112817e7a91365ba79bcbaa8cfd50b82ca6e692c9e013f
                ///&mode=CC&amount=111.00&mihpayid=403993715518502687&cin=100031210001625102018&bank_ref_num=908694&error=E000&bankcode=CC
                ///&addedon=2018-10-25 12:55:19&udf1=1000312&udf2=0041
                ///
                if (responseString.Split('&').GetValue(0).ToString().Split('=').GetValue(1).ToString() == "TransactionIdNotFound")
                {
                    HashText = responseString.Split('&').GetValue(1).ToString().Split('=').GetValue(1).ToString();
                }
                else
                {
                    HashText = ConfigurationManager.AppSettings["PayuSalt"].ToString() +
                                "|" + responseString.Split('&').GetValue(0).ToString().Split('=').GetValue(1).ToString() +
                                "|" + responseString.Split('&').GetValue(4).ToString().Split('=').GetValue(1).ToString() +
                                "|" + responseString.Split('&').GetValue(11).ToString().Split('=').GetValue(1).ToString() +
                                "|" + responseString.Split('&').GetValue(1).ToString().Split('=').GetValue(1).ToString() +
                                "|" + ConfigurationManager.AppSettings["PayUKey"].ToString();

                }
                HashText = GenerateSHA512String(HashText).ToLower();
                if (HashText != Responsehash)
                {
                    responseString = "InCorrectHash&" + responseString;
                    //responseString = "CorrectHash&" + responseString;
                }
                else
                {
                    if (responseString.Split('&').GetValue(0).ToString().Split('=').GetValue(1).ToString() == "TransactionIdNotFound")
                    {
                        responseString = "status=Failure&" + responseString.Split('&').GetValue(1).ToString() + "&hash=0&mode=0&amount=" + TotalAmount + "&mihpayid=0&cin=NA&bank_ref_num=NA&error=0&bankcode=0&addedon=" + DateTime.Now + "&udf1=" + BankCode + "&udf2=0";
                        responseString = "CorrectHash&" + responseString;
                    }
                    else
                    {
                        responseString = "CorrectHash&" + responseString;
                    }
                }

            }
            return responseString;
        }

        catch (Exception ex)
        {
            InsertErrorLog(ex.Message, "9910001", ConfigurationManager.AppSettings["PayuVerifyURL"].ToString(), 1);
            return null;
        }
    }

    //protected override string DecryptResponseString(string cipherText)
    //{
    //    ePayEncryptionDecryptionBL objEpay = new ePayEncryptionDecryptionBL();
    //    return objEpay.Decrypt(cipherText, KeyName, 128);
    //}
    //(C0Dr8m|12345|10|Shopping|Test|test @test.com||abc||15|||||||3sf0jURk)
    public override Dictionary<string, string> GetBankForwardString()
    {
        //string plainText = string.Format(ConfigurationManager.AppSettings["PayUKey"].ToString()+ "|"+GRN.ToString()+ "|"+ TotalAmount.ToString() + "|" +ProductInfo + "|" +RemitterName.ToString().Trim()+"|"+EmailId+"|||||||||||" + ConfigurationManager.AppSettings["PayuSalt"].ToString());
        string plainText = string.Format(ConfigurationManager.AppSettings["PayUKey"].ToString()+"|" +GRN.ToString()+"|"+ TotalAmount.ToString()+ "|"+ProductInfo+"|"+RemitterName.ToString()+ "|"+EmailId+ "|"+BankCode+"|"+Head_Name[0].ToString()+"|||||||||" + ConfigurationManager.AppSettings["PayuSalt"].ToString());
        plainText = GenerateSHA512String(plainText).ToLower();
        if (Convert.ToInt64(GRN.ToString()) != Convert.ToInt64(HttpContext.Current.Session["GrnNumber"]) || TotalAmount != Convert.ToDouble(HttpContext.Current.Session["NetAmount"]))
        {
            EgErrorHandller obj = new EgErrorHandller();
            obj.InsertError("ErrorAgencyBank" + '-' + Convert.ToString(GRN) + '-' + HttpContext.Current.Session["GrnNumber"].ToString());
            HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
        }
        return GetforwardDictionary(plainText);
    }

    public static string GenerateSHA512String(string inputString)
    {
        SHA512 sha512 = SHA512Managed.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(inputString);
        byte[] hash = sha512.ComputeHash(bytes);
        return GetStringFromHash(hash);
    }

    private static string GetStringFromHash(byte[] hash)
    {
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            result.Append(hash[i].ToString("X2"));
        }
        return result.ToString();
    }

    public override string GetRequestString(string plainText)
    {
        plainText = string.Format(ConfigurationManager.AppSettings["PayUKey"].ToString()+"|"+ plainText.Split('|').GetValue(0).ToString().Split('=').GetValue(1).ToString() + "|"+"verify_payment");
        HashText = plainText.Split('|').GetValue(0).ToString() + "|" + plainText.Split('|').GetValue(2).ToString() + "|" + plainText.Split('|').GetValue(1).ToString() + "|" + ConfigurationManager.AppSettings["PayuSalt"].ToString();
        HashText = GenerateSHA512String(HashText).ToLower();
        return plainText+"|"+HashText;
    }
    private void UploadData(string BankCode, string encData, Int64 GRn, string IpAddress, string Paymenttype)
    {
        EgFrmTOVerified ObjFrm = new EgFrmTOVerified();
        ObjFrm.BankCode = BankCode;
        ObjFrm.encData = encData;
        ObjFrm.Ipaddress = IpAddress;
        ObjFrm.GRN = GRn;
        ObjFrm.paymentType = Paymenttype;
        ObjFrm.BankResponseAudit();
    }
    public override string CallVerifyManualService(string cipherText)
    {
        throw new NotImplementedException();
    }
}