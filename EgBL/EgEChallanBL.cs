using System;

using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using EgDAL;
using Newtonsoft.Json;
using System.Web;
using System.Configuration;

namespace EgBL
{
    public class EgEChallanBL : EgUserRegistrationBL
    {
        GenralFunction gf = new GenralFunction();

        #region Properties
        public string lastflogin { get; set; }
        public DateTime lastslogin { get; set; }
        public string lastchangepass { get; set; }
        public int UserPro { get; set; }
        public int Id { get; set; }
        public int CKey { get; set; }
        public string ChallanDate { get; set; }
        public Int64 GRNNumber { get; set; }
        public string Barcode { get; set; }
        public string FormId { get; set; }
        public string Department { get; set; }
        public string TypeofPayment { get; set; }
        //public string Identity { get; set; }
        public int OfficeName { get; set; }
        public string Office { get; set; }
        public string PanNumber { get; set; }
        public string FullName { get; set; }
        public string ChallanYear { get; set; }
        public DateTime ChallanFromMonth { get; set; }
        public DateTime ChallanToMonth { get; set; }
        // public string Address { get; set; }
        public string Location { get; set; }
        public string TreasuryCode { get; set; }
        public string TreasuryName { get; set; }
        public string MobileNo { get; set; }
        public string PINCode { get; set; }
        public double DeductCommission { get; set; }
        public string TotalAmount { get; set; }
        public string ChequeDDNo { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string AmountInWords { get; set; }
        public string Remark { get; set; }
        public string SchemaName { get; set; }
        public double Amount { get; set; }
        public int ScheCode { get; set; }
        public int ProfileCode { get; set; }
        public string BudgetHead { get; set; }
        public string CIN { get; set; }
        public string Ref { get; set; }
        public string TransDate { get; set; }
        public int Profile { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }
        public int PDacc { get; set; }
        public string Status { get; set; }
        public string Zone { get; set; }
        public string Circle { get; set; }
        public string Ward { get; set; }
        public string Details { get; set; }
        public int ChallanNo { get; set; }
        public string Tcode { get; set; }
        public string filler { get; set; }
        public DataTable dtSchema { get; set; }
        public int Type { get; set; } // For General Challan value 0 otherwise -1
        public int DivCode { get; set; } // Saperation Pd And div  24 No 2017
        public string Ddocode { get; set; }
        public string DdoName { get; set; }
        public string F1 { get; set; }
        public string F2 { get; set; }
        public string F3 { get; set; }
        public string V1 { get; set; }
        public string V2 { get; set; }
        public string V3 { get; set; }
        public string PdaccountNumber { get; set; }
        //start DMFT
        public string PDAccName { get; set; }
        //End DMFT

        public int Serviceid { get; set; }
        public int ProcUserId { get; set; }

        #endregion

        #region Function
        /// <summary>
        /// Get Add More Detail on gRN 
        /// </summary>
        /// <returns>data table</returns>

        public void GetMerchant(DropDownList ddl)
        {
            gf.FillListControl(ddl, "EgGetMerchantCodeList", "MerchantCode", "MerchantCode", null);
            ddl.Items.Insert(0, new ListItem("--Select MerchantCode--", "0"));
        }

        public DataTable GetExtraDetails()
        {
            DataTable dt = new DataTable();
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
            dt = gf.Filldatatablevalue(PARM, "EgGetGrnExtraDetails", dt, null);
            return dt;
        }
        /// <summary>
        /// gets all the  user persional details on GRN
        /// </summary>
        /// <returns></returns>
        public int GetUserGrnDetail()
        {
            SqlParameter[] PM = new SqlParameter[1];
            PM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
            DataTable dt = new DataTable();
            dt = gf.Filldatatablevalue(PM, "EgGetUserGRNpersonalDetail", dt, null);

            int output = 0;
            if (dt.Rows.Count != 0)
            {
                FirstName = dt.Rows[0]["Name"].ToString();
                Address = dt.Rows[0]["Address"].ToString();
                City = dt.Rows[0]["City"].ToString().Trim();
                PinCode = dt.Rows[0]["Pincode"].ToString();
                Identity = dt.Rows[0]["Identity"].ToString();
                MobileNo = dt.Rows[0]["MobileNo"].ToString();
                output = 1;
            }
            else
            {
                output = 0;
            }
            return output;
        }
        /// <summary>
        /// fill Bank Droddownlist
        /// </summary>
        /// <param name=""></param>
        /// 
        public void GetChallanBanks_Payu(DropDownList ddl)
        {
            SqlParameter[] PM = new SqlParameter[0];
            gf.FillListControl(ddl, "EgGatewayBankList", "BankName", "BSRCode", PM);
            ddl.Items.Insert(0, new ListItem("--Select Bank--", "0"));
        }
        public void GetChallanBanks(DropDownList ddl)
        {
            gf.FillListControl(ddl, "EgChallanBankList", "BankName", "BSRCode", null);
            ddl.Items.Insert(0, new ListItem("--Select Bank--", "0"));
        }
        /// <summary>
        /// gets all the logined user persional details
        /// </summary>
        /// <returns></returns>
        public int GetUserDetail()
        {
            gf = new GenralFunction();
            SqlParameter[] PM = new SqlParameter[1];
            PM[0] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };


            DataTable dt = new DataTable();
            dt = gf.Filldatatablevalue(PM, "EgGetUserDetail", dt, null);

            int output = 0;
            if (dt.Rows.Count != 0)
            {
                FirstName = dt.Rows[0]["Name"].ToString();
                lastslogin = Convert.ToDateTime(dt.Rows[0]["Last_slogin_date"].ToString());
                if (dt.Rows[0]["Last_flogin_date"].ToString() == "")
                {
                    lastflogin = dt.Rows[0]["Last_flogin_date"].ToString();
                }
                else
                {
                    lastflogin = dt.Rows[0]["Last_flogin_date"].ToString();
                }
                if (dt.Rows[0]["PasswordChange_Date"].ToString() == "")
                {
                    lastchangepass = dt.Rows[0]["PasswordChange_Date"].ToString();
                }
                else
                {
                    lastchangepass = dt.Rows[0]["PasswordChange_Date"].ToString();
                }
                Address = dt.Rows[0]["Address"].ToString();
                City = dt.Rows[0]["City"].ToString().Trim();
                PinCode = dt.Rows[0]["Pincode"].ToString();
                Identity = dt.Rows[0]["Identity"].ToString();
                MobileNo = dt.Rows[0]["MobileNo"].ToString();
                output = 1;
            }
            else
            {
                output = 0;
            }
            return output;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetChallanBank()
        {
            DataTable dt = new DataTable();
            return gf.Filldatatablevalue(null, "EgChallanBankList", dt, null);
        }

        /// <summary>
        /// fill Bank Droddownlist
        /// </summary>
        /// <param name=""></param>
        public void GetBanks(DropDownList ddl)
        {
            gf.FillListControl(ddl, "EgGetBanks", "BankName", "BSRCode", null);
            ddl.Items.Insert(0, new ListItem("--Select Bank--", "0"));
        }

        public DataTable GetBankDetail()
        {
            DataTable dt = new DataTable();
           
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@bsrcode", SqlDbType.Char, 7) { Value = BankName };
            return gf.Filldatatablevalue(PARM, "EgGetBankList", dt, null);
           

        }
        /// <summary>
        /// // fill department Droddownlist
        /// </summary>
        /// <param name="ddl"></param>
        public void PopulateDepartmentList(DropDownList ddl)
        {
            SqlParameter[] PARM = new SqlParameter[1];
            gf.FillListControl(ddl, "EgGetDepartmentList", "deptnameEnglish", "deptcode", PARM);
            ddl.Items.Insert(0, new ListItem("--Select DepartmentList--", "0"));
        }
        /// <summary>
        /// // fill office Droddownlist
        /// </summary>
        /// <param name="ddl"></param>
        public void FillOfficeList(DropDownList ddl)
        {

            SqlParameter[] PARM = new SqlParameter[2];
            PARM[0] = new SqlParameter("@DeptCode", SqlDbType.Int) { Value = DeptCode };
            PARM[1] = new SqlParameter("@TreasuryCode", SqlDbType.VarChar, 50) { Value = Tcode };
            gf.FillListControl(ddl, "EgFillOfficeList", "officename", "officeid", PARM);
            ddl.Items.Insert(0, new ListItem("--Select Office--", "0"));
        }
        /// <summary>
        /// fill Treasury Droddownlist
        /// </summary>
        /// <param name="ddl"></param>
        public void FillTreasury(DropDownList ddl)
        {
            gf.FillListControl(ddl, "EgFillTreasury", "TreasuryName", "TreasuryCode", null);
            ddl.Items.Insert(0, new ListItem("--Select Treasury--", "0"));

        }

        /// <summary>
        /// fill Treasury Droddownlist
        /// </summary>
        /// <param name="ddl"></param>
        public void FillLocation(DropDownList ddl)
        {
            gf.FillListControl(ddl, "EgFillLocation", "DistrictName", "TreasuryCode", null);
            ddl.Items.Insert(0, new ListItem("--Select Location--", "0"));

        }

        /// <summary>
        /// fill Treasury office wise
        /// </summary>
        /// <param name="ddl"></param>
        public DataTable FillOfficeWiseTreasury()
        {
            DataTable dt = new DataTable();
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@officeId", SqlDbType.Int) { Value = OfficeName };
            return gf.Filldatatablevalue(PARM, "EgFillOfficeWiseTreasury", dt, null);
        }

        /// <summary>
        /// fill CTD information
        /// </summary>

        public DataTable GetCTDInformation()
        {
            DataTable dt = new DataTable();
            return gf.Filldatatablevalue(null, "EgGetCTDInformation", dt, null);
        }

        /// <summary>
        /// Check Exist Tin no
        /// </summary>

        //public int CheckExistTin(SqlTransaction Trans)
        //{
        //    SqlParameter[] PARM = new SqlParameter[1];
        //    PARM[0] = new SqlParameter("@tin", SqlDbType.VarChar, 50) { Value = Identity };
        //    return int.Parse(gf.ExecuteScaler(PARM, "EgCheckExistTIN", Trans));
        //    //    return int.Parse(gf.ExecuteScaler(PARM, "EgCheckExistTIN", null));

        //}

        /// <summary>
        /// Insert the challan data into tables
        /// </summary>
        /// <returns></returns>
        public virtual int InsertChallan()
        {
            GenralFunction gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[31];
            PARM[0] = new SqlParameter("@Profile", SqlDbType.Int) { Value = Profile };
            PARM[1] = new SqlParameter("@Identity", SqlDbType.VarChar, 50) { Value = Identity };
            PARM[2] = new SqlParameter("@OfficeName", SqlDbType.Int) { Value = OfficeName };
            PARM[3] = new SqlParameter("@PANNumber", SqlDbType.Char, 10) { Value = PanNumber };
            PARM[4] = new SqlParameter("@Location", SqlDbType.Char, 4) { Value = Location };
            PARM[5] = new SqlParameter("@FullName", SqlDbType.VarChar, 50) { Value = FullName };
            PARM[6] = new SqlParameter("@ChallanYear", SqlDbType.Char, 4) { Value = ChallanYear };
            PARM[7] = new SqlParameter("@ChallanFromMonth", SqlDbType.SmallDateTime) { Value = ChallanFromMonth };
            PARM[8] = new SqlParameter("@ChallanToMonth", SqlDbType.SmallDateTime) { Value = ChallanToMonth };
            PARM[9] = new SqlParameter("@City", SqlDbType.VarChar, 40) { Value = City };
            PARM[10] = new SqlParameter("@Address", SqlDbType.VarChar, 100) { Value = Address };
            PARM[11] = new SqlParameter("@PINCode", SqlDbType.Char, 6) { Value = PINCode };
            PARM[12] = new SqlParameter("@DeductCommission", SqlDbType.Money) { Value = DeductCommission };
            PARM[13] = new SqlParameter("@TotalAmount", SqlDbType.Money) { Value = TotalAmount };
            if (TypeofPayment == "M")
            {
                PARM[14] = new SqlParameter("@ChequeDDNo", SqlDbType.Char, 6) { Value = ChequeDDNo };
            }
            else
            {
                PARM[14] = new SqlParameter("@ChequeDDNo", SqlDbType.Char, 6) { Value = "" };
            }
            PARM[15] = new SqlParameter("@BankName", SqlDbType.Char, 7) { Value = BankName };
            PARM[16] = new SqlParameter("@Paymenttype", SqlDbType.Char, 1) { Value = TypeofPayment };
            PARM[17] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
            PARM[18] = new SqlParameter("@Id", SqlDbType.Int) { Value = Id };
            PARM[18].Direction = ParameterDirection.Output;
            PARM[19] = new SqlParameter("@Remarks", SqlDbType.VarChar, 200) { Value = Remark };
            PARM[20] = new SqlParameter("@Zone", SqlDbType.Char, 4) { Value = Zone };
            PARM[21] = new SqlParameter("@Circle", SqlDbType.Char, 4) { Value = Circle };
            PARM[22] = new SqlParameter("@Ward", SqlDbType.Char, 4) { Value = Ward };
            PARM[23] = new SqlParameter("@Details", SqlDbType.VarChar, 8000) { Value = Details };
            PARM[24] = new SqlParameter("@PdAcc", SqlDbType.Int) { Value = PDacc };
            PARM[25] = new SqlParameter("@DivCode", SqlDbType.Int) { Value = DivCode };
            PARM[26] = new SqlParameter("@MobileNo", SqlDbType.VarChar, 15) { Value = MobileNo };
            PARM[27] = new SqlParameter("@mytable", SqlDbType.Structured) { Value = dtSchema };// added 9 sep 2014
            PARM[28] = new SqlParameter("@IpAddress", SqlDbType.VarChar, 20) { Value = HttpContext.Current.Request.ServerVariables[ConfigurationManager.AppSettings["IPAddressHTTP"]].ToString() };//HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString() }; // add Ip Nov12 2019
            PARM[29] = new SqlParameter("@ServiceId", SqlDbType.Int) { Value = Serviceid };  // 21  April  2020
            PARM[30] = new SqlParameter("@ServiceType", SqlDbType.Int) { Value = ProcUserId };  // 21  April  2020
            int Rv = gf.UpdateData(PARM, "EgEChallan");

            if (Rv == 0)
            {
                Rv = -1;
            }
            else
            {
                Id = Convert.ToInt32(PARM[18].Value);
                Rv = int.Parse(PARM[18].Value.ToString());
            }
            return Rv;
        }

        /// <summary>
        /// insert schema details of user in table   (********************* Not in Use Now*********************** )
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        //public int InsertSchema(DataTable dt)
        //{
        //    int Result = 1;
        //    SqlParameter[] PM = new SqlParameter[1];
        //    PM[0] = new SqlParameter("@mytable", SqlDbType.Structured);
        //    PM[0].Value = dt;
        //    Result = gf.UpdateData(PM, "EgInsertSchamaData");
        //    return Result;
        //}

        /// <summary>
        /// Fill deprtment list
        /// </summary>
        /// <param name="ddl"></param>
        public void GetDepartmentList(DropDownList ddl)
        {
            SqlParameter[] PM = new SqlParameter[1];
            PM[0] = new SqlParameter("@DeptCode", SqlDbType.Int) { Value = DeptCode };
            gf.FillListControl(ddl, "EgDepartmentList", "DeptNameEnglish", "DeptCode", PM);
            ddl.Items.Insert(0, new ListItem("--Select DepartmentList--", "0"));
        }

        /// <summary>
        /// returns Schema for user according profile id
        /// </summary>
        /// <returns></returns>
        public DataTable GetSchema()
        {
            DataTable dt = new DataTable();
            if (UserPro != 0)
            {
                SqlParameter[] PM = new SqlParameter[2];
                PM[0] = new SqlParameter("@UserPro", SqlDbType.Int) { Value = UserPro };
                PM[1] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };

                return gf.Filldatatablevalue(PM, "EgGetUserProSchema", dt, null);
            }
            else
            {
                SqlParameter[] PARM = new SqlParameter[3];
                PARM[0] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
                PARM[1] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
                PARM[2] = new SqlParameter("@type", SqlDbType.Int) { Value = 1 };

                return gf.Filldatatablevalue(PARM, "EgChallanSchema", dt, null);

            }
        }
        /// <summary>
        /// This Function Not in USe
        /// </summary>
        /// <returns></returns>
        public DataTable GetDeptName()
        {
            DataTable dt = new DataTable();
            SqlParameter[] PM = new SqlParameter[3];
            PM[0] = new SqlParameter("@type", SqlDbType.Int) { Value = 5 };
            PM[1] = new SqlParameter("@UserPro", SqlDbType.Int) { Value = UserPro };
            PM[2] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
            return gf.Filldatatablevalue(PM, "EgDept", dt, null);
        }


