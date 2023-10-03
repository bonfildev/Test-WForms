﻿using SeSecEL.library;
using System;
using System.Data;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SeSecEL
{
    public partial class Parameters : Form
    {
        string Forma = "Parameters";
        SqlTools sql = new SqlTools();
        private void Parameters_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        private void gvParameters_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            this.Invalidate();
        }

        public Parameters()
        {
            InitializeComponent();
        }

        private void Parameters_Load(object sender, EventArgs e)
        {
            panelContainer.BackColor = System.Drawing.Color.FromArgb(CommonCache.BackGroundColorR, CommonCache.BackGroundColorG, CommonCache.BackGroundColorB);
            BuscarParametros();
        }

        private void BuscarParametros()
        {
            SqlTools sql = new SqlTools();
            Master form = new Master();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT ParameterID,UserID,Description,Value");
            strSQL.Append(" FROM [Parameters] ");
            DataTable tblConsulta = sql.FillDataTable(Forma.ToString(), form.lblUserName.Text, "BuscarParametros", strSQL);
            gvParameters.DataSource = tblConsulta;
            foreach (DataGridViewColumn C in gvParameters.Columns)
            {
                gvParameters.Columns[C.Index].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            gvParameters.Columns["ParameterID"].ReadOnly = true;
            gvParameters.Columns["UserID"].ReadOnly = true;
            gvParameters.Columns["Description"].ReadOnly = true;

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            StringBuilder strSQL = new StringBuilder();
            foreach (DataGridViewRow R in gvParameters.Rows)
            {
                if (gvParameters.Rows[R.Index].Cells[3].Value != null)
                {
                    strSQL.Append("UPDATE [Parameters] SET ");
                    strSQL.Append(sql.MC("Value", Convert.ToString(gvParameters.Rows[R.Index].Cells[3].Value.ToString()), false, false));
                    strSQL.Append("WHERE ParameterID = " + Convert.ToString(gvParameters.Rows[R.Index].Cells[0].Value.ToString()));
                    strSQL.Append("     AND UserID = " + CommonCache.UserID);
                    sql.ExecCommand(Forma.ToString(), CommonCache.UserName, "btnUpdateParameters_Click", strSQL);
                }
            }
        }

    }
}
