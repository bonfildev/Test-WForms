using SeSecEL.library;
using System;
using System.Text;
using System.Windows.Forms;

namespace SeSecEL
{
    public partial class Colors : Form
    {
        SqlTools sql = new SqlTools();
        public Colors()
        {
            InitializeComponent();
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                btnColor.Text = colorDialog1.Color.ToString();
                panelContainer.BackColor= colorDialog1.Color;
                UpdateColorParameter(colorDialog1.Color.R.ToString(), sql.ColorParamR);
                UpdateColorParameter(colorDialog1.Color.G.ToString(), sql.ColorParamG);
                UpdateColorParameter(colorDialog1.Color.B.ToString(), sql.ColorParamB);
                CommonCache.BackGroundColorR = colorDialog1.Color.R;
                CommonCache.BackGroundColorG = colorDialog1.Color.G;
                CommonCache.BackGroundColorB = colorDialog1.Color.B;
            }
        }
        private void UpdateColorParameter(string colorRGB,string Param)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendLine("IF NOT EXISTS(SELECT ParameterID FROM [Parameters]");
            strSQL.AppendLine("     WHERE [Parameters].ParameterID = " + Param);
            strSQL.AppendLine("     AND [Parameters].UserID = " + CommonCache.UserID + ")");
            strSQL.Append("INSERT [Parameters] (ParameterID, UserID, Value)");
            strSQL.Append(" VALUES (");
            strSQL.Append(sql.CI(Param, true));                                // ParameterID
            strSQL.Append(sql.CI(CommonCache.UserID, true));                        // UserID
            strSQL.Append(sql.CI(colorRGB, false, false));                          // Value
            strSQL.Append(")");
            strSQL.AppendLine("ELSE ");
            strSQL.Append("UPDATE [Parameters] SET ");
            strSQL.Append("ParameterID = " + sql.CI(Param, true));                         // ParameterID
            strSQL.Append("UserID = " + sql.CI(CommonCache.UserID, true));                      // UserID
            strSQL.Append("Value = " +sql.CI(colorRGB, false, false));                          // Value
            strSQL.AppendLine("     WHERE [Parameters].ParameterID = " + Param);
            strSQL.AppendLine("     AND [Parameters].UserID = " + CommonCache.UserID);

            sql.ExecCommand("Colors", CommonCache.UserName, "UpdateColorParameter", strSQL);
        }

        private void Colors_Load(object sender, EventArgs e)
        {
            panelContainer.BackColor = System.Drawing.Color.FromArgb(CommonCache.BackGroundColorR, CommonCache.BackGroundColorG, CommonCache.BackGroundColorB);
        }
    }
}
