using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System;

namespace EgBL
{
    public class EgManualBankServiceBL
    {

        #region Properties
        public string BankCode { get; set; }
        public string Errorname { get; set; }
        public string PageName { get; set; }
        public int ServiceType { get; set; }
        public string BankURL { get; set; }
        public Int64 GRNNumber { get; set; }
        public string CipherText { get; set; }
        public string PlainText { get; set; }
        public string IPAddress { get; set; }
        public string flag { get; set; }
        #endregion
        #region Function
        public DataTable ManualGRNlist()
        {
            DataTable dt = new DataTable();
            GenralFunction gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@bsrcode", SqlDbType.Char, 7) { Value = BankCode };
            return gf.Filldatatablevalue(PARM, "EgGetManualGRN_service", dt, null);
           
        }
        public string GetGrnManualDetails()
        {
            GenralFunction gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[2];
            PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
            PARM[1] = new SqlParameter("@BSRCode", SqlDbType.Char, 7) { Value = BankCode };
            return gf.ExecuteScaler(PARM, "EgGetManualChallanByGRN");
        }
        /// <summary>
        /// Get GRN BSRcode for call bank Manual service
        /// </summary>
        /// <returns>BSRCode</returns>
        public string GetGRNBsrCode()
        {
            GenralFunction gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
            return gf.ExecuteScaler(PARM, "EgGetGRNBSRcode");

        }

        //SBIePAY
        public DataTable  GetEPayGRN()
        {
            DataTable dt = new DataTable();
            GenralFunction gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@bsrcode", SqlDbType.Char, 7) { Value = BankCode };
            return gf.Filldatatablevalue(PARM, "EgGetEPayGRN",dt,null);
            
        }

        public string GetEPayChallanDetail()
        {
            GenralFunction gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[2];
            PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
            PARM[1] = new SqlParameter("@bsrcode", SqlDbType.Char, 7) { Value = BankCode };
            return gf.ExecuteScaler(PARM, "EgGetEPayChallanDetail");
        }
        //To garb the error for PNB web Service
        public int insertErrorLog()
        {
            GenralFunction gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[2];
            PARM[0] = new SqlParameter("@Errorname", SqlDbType.VarChar, 100) { Value = Errorname };
            PARM[1] = new SqlParameter("@PageName", SqlDbType.VarChar, 200) { Value = PageName };
            return gf.UpdateData(PARM, "EgCatchErrorLogInsert");
        }
        public int InsertBankServiceErrorLog()
        {
            GenralFunction gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[4];
            PARM[0] = new SqlParameter("@BsrCode", SqlDbType.Char, 7) { Value = BankCode };
            PARM[1] = new SqlParameter("@ServiceType", SqlDbType.TinyInt) { Value = ServiceType };
            PARM[2] = new SqlParameter("@BankURL", SqlDbType.VarChar, 100) { Value = BankURL };
            PARM[3] = new SqlParameter("@Errorname", SqlDbType.VarChar, 100) { Value = Errorname };
            return gf.UpdateData(PARM, "EgBankServiceErrorLog");
        }
        #endregion

        // Add Method To share Grn Detail to Paymanager 20 May 2019
        public string GetGrnDetailToPaymanager()
        {
            GenralFunction gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
            
            return gf.ExecuteScaler(PARM, "EgChallanDetailToPaymanagerByGrn");
        }

        public bool InsertAuditLog()
        {
            GenralFunction gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[4];
            PARM[0] = new SqlParameter("@bankCode", SqlDbType.Char, 7) { Value = BankCode };
            PARM[1] = new SqlParameter("@encData", SqlDbType.NVarChar, 4000) { Value = CipherText };
            PARM[2] = new SqlParameter("@url", SqlDbType.NVarChar, 200) { Value = BankURL };
            PARM[3] = new SqlParameter("@ipAddress", SqlDbType.NVarChar, 20) { Value = IPAddress };
            int result = Convert.ToInt32(gf.ExecuteScaler(PARM, "EgBankResponseAudit"));
            return result == 1 ? true : false;
        }
        /// <summary>
        /// test bank data
        /// </summary>
        /// <returns></returns>
        public int ManualBankServiceAuditData()
        {
            GenralFunction gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[4];
            PARM[0] = new SqlParameter("@bankCode", SqlDbType.Char, 7) { Value = BankCode };
            PARM[1] = new SqlParameter("@encData", SqlDbType.NVarChar, 4000) { Value = CipherText };
          
            PARM[2] = new SqlParameter("@flag", SqlDbType.Char, 1) { Value = flag };
            PARM[3] = new SqlParameter("@ipAddress", SqlDbType.NVarChar, 20) { Value = IPAddress };
            return gf.UpdateData(PARM, "EgManualChallanAuditLog");
        }
    }
}
