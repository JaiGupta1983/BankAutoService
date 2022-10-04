using EgBL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;

[AspNetCompatibilityRequirements
    (RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AutoBankStatusService" in code, svc and config file together.
public class AutoBankStatusService : IAutoBankStatusService
{
    public Dictionary<string, string> GRNData;
    bool flag = false;
    EncryptDecryptionBL objEncryption = new EncryptDecryptionBL();
    public string PrepareData()
    {
        try
        {
            EgAutoBankStatusServiceBL objServiceBL = new EgAutoBankStatusServiceBL();
            EncryptDecryptionBL objEncryption = new EncryptDecryptionBL();
            //objServiceBL.BSRCode = bsrcode;

            DataTable dt = new DataTable();
            dt = objServiceBL.GetPrepareData();
            Banks objBank = Banks.SelectBanks("6360010");
            string bankcode = "6360010";
            //string KeyName = objBank.KeyName;//dt.Rows[0]["keyname"].ToString();
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                try
                {
                    objServiceBL.GRN = Convert.ToInt64(dt.Rows[i]["GRN"].ToString());
                    objServiceBL.Amount = Convert.ToDouble(dt.Rows[i]["Amount"].ToString());
                    objServiceBL.BSRCode = dt.Rows[i]["BankName"].ToString();
                    objServiceBL.PaymentMode = dt.Rows[i]["PaymentType"].ToString();

                    if (bankcode != dt.Rows[i]["BankName"].ToString())
                    {
                        objBank = Banks.SelectBanks(dt.Rows[i]["BankName"].ToString());
                        bankcode = dt.Rows[i]["BankName"].ToString();
                    }

                    string KeyName = objBank.KeyName;
                    objServiceBL.BSRCode = dt.Rows[i]["BankName"].ToString();


                    string plainText = string.Format("GRN={0}|TOTALAMOUNT={1}", dt.Rows[i]["GRN"].ToString().ToString(), dt.Rows[i]["Amount"].ToString().ToString());
                    string checkSum = string.Empty;
                    string cipherTextReq = string.Empty;
                    try
                    {
                        checkSum = objBank.Version == "2.0" ? objEncryption.GetSHA256(plainText) : objEncryption.GetMD5Hash(plainText);
                    }
                    catch { }
                    try
                    {
                        objBank.checkSum = checkSum;
                        cipherTextReq = objBank.GetRequestString(plainText);
                        //cipherTextReq = BanksEncryptionDecryption.GetEncryptedString(plainText + "|checkSum=" + checkSum, KeyName);
                        objServiceBL.cipherText = cipherTextReq;
                        objServiceBL.PlainText = plainText;
                        objServiceBL.CheckSum = checkSum;
                        int res = objServiceBL.UpdatePrepareCipherTextData();

                        ProcessGRN(objServiceBL.BSRCode);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    EgErrorHandller obj = new EgErrorHandller();
                    obj.InsertError(ex.Message.ToString());
                    return "Request Unable To Process !";
                }
            }
            return "done";
        }
        catch (Exception ex)
        {
            EgErrorHandller obj = new EgErrorHandller();
            obj.InsertError(ex.Message.ToString());
            return "Request Unable To Process !";
        }
    }

    private string ProcessGRN(string bsrcode)
    {
        try
        {
            EgAutoBankStatusServiceBL objServiceBL = new EgAutoBankStatusServiceBL();
            objServiceBL.BSRCode = bsrcode;

            DataTable dt = new DataTable();
            dt = objServiceBL.GetCipherTextData();
            Banks objBank = Banks.SelectBanks(bsrcode);
            string encdata = string.Empty;

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                string CipherText = dt.Rows[i]["CipherTextRequest"].ToString();
                try
                {
                    encdata = objBank.CallVerifyService(CipherText);
                    objServiceBL.GRN = Convert.ToInt64(dt.Rows[i]["GRN"].ToString());
                    objServiceBL.Amount = Convert.ToDouble(dt.Rows[i]["Amount"].ToString());
                    objServiceBL.BSRCode = dt.Rows[i]["BankName"].ToString();
                    objServiceBL.cipherText = encdata;
                    int res = objServiceBL.InsertBankResponseCipherText();
                    UpdateBankResponse(objServiceBL.BSRCode);
                }
                catch (Exception ex) { throw ex; }
            }
            return "done";
        }
        catch (Exception ex)
        {
            EgErrorHandller obj = new EgErrorHandller();
            obj.InsertError(ex.Message.ToString());
            return "Request Unable To Process !";
        }
    }
    private string UpdateBankResponse(string bsrcode)
    {
        try
        {
            EgAutoBankStatusServiceBL objServiceBL = new EgAutoBankStatusServiceBL();
            objServiceBL.BSRCode = bsrcode;

            DataTable dt = new DataTable();
            dt = objServiceBL.GetBankResponseData();
            //bsrcode = "1000132";
            Banks objBank = Banks.SelectBanks(bsrcode);
            string KeyName = objBank.KeyName;

            if (bsrcode.Trim() == "9910001")
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    try
                    {
                        //string PlainText = "CorrectHash&status = success&txnid=65761992&hash=910be5cef47069fd061d972d2c3fd820ee7cf05f23277c022f82636fe8b64969e3595876e6dffd4aa4627a917cde775e1f50b92b28252df03bc2c2db07990b3e&mode=NB&amount=300.00&mihpayid=15717119531&cin=PNBG6576199222082022&bank_ref_num=222345596320&error=E000&bankcode=HDFB&addedon=2022-08-22 11:10:20&udf1=9910001&udf2=0030&udf3=0&udf4=0&udf5=0";
                        string PlainText = DecryptString(dt.Rows[i]["CipherTextResponse"].ToString().Trim(), KeyName.Trim(), objBank.Version);
                        List<string> lstPlainText = new List<string>();
                        lstPlainText = PlainText.Split('&').ToList();

                        objServiceBL.TRANS_STATUS = lstPlainText[1].Split('=').GetValue(1).ToString().Trim().Substring(0, 1);///status;
                        objServiceBL.GRN = Convert.ToInt64(lstPlainText[2].Split('=').GetValue(1));///txnid
                        objServiceBL.hash = lstPlainText[3].Split('=').GetValue(1).ToString().Trim();////hash
                        objServiceBL.payMode = lstPlainText[4].Split('=').GetValue(1).ToString().Trim();////mode
                        objServiceBL.PAID_AMT = lstPlainText[5].Split('=').GetValue(1).ToString().Trim(); ///amount
                        objServiceBL.BankReferenceNo = lstPlainText[6].Split('=').GetValue(1).ToString().Trim();/////mihpayid
                        objServiceBL.CIN = lstPlainText[7].Split('=').GetValue(1).ToString().Trim(); ////////cin
                        objServiceBL.bankRefNo = lstPlainText[8].Split('=').GetValue(1).ToString().Trim();//////bank_ref_num
                        objServiceBL.reason = lstPlainText[9].Split('=').GetValue(1).ToString().Trim();////error
                        objServiceBL.PayUBSRCode = lstPlainText[10].Split('=').GetValue(1).ToString().Trim();///////bankcode
                        objServiceBL.PAID_DATE = lstPlainText[11].Split('=').GetValue(1).ToString().Trim();////addedon
                        objServiceBL.BANK_CODE = lstPlainText[12].Split('=').GetValue(1).ToString().Trim();//////udf1
                        objServiceBL.Head = lstPlainText[13].Split('=').GetValue(1).ToString().Trim();//////udf2

                        int res = objServiceBL.InsertBankResponseData();
                    }
                    catch (Exception ex) { throw ex; }
                }
            }
            else if (bsrcode.Trim() == "1000132")
            {

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    try
                    {
                        List<string> lstPlainText = new List<string>();
                        GRNData = new Dictionary<string, string>();
                        //var str = "+d9B6KxXFnpsu6N3OrKy5KNPZon+gTZhQ4mbY7oaUUoMRa+uU/k1a+1EPbkrtdaAF4HQUrC2ciLiEHq0mTiWiMBD0J+qlfZTE5Ukzp0qUZPjo0selok4y8JTfKDe4VwEjca915XjCHuIwswdJMoAxEKi7JNIIynXRTDtHf8EU4u8bsypKsSxwJZYBHSKTiZU9O1vQfz5PnaHBUCEh/cFbwIvBpiMMG16EvN6eArSNPip5zkTbBj28VceeATNnLl/v6ZBaEoRUIxmfRO0bPzy2HvxQXhGV1pEXVLEy8a3JmimGxfTTe3Z5Dt2LMqD2zQtIlvj5eJzlCrzOFMoGI660sjgnZ52qhAmXFqiHIEYROy1Vf/jwrrUHglnG9OQDepe";
                        string PlainText = DecryptString(dt.Rows[i]["CipherTextResponse"].ToString().Trim(), KeyName.Trim(), objBank.Version);
                        //string PlainText = DecryptString(str.Trim(), "BwmHPemeQsQhpwEGWmyQtQ==", objBank.Version);
                        lstPlainText = PlainText.Split('|').ToList();

                        if (lstPlainText.Count > 13)
                        {
                            objServiceBL.GRN = Convert.ToInt64(lstPlainText[0].ToString().Trim());
                            objServiceBL.BANK_CODE = lstPlainText[13].ToString().Trim();
                            objServiceBL.BankReferenceNo = lstPlainText[1].ToString().Trim();
                            objServiceBL.CIN = lstPlainText[12].ToString().Trim();
                            objServiceBL.PAID_DATE = lstPlainText[10].ToString().Trim();
                            objServiceBL.PAID_AMT = lstPlainText[3].ToString().Trim();
                            objServiceBL.TRANS_STATUS = lstPlainText[2].Substring(0, 1).ToString().Trim();
                            objServiceBL.PayUBSRCode = lstPlainText[8].ToString().Trim();
                            objServiceBL.bankRefNo = lstPlainText[9].ToString().Trim();
                            objServiceBL.payMode = lstPlainText[5].ToString().Trim();

                            int res = objServiceBL.InsertBankResponseData();
                        }
                    }
                    catch (Exception ex) { throw ex; }
                }
            }
            else
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    try
                    {
                        List<string> lstPlainText = new List<string>();
                        GRNData = new Dictionary<string, string>();
                        string PlainText = DecryptString(dt.Rows[i]["CipherTextResponse"].ToString().Trim(), KeyName.Trim(), objBank.Version);
                        lstPlainText = PlainText.Split('|').ToList();

                        objServiceBL.GRN = Convert.ToInt64(lstPlainText[0].Split('=').GetValue(1).ToString().Trim());
                        objServiceBL.BANK_CODE = lstPlainText[1].Split('=').GetValue(1).ToString().Trim();
                        objServiceBL.BankReferenceNo = lstPlainText[2].Split('=').GetValue(1).ToString().Trim();
                        objServiceBL.CIN = lstPlainText[3].Split('=').GetValue(1).ToString().Trim();
                        objServiceBL.PAID_DATE = lstPlainText[4].Split('=').GetValue(1).ToString().Trim();
                        objServiceBL.PAID_AMT = lstPlainText[5].Split('=').GetValue(1).ToString().Trim();
                        objServiceBL.TRANS_STATUS = lstPlainText[6].Split('=').GetValue(1).ToString().Trim().Substring(0, 1);

                        int res = objServiceBL.InsertBankResponseData();
                    }
                    catch (Exception ex) { throw ex; }
                }
            }
            return "done";
        }
        catch (Exception ex)
        {
            EgErrorHandller obj = new EgErrorHandller();
            obj.InsertError(ex.Message.ToString());
            return "Request Unable To Process !";
        }
    }
    private string DecryptString(string CipherText, string KeyName, string version)
    {
        return BanksEncryptionDecryption.GetDecryptedString(CipherText, KeyName, version);
    }
}
