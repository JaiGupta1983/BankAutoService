using System;
using System.Data;
using System.Data.SqlClient;
using DL;
using System.Web.UI.WebControls;

namespace EgBL
{
    public class EgEChallanBankBL
    {
        GenralFunction gf;//= new GenralFunction();
        #region Properties
        public string Status { get; set; }
        public Int64 GRN { get; set; }
        public string CIN { get; set; }
        public string Ref { get; set; }
        public string BankCode { get; set; }
        public string EBankCode { get; set; }
        public double Amount { get; set; }
        public DateTime timeStamp { get; set; }
        public string encData { get; set; }
        public string url { get; set; }
        public string ip { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }//Priyanka Sharma
        public int UserType { get; set; }//Priyanka Sharma
        public int Mcode { get; set; }
        public string epayBSRCode { get; set; }
        public string bankRefNo { get; set; }
        public string payMode { get; set; }

        public string TransType { get; set; }//Payal Sharma
        public string Reason { get; set; }//Payal Sharma
        public int PayType { get; set; }//Payal Sharma
        #endregion
        #region Function
        /// <summary>
        ///  when transaction is  Cancel then update status 'u'
        /// </summary>
        /// <returns></returns>
        public int UpdateStatus()
        {
            gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[6];

            PARM[0] = new SqlParameter("@Status", SqlDbType.Char, 1) { Value = Status };
            PARM[1] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRN };
            PARM[2] = new SqlParameter("@Ref", SqlDbType.Char, 30) { Value = Ref };
            PARM[3] = new SqlParameter("@amount", SqlDbType.Money) { Value = Amount };
            PARM[4] = new SqlParameter("@timeStamp", SqlDbType.DateTime) { Value = timeStamp };
            PARM[5] = new SqlParameter("@bankCode", SqlDbType.Char, 7) { Value = BankCode };
            return gf.UpdateData(PARM, "EgUserBankStatus");
        }
        /// <summary>
        ///  when transaction is  complete then update status 's'
        /// </summary>
        /// <returns></returns>
        public int UpdateSuccessStatus()
        {
            gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[7];

            PARM[0] = new SqlParameter("@Status", SqlDbType.Char, 1) { Value = Status };
            PARM[1] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRN };
            PARM[2] = new SqlParameter("@CIN", SqlDbType.Char, 21) { Value = CIN };
            PARM[3] = new SqlParameter("@Ref", SqlDbType.Char, 30) { Value = Ref };
            PARM[4] = new SqlParameter("@amount", SqlDbType.Money) { Value = Amount };
            PARM[5] = new SqlParameter("@timeStamp", SqlDbType.DateTime) { Value = timeStamp };
            PARM[6] = new SqlParameter("@bankCode", SqlDbType.Char, 7) { Value = BankCode };
            return gf.UpdateData(PARM, "EgUserBankStatus");
        }
        /// <summary>
        ///  when transaction is not complete then update status 'p'
        /// </summary>
        /// <returns></returns>
        public int UpdatePendingStatus()
        {
            gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[3];

            PARM[0] = new SqlParameter("@Status", SqlDbType.Char, 1) { Value = Status };
            PARM[1] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRN };
            PARM[2] = new SqlParameter("@amount", SqlDbType.Money) { Value = Amount };
            return gf.UpdateData(PARM, "EgUserBankStatus");
        }

        /// <summary>
        /// Check if Stamp 10% Commission Case 
        /// </summary>
        /// <returns>
        /// true if case of 10% Commission case
        /// else false
        /// </returns>
        public bool CheckStamp10PercentCase()
        {
            gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRN };
            return Convert.ToBoolean(gf.ExecuteScaler(PARM, "EgCheckStampNonJudicialCase"));
        }


        /// <summary>
        ///  when transaction is  complete through webservice then update status 's'
        /// </summary>
        /// <returns></returns>
        public int UpdateSuccessStatusReconcile()
        {
            gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[7];

            PARM[0] = new SqlParameter("@Status", SqlDbType.Char, 1) { Value = Status };
            PARM[1] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRN };
            PARM[2] = new SqlParameter("@CIN", SqlDbType.Char, 21) { Value = CIN };
            PARM[3] = new SqlParameter("@Ref", SqlDbType.Char, 30) { Value = Ref };
            PARM[4] = new SqlParameter("@amount", SqlDbType.Money) { Value = Amount };
            PARM[5] = new SqlParameter("@timeStamp", SqlDbType.DateTime) { Value = timeStamp };
            PARM[6] = new SqlParameter("@bankCode", SqlDbType.Char, 7) { Value = BankCode };
            return gf.UpdateData(PARM, "EgUserBankStatusReconsile");
        }

        /// <summary>
        /// use in EgbankVerifiedChallan
        /// </summary>
        /// <returns>get userid acc to GRN </returns>
        public int GetUserIdforVerification()
        {
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@GRN", SqlDbType.Int) { Value = GRN };
            int output1 = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.conString, "EgGetUserIDforVerify", PARM));

            return output1;
        }
        /// <summary>
        /// use  for get IP address list for USer  verification
        /// </summary>
        /// <returns></returns>
        public DataTable GetIpAddress()
        {
            gf = new GenralFunction();
            SqlParameter[] PM = new SqlParameter[0];
            DataTable dt = new DataTable();
            return gf.Filldatatablevalue(PM, "EgGetIpaddress", dt, null);
        }

        /// <summary>
        ///   Priyanka Sharma on dated 1 august for get use information when  we need to session Condition 
        /// </summary>
        public void LoadUserEntries()
        {
            gf = new GenralFunction();
            SqlParameter[] PM = new SqlParameter[1];
            PM[0] = new SqlParameter("@GRN", SqlDbType.Int) { Value = GRN };
            SqlDataReader dr = gf.FillDataReader(PM, "EgFetchUserInfo");
            if (dr.HasRows)
            {
                dr.Read();
                UserId = int.Parse(dr[0].ToString().Trim());
                UserType = int.Parse(dr[1].ToString().Trim());
                Name = Convert.ToString(dr[2].ToString().Trim());
              
            }
            dr.Close();
            dr.Dispose();

        }

        /// <summary>
        /// test bank data
        /// </summary>
        /// <returns></returns>
        public int BankResponseAudit()
        {
            gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[5];
            PARM[0] = new SqlParameter("@bankCode", SqlDbType.Char, 7) { Value = BankCode };
            PARM[1] = new SqlParameter("@encData", SqlDbType.NVarChar, 4000) { Value = encData };
            PARM[2] = new SqlParameter("@url", SqlDbType.NVarChar, 200) { Value = url };
            PARM[3] = new SqlParameter("@ipAddress", SqlDbType.NVarChar, 20) { Value = ip };
            PARM[4] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRN };
            return gf.UpdateData(PARM, "EgBankResponseAudit");
        }


        /// <summary>
        /// Check GRN entry in Merchant Table it's Exists ant not 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>1 true ,0 false</returns>
        public int CheckGrnMerchantCode()
        {
            gf = new GenralFunction();
            int result = 0;
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRN };
            SqlDataReader dr = gf.FillDataReader(PARM, "EgCheckGRNMerchantCode");
            if (dr.HasRows)
            {
                dr.Read();
                result = int.Parse(dr[0].ToString().Trim());
               
            }
            dr.Close();
            dr.Dispose();
            return result;
        }
        /// <summary>
        /// Get Stamp RefCode on GRN number
        /// </summary>
        /// <returns></returns>
        public string GetGrnMerchantCodeRef()
        {
            gf = new GenralFunction();
            string Refe = "";
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRN };
            SqlDataReader dr = gf.FillDataReader(PARM, "EgGetGRNMerchantCodeRef");
            if (dr.HasRows)
            {
                dr.Read();
                Refe = dr[0].ToString().Trim();
              
            }
            dr.Close();
            dr.Dispose();
            return Refe;
        }
        public bool GetGrnShowData()
        {
            gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[3];
            PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRN };
            PARM[1] = new SqlParameter("@Amount", SqlDbType.Money) { Value = Amount };
            PARM[2] = new SqlParameter("@BsrCode", SqlDbType.Char ,7) { Value = BankCode };
            SqlDataReader dr = gf.FillDataReader(PARM, "EgGetChallanReceipt");
            if (dr.HasRows)
            {
                dr.Read();
                GRN = Convert.ToInt64(dr[0].ToString().Trim());
                Amount = Convert.ToDouble(dr[1].ToString().Trim());
                BankCode = dr[2].ToString().Trim();
                bankRefNo = dr[3].ToString().Trim();
                CIN = dr[4].ToString().Trim();
                timeStamp = Convert.ToDateTime(dr[5].ToString().Trim());
                Status = dr[6].ToString().Trim();
                dr.Close();
                dr.Dispose();
                return true;
            }
            else
            {
                dr.Close();
                dr.Dispose();
                return false;

            }
          
        }
        /// <summary>
        /// grt GRN  Merchant details
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>1 true ,0 false</returns>
        public void GetGrnMerchantDetails()
        {
            gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRN };
            SqlDataReader dr = gf.FillDataReader(PARM, "GetGrnMerchantDetails");
            if (dr.HasRows)
            {
                dr.Read();
                encData = dr["DETAIL"].ToString().Trim();
                Mcode = Convert.ToInt32(dr["MerchantCode"].ToString());
                url = dr["ResponseAddress"].ToString().Trim();
                
            }
            else
                url = "";
            dr.Close();
            dr.Dispose();
        }
        /// <summary>
        /// test bank data
        /// </summary>
        /// <returns></returns>
        public int BankResponseParameters()
        {
            gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[2];
            PARM[0] = new SqlParameter("@BankCode", SqlDbType.Char, 7) { Value = BankCode };
            PARM[1] = new SqlParameter("@parametername", SqlDbType.VarChar, 50) { Value = encData };
            return gf.UpdateData(PARM, "Insertbankresponseparameters");
        }


        /// <summary>
        /// Epay details
        /// </summary>
        /// <returns></returns>
        public int UpdateEpayStatus()
        {
            gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[4];
            PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRN };
            PARM[1] = new SqlParameter("@epayBankCode", SqlDbType.VarChar, 10) { Value = epayBSRCode };
            PARM[2] = new SqlParameter("@bankRef", SqlDbType.VarChar, 40) { Value = bankRefNo };
            PARM[3] = new SqlParameter("@PayMode", SqlDbType.VarChar, 10) { Value = payMode };
            return gf.UpdateData(PARM, "EgUpdateEpayStatus");
        }

        public int UpdatePAYUStatus()
        {
            gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[5];
            PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRN };
            PARM[1] = new SqlParameter("@payuBankCode", SqlDbType.VarChar, 10) { Value = epayBSRCode };
            PARM[2] = new SqlParameter("@bankRef", SqlDbType.VarChar, 40) { Value = bankRefNo };
            PARM[3] = new SqlParameter("@PayMode", SqlDbType.VarChar, 10) { Value = payMode };
            PARM[4] = new SqlParameter("@Reason", SqlDbType.VarChar, 10) { Value = Reason };
            return gf.UpdateData(PARM, "EgUpdatePAYUStatus");
        }

       
        #endregion
    }
}
