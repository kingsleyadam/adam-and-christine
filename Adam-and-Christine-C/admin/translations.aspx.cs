using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Adam_and_Christine_C.admin
{
    public partial class translations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateDropDown(ddLang, "tmGetLang");
                PopulateDropDown(ddGlossaryGrp, "tmGetGlossaryGrp");
                PopulateGlossaryWebGrp(ddEngText, "tmGetGlossaryWebGrp", Convert.ToInt32(ddGlossaryGrp.SelectedValue), 1);
                lblFullEnglishText.Text = fncGetTransText(Convert.ToInt32(ddEngText.SelectedValue), "US");
                txtFullTranstext.Text = fncGetTransText(Convert.ToInt32(ddGlossaryGrp.SelectedValue), "US");
            }
        }

        private void PopulateDropDown(DropDownList dd, string spName)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand(spName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable("Info");
                        dt.Load(dr);
                        foreach (DataRow row in dt.Rows)
                        {
                            if (!row[1].ToString().Contains("Please Select"))
                                dd.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
                        }
                    }
                }
            }
        }

        private void PopulateGlossaryWebGrp(DropDownList dd, string spName, int grpID, int langID)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand(spName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GrpID", grpID);
                cmd.Parameters.AddWithValue("@LangID", langID);
                con.Open();
                dd.Items.Clear();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        DataTable dt = new DataTable("Info");
                        dt.Load(dr);
                        foreach (DataRow row in dt.Rows)
                        {
                            dd.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
                        }
                    }
                }
            }
        }

        protected void ddGlossaryGrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateGlossaryWebGrp(ddEngText, "tmGetGlossaryWebGrp", Convert.ToInt32(ddGlossaryGrp.SelectedValue), 1);
            lblFullEnglishText.Text = fncGetTransText(Convert.ToInt32(ddEngText.SelectedValue), "US");
            txtFullTranstext.Text = fncGetTransText(Convert.ToInt32(ddEngText.SelectedValue), (string)ddLang.SelectedValue);
        }

        private string fncGetTransText(int glossID, string iso)
        {
            string transtext = "";
            SqlDataReader dr = null;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand("select dbo.fnctmGetTranstext(@GlossID, @LangID) as functionresult", con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@GlossID", glossID);
                cmd.Parameters.AddWithValue("@LangID", iso);

                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    transtext = dr["functionresult"].ToString();
                }
                con.Close();
                dr.Close();
            }

            return transtext;
        }

        protected void ddLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFullTranstext.Text = fncGetTransText(Convert.ToInt32(ddEngText.SelectedValue), (string)ddLang.SelectedValue);
        }

        protected void ddEngText_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblFullEnglishText.Text = fncGetTransText(Convert.ToInt32(ddEngText.SelectedValue), "US");
            txtFullTranstext.Text = fncGetTransText(Convert.ToInt32(ddEngText.SelectedValue), (string)ddLang.SelectedValue);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int glossID = Convert.ToInt32(ddEngText.SelectedValue);
            string iso = ddLang.SelectedValue, transtext = txtFullTranstext.Text;

            if (glossID != 0 && iso != null && transtext.Length > 0)
            {
                UpdateTranstext("tmUpdateTranstext",glossID,iso,transtext);
                lblFullEnglishText.Text = fncGetTransText(Convert.ToInt32(ddEngText.SelectedValue), "US");
                if (iso == "US")
                {
                    if (transtext.Length > 50)
                    {
                        ddEngText.Items[ddEngText.SelectedIndex].Text = transtext.Substring(0, 50) + "...";
                    }
                    else
                    {
                        ddEngText.Items[ddEngText.SelectedIndex].Text = transtext;
                    }
                }
            }
        }

        private void UpdateTranstext(string spName, int glossID, string iso, string transtext)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand(spName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GlossID", glossID);
                cmd.Parameters.AddWithValue("@iso", iso);
                cmd.Parameters.AddWithValue("@Transtext", transtext);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            AddNewTranstext("tmAddTranstext", (string)ddLang.SelectedValue, txtFullTranstext.Text, Convert.ToInt32(ddGlossaryGrp.SelectedValue));
            PopulateGlossaryWebGrp(ddEngText, "tmGetGlossaryWebGrp", Convert.ToInt32(ddGlossaryGrp.SelectedValue), 1);
            lblFullEnglishText.Text = fncGetTransText(Convert.ToInt32(ddEngText.SelectedValue), "US");
            txtFullTranstext.Text = fncGetTransText(Convert.ToInt32(ddEngText.SelectedValue), (string)ddLang.SelectedValue);
        }

        private void AddNewTranstext(string spName, string iso, string transtext, int grpID)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand(spName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@iso", iso);
                cmd.Parameters.AddWithValue("@Transtext", transtext);
                cmd.Parameters.AddWithValue("@GrpID", grpID);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteTranstext("tmDeleteTranstext", Convert.ToInt32(ddEngText.SelectedValue));
            PopulateGlossaryWebGrp(ddEngText, "tmGetGlossaryWebGrp", Convert.ToInt32(ddGlossaryGrp.SelectedValue), 1);
            lblFullEnglishText.Text = fncGetTransText(Convert.ToInt32(ddEngText.SelectedValue), "US");
            txtFullTranstext.Text = fncGetTransText(Convert.ToInt32(ddEngText.SelectedValue), (string)ddLang.SelectedValue);
        }

        private void DeleteTranstext(string spName, int glossID)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand(spName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GlossID", glossID);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}