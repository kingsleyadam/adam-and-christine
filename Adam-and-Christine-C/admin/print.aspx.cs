using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Adam_and_Christine_C.admin
{
    public partial class print : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string inviteIDs;
            if (Request.QueryString["i"] != null)
                inviteIDs = Request.QueryString["i"].ToString();
            else 
                inviteIDs = "0";

            if (inviteIDs.Length > 0)
            {
                repInvites.DataSource = GetInviteDataSet(inviteIDs);
                repInvites.DataBind();
            }

        }

        private DataSet GetInviteDataSet(string inviteIDs)
        {
            DataSet ds = new DataSet();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand("admGetInvites2Print", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InviteIDs", inviteIDs);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        DataTable emp = new DataTable("Invites");
                        emp.Load(dr);
                        ds.Tables.Add(emp);
                    }
                }
            }
            return ds;
        }

        protected void repInvites_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label lblInviteID = (Label)e.Item.FindControl("lblInviteID");
            Image imgQRCode = (Image)e.Item.FindControl("imgQRCode");
            if (lblInviteID != null && imgQRCode != null)
            {
                int inviteID = Convert.ToInt32(lblInviteID.Text);
                imgQRCode.ImageUrl = "~/img/qrcodes/" + inviteID.ToString() + ".jpg";
            }
        }
    }
}