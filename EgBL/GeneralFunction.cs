using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Data.SqlClient;
using EgDAL;
using System.Data;
using DL;

namespace EgBL
{
    public class GenralFunction
    {
       // DataFunctions df = new DataFunctions();
        DataSet ds1;
        DataTable dt1;
        public SqlTransaction Trans;
        SqlCommand cmd = new SqlCommand();
        SqlConnection con = new SqlConnection();
        #region "Filling Gridview Control"
        public void FillGridViewControl(GridView Grd, SqlParameter[] PM, String SpName)
        {
            ds1 = SqlHelper.ExecuteDataset(SqlHelper.conString, CommandType.StoredProcedure, SpName, PM);
            Grd.DataSource = ds1;
            Grd.DataBind();
            ds1.Dispose();
        }
        #endregion


        #region "Filling ListBoxControl Control"
        public void ListBoxControl(ListBox lst, String SpName, string datatext, string datavalue, SqlParameter[] PM)
        {
            dt1 = SqlHelper.ExecuteDataTable(SqlHelper.conString, CommandType.StoredProcedure, SpName, PM);

            lst.Items.Clear();
            lst.DataTextField = datatext;
            lst.DataValueField = datavalue;
            lst.ClearSelection();
            //lst.SelectedValue = "0";
            if (dt1.Rows.Count != 0)
            {
                lst.DataSource = dt1;
                lst.DataBind();
            }
            dt1.Dispose();
        }
        #endregion
        
        #region "Filling Repeater Control"
        public void FillRepeaterControl(Repeater Rept, SqlParameter[] PM, String SpName)
        {
            ds1 = SqlHelper.ExecuteDataset(SqlHelper.conString, CommandType.StoredProcedure, SpName, PM);
            Rept.DataSource = ds1;
            Rept.DataBind();
            ds1.Dispose();
        }
        //public void FillListtControl(ListView Rept, SqlParameter[] PM, String SpName)
        //{
        //    ds1 = SqlHelper.ExecuteDataset(SqlHelper.conString, CommandType.StoredProcedure, SpName, PM);
        //    Rept.DataSource = ds1;
        //    Rept.DataBind();
        //    ds1.Dispose();
        //}
        #endregion
        #region"filll datatable"
        public DataTable Filldatatablevalue(SqlParameter[] PM, string SpName, DataTable dt, string[] TableName)
        {
            SqlHelper.FillDataTable(SqlHelper.conString, CommandType.StoredProcedure, SpName, dt, TableName, PM);

            return dt; 

        }
        #endregion
        #region "Filling ListView Control"
        public void FillListControl(DropDownList lst, String SpName, string datatext, string datavalue, SqlParameter[] PM)
        {
            ds1 = SqlHelper.ExecuteDataset(SqlHelper.conString, CommandType.StoredProcedure, SpName, PM);
            //if (PM == null)
            //{
            //    ds1 = SqlHelper.ExecuteDataset(SqlHelper.conString, CommandType.StoredProcedure, SpName);
            //}
            //else if (PM != null)
            //{
            //    ds1 = SqlHelper.ExecuteDataset(SqlHelper.conString, CommandType.StoredProcedure, SpName, PM);
            //}
            lst.Items.Clear();
            lst.DataTextField = datatext;
            lst.DataValueField = datavalue;
            lst.ClearSelection();
            //lst.SelectedValue = "0";
            if (ds1.Tables[0].Rows.Count != 0)
            {
                lst.DataSource = ds1;
                lst.DataBind();
            }
            ds1.Dispose();
        }
        #endregion
        #region "For Indert,Update and Delete"

        public SqlTransaction BeginTrans1()
        {

            con.ConnectionString = SqlHelper.conString;

            con.Open();
            Trans = con.BeginTransaction();
            return Trans;
        }

        public void EndTrans1()
        {
            Trans.Commit();
            con.Close();
        }


        public void RollBack1()
        {
            Trans.Rollback();
            con.Close();
        }


