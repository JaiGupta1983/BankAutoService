using EgBL;
using System;

/// <summary>
/// Summary description for PushData
/// </summary>
public class PushData
{
    public Int64 GRN { get; set; }
    public string BSRCode { get; set; }
    public string GRNPushData()
    {
        EgManualBankServiceBL objEgManualBankServiceBL = new EgManualBankServiceBL();
        objEgManualBankServiceBL.GRNNumber = GRN;
        objEgManualBankServiceBL.BankCode = objEgManualBankServiceBL.GetGRNBsrCode();
        string ChallanDetails = objEgManualBankServiceBL.GetGrnManualDetails();
        Banks objPush = Banks.SelectBanks(BSRCode);
        string CipherText = BanksEncryptionDecryption.GetEncryptedString(ChallanDetails, objPush.KeyName);
        return objPush.CallManualDataPushService(CipherText);
    }
}