        public DataTable GetSchemaME()   // Add Method For Me Challan
        {
            DataTable dt = new DataTable();
            if (UserPro != 0)
            {
                SqlParameter[] PM = new SqlParameter[2];
                PM[0] = new SqlParameter("@UserPro", SqlDbType.Int) { Value = UserPro };
                PM[1] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };

                return gf.Filldatatablevalue(PM, "EgGetUserProSchemaME", dt, null);
            }
            else
            {
                SqlParameter[] PARM = new SqlParameter[3];
                PARM[0] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
                PARM[1] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
                PARM[2] = new SqlParameter("@type", SqlDbType.Int) { Value = 1 };

                return gf.Filldatatablevalue(PARM, "EgChallanSchemaME", dt, null);

            }
        }


        public void GetProfileListME(DropDownList ddl) //added 30 may 2018 to get ME profile for office use
        {

            gf = new GenralFunction();
            SqlParameter[] PM = new SqlParameter[1];
            PM[0] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
            gf.FillListControl(ddl, "EgGetProfileList_ME", "UserProfile", "UserPro", PM);
            ddl.Items.Insert(0, new ListItem("--Select Profile--", "0"));
        }
        /// <summary>
        /// gets all the dat of challan table
        /// </summary>
        /// <returns></returns>
        public int EChallanView()
        {
            DataTable objsdr = new DataTable();
            SqlParameter[] PARM = new SqlParameter[4];
            PARM[0] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
            PARM[1] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
            PARM[2] = new SqlParameter("@Usertype", SqlDbType.VarChar, 3) { Value = UserType };
            PARM[3] = new SqlParameter("@ChallanNo", SqlDbType.Int) { Value = ChallanNo };
            if (FormId == "S")
            {
                objsdr = gf.Filldatatablevalue(PARM, "EgEchallanView_Success", objsdr, null);
            }
            else
            {
                objsdr = gf.Filldatatablevalue(PARM, "EgEchallanView", objsdr, null);
            }

            //objsdr = gf.Filldatatablevalue(PARM, "EgEchallanView", objsdr, null);

            int output = 0;
            if (objsdr.Rows.Count != 0)
            {
                ChallanDate = objsdr.Rows[0]["ChallanDate"].ToString();
                Department = objsdr.Rows[0]["Profile"].ToString();
                TypeofPayment = objsdr.Rows[0]["Paymenttype"].ToString();
                Identity = objsdr.Rows[0]["Identity"].ToString();
                OfficeName = int.Parse(objsdr.Rows[0]["OfficeName"].ToString());
                Office = objsdr.Rows[0]["office"].ToString();
                PanNumber = objsdr.Rows[0]["PANNumber"].ToString();
                Location = objsdr.Rows[0]["DeptNameEnglish"].ToString();
                TreasuryCode = objsdr.Rows[0]["Location"].ToString();
                TreasuryName = objsdr.Rows[0]["TreasuryName"].ToString();
                DistrictName = objsdr.Rows[0]["District"].ToString();
                FullName = objsdr.Rows[0]["FullName"].ToString();
                ChallanYear = objsdr.Rows[0]["ChallanYear"].ToString();
                ChallanFromMonth = Convert.ToDateTime(objsdr.Rows[0]["ChallanFromMonth"]);
                ChallanToMonth = Convert.ToDateTime(objsdr.Rows[0]["ChallanToMonth"]);
                CityName = objsdr.Rows[0]["City"].ToString();
                Address = objsdr.Rows[0]["Address"].ToString();
                PINCode = objsdr.Rows[0]["PINCode"].ToString();
                DeductCommission = Convert.ToDouble(objsdr.Rows[0]["DeductCommission"]);
                TotalAmount = objsdr.Rows[0]["TotalAmount"].ToString();
                ChequeDDNo = objsdr.Rows[0]["ChequeDDNo"].ToString();
                BankName = objsdr.Rows[0]["BankName"].ToString();
                BankCode = objsdr.Rows[0]["BankCode"].ToString();
                GRNNumber = Convert.ToInt64(objsdr.Rows[0]["GRN"]);
                PanNumber = objsdr.Rows[0]["PanNumber"].ToString();
                Remark = objsdr.Rows[0]["Remarks"].ToString();
                ProfileCode = Convert.ToInt32(objsdr.Rows[0]["ProfileCode"]);
                TransDate = Convert.ToString(objsdr.Rows[0]["TransDate"]);
                ChallanNo = Convert.ToInt32(objsdr.Rows[0]["ChallanNo"]);
                Tcode = Convert.ToString(objsdr.Rows[0]["Tcode"]);
                Status = Convert.ToString(objsdr.Rows[0]["Status"]);

                if (objsdr.Rows[0]["CIN"] != DBNull.Value)
                {
                    CIN = Convert.ToString(objsdr.Rows[0]["CIN"]);
                }
                if (objsdr.Rows[0]["Ref"] != DBNull.Value)
                {
                    Ref = Convert.ToString(objsdr.Rows[0]["Ref"]);
                }
                //start DMFT
                PDAccName = objsdr.Rows[0]["PdAccName"].ToString();
                //End DMFT
                PDacc = Convert.ToInt32(objsdr.Rows[0]["PdAcc"]);
                DivCode = Convert.ToInt32(objsdr.Rows[0]["DivCode"]);
                Details = Convert.ToString(objsdr.Rows[0]["Details"]);

                MobileNo = objsdr.Rows[0]["MobileNo"].ToString();
                output = 1;

            }
            else
            {
                output = 0;
            }

            return output;

        }

