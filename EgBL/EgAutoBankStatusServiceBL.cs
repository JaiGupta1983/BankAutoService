using System;
using System.Data;
using System.Data.SqlClient;

namespace EgBL
{
    public class EgAutoBankStatusServiceBL
    {
        public string BSRCode { get; set; }
        public Int64 GRN { get; set; }
        public double Amount { get; set; }
        public string encdata { get; set; }
        public string PaymentMode { get; set; }
        public string PlainText { get; set; }
        public string CheckSum { get; set; }
        public string cipherText { get; set; }


        public string BANK_CODE { get; set; }
        public string BankReferenceNo { get; set; }
        public string CIN { get; set; }
        public string PAID_DATE { get; set; }
        public string PAID_AMT { get; set; }
        public string TRANS_STATUS { get; set; }
        public string epayBSRCode { get; set; }
        public string bankRefNo { get; set; }
        public string payMode { get; set; }
        public string hash { get; set; }
        public string reason { get; set; }
        public string PayUBSRCode { get; set; }
        public string Head { get; set; }


        GenralFunction gf = new GenralFunction();
        EncryptDecryptionBL objEncryption = new EncryptDecryptionBL();

        public DataTable GetPrepareData()
        {
            try
            {
                DataTable dt = new DataTable();
                //SqlParameter[] PARM = new SqlParameter[1];
                //PARM[0] = new SqlParameter("@bsrcode", SqlDbType.Char, 7) { Value = BSRCode };
                dt = gf.Filldatatablevalue(null, "EgPreBankProcessData", dt, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int UpdatePrepareCipherTextData()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] PARM = new SqlParameter[6];

                PARM[0] = new SqlParameter("@Grn", SqlDbType.BigInt) { Value = GRN };
                PARM[1] = new SqlParameter("@Amount", SqlDbType.Money) { Value = Amount };
                PARM[2] = new SqlParameter("@BSRCode", SqlDbType.Char, 7) { Value = BSRCode };
                PARM[3] = new SqlParameter("@cipherText", SqlDbType.VarChar, 500) { Value = cipherText };
                PARM[4] = new SqlParameter("@PlainText", SqlDbType.VarChar, 200) { Value = PlainText };
                PARM[5] = new SqlParameter("@CheckSum", SqlDbType.VarChar, 500) { Value = CheckSum };

                int Rv = gf.UpdateData(PARM, "EgUpdateCipherTextRequestData");
                return Rv;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetCipherTextData()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] PARM = new SqlParameter[1];
                PARM[0] = new SqlParameter("@bsrcode", SqlDbType.Char, 7) { Value = BSRCode };
                dt = gf.Filldatatablevalue(PARM, "EgGetCipherTextData", dt, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int InsertBankResponseCipherText()
        {
            try
            {
                SqlParameter[] PARM = new SqlParameter[4];

                PARM[0] = new SqlParameter("@Grn", SqlDbType.BigInt) { Value = GRN };
                PARM[1] = new SqlParameter("@Amount", SqlDbType.Money) { Value = Amount };
                PARM[2] = new SqlParameter("@BSRCode", SqlDbType.Char, 7) { Value = BSRCode };
                PARM[3] = new SqlParameter("@cipherText", SqlDbType.VarChar, 500) { Value = cipherText };
                int Rv = gf.UpdateData(PARM, "EgUpdateCipherTextResponseData");
                return Rv;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetBankResponseData()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] PARM = new SqlParameter[1];
                PARM[0] = new SqlParameter("@bsrcode", SqlDbType.Char, 7) { Value = BSRCode };
                dt = gf.Filldatatablevalue(PARM, "EgGetCipherTextResponseData", dt, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int InsertBankResponseData()
        {
            try
            {
                GenralFunction gf = new GenralFunction();
                SqlParameter[] PARM = new SqlParameter[11];

                PARM[0] = new SqlParameter("@Status", SqlDbType.Char, 1) { Value = TRANS_STATUS };
                PARM[1] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRN };
                PARM[2] = new SqlParameter("@CIN", SqlDbType.Char, 21) { Value = CIN };
                PARM[3] = new SqlParameter("@Ref", SqlDbType.Char, 30) { Value = BankReferenceNo };
                PARM[4] = new SqlParameter("@amount", SqlDbType.Money) { Value = Convert.ToDouble(PAID_AMT) };
                PARM[5] = new SqlParameter("@timeStamp", SqlDbType.DateTime) { Value = Convert.ToDateTime(PAID_DATE) };
                PARM[6] = new SqlParameter("@bankCode", SqlDbType.Char, 7) { Value = BANK_CODE };
                PARM[7] = new SqlParameter("@payuBankCode", SqlDbType.VarChar, 10) { Value = PayUBSRCode };
                PARM[8] = new SqlParameter("@bankRef", SqlDbType.VarChar, 40) { Value = bankRefNo };
                PARM[9] = new SqlParameter("@PayMode", SqlDbType.VarChar, 10) { Value = payMode };
                PARM[10] = new SqlParameter("@Reason", SqlDbType.VarChar, 10) { Value = reason };
                int result = gf.UpdateData(PARM, "InsertGRNIntoAutoBankStatusService");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
