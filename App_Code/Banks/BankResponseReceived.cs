using EgBL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for BankResponseReceived
/// </summary>
public class BankResponseReceived
{
    public string BankCode { get; set; }
    public string CipherText { get; set; }
    public string URL { get; set; }
    public string IPAddress { get; set; }
    public string Message { get; set; }
    public int MerchantCode { get; set; }
    private int UserId { get; set; }
    private int UserType { get; set; }
    private string Name { get; set; }

    public Dictionary<string, string> GRNData;
    public bool UpdateResponse()
    {
        bool result = false;
        try
        {
            if (CheckIPAuthentication_And_Audit() && (BankCode != null && BankCode != "") && (CipherText != null && CipherText != ""))
            {
                ShowChallanData objShowChallanData = new global::ShowChallanData();
                Banks objResponse;
                bool resultdata;
                try { objResponse = Banks.SelectBanks(BankCode); } catch { Message = "Invalid BSRCode"; return false; }
                UpdateStatus objUpdate = new global::UpdateStatus();
                objUpdate.CipherText = CipherText;
                objUpdate.KeyName = objResponse.KeyName;
                objUpdate.Version = objResponse.Version;

                if (objUpdate.Version == "2.0")      /// Verify With DV Call

                {
                    string PlainText = BanksEncryptionDecryption.GetDecryptedString(objUpdate.CipherText, objUpdate.KeyName, objUpdate.Version);
                    List<string> lstPlainText = new List<string>();
                    lstPlainText = PlainText.Split('|').ToList();
                    VerifiedBankClass objVerifyBank = new VerifiedBankClass();
                    objVerifyBank.BSRCode = lstPlainText[1].Split('=').GetValue(1).ToString().Trim(); //BsrCode
                    objVerifyBank.GRN = Convert.ToInt64(lstPlainText[0].Split('=').GetValue(1).ToString().Trim()); //GRN
                    objVerifyBank.TotalAmount = Convert.ToDouble(lstPlainText[5].Split('=').GetValue(1).ToString().Trim()); //Amount
                    objVerifyBank.PaymentMode = "N";
                    string msg = objVerifyBank.Verify();

                    //EgEChallanBankBL objEgEChallanBankBL = new EgEChallanBankBL();
                    //objEgEChallanBankBL.GRN = objVerifyBank.GRN;
                    //objEgEChallanBankBL.LoadUserEntries(Convert.ToInt64(objVerifyBank.GRN));
                    LoadUserEntries(Convert.ToInt64(objVerifyBank.GRN));
                    MerchantCode = checkGRNMerchantCode(Convert.ToInt64(objVerifyBank.GRN));
                    SetSessionValues(MerchantCode);

                    objShowChallanData.GRN = Convert.ToInt64(lstPlainText[0].Split('=').GetValue(1).ToString().Trim()); //GRN
                    objShowChallanData.TotalAmount = Convert.ToDouble(lstPlainText[5].Split('=').GetValue(1).ToString().Trim()); //Amount
                    objShowChallanData.BsrCode = lstPlainText[1].Split('=').GetValue(1).ToString().Trim(); //BsrCode
                    resultdata = objShowChallanData.ShowData();
                    GRNData = objShowChallanData.GRNData;
                    Message = objShowChallanData.Message;
                    return resultdata;
                }
                else
                { 
                 result = objUpdate.UpdateGRNStatus();
                }

                if (!result)
                {
                    if (objUpdate.Message == "" || objUpdate.Message == null)
                        objUpdate.Message = "Due to some technical issue Unable to process";
                    Message = objUpdate.Message;
                    return result;
                }
                Message = objUpdate.Message;
                GRNData = objUpdate.GRNData;
                LoadUserEntries(Convert.ToInt64(GRNData["GRN"]));
                MerchantCode = checkGRNMerchantCode(Convert.ToInt64(GRNData["GRN"]));
                SetSessionValues(MerchantCode);
               // ShowChallanData objShowChallanData = new global::ShowChallanData();
                objShowChallanData.GRN = Convert.ToInt64(GRNData["GRN"]);
                objShowChallanData.TotalAmount = Convert.ToDouble(GRNData["PAID_AMT"]);
                objShowChallanData.BsrCode = BankCode;
                // bool resultdata = objShowChallanData.ShowData();
                resultdata = objShowChallanData.ShowData();
                GRNData = objShowChallanData.GRNData;
                Message = objShowChallanData.Message;
                return resultdata;
            }
            else if ((BankCode == null || BankCode == "") || (CipherText == null || CipherText == ""))
            {
                Message = "InSufficient Data";
                return false;
            }
            else
            {
                Message = "Invalid IP";
                return false;
            }
        }
        catch (Exception ex)
        {
            EgErrorHandller obj = new EgErrorHandller();
            obj.InsertError(ex.Message + ",_UpdateResponse");
            Message = "Error in response data";
            return false;
        }
    }
    private bool CheckIPAuthentication_And_Audit()
    {
        try
        {
            GenralFunction gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[4];
            PARM[0] = new SqlParameter("@bankCode", SqlDbType.Char, 7) { Value = BankCode };
            PARM[1] = new SqlParameter("@encData", SqlDbType.NVarChar, 4000) { Value = ConfigurationManager.AppSettings["ServerName"].ToString() + " => " + CipherText };
            PARM[2] = new SqlParameter("@url", SqlDbType.NVarChar, 200) { Value = URL };
            PARM[3] = new SqlParameter("@ipAddress", SqlDbType.NVarChar, 20) { Value = HttpContext.Current.Request.ServerVariables[ConfigurationManager.AppSettings["IPAddressHTTP"]].ToString() };//Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]) };
            int result = Convert.ToInt32(gf.ExecuteScaler(PARM, "EgBankResponseAudit"));
            return result == 1 ? true : false;
        }
        catch(Exception ex) {
            throw new Exception("BSR:" + BankCode + "Error In CheckIPAuthentication_And_Audit" + ex.Message);
        }
    }
    private int checkGRNMerchantCode(Int64 GRN)
    {
        try
        {
            EgEChallanBankBL objEgEChallanBankBL = new EgEChallanBankBL();
            objEgEChallanBankBL.GRN = GRN; //Convert.ToInt32(GRNData["GRN"]);
            return objEgEChallanBankBL.CheckGrnMerchantCode();
        }
        catch (Exception ex)
        {
            throw new Exception("Error in checkGRNMerchantCode" + ex.Message);
        }
    }
    private void LoadUserEntries(Int64 GRN)
    {
        EgEChallanBankBL objEgEChallanBankBL = new EgEChallanBankBL();
        objEgEChallanBankBL.GRN = GRN; //Convert.ToInt32(GRNData["GRN"]);
        objEgEChallanBankBL.LoadUserEntries();
        UserId = objEgEChallanBankBL.UserId;
        UserType = objEgEChallanBankBL.UserType;
        Name = objEgEChallanBankBL.Name;
    }

