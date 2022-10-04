using EgBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Banks
/// </summary>
public abstract class Banks
{
    #region Properties
    public Int64 GRN { get; set; }
    public double TotalAmount { get; set; }
    //public string BankCode { get; set; }
    public string PaymentMode { get; set; }
    public string RemitterName { get; set; }
    public string LocationCode { get; set; }
    public string message { get; set; }
    public string merchantCode { get; set; }
    //public string URL { get; set; }
    public string KeyName { get; set; }
    public string IpAddress { get; set; }
    public string flag { get; set; }
    public string filler { get; set; }
    public string[] Head_Name;
    public string[] Head_Amount;
    public string TIN { get; set; }
    public string checkSum { get; set; }
    public string URL { get; set; } //payal sharma
    public string BsrCode { get; set; }   // 29/09/2021 Modify Payment Process
    public string Version { get; set; }
    #endregion

    public static Dictionary<string, Banks> bankCodeList = new Dictionary<string, Banks>()
        {
            { "9910001", new PAYU() },
            { "0304017", new PNB() },
            { "0006326", new SBI("N") },
            { "0200113", new BOB("N") },
            { "0292861", new UnionBank() },
            { "6910213", new IDBI() },
            { "0280429", new CBI() },
            { "0361193", new OBC() },
            { "0240539", new Canara() },
            { "1000132", new SBIePay() },
            { "0171051", new TestBank() },
            { "0220123", new BOI() },
            { "0281065", new CBI() },
            { "9920001", new PineLab() },
            { "6390013", new ICICI() },
             { "6360010", new AXIS() }
        };
    public static Banks SelectBanks(string BSRCode, string PaymentMode = null)
    {
        Dictionary<string, Banks> bankCodeList = new Dictionary<string, Banks>()
        {
            { "9910001", new PAYU() },
            { "0304017", new PNB() },
            { "0006326", new SBI(PaymentMode) },
            { "0200113", new BOB(PaymentMode) },
            { "0292861", new UnionBank() },
            { "6910213", new IDBI() },
            { "0280429", new CBI() },
            { "0361193", new OBC() },
            { "0240539", new Canara() },
            { "1000132", new SBIePay() },
            { "0171051", new TestBank() },
            { "0220123", new BOI() },
            { "0281065", new CBI() },
            { "9920001", new PineLab() },
            { "6390013", new ICICI() },
             { "6360010", new AXIS() }

        };
        try
        {
            return bankCodeList[BSRCode];
        }
        catch (KeyNotFoundException k)
        {
            throw k;
        }
    }

    public virtual Dictionary<string, string> GetforwardDictionary(string EncData)
    {
        Dictionary<string, string> GetforwardDictionary = new Dictionary<string, string>()
        {
            { "encdata", EncData },
            { "merchant_code", merchantCode },
            { "URL", URL }
        };
        return GetforwardDictionary;

    }


    public virtual string GetRequestString(string plainText)
    {
        return BanksEncryptionDecryption.GetEncryptedString(plainText + "|checkSum=" + checkSum, KeyName);
    }
    //public virtual string GetBankForwardString()
    //{
    //    string plainText = string.Format("GRN={0}|HEAD_OF_ACC1={1}|AMT1={2}|HEAD_OF_ACC2={3}|AMT2={4}|HEAD_OF_ACC3={5}|AMT3={6}|HEAD_OF_ACC4={7}|AMT4={8}|HEAD_OF_ACC5={9}|AMT5={10}|HEAD_OF_ACC6={11}|AMT6={12}|HEAD_OF_ACC7={13}|AMT7={14}|HEAD_OF_ACC8={15}|AMT8={16}|HEAD_OF_ACC9={17}|AMT9={18}|REMITTER_NAME={19}|TOTALAMOUNT={20}|PayMode={21}|REG-TIN-NO={22}|LocationCode={23}|Filler={24}", GRN.ToString(), Head_Name[0], Head_Amount[0], Head_Name[1], Head_Amount[1], Head_Name[2], Head_Amount[2], Head_Name[3], Head_Amount[3], Head_Name[4], Head_Amount[4], Head_Name[5], Head_Amount[5], Head_Name[6], Head_Amount[6], Head_Name[7], Head_Amount[7], Head_Name[8], Head_Amount[8], RemitterName, TotalAmount.ToString(), PaymentMode, TIN, LocationCode, filler);
    //    EncryptDecryptionBL objEncryption = new EncryptDecryptionBL();
    //    checkSum = objEncryption.GetMD5Hash(plainText);
    //    return plainText + "|checkSum=" + checkSum;
    //}

    public virtual Dictionary<string, string> GetBankForwardString()
    {
        string plainText = string.Format("GRN={0}|HEAD_OF_ACC1={1}|AMT1={2}|HEAD_OF_ACC2={3}|AMT2={4}|HEAD_OF_ACC3={5}|AMT3={6}|HEAD_OF_ACC4={7}|AMT4={8}|HEAD_OF_ACC5={9}|AMT5={10}|HEAD_OF_ACC6={11}|AMT6={12}|HEAD_OF_ACC7={13}|AMT7={14}|HEAD_OF_ACC8={15}|AMT8={16}|HEAD_OF_ACC9={17}|AMT9={18}|REMITTER_NAME={19}|TOTALAMOUNT={20}|PayMode={21}|REG-TIN-NO={22}|LocationCode={23}|Filler={24}", GRN.ToString(), Head_Name[0], Head_Amount[0], Head_Name[1], Head_Amount[1], Head_Name[2], Head_Amount[2], Head_Name[3], Head_Amount[3], Head_Name[4], Head_Amount[4], Head_Name[5], Head_Amount[5], Head_Name[6], Head_Amount[6], Head_Name[7], Head_Amount[7], Head_Name[8], Head_Amount[8], RemitterName, TotalAmount.ToString(), PaymentMode, TIN, LocationCode, filler);
        EncryptDecryptionBL objEncryption = new EncryptDecryptionBL();
        checkSum = objEncryption.GetMD5Hash(plainText);
        plainText = plainText + "|checkSum=" + checkSum;
        string EncData = BanksEncryptionDecryption.GetEncryptedString(plainText, KeyName);

        if (Convert.ToInt64(GRN.ToString()) != Convert.ToInt64(HttpContext.Current.Session["GrnNumber"]) || TotalAmount != Convert.ToDouble(HttpContext.Current.Session["NetAmount"]))
        {
            EgErrorHandller obj = new EgErrorHandller();
            obj.InsertError("ErrorBank" + '-' + Convert.ToString(GRN) + '-' + Convert.ToString(HttpContext.Current.Session["GrnNumber"]));
            HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
        }
        return GetforwardDictionary(EncData);


    }
    public void InsertErrorLog(string ErrorName, string BankCode, string BankUrl, int ServiceType)
    {
        var objEgManualBankServiceBl = new EgManualBankServiceBL { Errorname = ErrorName };
        objEgManualBankServiceBl.BankCode = BankCode;
        objEgManualBankServiceBl.ServiceType = ServiceType;
        objEgManualBankServiceBl.BankURL = BankUrl;
        objEgManualBankServiceBl.InsertBankServiceErrorLog();
    }
    public abstract string CallManualDataPushService(string cipherText);
    public abstract string CallVerifyService(string cipherText);
    public abstract string CallVerifyManualService(string cipherText);
}