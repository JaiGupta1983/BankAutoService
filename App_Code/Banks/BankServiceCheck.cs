using EgBL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

/// <summary>
/// Summary description for BankServiceCheck
/// </summary>
public class BankServiceCheck
{
    EncryptDecryptionBL objEncryption;
    private string flag;

    public Int64 GRN { get; set; }
    public double TotalAmount { get; set; }
    public string BSRCode { get; set; }
    public string PaymentMode { get; set; }

    private string CipherText { get; set; }
    private string KeyName { get; set; }
    private string Message { get; set; }
    public string PlainText { get; set; }
    public bool status { get; set; }
    private Dictionary<string, string> GRNData { get; set; }

    public bool Verify()
    {
        string cipherText = "";
        string returnData = "";
        string GEN = "";
        if (GRN > Convert.ToInt64(ConfigurationManager.AppSettings["OldLastGRN"]))
        {
            GEN = GRN.ToString();
        }
        else
        {
            GEN = "000000000" + Convert.ToString(GRN);
            GEN = GEN.Substring(GEN.Length - 10, 10);
        }
        string plainText = string.Format("GRN={0}|TOTALAMOUNT={1}", GEN.ToString(), TotalAmount.ToString());
        objEncryption = new EncryptDecryptionBL();
        string checkSum = objEncryption.GetMD5Hash(plainText);
        Banks objBank = Banks.SelectBanks(BSRCode);
        objBank.checkSum = checkSum;
        KeyName = objBank.KeyName;
        cipherText = objBank.GetRequestString(plainText);

        if (PaymentMode == "M")
            returnData = objBank.CallVerifyManualService(cipherText);
        else
            returnData = objBank.CallVerifyService(cipherText);
        CipherText = returnData;
        if (returnData == null || returnData == "")
            return false;
        else
            return UpdateGRNStatus();
    }
    public bool UpdateGRNStatus()
    {
        bool isEpay = KeyName == "BwmHPemeQsQhpwEGWmyQtQ==" ? true : false;
        bool isPayu = KeyName == "PayU" ? true : false;
        List<string> lstPlainText = new List<string>();
        GRNData = new Dictionary<string, string>();
        PlainText = DecryptString();
        if (PlainText == "Incorrect hash value.")
            return false;
        if (!isPayu)
        {
            lstPlainText = PlainText.Split('|').ToList();
            if (isEpay && lstPlainText.Count > 13)
            {
                GRNData.Add("GRN", lstPlainText[0]);
                GRNData.Add("BANK_CODE", lstPlainText[13]);
                GRNData.Add("BankReferenceNo", lstPlainText[1]);
                GRNData.Add("CIN", lstPlainText[12]);
                GRNData.Add("PAID_DATE", lstPlainText[10]);
                GRNData.Add("PAID_AMT", lstPlainText[3]);
                GRNData.Add("TRANS_STATUS", lstPlainText[2].Substring(0, 1));
                GRNData.Add("epayBSRCode", lstPlainText[8]);
                GRNData.Add("bankRefNo", lstPlainText[9]);
                GRNData.Add("payMode", lstPlainText[5]);
            }
            else if (!isEpay)
            {
                GRNData.Add("GRN", lstPlainText[0].Split('=').GetValue(1).ToString().Trim());
                GRNData.Add("BANK_CODE", lstPlainText[1].Split('=').GetValue(1).ToString().Trim());
                GRNData.Add("BankReferenceNo", lstPlainText[2].Split('=').GetValue(1).ToString().Trim());
                GRNData.Add("CIN", lstPlainText[3].Split('=').GetValue(1).ToString().Trim());
                GRNData.Add("PAID_DATE", lstPlainText[4].Split('=').GetValue(1).ToString().Trim());
                GRNData.Add("PAID_AMT", lstPlainText[5].Split('=').GetValue(1).ToString().Trim());
                GRNData.Add("TRANS_STATUS", lstPlainText[6].Split('=').GetValue(1).ToString().Trim().Substring(0, 1));
                //foreach (string data in lstPlainText)
                //{
                //    GRNData.Add(data.Split('=').GetValue(0).ToString().Trim(), data.Split('=').GetValue(1).ToString().Trim());
                //}
            }
            else
            {
                EgErrorHandller obj = new EgErrorHandller();
                obj.InsertError(PlainText + "|" + "ELSE_CASE_UpdateStatus");
            }
        }
        if (!ValidateData(isEpay, isPayu))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    

    private bool ValidateData(bool isEpay, bool PayU)
    {
        if (GRN == 0 && TotalAmount == 0.00)
        {
            try
            {
                GRN = Convert.ToInt64(GRNData["GRN"]);
            }
            catch
            {
                Message = "Invalid GRN";
                return false;
            }
            try
            {
                TotalAmount = Convert.ToDouble(GRNData["PAID_AMT"]);
            }
            catch
            {
                Message = "Invalid Amount";
                return false;
            }
        }
        if (GRNData["PAID_AMT"] == "NA" && isEpay)
        {
            Message = "Status updated as Unsuccessfull ";
            GRNData["GRN"] = GRN.ToString();
            GRNData["PAID_AMT"] = TotalAmount.ToString();
            GRNData["TRANS_STATUS"] = "F";
            GRNData["PAID_DATE"] = DateTime.Now.ToString();
            return true;
        }
        if (Convert.ToInt64(GRNData["GRN"]) != GRN || Convert.ToDouble(GRNData["PAID_AMT"]) != TotalAmount)
        {
            Message = "Verified Unsuccessful.";
            GRNData["GRN"] = GRN.ToString();
            GRNData["PAID_AMT"] = TotalAmount.ToString();
            GRNData["TRANS_STATUS"] = "F";
            return true;
        }
        try
        {
            Convert.ToDateTime(GRNData["PAID_DATE"]);
        }
        catch
        {
            Message = "Invalid Date";
            return false;
        }
        try
        {
            if (Convert.ToDouble(GRNData["PAID_AMT"]) <= 0.00)
            {
                Message = "Amount Less Than 0.00";
                return false;
            }
        }
        catch
        {
            return false;
        }
        if (GRNData["BANK_CODE"].Length != 7)
        {
            Message = "Invalid BankCode";
            return false;
        }
        switch (GRNData["TRANS_STATUS"].Substring(0, 1).ToUpper())
        {
            case "S":
                if ((GRNData["CIN"].Length != 21 && !isEpay) || (GRNData["CIN"].Length != 20 && isEpay))
                {
                    Message = "Invalid CIN";
                    return false;
                }
                else if (GRNData["BankReferenceNo"] == null || GRNData["BankReferenceNo"] == "")
                {
                    Message = "Invalid Reference No";
                    return false;
                }
                else
                {
                    Message = "Status updated as successfull ";
                    return true;
                }
            case "P":
                Message = "Status updated as Pending ";
                return true;
            case "F":
                Message = "Status updated as Unsuccessfull ";
                return true;
            default:
                Message = "Invalid Status";
                return false;
        }
        return true;
    }
    private string DecryptString()
    {
        if (KeyName == "PayU")
        {
            List<string> lstPlainText = new List<string>();
            lstPlainText = CipherText.Split('&').ToList();
            if (lstPlainText[0] == "CorrectHash")
            {
                GRNData.Add("TRANS_STATUS", lstPlainText[1].Split('=').GetValue(1).ToString().Substring(0, 1));///status
                GRNData.Add("GRN", lstPlainText[2].Split('=').GetValue(1).ToString());///txnid
                GRNData.Add("hash", lstPlainText[3].Split('=').GetValue(1).ToString());////hash
                GRNData.Add("payMode", lstPlainText[4].Split('=').GetValue(1).ToString());////mode
                GRNData.Add("PAID_AMT", lstPlainText[5].Split('=').GetValue(1).ToString()); ///amount
                GRNData.Add("BankReferenceNo", lstPlainText[6].Split('=').GetValue(1).ToString());/////mihpayid
                GRNData.Add("CIN", lstPlainText[7].Split('=').GetValue(1).ToString()); ////////cin
                GRNData.Add("bankRefNo", lstPlainText[8].Split('=').GetValue(1).ToString());//////bank_ref_num
                GRNData.Add("reason", lstPlainText[9].Split('=').GetValue(1).ToString());////error
                GRNData.Add("PayUBSRCode", lstPlainText[10].Split('=').GetValue(1).ToString());///////bankcode
                GRNData.Add("PAID_DATE", lstPlainText[11].Split('=').GetValue(1).ToString());////addedon
                GRNData.Add("BANK_CODE", lstPlainText[12].Split('=').GetValue(1).ToString());//////udf1
                GRNData.Add("Head", lstPlainText[13].Split('=').GetValue(1).ToString());//////udf2
            }
            else
            {
                return "Incorrect hash value.";
            }
            return "";
        }
        return BanksEncryptionDecryption.GetDecryptedString(CipherText, KeyName);
    }
   
}