    private void SetSessionValues(int MerchantCode)
    {
        try
        {
            if (MerchantCode != 0)
            {
                HttpContext.Current.Session["UserID"] = 73;
                HttpContext.Current.Session["UserType"] = 9;
                HttpContext.Current.Session["UserName"] = "Guest";
                HttpContext.Current.Session["MenuDataSet"] = "";
            }
            else
            {
                HttpContext.Current.Session["UserID"] = UserId;
                HttpContext.Current.Session["UserType"] = UserType;
                HttpContext.Current.Session["UserName"] = Name;
                HttpContext.Current.Session["MenuDataSet"] = "";
            }
        }
        catch {
            throw new Exception("Error in SetSessionValues");
        }
    }
    /// <summary>
    /// /payu
    /// </summary>
    /// <returns></returns>
    public bool UpdatePayuResponse()
    {
        try
        {
            BankCode = GRNData["BANK_CODE"];
            CipherText = string.Join("|", GRNData.Select(x => x.Value).ToArray());
            if (CheckIPAuthentication_And_Audit() && (BankCode != null && BankCode != "") && (CipherText != null && CipherText != ""))
            {

                VerifiedBankClass objVerifyBank = new VerifiedBankClass();
                objVerifyBank.BSRCode = GRNData["BANK_CODE"];
                objVerifyBank.GRN = Convert.ToInt64(GRNData["GRN"]);
                objVerifyBank.TotalAmount = Convert.ToDouble(GRNData["PAID_AMT"]);
                objVerifyBank.PaymentMode = "N";
                string msg = objVerifyBank.Verify();

                LoadUserEntries(Convert.ToInt64(objVerifyBank.GRN));
                MerchantCode = checkGRNMerchantCode(Convert.ToInt64(objVerifyBank.GRN));
                SetSessionValues(MerchantCode);
                //sha512(SALT | status |||||| udf5 | udf4 | udf3 | udf2 | udf1 | email | firstname | productinfo | amount | txnid | key)
                //string plainText = string.Format(ConfigurationManager.AppSettings["PayuSalt"].ToString() + "|" + GRNData["TRANS_STATUS"] + "|||||||||" + GRNData["udf2"] + "|" + GRNData["udf1"] + "|" + GRNData["email"] + "|" + GRNData["firstname"] + "|" + GRNData["productinfo"] + "|" + GRNData["PAID_AMT"] + "|" + GRNData["GRN"] + "|" + ConfigurationManager.AppSettings["PayUKey"].ToString());
                //plainText = GenerateSHA512String(plainText).ToLower();
                //if (plainText == GRNData["hash"])
                //{
                //    Banks objResponse;
                //    try { objResponse = Banks.SelectBanks(BankCode); } catch { Message = "Invalid BSRCode"; return false; }
                //    UpdateStatus objUpdate = new global::UpdateStatus();
                //    objUpdate.CipherText = CipherText;
                //    objUpdate.KeyName = objResponse.KeyName;
                //    objUpdate.GRNData = GRNData;
                //bool result = objUpdate.UpdateGRNStatus_PayU();
                ShowChallanData objShowChallanData = new global::ShowChallanData();
                objShowChallanData.GRN = Convert.ToInt64(GRNData["GRN"]);
                objShowChallanData.TotalAmount = Convert.ToDouble(GRNData["PAID_AMT"]);
                objShowChallanData.BsrCode = BankCode;
                bool resultdata = objShowChallanData.ShowData();
                GRNData = objShowChallanData.GRNData;
                Message = objShowChallanData.Message;
               
                return resultdata;

            }
            else if ((BankCode == null || BankCode == "") || (CipherText == null || CipherText == ""))
            {
                Message = "InSufficient Data";
                return false;
            }
            else
            {
                Message = "Invalid IP";
                return false;
            }
        }
        catch (Exception ex)
        {
            EgErrorHandller obj = new EgErrorHandller();
            obj.InsertError(ex.Message + ", " + ex.StackTrace + "_UpdateResponse");
            Message = "Error in response data";
            return false;
        }
    }

    //public static string GenerateSHA512String(string inputString)
    //{
    //    SHA512 sha512 = SHA512Managed.Create();
    //    byte[] bytes = Encoding.UTF8.GetBytes(inputString);
    //    byte[] hash = sha512.ComputeHash(bytes);
    //    return GetStringFromHash(hash);
    //}
    //private static string GetStringFromHash(byte[] hash)
    //{
    //    StringBuilder result = new StringBuilder();
    //    for (int i = 0; i < hash.Length; i++)
    //    {
    //        result.Append(hash[i].ToString("X2"));
    //    }
    //    return result.ToString();
    //}
}