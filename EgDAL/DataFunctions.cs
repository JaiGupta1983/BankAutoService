using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;

namespace EgDAL
{
    public class DataFunctions 
    {
        SqlConnection con;
       // SqlConnection con1;
        SqlCommand cmd;
        //Boolean TransactionStarted;
        //SqlTransaction Transac;
       

        //public string db_class(String connect)
        //{
        //    con = new SqlConnection(conn_string());
        //    con.Open();
        //    con1 = new SqlConnection(conn_string1());
        //    con1.Open();
        //    return "0";
        //}

        public string db_open()
        {
            if (con == null)
            {
                con = new SqlConnection(conn_string());
            }
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            return "0";
        }

        //public string db_openegram()
        //{
        //    if (con == null)
        //    {
        //        con1 = new SqlConnection(conn_string1());
        //    }
        //    if (con1.State == ConnectionState.Closed)
        //        con1.Open();

        //    return "0";
        //}

        public string db_close()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return "0";
        }

        //public string db_closeegram()
        //{
        //    if (con1.State == ConnectionState.Open)
        //    {
        //        con1.Close();
        //    }
        //    return "0";

        //}

        public string conn_string()
        {
            string connstring;
            connstring=ConfigurationManager.ConnectionStrings["Egras"].ConnectionString;
            return connstring;
        }

        //public string conn_string1()
        //{
        //    string connstring1;
        //    return connstring1 = ConfigurationManager.ConnectionStrings["egram"].ConnectionString;
        //}

        public SqlDataReader FillReader(SqlCommand SQL, string SpName)
        {
            SqlDataReader Reader=null;
            try
            {
              
                    con = new SqlConnection(conn_string());
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    SQL.CommandType = CommandType.StoredProcedure;
                    SQL.CommandText = SpName;
                    SQL.Connection = con;
                     Reader  = SQL.ExecuteReader(CommandBehavior.CloseConnection);
                     
            }
            catch (Exception ex)
            {
                //InsertErr(ex.Message, SpName);

            }
            
            finally
            {
                //con.Close();
                //SQL.Dispose();
                //dr.Close();
            }
           // if (Reader != null)
            return Reader;
           
        }

        public DataSet FillDataSet(SqlCommand SQL, string SpName,string TbName)
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();

            try
            {
                con = new SqlConnection(conn_string());
                SQL.Connection = con;
                SQL.CommandType = CommandType.StoredProcedure;
                SQL.CommandText = SpName;
                da = new SqlDataAdapter(SQL);
                da.Fill(ds,TbName);

            }
            catch (Exception ex)
            {
               // InsertErr(ex.Message, SpName);

            }
            finally
            {
                //SQL.Dispose();
                //con.Close();
            }
            return ds;
        }




        //public void InsertBulk(DataTable dt)
        //{
          
        //    con = new SqlConnection(conn_string());
        //    if (con.State == ConnectionState.Closed)
        //        con.Open();
        //    using (SqlBulkCopy copy = new SqlBulkCopy(con))
        //    {
        //        copy.DestinationTableName = "MappingMajorHeadToAuditor";
        //        copy.ColumnMappings.Add("TreasuryCode", "[TreasuryCode]");
        //        copy.ColumnMappings.Add("auditorcode", "[AuditorCode]");
        //        copy.ColumnMappings.Add("type", "[type]");
        //        copy.ColumnMappings.Add("majorhead", "[MajorHead]");
        //        copy.ColumnMappings.Add("objecthead", "[ObjectHead]");
        //        copy.ColumnMappings.Add("ddocode", "[DDOCode]");
        //        copy.ColumnMappings.Add("isactive", "[isActive]");
        //        copy.ColumnMappings.Add("AssignDate", "[AssignDate]");
        //        copy.WriteToServer(dt);
        //    }
        //    con.Close();
        //}
        // For insert Update and delete


     
        public string UpdateData(SqlCommand SQL, string SpName) 
        {
            try
            {

                SQL.CommandType = CommandType.StoredProcedure;
                SQL.CommandText = SpName;
                con = new SqlConnection(conn_string());
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SQL.Connection = con;
                SQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //InsertErr(ex.Message, SpName);
                return ex.Message;
            }
            finally
            {
                con.Close();
                SQL.Dispose();
               
            }
            
            return "0";
        }

        //public string InsertErr(string msg, string SpName)
        //{
        //    ErrorHandler eh = new ErrorHandler();
        //    string user = "";
        //    try
        //    {
        //        string url = HttpContext.Current.Request.Url.ToString();  // 'Request.Url.ToString()
        //        if (HttpContext.Current.Session["UserId"] == null)
        //        {
        //            user = "Undefine";
        //        }
        //        else
        //        {
        //           user = HttpContext.Current.Session["UserId"].ToString();
        //        }
               
        //        eh.AddError(msg, url, SpName,Convert .ToInt16 (user));
        //    }
        //    catch (Exception ex)
        //    {
        //        InsertErr(ex.Message, SpName);
        //    }
        //    return "0";
        //}

        //public string InsertDataWithTransaction(SqlCommand SQL, string SpName)
        //{
        //    SQL.CommandType = CommandType.StoredProcedure;
        //    SQL.CommandText = SpName;
        //    SQL.Connection = con;
        //    SQL.Transaction = Transac;

        //    try
        //    {
        //        SQL.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (TransactionStarted)
        //        {
        //            Transac.Rollback();
        //            TransactionStarted = false;
        //            InsertErr(ex.Message, SpName);
        //            return ex.Message;
        //        }

        //    }
        //    return "0";

        //}

        public string ExecuteScaler(SqlCommand SQL, string name)
        {
            string strQty;
            try
            {
                con = new SqlConnection(conn_string());
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SQL.Connection = con;
                SQL.CommandType = CommandType.StoredProcedure;
                SQL.CommandText = name;
                strQty = SQL.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                //InsertErr(ex.Message, name);
                return ex.Message;
            }
            finally
            {
                con.Close();
            }
            return strQty;

        }
        //public string db_openWithTransaction()
        //{
        //    if (con == null)
        //    {
        //        con = new SqlConnection(conn_string());
        //    }

        //    if (con.State == ConnectionState.Closed)
        //    {
        //        con.Open();
        //    }

        //    TransactionStarted =false;
        //    Transac = null;

        //    if (!TransactionStarted)
        //    {
        //        Transac = con.BeginTransaction();
        //        TransactionStarted = true;
        //    }
        //    return "0";

        //}

        //public string db_CloseWithTransaction()
        //{
        //    if (con.State == ConnectionState.Open)
        //    {
        //        if (TransactionStarted)
        //        {
        //            Transac.Commit();
        //            Transac = null;
        //            TransactionStarted = false;
        //        }
        //        con.Close();
        //    }
        //    return "0";

        //}
    }
}
