using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
/// <summary>
/// Summary description for SBIePay
/// </summary>
public class PineLab : Banks
{
    private string BankCode;
    public PineLab()
    {
        KeyName = "840917F43186472EB2261ABCDE5CA576";
        BankCode = "9920001";
        //BankCode = "1000134";
    }

    public override string CallManualDataPushService(string cipherText)
    {
        throw new NotImplementedException();
    }

    public override string CallVerifyService(string cipherText)
    {
        string returnData = string.Empty;
        //string Jsonencdata = "{\"GRN\":\"27672869\",\"Bank_Code\": \"9920001\",\"BankReferenceNo\": \"CK58102178\",\"CIN\": \" 992000106494921012019\",\"Paid_Date\": \"2019/11/27 13:27:00\",\"Paid_Amt\":\"259.00\",\"TRANS_STATUS\":\"S\",\"GT_BSRCode\":\"eChallan\",\"BankRefNo\": \"XXXXX\",\"PayMode\": \"CC\",\"SecurityToken\": \"<value created using attached logic > \"}";
        //string Jsonencdata = "{\"GRN\":\"7221639\",\"Bank_Code\": \"9920001\",\"BankReferenceNo\": \"CK58102178\",\"CIN\": \"992000106494921012019\",\"Paid_Date\": \"2019/11/27 13:27:00\",\"Paid_Amt\":\"01.1\",\"TRANS_STATUS\":\"S\",\"GT_BSRCode\":\"eChallan\",\"BankRefNo\": \"XXXXX\",\"PayMode\": \"CC\",\"SecurityToken\": \"57043BDAAB2C10EFFFBDB8BC81CE62C5661B3B0C5A257B03B03F80E6AFCC115E\"}";
        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            byte[] byteArray = Encoding.UTF8.GetBytes(cipherText);
            //WebRequest request = WebRequest.Create("http://182.74.238.200/API/CloudBasedIntegration/V2/GetChallanDetails");
            WebRequest request = WebRequest.Create("https://www.plutuscloudservice.in:8201/API/CloudBasedIntegration/V2/GetChallanDetails");
            request.Method = "POST";
            request.ContentLength = byteArray.Length;
            request.ContentType = "application/json";
            //IpAddress = "10.130.34.152"; //Dns.GetHostByName("https://www.plutuscloudserviceuat.in").AddressList[0].ToString();
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(cipherText);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    returnData = streamReader.ReadToEnd();
                }
            }
            return returnData;
            //return "{\"GRN\":\"7221639\",\"Bank_Code\":\"9920001\",\"BankReferenceNo\":\"2294973389\",\"CIN\":\"992000100006910012020\",\"Paid_Date\":\"2020 / 01 / 10 19:54:58\",\"Paid_Amt\":\"01.1\",\"TRANS_STATUS\":\"S\",\"GT_BSRCode\":\"eCHALLAN\",\"BankRefNo\":\"000020\",\"PayMode\":\"CC\",\"SecurityToken\":\"57043BDAAB2C10EFFFBDB8BC81CE62C5661B3B0C5A257B03B03F80E6AFCC115E\"}";
        }
        catch (Exception ex)
        {
            InsertErrorLog(ex.Message, "9920001", "https://www.plutuscloudservice.in:8201/API/CloudBasedIntegration/V2/GetChallanDetails", 1);
            return null;
        }
        //return returnData;
    }
    public override string CallVerifyManualService(string cipherText)
    {
        throw new NotImplementedException();
    }
    public string GetPineLabRequestString(SortedList<String, String> sortdLstRequestFields)
    {
        // Hex Decode the Secure Salt for use in using the HMACSHA256 hasher
        // hex decoding eliminates this source of error as it is independent of the character encoding
        // hex decoding is precise in converting to a byte array and is the preferred form for representing binary values as hex strings. 

        byte[] convertedHash = new byte[KeyName.Length / 2];
        for (int i = 0; i < KeyName.Length / 2; i++)
        {
            convertedHash[i] = (byte)Int32.Parse(KeyName.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
        }

        // Build string from collection in preperation to be hashed
        StringBuilder sb = new StringBuilder();
        foreach (KeyValuePair<string, string> kvp in sortdLstRequestFields)
        {
            sb.Append(kvp.Key + "=" + kvp.Value + "|");
        }
        sb.Remove(sb.Length - 1, 1);

        // Create secureHash on string
        string hexHash = "";

        using (HMACSHA256 hasher = new HMACSHA256(convertedHash))
        {
            byte[] hashValue = hasher.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            foreach (byte b in hashValue)
            {
                hexHash += b.ToString("X2");
            }
        }

        return hexHash;
    }
}