        public SqlTransaction Begintrans()
        {
            return BeginTrans1();
        }

        public void Endtrans()
        {
            EndTrans1();
        }

        public void Rollaback()
        {
            RollBack1();
        }


        public int UpdateDataTrans(SqlParameter[] PM, string SpName, SqlTransaction Trans)
        {
            int RV = 0;
            //SqlTransaction Trans //= SqlHelper.BeginTrans();
            //con.ConnectionString = df.conn_string();
            //con.Open();
            //cmd.Connection = con;
            //SqlTransaction cmdtrans = con.BeginTransaction();
            //cmd.Transaction = cmdtrans;
            //int re = 0;
            try
            {
                //cmdtrans.Save("sp1");
                RV = SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, SpName, PM);

                //SqlHelper .ExecuteNonQuery(
                //if (RV != 0)
                //{
                //    //Trans.Commit();
                //    //  cmdtrans.Commit();
                //    re = 1;
                //}
                //else
                //{
                //    re = 0;
                //}
                return RV;
            }
            catch (Exception ex)
            {
                //Trans.Rollback();
            }
            finally
            {
                cmd.Dispose();

                con.Close();
            }
            return RV;

        }
        public int UpdateData(SqlParameter[] PM, string SpName)
        {
            //con.ConnectionString = df.conn_string();
            //con.Open();
            //cmd.Connection = con;
            //SqlTransaction cmdtrans = con.BeginTransaction();
            //cmd.Transaction = cmdtrans;
            int re = 0;
            try
            {
                //cmdtrans.Save("sp1");
                int RV = SqlHelper.ExecuteNonQuery(SqlHelper.conString, CommandType.StoredProcedure, SpName, PM);
                //SqlHelper .ExecuteNonQuery(
                if (RV != 0)
                {
                    //  cmdtrans.Commit();
                    re = 1;
                }
                else
                {
                    re = 0;
                }
            }
            catch (Exception ex)
            {
                //cmdtrans.Rollback();
            }
            finally
            {
                cmd.Dispose();

                con.Close();
            }
            return re;
        }
        //public String UpdateDataNew(SqlParameter[] PM, string SpName)
        //{
        //    //con.ConnectionString = df.conn_string();
        //    //con.Open();
        //    //cmd.Connection = con;
        //    //SqlTransaction cmdtrans = con.BeginTransaction();
        //    //cmd.Transaction = cmdtrans;

        //    try
        //    {
        //        //cmdtrans.Save("sp1");
        //        return  SqlHelper.ExecuteNonQuery(SqlHelper.conString, CommandType.StoredProcedure, SpName, PM);
        //        //SqlHelper .ExecuteNonQuery(

        //     }
        //    catch (Exception ex)
        //    {
        //        //cmdtrans.Rollback();
        //    }
        //    finally
        //    {

        //    }

        //}
        #endregion
        #region "Filling DataSet"
        public DataSet Filldatasetvalue(SqlParameter[] PM, string SpName, DataSet ds, string[] TableName)
        {
            SqlHelper.FillDataset(SqlHelper.conString, CommandType.StoredProcedure, SpName, ds, TableName, PM);
            return ds;
        }
        #endregion
        #region "Filling DataReader"
        public SqlDataReader FillDataReader(SqlParameter[] PM, string SpName)
        {
            SqlDataReader dr = SqlHelper.ExecuteReader(SqlHelper.conString, CommandType.StoredProcedure, SpName, PM);
            return dr;
        }
        #endregion
        public string ExecuteScaler(SqlParameter[] PM, string SpName, SqlTransaction Trans)
        {
            return Convert.ToString(SqlHelper.ExecuteScalar(Trans, SpName, PM));
        }
        public string ExecuteScaler(SqlParameter[] PM, string SpName)
        {
            return Convert.ToString(SqlHelper.ExecuteScalar(SqlHelper.conString, SpName, PM));
        }
    }
}





