        /// <summary>
        /// fill the Schema and Amount for user 
        /// </summary>
        /// <param name="grd"></param>
        public DataTable fillChallan()
        {
            DataTable dt = new DataTable();
            SqlParameter[] PARM = new SqlParameter[2];
            PARM[0] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
            PARM[1] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
          
            return gf.Filldatatablevalue(PARM, "EgGRNSchemas",dt,null);
            
        }
        /// <summary>
        /// CHANGE FOR DATATABLE 
        /// </summary>
        /// <returns></returns>
        public DataTable fillSchemaChallan()
        {
            DataTable dt = new DataTable();
            SqlParameter[] PARM = new SqlParameter[2];
            PARM[0] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
            PARM[1] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
            return gf.Filldatatablevalue(PARM, "EgGRNSchemas", dt, null);
        }
        ///// <summary>
        ///// Not in use
        ///// </summary>
        ///// <returns></returns>
        //public DataTable fillChallan()
        //{
        //    SqlParameter[] PARM = new SqlParameter[3];
        //    PARM[0] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
        //    PARM[1] = new SqlParameter("@type", SqlDbType.Int) { Value = 3 };
        //    PARM[2] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
        //    DataTable dt = new DataTable();
        //    return gf.Filldatatablevalue(PARM, "EgEChallan", dt, null);

