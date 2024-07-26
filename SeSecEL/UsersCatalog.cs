using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using SeSecEL.library;

namespace SeSecEL
{
    public partial class UsersCatalog : Form
    {
        string Forma = "CatalogoUsuarios";
        public UsersCatalog()
        {
            InitializeComponent();
        }

        private void InitCampos()
        {
            txtUserID.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            chkInactive.Checked = false;
            txtPassword.Text = string.Empty;
        }
        private void LeeCampos(string id)
        {
            SqlTools sql = new SqlTools();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT  UserID,FirstName,LastName,email,Inactive,Profile,Password FROM Users ");
            strSQL.Append(" WHERE UserID = " + id); 
            SqlDataReader drUser = sql.OpenDataReader("Login", "", "LeeCampos", strSQL); 
            if (drUser != null)
            {
                try
                {
                    if (drUser.Read())
                    {
                        txtUserID.Text = drUser["UserID"].ToString();
                        txtFirstName.Text = drUser["FirstName"].ToString();
                        txtLastName.Text = drUser["LastName"].ToString();
                        txtEmail.Text = drUser["email"].ToString();
                        chkInactive.Checked = Convert.ToBoolean(drUser["Inactive"].ToString());
                        txtPassword.Text = drUser["Password"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    sql.WriteToFile(ex.Message);
                }
                drUser.Close();
            }
        }

        private bool ValidaAlta()
        {
            bool bValida = false;
            if(txtFirstName.Text.Length == 0)
            {
                txtFirstName.Focus();
                bValida = true;
            }
            else if (txtLastName.Text.Length == 0)
            {
                txtLastName.Focus();
                bValida = true;
            }
            else if (txtEmail.Text.Length == 0)
            {
                txtEmail.Focus();
                bValida = true;
            }
            else if (txtPassword.Text.Length == 0)
            {
                txtPassword.Focus();
                bValida = true;
            }
            return bValida;
        }

        private void ProcesoAlta()
        {
            if (!ValidaAlta())
            {
                if (txtUserID.Text.Length == 0)
                {
                    Alta();
                }
                else
                {
                    Modifica(txtUserID.Text);
                }
                Buscar();
            }
            else
                MessageBox.Show("Falta Informacion", "error", MessageBoxButtons.OK);
        }
    

        private void Alta()
        {
            SqlTools sql = new SqlTools();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("INSERT Users(FirstName, LastName, email, Inactive, Password, CreateUser, CreateDate) ");
            strSQL.Append("VALUES ( ");
            strSQL.Append("'" + txtFirstName.Text + "'");
            strSQL.Append("'" + txtLastName.Text + "'");
            strSQL.Append("'" + txtEmail.Text + "'");
            strSQL.Append("'" + (chkInactive.Checked ? "1" : "0") + "'");
            strSQL.Append("'" + txtPassword.Text + "'");
            strSQL.Append("'" + CommonCache.UserID + "'");
            strSQL.Append("GetDate()");
            strSQL.Append(")");
            if (sql.ExeccuteCommand(Forma.ToString(), "ProcesoAlta", strSQL) != 0)
            {
                AddParameters(txtEmail.Text);
                MessageBox.Show("Registro actualizado","",MessageBoxButtons.OK);
            }
        }
        private void Modifica(string ID)
        {
            SqlTools sql = new SqlTools();
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendLine("UPDATE Users SET  ");
            strSQL.AppendLine("FirstName = '" + txtFirstName.Text + "'");
            strSQL.AppendLine("LastName = '" + txtLastName.Text + "'");
            strSQL.AppendLine("email = '" + txtEmail.Text + "'");
            strSQL.AppendLine("Inactive = '" + (chkInactive.Checked ? "1" : "0") + "'");
            strSQL.AppendLine("Password = '" + txtPassword.Text + "'");
            strSQL.AppendLine("UpdateUser = '" +  CommonCache.UserID + "'");
            strSQL.AppendLine("UpdateDate = " + "GetDate()");
            strSQL.AppendLine(" WHERE UserID = " + ID);
            if (sql.ExeccuteCommand(Forma.ToString(),  "Modifica", strSQL) != 0)
            {
                MessageBox.Show("Registro actualizado", "", MessageBoxButtons.OK);
            }
        }

        private void AddParameters(string email)
        {
            SqlTools sql = new SqlTools();
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendLine("Select UserID FROM Users wehre email = '" + email +"'");
            string UserID = sql.GetID(Forma,   strSQL.ToString());
            if (UserID.Length != 0)
            {
                strSQL.Append("DELETE [Parameters]");
                strSQL.Append("WHERE  UserID = " + UserID);
                sql.ExeccuteCommand(Forma.ToString(),  "Modifica", strSQL);
                strSQL.Clear();
                strSQL.Append("INSERT INTO [Parameters]");
                strSQL.Append("SELECT ParameterID," + UserID +",Value,Description FROM [Parameters]");
                sql.ExeccuteCommand(Forma.ToString(), "Modifica", strSQL);
            }

        }
        private void Baja()
        {

        }
        private void Buscar()
        {
            SqlTools sql = new SqlTools();
            Master form = new Master();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT UserID,FirstName,LastName,email,Inactive,Profile,Password ");
            strSQL.Append(" FROM USERS "); 
            DataTable tblConsulta = sql.FillDataTable(Forma.ToString(), form.lblUserName.Text, "Buscar", strSQL);
            gvBuscar.DataSource = tblConsulta;
            AddEditColumn(ref gvBuscar);
        }

        private void AddEditColumn(ref DataGridView gv)
        {

            DataGridViewButtonColumn editbutton = new DataGridViewButtonColumn();

            editbutton.FlatStyle = FlatStyle.Popup;

            editbutton.HeaderText = "Edit";
            editbutton.Name = "Edit";
            editbutton.UseColumnTextForButtonValue = true;
            editbutton.Text = "Edit";

            editbutton.Width = 60;
            if (!gv.Columns.Contains(editbutton.Name = "$Edit"))
            {
                gv.Columns.Add(editbutton);
            }
        }

        private void btnNew_Click(object sender, EventArgs e) => InitCampos();
        private void btnSave_Click(object sender, EventArgs e) => ProcesoAlta();
        private void btnDelete_Click(object sender, EventArgs e) => Baja();
        private void btnBuscar_Click(object sender, EventArgs e) => Buscar();

        private void gvBuscar_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 7)
                {
                    string id = Convert.ToString(gvBuscar.Rows[e.RowIndex].Cells[0].Value.ToString());
                    LeeCampos(id);
                }
            }
            catch ( Exception ex)
            {
                new SqlTools().WriteToFile(ex.ToString());
            }
        }
    }
}
