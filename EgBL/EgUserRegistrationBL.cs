using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
namespace EgBL
{
    public class EgUserRegistrationBL
    {
        GenralFunction gf;
        //SqlDataReader dr;
        #region Properties
        public string LoginID { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TreasuryCode { get; set; }
        public string City { get; set; }
        public int State { get; set; }
        public int Country { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string Address { get; set; }
        public string VerificationCode { get; set; }
        public string Dept { get; set; }
        public string PinCode { get; set; }
        public string Identity { get; set; }
        public string CinNo { get; set; }
        public string Code { get; set; }
        public string UserType { get; set; }
        public string AttemptNumber { get; set; }
        public int UserId { get; set; }
        public int Type { get; set; }
        public int DeptCode { get; set; }
        public DateTime DOB { get; set; }
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public string DateOfbirth { get; set; }
        public string OTP { get; set; }
        public string VCode { get; set; }

        public int OfficeId { get; set; }
        //public string MaritalStatus { get; set; }
        #endregion
        #region Method
        /// <summary>
        /// use for fill district dropdownlist
        /// </summary>
        /// <param name="ddl"></param>
        public void FillStateList(DropDownList ddl)
        {
            gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[1];
            gf.FillListControl(ddl, "EgGetState", "State", "id", PARM);
            ddl.Items.Insert(0, new ListItem("--Select State--", "0"));
        }
        /// <summary>
        /// use for fill district dropdownlist
        /// </summary>
        /// <param name="ddl"></param>
        public void FillDistrictList(DropDownList ddl)
        {
            gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[1];
            gf.FillListControl(ddl, "EgFillDistrictList", "DistrictName", "DistrictCode", PARM);
            ddl.Items.Insert(0, new ListItem("--Select City--", "0"));
        }
        /// <summary>
        ///  Get Sub Treasury List Treasury Wise
        /// </summary>
        /// <param name="ddl"></param>
        public void FillSubTreasuryList(DropDownList ddl)
        {
            gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@TreasuryCode", SqlDbType.Char, 4) { Value = TreasuryCode };
            gf.FillListControl(ddl, "EgGetSubTreasuryList", "TreasuryName", "TreasuryCode", PARM);
            ddl.Items.Insert(0, new ListItem("--Select Sub-Treasury--", "0"));
        }
        // Get securty Question 
        public void GetSecurityQuestion(DropDownList ddl) // fill Bank Droddownlist
        {
            gf = new GenralFunction();
          
            gf.FillListControl(ddl, "EgSecQuestionList", "Question", "QuestionId", null);
            ddl.Items.Insert(0, new ListItem("--Select Question--", "0"));
        }
        // Get Security Questions and answers
        public string GetQuestionAndAnswer()
        {
            gf = new GenralFunction();
            string Question = "";
            SqlParameter[] PM = new SqlParameter[1];
            DataTable dt = new DataTable();
            PM[0] = new SqlParameter("@QuestionId", SqlDbType.Int);
            PM[0].Value = QuestionId;
            Question = gf.ExecuteScaler(PM, "EgRandomQuestion");
            return Question;
        }
        /// <summary>
        /// fill Gridview for show Department list in checkbox  
        /// </summary>
        /// <param name="rpt"></param>
        public DataTable  FillDepartmentGrid()
        {
            DataTable dt = new DataTable();
            gf = new GenralFunction();
            SqlParameter[] PM = new SqlParameter[1];
            return  gf.Filldatatablevalue(PM, "EgGetDeptList", dt, null);
           
        }
        public string FillDepartmentGridUserWise()
        {
            gf = new GenralFunction();
            SqlParameter[] PM = new SqlParameter[1];
            PM[0] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
            return gf.ExecuteScaler(PM, "EgGetDeptUserWise");

        }
        /// <summary>
        /// Check User Already exist and not 
        /// </summary>
        /// <returns></returns>
        public int CheckExistingLogin()
        {
            //DataTable dt = new DataTable();
            string result = ""; ;
            gf = new GenralFunction();
            SqlParameter[] PM = new SqlParameter[1];
            PM[0] = new SqlParameter("@LoginID", SqlDbType.VarChar, 20) { Value = LoginID };
            result = gf.ExecuteScaler(PM, "EgCheckExistingLogin");
            if (result.Trim() == "")
                return 0;
            else
                return Convert.ToInt32(result);
        }
        
       


        /// <summary>
        /// Check Office Login 26 August 2021 Add Off with LoginId
        /// </summary>
        /// <returns></returns>
        public int CheckExistingOfficeLogin()
        {
            //DataTable dt = new DataTable();
            string result = ""; ;
            gf = new GenralFunction();
            SqlParameter[] PM = new SqlParameter[2];
            PM[0] = new SqlParameter("@LoginID", SqlDbType.VarChar, 20) { Value = LoginID };
            PM[1] = new SqlParameter("@OfficeId", SqlDbType.Int) { Value = OfficeId };
            result = gf.ExecuteScaler(PM, "EgCheckExistingOfficeLogin");
            if (result.Trim() == "")
                return 0;
            else
                return Convert.ToInt32(result);
        }



        /// <summary>
        /// Insert User Info 
        /// </summary>
        /// <returns>1 for successfull insert and 0 for not insert </returns>
        public string InsertUserData1()
        {   string a="";
            gf = new GenralFunction();
            SqlParameter[] PM = new SqlParameter[21];
            PM[0] = new SqlParameter("@LoginID", SqlDbType.VarChar, 20) { Value = LoginID };
            PM[1] = new SqlParameter("@Dept", SqlDbType.VarChar, 1000) { Value = Dept };
            PM[2] = new SqlParameter("@FirstName", SqlDbType.VarChar, 50) { Value = FirstName };
            PM[3] = new SqlParameter("@LastName", SqlDbType.VarChar, 50) { Value = LastName };
            PM[4] = new SqlParameter("@DOB", SqlDbType.DateTime) { Value = DOB };
            PM[5] = new SqlParameter("@Address", SqlDbType.VarChar, 100) { Value = Address };
            PM[6] = new SqlParameter("@City", SqlDbType.VarChar, 40) { Value = City };
            PM[7] = new SqlParameter("@State", SqlDbType.Int) { Value = State };
            PM[8] = new SqlParameter("@Country", SqlDbType.Int) { Value = Country };
            PM[9] = new SqlParameter("@MobilePhone", SqlDbType.VarChar, 10) { Value = MobilePhone };
            PM[10] = new SqlParameter("@PinCode", SqlDbType.Char, 6) { Value = PinCode };
            PM[11] = new SqlParameter("@Email", SqlDbType.VarChar, 50) { Value = Email };
            PM[12] = new SqlParameter("@Password", SqlDbType.VarChar, 250) { Value = Password };
            PM[13] = new SqlParameter("@VerificationCode", SqlDbType.VarChar, 50) { Value = VerificationCode };
            PM[14] = new SqlParameter("@AttemptNumber", SqlDbType.VarChar, 150) { Value = AttemptNumber };
            PM[15] = new SqlParameter("@Identity", SqlDbType.VarChar, 50) { Value = Identity };
            PM[16] = new SqlParameter("@UserType", SqlDbType.TinyInt) { Value = UserType };
            PM[17] = new SqlParameter("@QuestionId", SqlDbType.Int) { Value = QuestionId };
            PM[18] = new SqlParameter("@Question", SqlDbType.NVarChar, 100) { Value = Question };
            PM[19] = new SqlParameter("@VCode", SqlDbType.Char, 6) { Value = VCode };
            PM[20] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
            PM[20].Direction = ParameterDirection.Output;
            PM[19].Direction = ParameterDirection.Output;
            int x = (gf.UpdateData(PM, "EgUserRegistrationWithMobileNo"));
           
            if (Convert.ToInt32(PM[19].Value.ToString()) > 0)
            {
                a = PM[19].Value.ToString();
                UserId = Convert.ToInt32(PM[20].Value);
            }
            else
                a = PM[19].Value.ToString();

            return a.Trim().ToString();
        }
        public int InsertUserData()
        {
            gf = new GenralFunction();
            SqlParameter[] PM = new SqlParameter[19];
            PM[0] = new SqlParameter("@LoginID", SqlDbType.VarChar, 20) { Value = LoginID };
            PM[1] = new SqlParameter("@Dept", SqlDbType.VarChar, 1000) { Value = Dept };
            PM[2] = new SqlParameter("@FirstName", SqlDbType.VarChar, 50) { Value = FirstName };
            PM[3] = new SqlParameter("@LastName", SqlDbType.VarChar, 50) { Value = LastName };
            PM[4] = new SqlParameter("@DOB", SqlDbType.DateTime) { Value = DOB };
            PM[5] = new SqlParameter("@Address", SqlDbType.VarChar, 100) { Value = Address };
            PM[6] = new SqlParameter("@City", SqlDbType.VarChar, 40) { Value = City };
            PM[7] = new SqlParameter("@State", SqlDbType.Int) { Value = State };
            PM[8] = new SqlParameter("@Country", SqlDbType.Int) { Value = Country };
            PM[9] = new SqlParameter("@MobilePhone", SqlDbType.VarChar, 10) { Value = MobilePhone };
            PM[10] = new SqlParameter("@PinCode", SqlDbType.Char, 6) { Value = PinCode };
            PM[11] = new SqlParameter("@Email", SqlDbType.VarChar, 50) { Value = Email };
            PM[12] = new SqlParameter("@Password", SqlDbType.VarChar, 250) { Value = Password };
            PM[13] = new SqlParameter("@VerificationCode", SqlDbType.VarChar, 50) { Value = VerificationCode };
            PM[14] = new SqlParameter("@AttemptNumber", SqlDbType.VarChar, 150) { Value = AttemptNumber };
            PM[15] = new SqlParameter("@Identity", SqlDbType.VarChar, 50) { Value = Identity };
            PM[16] = new SqlParameter("@UserType", SqlDbType.TinyInt) { Value = UserType };
            PM[17] = new SqlParameter("@QuestionId", SqlDbType.Int) { Value = QuestionId };
            PM[18] = new SqlParameter("@Question", SqlDbType.NVarChar, 100) { Value = Question };
            int x = gf.UpdateData(PM, "EgUserRegistration");
            return x;
        }
        /// <summary>
        /// after userinsert info get userid for insert
        ///  in usertypecode
        /// </summary>
        /// <returns> userid</returns>
        public int GetUserId()
        {
            gf = new GenralFunction();
            SqlParameter[] PM = new SqlParameter[1];
            PM[0] = new SqlParameter("@LoginID", SqlDbType.VarChar, 50) { Value = LoginID };
            UserId = Convert.ToInt32(gf.ExecuteScaler(PM, "EgGetLoginUserID"));
           
            return UserId;
        }
        public int UpdateUserData()
        {
            gf = new GenralFunction();
            SqlParameter[] PM = new SqlParameter[14];
            PM[0] = new SqlParameter("@FirstName", SqlDbType.VarChar, 50) { Value = FirstName };
            PM[1] = new SqlParameter("@LastName", SqlDbType.VarChar, 50) { Value = LastName };
            PM[2] = new SqlParameter("@DOB", SqlDbType.DateTime) { Value = DOB };
            PM[3] = new SqlParameter("@Address", SqlDbType.VarChar, 100) { Value = Address };
            PM[4] = new SqlParameter("@City", SqlDbType.VarChar, 40) { Value = City };
            PM[5] = new SqlParameter("@State", SqlDbType.Int) { Value = State };
            PM[6] = new SqlParameter("@Country", SqlDbType.Int) { Value = Country };
            PM[7] = new SqlParameter("@MobilePhone", SqlDbType.VarChar, 10) { Value = MobilePhone };
            PM[8] = new SqlParameter("@PinCode", SqlDbType.Char, 6) { Value = PinCode };
            PM[9] = new SqlParameter("@Email", SqlDbType.VarChar, 50) { Value = Email };
            PM[10] = new SqlParameter("@Identity", SqlDbType.VarChar, 50) { Value = Identity };
            PM[11] = new SqlParameter("@QuestionId", SqlDbType.Int) { Value = QuestionId };
            PM[12] = new SqlParameter("@Quention ", SqlDbType.NVarChar, 100) { Value = Question };
            PM[13] = new SqlParameter("@userId", SqlDbType.Int) { Value = UserId };
            int x = gf.UpdateData(PM, "EgEditProfile");
            return x;
        }
        public void EditData()
        {
            gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };

            SqlDataReader dr = gf.FillDataReader(PARM, "EgGetUserEditDetail");
            if (dr.HasRows)
            {
                dr.Read();
                FirstName = dr["FirstName"].ToString();
                LastName = dr["LastName"].ToString();

                DateOfbirth = dr["DOB"].ToString();

                Email = dr["Email"].ToString();
                Address = dr["Address"].ToString();
                if (dr["Country"].ToString() == "") { Country = 0; }
                else { Country = Convert.ToInt32(dr["Country"].ToString()); }
                if (dr["State"].ToString() == "") { State = 0; }
                else { State = Convert.ToInt32(dr["State"].ToString()); }
                if (dr["City"].ToString() == "") { City = ""; }
                else { City = dr["City"].ToString().Trim(); }
                MobilePhone = dr["MobilePhone"].ToString();
                Identity = dr["Identity"].ToString();
                if (dr["QuestionId"].ToString() == "") { QuestionId = 0; }
                else { QuestionId = Convert.ToInt32(dr["QuestionId"].ToString()); }

                Question = dr["Question"].ToString();
                PinCode = dr["PinCode"].ToString();
                dr.Close();
                dr.Dispose();
            }
            else
            {
                dr.Close();
                dr.Dispose();
            }
        }
        /// <summary>
        /// Get user Office Id from Identity Column of Office In Case of create sub Office
        /// </summary>
        /// <returns> OfficeID</returns>
        public string GetOfficeID()
        {
            gf = new GenralFunction();
            string UserIdentity = "";
            SqlParameter[] PM = new SqlParameter[1];
            PM[0] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
            UserIdentity = gf.ExecuteScaler(PM, "EgGetUserOfficeID");
            return UserIdentity.Trim();
        }
        /// <summary>
        /// Get Usre Personal details for send Mobile message
        /// </summary>
        public void GetUserVerifyDetails()
        {
            gf = new GenralFunction();
            SqlParameter[] PARM = new SqlParameter[1];
            PARM[0] = new SqlParameter("@UserId", SqlDbType.BigInt) { Value = UserId };

            SqlDataReader dr = gf.FillDataReader(PARM, "EgGetUserVerificationDetails");
            if (dr.HasRows)
            {
                dr.Read();
                FirstName = dr["FullName"].ToString();
                MobilePhone = dr["MobilePhone"].ToString();
                dr.Close();
                dr.Dispose();
            }
            else
            {
                dr.Close();
                dr.Dispose();
            }
        }
        public string VerifyMobile()
        {
            gf = new GenralFunction();
            SqlParameter[] PM = new SqlParameter[2];
            PM[0] = new SqlParameter("@VerifyCode", SqlDbType.Char, 6) { Value = OTP };
            PM[1] = new SqlParameter("@UserID", SqlDbType.Int) { Value =UserId };
            string res = gf.ExecuteScaler(PM, "EgVerifySMSCode_App");
            return res;
        }
        //public string VerifyMobileEdit()
        //{
        //    gf = new GenralFunction();
        //    SqlParameter[] PM = new SqlParameter[2];
        //    PM[0] = new SqlParameter("@VerifyCode", SqlDbType.Char, 6) { Value = OTP };
        //    PM[1] = new SqlParameter("@UserId", SqlDbType.BigInt) { Value = UserId };

        //    string res = gf.ExecuteScaler(PM, "EgVerifySMSCode");
        //    return res;
        //}

        //public string ResendCode()
        //{
        //    gf = new GenralFunction();
        //    SqlParameter[] PM = new SqlParameter[1];
        //    PM[0] = new SqlParameter("@MobileNo", SqlDbType.Char,10) { Value = MobilePhone };

        //    string res = gf.ExecuteScaler(PM, "EgMobileNoVerify_App");
        //    return res.Trim();

        //}

        public string ExistMobileNo()
        {
            gf = new GenralFunction();
            SqlParameter[] PM = new SqlParameter[3];
            PM[0] = new SqlParameter("@UserId", SqlDbType.Int) { Value = UserId };
            PM[1] = new SqlParameter("@MobileNo", SqlDbType.Char, 10) { Value = MobilePhone.Trim() };
            PM[2] = new SqlParameter("@UserType", SqlDbType.TinyInt) { Value = UserType };
            string res = gf.ExecuteScaler(PM, "EgMobileNoExist");
            return res.Trim();
        }
        /// <summary>
        /// Get Office Name or department name on Welcome 
        /// </summary>
        /// <returns></returns>
        public string GetOfficeOrDeptName()
        {
            gf = new GenralFunction();
            SqlParameter[] PM = new SqlParameter[2];
            PM[0] = new SqlParameter("@userid", SqlDbType.Int) { Value = UserId };
            PM[1] = new SqlParameter("@usertype", SqlDbType.Char, 10) { Value = UserType };
            string res = gf.ExecuteScaler(PM, "EgGetOfficeOrDeptName");
            return res.Trim();
        }

    }
    #endregion
}
