using SeSecEL.library;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SeSecEL
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        //----------------- 
        //Drag Form
        //----------------- 
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);
        //----------------- 
        SqlTools sql = new SqlTools();
        private bool ValidaConexion()
        { 
            SqlConnection cn = new SqlConnection();
            cn = sql.TestConnection("","");
            if(cn == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ValidaUsuario()
        { 
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT UserID From Users ");
            strSQL.Append(" WHERE email = '" + txtUsuario.Text + "'");
            strSQL.Append(" AND Password = '" + txtContraseña.Text + "'");
            strSQL.Append(" AND Inactive = 0");
            SqlDataReader drUser = sql.OpenDataReader("Login", "", "LeeCampos", strSQL);
            bool bValida = false;
            if (drUser != null)
            {
                if (drUser.Read())
                {
                    bValida = true;
                }
                else
                    bValida = false;
            }
            return bValida;
        }
        private string GetUserName()
        { 
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT UserName = FirstName + ' ' + LastName FROM Users ");
            strSQL.Append(" WHERE email = '" + txtUsuario.Text + "'");
            strSQL.Append(" AND Password = '" + txtContraseña.Text + "'");
            SqlDataReader drUser = sql.OpenDataReader("Login", "", "LeeCampos", strSQL);
            string UserName = "";
            if (drUser != null)
            {
                try
                {
                    if (drUser.Read())
                    {
                        UserName = drUser["UserName"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    sql.WriteToFile(ex.Message);
                }
                drUser.Close();
                drUser = null;
            }
            return UserName;
        }
        private string GetUserId()
        { 
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT UserID FROM Users ");
            strSQL.Append(" WHERE email = '" + txtUsuario.Text + "'");
            strSQL.Append(" AND Password = '" + txtContraseña.Text + "'");
            SqlDataReader drUser = sql.OpenDataReader("Login", "", "LeeCampos", strSQL);
            string drUserID = "";
            if (drUser != null)
            {
                try
                {
                    if (drUser.Read())
                    {
                        drUserID = drUser["UserID"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    sql.WriteToFile(ex.Message);
                }
                drUser.Close();
                drUser = null;
            }
            return drUserID;
        }

        private int GetBackgroundColor(string param)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT Value FROM Parameters ");
            strSQL.Append(" WHERE UserID = '" + CommonCache.UserID + "'");
            strSQL.Append(" AND ParameterID = '" + param + "'");
            SqlDataReader drParameter = sql.OpenDataReader("Login", "", "LeeCampos", strSQL);
            int Param = 255;
            if (drParameter != null)
            {
                try
                {
                    if (drParameter.Read())
                    {
                        Param =Int32.Parse(drParameter["Value"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    sql.WriteToFile(ex.Message);
                }
                drParameter.Close();
            }
            return Param;
        }
        private double GetSensitivity(string param)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT Value FROM Parameters ");
            strSQL.Append(" WHERE UserID = '" + CommonCache.UserID + "'");
            strSQL.Append(" AND ParameterID = '" + param + "'");
            SqlDataReader drParameter = sql.OpenDataReader("Login", "", "LeeCampos", strSQL);
            double Param = 0.000001;
            if (drParameter != null)
            {
                try
                {
                    if (drParameter.Read())
                    {
                        Param = Int32.Parse(drParameter["Value"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    sql.WriteToFile(ex.Message);
                }
                drParameter.Close();
            }
            return Param;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Inicia();
        }
        
        private void Inicia()
        {
            if (ValidaConexion())
            {
                if (ValidaUsuario())
                {
                    this.Hide();
                    Master frm = new Master();
                    frm.lblUserName.Text = GetUserName();
                    frm.lblEmail.Text = txtUsuario.Text;
                    CommonCache.UserID = GetUserId();
                    CommonCache.UserName = GetUserName();
                    CommonCache.Email = txtUsuario.Text;
                    CommonCache.BackGroundColorR = GetBackgroundColor(sql.ColorParamR);
                    CommonCache.BackGroundColorG = GetBackgroundColor(sql.ColorParamG);
                    CommonCache.BackGroundColorB = GetBackgroundColor(sql.ColorParamB);
                    CommonCache.Sensitivity = GetSensitivity(sql.Sensitivity);
                    frm.Show();
                }
                else
                    MessageBox.Show("Usuario o contraseña invalidos", "error", MessageBoxButtons.OK);
            }
            else
                MessageBox.Show("Error de conexion ala base de datos", "error", MessageBoxButtons.OK);
        }

        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "USUARIO")
            {
                txtUsuario.Text = "";
                txtUsuario.ForeColor = Color.LightGray;
            }
        }

        private void txtUsuario_Leave(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "")
            {
                txtUsuario.Text = "USUARIO";
                txtUsuario.ForeColor = Color.DimGray;
            }
        }

        private void txtContraseña_Enter(object sender, EventArgs e)
        {
            if (txtContraseña.Text == "CONTRASEÑA")
            {
                txtContraseña.Text = "";
                txtContraseña.ForeColor = Color.LightGray;
                txtContraseña.UseSystemPasswordChar = true;
            }
        }

        private void txtContraseña_Leave(object sender, EventArgs e)
        {
            if (txtContraseña.Text == "")
            {
                txtContraseña.Text = "CONTRASEÑA";
                txtContraseña.ForeColor = Color.DimGray;
                txtContraseña.UseSystemPasswordChar = false;
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
         
        private void Login_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtUsuario.Text = "diego@gmail.com";
            txtContraseña.Text = "123";

        }

        private void txtContraseña_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                Inicia();
            }
        }

    }
}
