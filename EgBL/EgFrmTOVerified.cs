using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using EgDAL;
using DL;

namespace EgBL
{

    public class EgFrmTOVerified
    {
        GenralFunction gf = new GenralFunction();

        #region Properties
        public string BankCode { get; set; }
        public string encData { get; set; }
        public string plainText { get; set; }
        public string flag { get; set; }
        public string Ipaddress { get; set; }
        public string paymentType { get; set; }
        public Int64 GRN { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Int16 Type { get; set; }
        #endregion

        #region Function
        public DataTable FillVerifyChallan()
        {
            DataTable dt = new DataTable();
            SqlParameter[] PARM = new SqlParameter[6];
            PARM[0] = new SqlParameter("@bsrcode", SqlDbType.Char, 7) { Value = BankCode };
            PARM[1] = new SqlParameter("@paymentType", SqlDbType.Char, 1) { Value = paymentType };
            PARM[2] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRN };
            PARM[3] = new SqlParameter("@FromDate", SqlDbType.Date) { Value = FromDate };
            PARM[4] = new SqlParameter("@Todate", SqlDbType.Date) { Value = ToDate };
            PARM[5] = new SqlParameter("@Type", SqlDbType.SmallInt) { Value = Type };
            return gf.Filldatatablevalue(PARM, "EgTOVerified", dt, null);
        }

       

        /// <summary>
        /// test bank data
        /// </summary>
        /// <returns></returns>
        public int BankResponseAudit()
        {
            SqlParameter[] PARM = new SqlParameter[5];
            PARM[0] = new SqlParameter("@bankCode", SqlDbType.Char, 7) { Value = BankCode };
            PARM[1] = new SqlParameter("@encData", SqlDbType.NVarChar, 4000) { Value = encData };
            PARM[2] = new SqlParameter("@IPAddress", SqlDbType.VarChar, 20) { Value = Ipaddress };
            PARM[3] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRN };
            PARM[4] = new SqlParameter("@PaymentType", SqlDbType.Char, 1) { Value = paymentType };
            return gf.UpdateData(PARM, "EgBankServiceAudit");
        }

        /// <summary>
        /// test bank data
        /// </summary>
        /// <returns></returns>
        public int BankServiceAuditData()
        {
            SqlParameter[] PARM = new SqlParameter[4];
            PARM[0] = new SqlParameter("@bankCode", SqlDbType.Char, 7) { Value = BankCode };
            PARM[1] = new SqlParameter("@encData", SqlDbType.NVarChar, 4000) { Value = encData };
            PARM[2] = new SqlParameter("@plainText", SqlDbType.NVarChar, 4000) { Value = plainText };
            PARM[3] = new SqlParameter("@flag", SqlDbType.Char, 1) { Value = flag };
            return gf.UpdateData(PARM, "EgBankServiceAuditData");
        }
        /// <summary>
        /// Get Payment Type of GRN
        /// </summary>
        /// <returns></returns>
        public string GetPaymentType()
        {
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRN };
            return gf.ExecuteScaler(PARM, "EgGetPaymentType");
        }


        /// <summary>
        /// Call dOuble Verifuvation Service through Sql job 2 August 2019
        /// </summary>
        /// <returns></returns>
        public DataTable GetGrnForVerification()
        {
            DataTable dt = new DataTable();
            dt = gf.Filldatatablevalue(null, "EgGetGrnForVerification", dt, null);
            return dt;
        }
        public int GrnPending()
        {
            gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[2];
            PARM[0] = new SqlParameter("@Grn", SqlDbType.BigInt) { Value = GRN };
            PARM[1] = new SqlParameter("@Flag", SqlDbType.Char, 1) { Value = flag };
            return gf.UpdateData(PARM, "EgPendingGrnForBankChallan");
        }
        #endregion

    }
}
