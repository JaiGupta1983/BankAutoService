using System;
using System.Data;
using System.Data.SqlClient;

namespace EgBL
{
    public class EgBankForwarBL
    {
        GenralFunction gf = new GenralFunction();
        public Int64 Grn { get; set; }
        public string encdata { get; set; }
        public string flag { get; set; }
        public string BankCode { get; set; }
        public int InsertAudit()
        {
            SqlParameter[] PARM = new SqlParameter[4];
            PARM[0] = new SqlParameter("@bankCode", SqlDbType.Char, 7) { Value = BankCode };
            PARM[1] = new SqlParameter("@encData", SqlDbType.NVarChar, 4000) { Value = encdata };
            PARM[2] = new SqlParameter("@flag", SqlDbType.Char, 1) { Value = flag };
            PARM[3] = new SqlParameter("@Grn", SqlDbType.BigInt) { Value = Grn };
            return gf.UpdateData(PARM, "EgBankforwardAudit");
        }
    }
}
