using System.Data;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using VanDriverAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Emit;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Azure;
using System.Xml.Linq;
using System.Reflection;
using Microsoft.Data.SqlClient.DataClassification;

namespace VanDriverAPI.App_Data
{
    public class DataLayer
    {
        private readonly IConfiguration _configuration;

        private SqlConnection mobjConn;
        private string mstrConnString;
        private int mintCmdTimeOut;
        private string mstrErr;
        private string mstrErrSource;


        public string ConnectionString
        {
            get
            {
                return mstrConnString;
            }
            set
            {
                mstrConnString = value;
            }
        }


        public DataLayer(IConfiguration configuration)
        {
            _configuration = configuration;
            //var con = _configuration["ConnectionStrings:DBCon"];
            var con = _configuration.GetConnectionString("DefaultConnection");


            //Console.WriteLine(con);
            if (con != null)
            {
                mstrConnString = con;
            }
            else
            {
                mstrConnString = null;
            }
        }

        public DataLayer(string connString)
        {
            ConnectionString = connString;
            mstrConnString = connString;
        }

        public SqlConnection GetConnection(string strConn)
        {
            string strFunctionName = "GetConnection";
            try
            {
                if (mobjConn == null || mobjConn.State != System.Data.ConnectionState.Open)
                {
                    mobjConn = new SqlConnection(strConn);
                    mobjConn.Open();
                }

                return mobjConn;
            }
            catch (Exception ex)
            {
                mstrErr = ex.ToString();
                throw new Exception(strFunctionName + " : " + mstrErr);
            }
            finally
            {
                mobjConn.Close();

            }
        }

        public DataSet LoginCredentials(string loginId, string password)
        {
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("LoginAuthentication", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", loginId);
                    command.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        conn.Open(); 
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                        {
                            dataAdapter.Fill(ds);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }

            return ds;
        }


        public DataSet React_MasterCity()
        {

            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("React_MasterCity", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        dataAdapter.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        ds = null;
                    }
                }
            }
            return ds;

        }

        public DataSet React_MasterCheckEmail(string email)
        {

            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("React_MasterCheckEmail", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);


                    try
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        dataAdapter.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        ds = null;
                    }
                }
            }
            return ds;
        }

        public DataSet React_Validate(string Email, string Mobile, int ExamYear, string ExamSession)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("React_Validate", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExamYear", ExamYear);
                    cmd.Parameters.AddWithValue("@ExamSession", ExamSession);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@Mobile", Mobile);
                    try
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error : {ex.Message}");
                        ds = null;
                    }
                }
            }
            return ds;
        }


        public DataSet React_MasterState(string statecode)
        {

            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("React_MasterState", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@statecode", statecode);


                    try
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        dataAdapter.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        ds = null;
                    }
                }
            }
            return ds;

        }

        public DataSet React_MasterTitle(string title)
        {

            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("React_MasterTitle", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Title", title);


                    try
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        dataAdapter.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        ds = null;
                    }
                }
            }
            return ds;

        }
        public DataSet React_ExamSession()
        {

            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("React_ExamSession", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        dataAdapter.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        ds = null;
                    }
                }
            }
            return ds;

        }

        public DataSet React_Master_Examination_sendEmail()
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("React_Master_Examination_Email", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        dataAdapter.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        ds = null;
                    }
                }
            }
            return ds;
        }


        public DataSet React_ForgotPassword(string Input, string ExamSession)
        {

            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("React_ForgotPassword", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Input", Input);
                    command.Parameters.AddWithValue("@ExamyearSession", ExamSession);


                    try
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        dataAdapter.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        ds = null;
                    }
                }
            }
            return ds;

        }



    }
}
