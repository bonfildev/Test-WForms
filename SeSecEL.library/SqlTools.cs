using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace SeSecEL.library
{
    public class SqlTools : Constantes
    {
        public string MSGError = string.Empty;
        public const int TimeOut = 1200;
        public string FormatDate() => ("yyyy-MM-dd");
        public long ExeccuteCommand(string pagina, string funcion, StringBuilder strSQL)
        {
            SqlConnection cnComando = OpenSQLConnection(pagina);
            long Rows = 0;
            MSGError = string.Empty;

            if (cnComando != null)
            {
                try
                {
                    SqlTransaction trComando;
                    SqlCommand cmComando = new SqlCommand(strSQL.ToString(), cnComando);
                    trComando = cnComando.BeginTransaction();

                    cmComando.CommandTimeout = TimeOut;
                    cmComando.Transaction = trComando;
                    cmComando.CommandType = CommandType.Text;

                    Rows = cmComando.ExecuteNonQuery();
                    trComando.Commit();
                    trComando.Dispose();
                    cmComando.Dispose();
                    cmComando = null;
                }
                catch (Exception ex)
                {
                    Rows = 0;
                    MSGError = "SQL_Tools.execCommand:" + ex.Message + " " + strSQL.ToString();
                    WriteToFile(ex.Message);

                }
                finally
                {
                    cnComando.Close();
                }
            }
            return Math.Abs(Rows);
        } // End ExecCommand 

        private string GetConnection() => ConfigurationManager.AppSettings["StringConection"];
        public SqlConnection OpenSQLConnection(string Pagina)
        {
            SqlConnection cnSQL = new SqlConnection { ConnectionString = GetConnection() };
            MSGError = string.Empty;
            try
            {
                cnSQL.Open();
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message);
                MSGError = "SQL_Tools.OpenSQLConnection:" + ex.Message;
                cnSQL = null;
            }
            return cnSQL;
        }
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }

        public SqlConnection TestConnection(string pagina, string UserName)
        {
            SqlConnection cnSQL = null;
            cnSQL = OpenSQLConnection(pagina);
            return cnSQL;
        }

        public SqlDataReader OpenDataReader(string pagina, string UserName, string funcion, StringBuilder strSQL)
        {
            SqlConnection cnSQL = null;
            SqlCommand cmSQL = null;
            cnSQL = OpenSQLConnection(pagina);
            SqlDataReader drSQL = null;
            MSGError = string.Empty;

            if (cnSQL != null)
            {
                try
                {
                    cmSQL = new SqlCommand { CommandText = strSQL.ToString(), Connection = cnSQL, CommandTimeout = TimeOut };
                    drSQL = cmSQL.ExecuteReader();
                }
                catch (Exception ex)
                {
                    MSGError = "SQL_Tools.OpenDataReader:" + ex.Message + " " + strSQL.ToString();
                    WriteToFile(ex.Message);
                }
            }
            return drSQL;
        }
        public DataTable FillDataTable(string pagina, string UserName, string funcion, StringBuilder strSQL)
        {
            using (DataTable tbl = new DataTable("consulta"))
            {
                MSGError = string.Empty;

                try
                {
                    SqlConnection cnDataSet = OpenSQLConnection(pagina);
                    if (cnDataSet != null)
                    {
                        using (SqlDataAdapter daDataSet = new SqlDataAdapter(strSQL.ToString(), cnDataSet))
                        {
                            daDataSet.Fill(tbl);
                        }
                        cnDataSet.Close();
                        cnDataSet = null;
                    }
                    else
                    {
                        tbl.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    WriteToFile(ex.Message);
                    MSGError = "SQL_Tools.FillDataSet  " + pagina + " " + funcion + ": " + ex.Message + strSQL;
                    return null;
                }
                return tbl;
            }

        }

        private bool GetUpperCase
        {
            get
            {
                bool.TryParse(ConfigurationManager.AppSettings["UpperCase"], out bool bValor);
                return bValor;
            }
        }
        public string GetID(string forma, string strQuery)
        {
            SqlConnection cnClave = OpenSQLConnection(forma);
            string strValor = "";
            MSGError = string.Empty;

            if (cnClave != null)
            {
                SqlDataReader drClave;
                try
                {
                    SqlCommand cmClave = new SqlCommand { Connection = cnClave, CommandText = strQuery, CommandTimeout = TimeOut };
                    drClave = cmClave.ExecuteReader();
                    if (drClave.Read())
                    {
                        if (drClave[0].GetType().FullName == "System.DateTime")
                        {
                            if (drClave[0].ToString().Length != 0)
                            {
                                strValor = Convert.ToDateTime(drClave[0].ToString()).ToString(FormatDate());
                            }
                        }
                        else
                        {
                            strValor = drClave[0].ToString().Trim();
                        }
                    }
                    drClave.Close();
                    cmClave.Dispose();
                    cmClave = null;
                    drClave = null;
                }
                catch (Exception ex)
                {
                    strValor = "";
                    MSGError = "SQL_Tools.GetClave:" + ex.Message + " " + strQuery;
                    WriteToFile(ex.Message + " " + strQuery);
                }
                cnClave.Close();
            }
            return strValor;
        }
    }
}