        //}
        /// <summary>
        /// Get Schema on Challan page when we repeat and Update GRN number
        /// </summary>
        /// <returns></returns>
        //public DataTable FillChallanSchema()
        //{
        //    SqlParameter[] PARM = new SqlParameter[2];
        //    PARM[0] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
        //    PARM[1] = new SqlParameter("@GRN", SqlDbType.Int) { Value = GRNNumber };
        //    DataTable dt = new DataTable();
        //    return gf.Filldatatablevalue(PARM, "EgChallanSchema", dt, null);
        //}
        /// <summary>
        /// Update Challan /GRN 
        /// </summary>
        /// <returns></returns>
        //public int EgUpdateEChallan()
        //{
        //    GenralFunction gf = new GenralFunction();
        //    SqlParameter[] PARM = new SqlParameter[28];
        //    PARM[0] = new SqlParameter("@Profile", SqlDbType.Int) { Value = Profile };
        //    PARM[1] = new SqlParameter("@Identity", SqlDbType.VarChar, 50) { Value = Identity };
        //    PARM[2] = new SqlParameter("@OfficeName", SqlDbType.Int) { Value = OfficeName };
        //    PARM[3] = new SqlParameter("@PANNumber", SqlDbType.Char, 10) { Value = PanNumber };
        //    PARM[4] = new SqlParameter("@Location", SqlDbType.Char, 4) { Value = Location };
        //    PARM[5] = new SqlParameter("@FullName", SqlDbType.VarChar, 50) { Value = FullName };
        //    PARM[6] = new SqlParameter("@ChallanYear", SqlDbType.Char, 4) { Value = ChallanYear };
        //    PARM[7] = new SqlParameter("@ChallanFromMonth", SqlDbType.SmallDateTime) { Value = ChallanFromMonth };
        //    PARM[8] = new SqlParameter("@ChallanToMonth", SqlDbType.SmallDateTime) { Value = ChallanToMonth };
        //    PARM[9] = new SqlParameter("@City", SqlDbType.VarChar, 40) { Value = City };
        //    PARM[10] = new SqlParameter("@Address", SqlDbType.VarChar, 100) { Value = Address };
        //    PARM[11] = new SqlParameter("@PINCode", SqlDbType.Char, 6) { Value = PINCode };
        //    PARM[12] = new SqlParameter("@DeductCommission", SqlDbType.Money) { Value = DeductCommission };
        //    PARM[13] = new SqlParameter("@TotalAmount", SqlDbType.Money) { Value = TotalAmount };
        //    PARM[14] = new SqlParameter("@ChequeDDNo", SqlDbType.VarChar, 10) { Value = ChequeDDNo };
        //    PARM[15] = new SqlParameter("@BankName", SqlDbType.Char, 7) { Value = BankName };
        //    PARM[16] = new SqlParameter("@Paymenttype", SqlDbType.Char, 1) { Value = TypeofPayment };
        //    PARM[17] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
        //    PARM[18] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
        //    PARM[19] = new SqlParameter("@Remarks", SqlDbType.VarChar, 200) { Value = Remark };
        //    PARM[20] = new SqlParameter("@Zone", SqlDbType.Char, 4) { Value = Zone };
        //    PARM[21] = new SqlParameter("@Circle", SqlDbType.Char, 4) { Value = Circle };
        //    PARM[22] = new SqlParameter("@Ward", SqlDbType.Char, 4) { Value = Ward };
        //    PARM[23] = new SqlParameter("@Details", SqlDbType.VarChar, 8000) { Value = Details };
        //    PARM[24] = new SqlParameter("@PdAcc", SqlDbType.Int) { Value = PDacc };
        //    PARM[25] = new SqlParameter("@DivCode", SqlDbType.Int) { Value = DivCode };
        //    PARM[26] = new SqlParameter("@MobileNo", SqlDbType.VarChar, 15) { Value = MobileNo };
        //    PARM[27] = new SqlParameter("@mytable", SqlDbType.Structured) { Value = dtSchema };
        //    int Rv = gf.UpdateData(PARM, "EgUpdateEChallan");

