using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Framework.DataAcess
{
    public class CommonDA
    {
        public static string strConn
        {
            get { return ConfigurationManager.ConnectionStrings["Pcl"].ConnectionString; }
        }

        public static int ExecuteNonQuery(CommandType CommandType, string commandText)
        {
            return ExecuteNonQuery(CommandType, commandText, null);
        }

        public static int CommandTimeout
        {
            get { return 0; }
        }

        public static int ExecuteNonQuery(CommandType CommandType, string commandText, SqlParameter[] p)
        {
            SqlConnection cnn = new SqlConnection(strConn);
            cnn.Open();
            SqlCommand cmd = new SqlCommand(commandText, cnn);
            cmd.CommandType = CommandType;
            cmd.CommandTimeout = CommandTimeout;
            if (p != null)
            {
                for (int i = 0; i < p.Length; i++)
                {
                    cmd.Parameters.Add(p[i]);
                }
            }
            int retval = cmd.ExecuteNonQuery();
            cnn.Close();
            return retval;
        }

        public static int ExecuteNonQueryB(CommandType CommandType, string commandText, SqlParameter[] p)
        {
            SqlConnection cnn = new SqlConnection(strConn);
            cnn.Open();
            SqlCommand cmd = new SqlCommand(commandText, cnn);
            cmd.CommandType = CommandType;
            cmd.CommandTimeout = CommandTimeout;
            if (p != null)
            {
                for (int i = 0; i < p.Length; i++)
                {
                    cmd.Parameters.Add(p[i]);
                }
            }
            int retval = cmd.ExecuteNonQuery();
            cnn.Close();
            return retval;
        }

        public static object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, null);
        }

        public static object ExecuteScalar(string commandText, SqlParameter[] p)
        {
            return ExecuteScalar(commandText, p, CommandType.StoredProcedure);
        }

        public static object ExecuteScalar(string commandText, SqlParameter[] p, CommandType commandType)
        {
            try
            {
                SqlConnection cnn = new SqlConnection(strConn);
                cnn.Open();
                SqlCommand cmd = new SqlCommand(commandText, cnn);
                cmd.CommandType = commandType;
                cmd.CommandTimeout = CommandTimeout;

                if (p != null)
                {
                    for (int i = 0; i < p.Length; i++)
                    {
                        cmd.Parameters.Add(p[i]);
                    }
                }
                object retval = cmd.ExecuteScalar();
                cnn.Close();
                return retval;
            }
            catch (Exception ex)
            {
                string teste = ex.StackTrace;
                throw new Exception(ex.ToString());

            }
        }

        public static IDataReader ExecuteReader(string commandText)
        {
            return ExecuteReader(commandText, null);
        }

        public static IDataReader ExecuteReader(string commandText, SqlParameter[] p, CommandType pCommandType = CommandType.StoredProcedure)
        {
            SqlConnection cnn = new SqlConnection(strConn);
            cnn.Open();
            SqlCommand cmd = new SqlCommand(commandText, cnn);
            cmd.CommandType = pCommandType; // CommandType.StoredProcedure;
            cmd.CommandTimeout = CommandTimeout;

            if (p != null)
            {
                for (int i = 0; i < p.Length; i++)
                {
                    cmd.Parameters.Add(p[i]);
                }
            }
            IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }

        public static DataSet ExecuteDataSet(string commandText)
        {
            return ExecuteDataSet(commandText, null);
        }

        public static DataSet ExecuteDataSet(string commandText, SqlParameter[] p)
        {
            SqlConnection cnn = new SqlConnection(strConn);
            cnn.Open();
            SqlCommand cmd = new SqlCommand(commandText, cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = CommandTimeout;

            if (p != null)
            {
                for (int i = 0; i < p.Length; i++)
                {
                    cmd.Parameters.Add(p[i]);
                }
            }
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            cnn.Close();
            return ds;
        }

        public static DataTable ExecuteDataTable(string commandText, CommandType commandType)
        {
            return ExecuteDataTable(commandText, null, commandType);
        }

        public static DataTable ExecuteDataTable(string commandText)
        {
            return ExecuteDataTable(commandText, null);
        }

        public static DataTable ExecuteDataTable(string commandText, SqlParameter[] p)
        {
            return ExecuteDataTable(commandText, p, CommandType.StoredProcedure);
        }

        // internal
        public static DataTable ExecuteDataTable(string commandText, SqlParameter[] p, CommandType commandType)
        {
            try
            {
                SqlConnection cnn = new SqlConnection(strConn);
                cnn.Open();
                SqlCommand cmd = new SqlCommand(commandText, cnn);
                cmd.CommandType = commandType;
                cmd.CommandTimeout = CommandTimeout;

                if (p != null)
                {
                    for (int i = 0; i < p.Length; i++)
                    {
                        cmd.Parameters.Add(p[i]);
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                cnn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public static DataTable ExecuteDataTableB(string commandText, SqlParameter[] p)
        {
            return ExecuteDataTable(commandText, p, CommandType.StoredProcedure);
        }

        internal static DataTable ExecuteDataTableB(string commandText, SqlParameter[] p, CommandType commandType)
        {
            try
            {
                SqlConnection cnn = new SqlConnection(strConn);
                cnn.Open();
                SqlCommand cmd = new SqlCommand(commandText, cnn);
                cmd.CommandType = commandType;
                cmd.CommandTimeout = CommandTimeout;

                if (p != null)
                {
                    for (int i = 0; i < p.Length; i++)
                    {
                        cmd.Parameters.Add(p[i]);
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                cnn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


    }
}
