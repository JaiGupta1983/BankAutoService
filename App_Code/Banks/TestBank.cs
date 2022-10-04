using EgBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TestBank
/// </summary>
public class TestBank : Banks
{
    public TestBank()
    {
        KeyName = "RAJASTHAN_EGRASS";
    }

    public override string CallManualDataPushService(string cipherText)
    {
        throw new NotImplementedException();
    }

    public override string CallVerifyManualService(string cipherText)
    {
        throw new NotImplementedException();
    }
    public override Dictionary<string, string> GetBankForwardString()
    {
        string plainText = string.Format("HEAD_OF_ACC9={17}|HEAD_OF_ACC8={15}|HEAD_OF_ACC7={13}|HEAD_OF_ACC6={11}|HEAD_OF_ACC5={9}|HEAD_OF_ACC4={7}|HEAD_OF_ACC3={5}|HEAD_OF_ACC2={3}|HEAD_OF_ACC1={1}|TOTALAMOUNT={20}|Filler={24}|LocationCode={23}|GRN={0}|PayMode={21}|AMT9={18}|AMT8={16}|AMT7={14}|AMT6={12}|AMT5={10}|AMT4={8}|AMT3={6}|REMITTER_NAME={19}|AMT2={4}|AMT1={2}|REG-TIN-NO={22}", GRN.ToString(), Head_Name[0], Head_Amount[0], Head_Name[1], Head_Amount[1], Head_Name[2], Head_Amount[2], Head_Name[3], Head_Amount[3], Head_Name[4], Head_Amount[4], Head_Name[5], Head_Amount[5], Head_Name[6], Head_Amount[6], Head_Name[7], Head_Amount[7], Head_Name[8], Head_Amount[8], RemitterName, TotalAmount, PaymentMode, TIN, LocationCode, filler);
        EncryptDecryptionBL objEncryption = new EncryptDecryptionBL();
        checkSum = objEncryption.GetMD5Hash(plainText);
        plainText = plainText + "|checkSum=" + checkSum;
        string EncData = BanksEncryptionDecryption.GetEncryptedString(plainText, KeyName);
     
        
        if (Convert.ToInt64(GRN.ToString()) != Convert.ToInt64(HttpContext.Current.Session["GrnNumber"]) || TotalAmount != Convert.ToDouble(HttpContext.Current.Session["NetAmount"]))
        {
            EgErrorHandller obj = new EgErrorHandller();
            obj.InsertError("ErrorAgencyBank" +'-'+Convert.ToString(GRN) + '-' + HttpContext.Current.Session["GrnNumber"].ToString());
            HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
        }
        return GetforwardDictionary(EncData);
    }
    public override string CallVerifyService(string cipherText)
    {
        throw new NotImplementedException();
    }
}