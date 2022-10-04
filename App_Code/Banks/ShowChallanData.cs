using EgBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ShowChallanData
/// </summary>
public class ShowChallanData
{
    public Int64 GRN { get; set; }
    public double TotalAmount { get; set; }
    public string BsrCode { get; set; }
    public string Message { get; set; }
    public Dictionary<string, string> GRNData;

    public bool ShowData()
    {
        EgEChallanBankBL objEgEChallanBankBL = new EgEChallanBankBL();
        objEgEChallanBankBL.GRN = GRN;
        objEgEChallanBankBL.Amount = TotalAmount;
        objEgEChallanBankBL.BankCode = BsrCode;
        bool result = objEgEChallanBankBL.GetGrnShowData();
        
        if (result)
        {
            GRNData = new Dictionary<string, string>();
            GRNData.Add("GRN", objEgEChallanBankBL.GRN.ToString());
            GRNData.Add("PAID_AMT", objEgEChallanBankBL.Amount.ToString("F"));
            GRNData.Add("BankReferenceNo", objEgEChallanBankBL.bankRefNo);
            GRNData.Add("CIN", objEgEChallanBankBL.CIN);
            GRNData.Add("PAID_DATE", objEgEChallanBankBL.timeStamp.ToString());
            GRNData.Add("BANK_CODE", objEgEChallanBankBL.BankCode);
            GRNData.Add("TRANS_STATUS", objEgEChallanBankBL.Status);
            
            switch (GRNData["TRANS_STATUS"].Substring(0, 1).ToUpper())
            {
                case "S":
                    Message = "Status updated as successfull ";
                    break;
                case "P":
                    Message = "Status updated as Pending ";
                    break;
                case "F":
                    Message = "Status updated as Unsuccessfull ";
                    break;
                default:
                    Message = "Invalid Status";
                    break;
            }
        }
        else
        {
            Message = "Invalid Data";
        }
        return result;

    }
}