        //    if (Rv == 0)
        //    {
        //        Rv = -1;
        //    }
        //    else
        //    {
        //        return Rv;
        //    }
        //    return Rv;
        //}

        public DataTable GetSchemas()
        {
            DataTable dt = new DataTable();
            SqlParameter[] PARM = new SqlParameter[3];
            PARM[0] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
            PARM[1] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
            PARM[2] = new SqlParameter("@type", SqlDbType.Int) { Value = 0 };
            return gf.Filldatatablevalue(PARM, "EgChallanSchema", dt, null);

        }
        /// <summary>
        /// Get UserID by GRN
        /// </summary>
        /// <param return > UserID</param> 
        //public string GetGrnUserID(SqlTransaction Trans)
        //{
        //    SqlParameter[] PARM = new SqlParameter[1];
        //    PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
        //    string user = gf.ExecuteScaler(PARM, "EgGetGrnUserID", Trans);
        //    return user;
        //}

        //public void fillChallanRpt(GridView grd)
        //{
        //    SqlParameter[] PARM = new SqlParameter[2];
        //    PARM[0] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
        //    PARM[1] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };

        //    gf.FillGridViewControl(grd, PARM, "EgGRNSchemasRpt");
        //}

        /// <summary>
        /// Creating the manual challan view
        /// </summary>
        /// <returns></returns>
        public int ManualChallanView()
        {
            var dt = new DataTable();
            var parm = new SqlParameter[1];
            parm[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
            dt = gf.Filldatatablevalue(parm, FormId.Trim() == "1".Trim() ? "EgEchallanViewRpt" : "EgEchallanViewRptGuest", dt, null);

            int output;
            if (dt.Rows.Count != 0)
            {
                GRNNumber = Convert.ToInt64(dt.Rows[0]["GRN"]);
                PdaccountNumber = dt.Rows[0]["pdacc"].ToString();
                ChallanDate = dt.Rows[0]["ChallanDate"].ToString();
                Office = dt.Rows[0]["Office"].ToString();
                Identity = dt.Rows[0]["Identity"].ToString();
                PanNumber = dt.Rows[0]["PANNumber"].ToString();
                TreasuryCode = dt.Rows[0]["Location"].ToString();
                FullName = dt.Rows[0]["FullName"].ToString();
                ChallanYear = dt.Rows[0]["ChallanYear"].ToString();
                ChallanFromMonth = Convert.ToDateTime(dt.Rows[0]["ChallanFromMonth"]);
                ChallanToMonth = Convert.ToDateTime(dt.Rows[0]["ChallanToMonth"]);
                Address = dt.Rows[0]["Address"].ToString();
                PINCode = dt.Rows[0]["PinCode"].ToString();
                DeductCommission = Convert.ToDouble(dt.Rows[0]["DeductCommission"]);
                TotalAmount = dt.Rows[0]["TotalAmount"].ToString();
                ChequeDDNo = dt.Rows[0]["ChequeDDNo"].ToString();
                BankName = dt.Rows[0]["BankName"].ToString();
                Remark = dt.Rows[0]["Remarks"].ToString();
                TransDate = Convert.ToString(dt.Rows[0]["TransDate"]);
                TreasuryName = dt.Rows[0]["TreasuryName"].ToString();

                //F1 v1 F2 V2 F3 V3
                if (dt.Rows[0]["CIN"] != DBNull.Value)
                {
                    CIN = Convert.ToString(dt.Rows[0]["CIN"]);
                }
                if (dt.Rows[0]["REF"] != DBNull.Value)
                {
                    Ref = Convert.ToString(dt.Rows[0]["REF"]);
                }
                Ddocode = dt.Rows[0]["ddocode"].ToString();
                DdoName = dt.Rows[0]["DDOName"].ToString();
                F1 = dt.Rows[0]["F1"].ToString();
                F2 = dt.Rows[0]["F2"].ToString();
                F3 = dt.Rows[0]["F3"].ToString();
                V3 = dt.Rows[0]["V3"].ToString();
                //Department = dt.Rows[0]["DeptName"].ToString();
                output = 1;
            }
            else
            {
                output = 0;
            }

            return output;
        }




        /// <summary>
        /// Fill Bank DropDown in case of Manual Challan Entry
        /// </summary>
        /// <param name="ddl">BankName,BSRCode</param>
        public void FillBanks(DropDownList ddl)
        {
            SqlParameter[] PARM = new SqlParameter[2];
            PARM[0] = new SqlParameter("@treasurycode", SqlDbType.Char, 4) { Value = TreasuryCode };
            PARM[1] = new SqlParameter("@type", SqlDbType.Char, 4) { Value = Type };
            gf.FillListControl(ddl, "EgGetBanksList", "BankName", "BSRCode", PARM);

            ddl.Items.Insert(0, new ListItem("--Select Bank--", "0"));
        }

        ///<summary>
        ///  Fill Bank List For Minus Challan
        /// </summary>
        /// 

        public void FillBanks_Minus(DropDownList ddl)
        {
            SqlParameter[] PARM = new SqlParameter[2];
            PARM[0] = new SqlParameter("@treasurycode", SqlDbType.Char, 4) { Value = TreasuryCode };
            PARM[1] = new SqlParameter("@type", SqlDbType.Char, 4) { Value = Type };
            gf.FillListControl(ddl, "[EgGetBanksList_Minus]", "BankName", "BSRCode", PARM);

            ddl.Items.Insert(0, new ListItem("--Select Bank--", "0"));
        }





        /// <summary>
        /// Check Enter PD Account Exist and Not
        /// </summary>
        /// <returns>1,0</returns>
        public string CheckExistPDacc()
        {
            SqlDataReader dr;
            string Result = "";
            SqlParameter[] PM = new SqlParameter[1];
            PM[0] = new SqlParameter("@PDacc", SqlDbType.Int) { Value = PDacc };

            dr = gf.FillDataReader(PM, "EgCheckExistPDaccount");
            if (dr.HasRows != false)
            {
                if (dr.Read())
                {
                    Result = dr[0].ToString();

                }

            }
            else
            {
                Result = "0";
                return Result;
            }
            dr.Close();
            dr.Dispose();
            return Result.ToString();
        }
        /// <summary>
        /// Get Payment Type 
        /// </summary>
        /// <returns>M/N</returns>
        public string GetPaymentType()
        {

            SqlDataReader dr;
            string Result = "";
            SqlParameter[] PM = new SqlParameter[1];
            PM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };

            dr = gf.FillDataReader(PM, "EgGetUserPaymentType");
            if (dr.HasRows != false)
            {
                if (dr.Read())
                {
                    Result = dr[0].ToString();

                }

            }
            else
            {
                Result = "0";
                return Result;
            }
            dr.Close();
            dr.Dispose();
            return Result.ToString();
        }
        /// <summary>
        /// Check GRN number has Extra Details and not
        /// </summary>
        /// <returns>1,0</returns>
        public int CheckExistGrnExtraDetails()
        {
            int result = 0;
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
            SqlDataReader dr = gf.FillDataReader(PARM, "EgCheckExistGrnExtraDetails");
            if (dr.HasRows)
            {
                dr.Read();
                result = int.Parse(dr[0].ToString().Trim());
                //dr.Close();
                //dr.Dispose();
            }
            dr.Close();
            dr.Dispose();
            return result;
        }

