using EgBL;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Summary description for UpdateStatus
/// </summary>
public class UpdateStatus
{
    public string CipherText { get; set; }
    public Int64 GRN { get; set; }
    public double TotalAmount { get; set; }
    public string KeyName { get; set; }
    public string Message { get; set; }
    public Dictionary<string, string> GRNData;
    public string Version { get; set; }
    public bool UpdateGRNStatus()
    {
        bool isEpay = KeyName == "BwmHPemeQsQhpwEGWmyQtQ==" ? true : false;
        string PlainText = DecryptString();
        List<string> lstPlainText = new List<string>();
        GRNData = new Dictionary<string, string>();
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
        if (!ValidateData(isEpay,false))
        {
            return false;
        }
        else
        {
            return UpdateData(isEpay,false);
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
                if ((GRNData["CIN"] == null || GRNData["CIN"].ToString() == "0"))
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
    private bool UpdateData(bool isEpay, bool isPayU)
    {
        EgEChallanBankBL objEgEChallanBankBL = new EgEChallanBankBL();
        objEgEChallanBankBL.GRN = Convert.ToInt64(GRNData["GRN"]);
        objEgEChallanBankBL.BankCode = GRNData["BANK_CODE"];
        objEgEChallanBankBL.CIN = GRNData["CIN"];
        objEgEChallanBankBL.Ref = GRNData["BankReferenceNo"];
        objEgEChallanBankBL.Amount = Convert.ToDouble(GRNData["PAID_AMT"]);
        objEgEChallanBankBL.Status = GRNData["TRANS_STATUS"];
        objEgEChallanBankBL.timeStamp = Convert.ToDateTime(GRNData["PAID_DATE"]);
        int result = objEgEChallanBankBL.UpdateSuccessStatus();
        if (result < 1)
            Message = "Invalid GRN Data";
        if (isEpay && result >= 1)
        {
            objEgEChallanBankBL.epayBSRCode = GRNData["epayBSRCode"];
            objEgEChallanBankBL.bankRefNo = GRNData["bankRefNo"];
            objEgEChallanBankBL.payMode = GRNData["payMode"];
            objEgEChallanBankBL.UpdateEpayStatus();
        }
        else if (isPayU && result >= 1)
        {
            objEgEChallanBankBL.epayBSRCode = GRNData["PayUBSRCode"];//////DebitBankCode
            objEgEChallanBankBL.bankRefNo = GRNData["bankRefNo"];///////BankRefNo
            objEgEChallanBankBL.payMode = GRNData["payMode"];///////Transtype
            objEgEChallanBankBL.Reason = GRNData["reason"];
            objEgEChallanBankBL.UpdatePAYUStatus();
        }

        return result >= 1 ? true : false;
    }
    private string DecryptString()
    {
        return BanksEncryptionDecryption.GetDecryptedString(CipherText, KeyName, Version);
    }


    //////payu

    public bool UpdateGRNStatus_PayU()
    {

        if (!ValidateData(false, true))
        {
            return false;
        }
        else
        {
            return UpdateData(false, true);
        }
    }

}