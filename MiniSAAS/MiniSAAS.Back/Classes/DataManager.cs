using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace MiniSAAS.Back.Classes
{
    public class DataManager
    {
        public static DataTable GetData(string sql)
        {
            DataTable dt = new DataTable();
            SqlConnection con = null;
            SqlDataAdapter da = null;
            try
            {
                con = new SqlConnection(Constants.CONNECTION_STRING);
                con.Open();
                da = new SqlDataAdapter(sql, con);
                da.Fill(dt);
                da.Dispose();
                con.Close();
                con.Dispose();
            }
            catch (Exception ex)
            {
                da.Dispose();
                con.Close();
                con.Dispose();
                throw;
            }
            return dt;
        }

        public static int SetData(string sql)
        {
            SqlConnection con = null;
            SqlCommand com = null;
            try
            {
                con = new SqlConnection(Constants.CONNECTION_STRING);
                con.Open();
                com = new SqlCommand(sql, con);
                return com.ExecuteNonQuery();
            }
            catch (Exception)
            {
                con.Close();
                con.Dispose();
                com.Dispose();
                throw;
            }
        }
    }
}