        public DataTable EChallanViewRpt()
        {
            DataTable objsdr = new DataTable();
            if (UserId == 73 || UserType == "4" || UserType == "2")
            {
                SqlParameter[] PARM = new SqlParameter[1];
                PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
                objsdr = gf.Filldatatablevalue(PARM, "EgEchallanViewRptGuest", objsdr, null);
            }
            else
            {
                SqlParameter[] PARM = new SqlParameter[1];
                PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
                objsdr = gf.Filldatatablevalue(PARM, "EgEchallanViewRpt", objsdr, null);
            }
            return objsdr;
        }

        /// <summary>
        /// PDF Work
        /// </summary>
        /// <returns></returns>
        public DataTable EChallanViewPDF()
        {
            DataTable dt1 = new DataTable();
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
            dt1 = gf.Filldatatablevalue(PARM, "[EgPDFchallanViewrpt]", dt1, null);
            return dt1;
        }
        /// <summary>
        /// Sub PDF Work
        /// </summary>
        /// <returns></returns>
        public DataTable EChallanViewSubRptPDF()
        {
            DataTable dt1 = new DataTable();
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
            dt1 = gf.Filldatatablevalue(PARM, "EgPDFchallanViewsubrpt", dt1, null);
            return dt1;
        }
        public DataTable GetOfficeList()
        {
            DataTable dt = new DataTable();
            SqlParameter[] PARM = new SqlParameter[2];
            PARM[0] = new SqlParameter("@DeptCode", SqlDbType.Int) { Value = DeptCode };
            PARM[1] = new SqlParameter("@TreasuryCode", SqlDbType.VarChar, 50) { Value = TreasuryCode };

            dt = gf.Filldatatablevalue(PARM, "EgFillOfficeList", dt, null);
            return dt;
        }
        /// <summary>
        /// Get  Location List 
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetLocation()
        {
            DataTable dt = new DataTable();
            dt = gf.Filldatatablevalue(null, "EgFillLocation", dt, null);
            return dt;
        }
        /// <summary>
        /// fill Bank Droddownlist
        /// </summary>
        /// <param name=""></param>
        public void GetPdAccountList(DropDownList ddl)
        {
            SqlParameter[] PARM = new SqlParameter[3];
            DataTable dt = new DataTable();
            PARM[0] = new SqlParameter("@BudgetHead", SqlDbType.Char, 13) { Value = BudgetHead };
            PARM[1] = new SqlParameter("@TreasuryCode", SqlDbType.Char, 4) { Value = TreasuryCode };
            PARM[2] = new SqlParameter("@OfficeId", SqlDbType.Int) { Value = OfficeName };
            gf.Filldatatablevalue(PARM, "EgGetPdAccountList", dt, null);
            if (dt.Rows.Count > 1)
            {
                ddl.DataSource = dt;
                ddl.DataTextField = dt.Columns[0].ToString();
                ddl.DataValueField = dt.Columns[1].ToString();
                ddl.DataBind();
            }
            else
            {
                ddl.Items.Clear();
                ddl.Items.Insert(0, new ListItem("--- Select ---", "0"));
            }
            //gf.FillListControl(ddl, "EgGetPdAccountList", "PDAccName", "PDAccNo", PARM);
            //ddl.Items.Insert(0, new ListItem("--Select PD Account --", "0"));
        }

