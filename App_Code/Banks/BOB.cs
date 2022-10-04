using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;

/// <summary>
/// Summary description for BOB
/// </summary>
public class BOB : Banks
{
    public BOB(string paymentmode)
    {
        //KeyName = "BOB";
        KeyName = paymentmode == "M" ? "BOB1" : "BOB";
        Version = paymentmode == "M" ? "2.0" : "1.0";
    }

    public override string CallManualDataPushService(string cipherText)
    {
        throw new NotImplementedException();
    }

    public override string CallVerifyManualService(string cipherText)
    {
        eChallanPushReqChild objbobchild = new eChallanPushReqChild();
        objbobchild.value = cipherText;
        //objbobchild.value = "0BejkDOcfdVNupd/NWqMQ4NLYysrESqzA49UxX4KG55O2wM4tAIuxPaHl58W2wagPEaXP30WutYBsVnw";
        //objbobchild.value = "nYXEzjza+zo6gforhYOmYruMXzoOgew3ynvWDsUAhp5uTnc7QR0uLvPyZeK7SrqX7A9sgBWu5bP2";

        objbobchild.stateCode = "RJ";

        eChallanPushReqParent objbobParent = new eChallanPushReqParent();
        objbobParent.eChallanPaymentInqReq = objbobchild;

        try
        {
            using (WebClient webClient1 = new WebClient())
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                //webClient.BaseAddress = StaticItems.EndPoint;
                //var url = "https://103.85.40.22:4443/cmsforwarderUAT";
                var url = System.Web.Configuration.WebConfigurationManager.AppSettings["BOBM"];
                webClient1.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                webClient1.Headers[HttpRequestHeader.ContentType] = "application/json";
                string data1 = JsonConvert.SerializeObject(objbobParent);
                var response = webClient1.UploadString(url, "POST", data1);
                var result = JsonConvert.DeserializeObject<eChallanPushResParent>(response);
                return result.eChallanPaymentInqResp.value;
                //return "SKaKNJKaODnbgqLNzXlY2Xn0tKmzCkYpa7sx4uvj8Z6jrI9CaNw4yTdY/w9mbUfVeJWH2MmW9jUUl2BbnMsZRiiqdRFfI4/sDZX/Py+qosI/xM2QYolQ8H50cxkcWv4NSP1UfdUVdep47eZ2J8Maw+JDb75rbEOHrLGLIjF7L1s0tl8aDBbcYjbZasOxeKlylQMLQqHveqTYOAusp++IdZu8eu7KpMfz8WdJZYgJvXePkY5MdH4tf3ikK23dB5QmDuZ5GQ9u0+6AYEzCYrR7cqAeRb/MkawX";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public override string CallVerifyService(string cipherText)
    {
        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
           
            using (BOBWebServ.WebService bobService = new BOBWebServ.WebService())
            {

                string BankUrl = ConfigurationManager.AppSettings["BOBWebServ.GetRajGrasPmtStatusSoap"].Split('/').GetValue(2).ToString().Replace("www.", "");
                IpAddress = Dns.GetHostByName(BankUrl.Split(':').GetValue(0).ToString()).AddressList[0].ToString();
                ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();
                return bobService.BOBRajGrasVerifyPayment(cipherText);

            }

            //using (BOBWebServ.BOBRajeGrasVerify bobService = new BOBWebServ.BOBRajeGrasVerify())
            //{

            //    string BankUrl = ConfigurationManager.AppSettings["BOBWebServ.BOBRajGRASVerifyPmt"].Split('/').GetValue(2).ToString().Replace("www.", "");
            //    IpAddress = Dns.GetHostByName(BankUrl.Split(':').GetValue(0).ToString()).AddressList[0].ToString();
            //    ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();

            //    return bobService.BOBRajGrasVerifyPayment(cipherText);

            //}
        }
        catch (Exception ex)
        {
            InsertErrorLog(ex.Message, "0200113", ConfigurationManager.AppSettings["BOBWebServ.GetRajGrasPmtStatusSoap"].Split('/').GetValue(2).ToString().Replace("www.", ""), 1);
            return null;
        }
    }

    

    //protected override string DecryptResponseString(string cipherText)
    //{
    //    SbiEncryptionDecryption objEncry = new SbiEncryptionDecryption();
    //    return objEncry.Decrypt(cipherText, System.Web.HttpContext.Current.Server.MapPath("../WebPages/Key/" + KeyName + ".key"));
    //}

    //protected override string GetBankForwardString()
    //{
    //    string plainText = string.Format("GRN={0}|HEAD_OF_ACC1={1}|AMT1={2}|HEAD_OF_ACC2={3}|AMT2={4}|HEAD_OF_ACC3={5}|AMT3={6}|HEAD_OF_ACC4={7}|AMT4={8}|HEAD_OF_ACC5={9}|AMT5={10}|HEAD_OF_ACC6={11}|AMT6={12}|HEAD_OF_ACC7={13}|AMT7={14}|HEAD_OF_ACC8={15}|AMT8={16}|HEAD_OF_ACC9={17}|AMT9={18}|REMITTER_NAME={19}|TOTALAMOUNT={20}|PayMode={21}|REG-TIN-NO={22}|LocationCode={23}|Filler={24}", GRN.ToString(), Head_Name[0], Head_Amount[0], Head_Name[1], Head_Amount[1], Head_Name[2], Head_Amount[2], Head_Name[3], Head_Amount[3], Head_Name[4], Head_Amount[4], Head_Name[5], Head_Amount[5], Head_Name[6], Head_Amount[6], Head_Name[7], Head_Amount[7], Head_Name[8], Head_Amount[8], RemitterName, TotalAmount, PaymentMode, TIN, LocationCode, flag);
    //    EncryptDecryptionBL objEncryption = new EncryptDecryptionBL();
    //    checkSum = objEncryption.GetMD5Hash(plainText);
    //    return plainText + "|checkSum=" + checkSum;
    //}

    public override string GetRequestString(string plainText)
    {
        return BanksEncryptionDecryption.GetEncryptedString(plainText, KeyName, Version);
    }
}
public class eChallanPushReqParent
{
    public eChallanPushReqChild eChallanPaymentInqReq { get; set; }
}
public class eChallanPushResParent
{
    public eChallanPushReqChild eChallanPaymentInqResp { get; set; }
}
public class eChallanPushReqChild
{
    public string value { get; set; }
    public string stateCode { get; set; }
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