        /// <summary>
        ///  11 Aug profil lsit
        /// </summary>
        /// <param name="ddl"></param>
        //public override void GetProfileList(DropDownList ddl)
        public void GetProfileList(DropDownList ddl)
        {

            gf = new GenralFunction();
            SqlParameter[] PM = new SqlParameter[1];
            PM[0] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
            gf.FillListControl(ddl, "EgGetProfileList", "UserProfile", "UserPro", PM);
            ddl.Items.Insert(0, new ListItem("--Select Profile--", "0"));
        }
        #endregion
        /// <summary>
        /// Get UserId and UserType 
        /// </summary>
        /// <returns>UserId/UserType</returns>
        public void GetGrnUserIdAndType()
        {
            SqlDataReader dr;
            SqlParameter[] PM = new SqlParameter[1];
            PM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
            dr = gf.FillDataReader(PM, "EgGetGrnUserIdAndType");
            if (dr.HasRows != false)
            {
                if (dr.Read())
                {
                    UserId = Convert.ToInt32(dr["UserID"].ToString());
                    UserType = dr["UserType"].ToString();

                }

            }
            dr.Close();
            dr.Dispose();
        }


        /////
        //Create Challan With Service   18 july 2018

        ///


        public DataTable GetServiceSchema(int ServiceId, int DeptCode)
        {
            DataTable dt = new DataTable();
            SqlParameter[] PM = new SqlParameter[3];
            PM[0] = new SqlParameter("@DeptCode", SqlDbType.Int) { Value = DeptCode };
            PM[1] = new SqlParameter("@ServiceId", SqlDbType.Int) { Value = ServiceId };
            PM[2] = new SqlParameter("@UserType", SqlDbType.Int) { Value = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserType"]) };
            return gf.Filldatatablevalue(PM, "EgGetServiceSchema", dt, null);
        }






        /// <summary>
        /// Checks mapping of budgetheads with departments
        /// </summary>
        /// <returns></returns>
        public int CheckBudgetHeadWithDept()
        {
            //SqlDataReader dr;
            GenralFunction gf = new GenralFunction();
            SqlParameter[] PM = new SqlParameter[1];
            PM[0] = new SqlParameter("@mytable", SqlDbType.Structured) { Value = dtSchema };
            DataTable dt = new DataTable();
            dt = gf.Filldatatablevalue(PM, "EgCheckBudgetHeadWithDept", dt, null);
            return Convert.ToInt32(dt.Rows[0][0]);
            //return Convert.ToInt32(gf.ExecuteScaler(PM, "EgCheckBudgetHeadWithDept"));
        }
        /// <summary>
        ///  gets multiple office details
        /// </summary>
        /// <returns></returns>
        public DataTable GetOfficeDetails()
        {
            DataTable dt = new DataTable();
            SqlParameter[] PARM = new SqlParameter[2];
            PARM[0] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
            PARM[1] = new SqlParameter("@UserId", SqlDbType.Int) { Value = 73 };
            dt = gf.Filldatatablevalue(PARM, "EgEChallanOfficeSchema", dt, null);
            return dt;
        }
        /// <summary>
        /// fill Treasury with JSON
        /// </summary 29 may 2019>
        /// <param name="ddl"></param>
        /// <summary>
        /// fill Treasury with JSON
        /// </summary>
        /// <param name="ddl"></param>
        public string FillTreasury()
        {
            gf = new GenralFunction();
            DataTable dt = new DataTable();
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@tcode", SqlDbType.Char, 4) { Value = Tcode };
            gf.Filldatatablevalue(PARM, "EgFillTreasuryTY11", dt, null);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(dt);
            return JSONString;

        }


        /// <summary>
        ///  gets GRN OR Challan Detail For Challan View
        /// </summary>
        /// <returns></returns>
        public DataTable GetGrnOrChallanDetail()
        {
            DataTable dt = new DataTable();
            SqlParameter[] PARM = new SqlParameter[4];
            PARM[0] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
            PARM[1] = new SqlParameter("@GRN", SqlDbType.BigInt) { Value = GRNNumber };
            PARM[2] = new SqlParameter("@Usertype", SqlDbType.VarChar, 3) { Value = UserType };
            PARM[3] = new SqlParameter("@ChallanNo", SqlDbType.Int) { Value = ChallanNo };

            dt = gf.Filldatatablevalue(PARM, "EgeChallanView_ScrollDate", dt, null);
            return dt;
        }

        /// <summary>
        /// Create Method  Forward Challan String to Bank Replace old Process 23 July 2021
        /// </summary>
        /// <returns></returns>

        public DataTable GetBankForwardDetail()
        {
            DataTable dt = new DataTable();

            SqlParameter[] PARM = new SqlParameter[2];
            PARM[0] = new SqlParameter("@Grn", SqlDbType.BigInt) { Value = GRNNumber };
            PARM[1] = new SqlParameter("@UserId", SqlDbType.BigInt) { Value = UserId };
            return gf.Filldatatablevalue(PARM, "EgBankforwardData", dt, null);


        }


    }
}
