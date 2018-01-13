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


namespace Adam_and_Christine_C
{
    public partial class confirm : System.Web.UI.Page
    {
        bool attending;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataSet inviteds = null;
                int inviteID = 0;
                string iso = "";

                //Check the validity of the invite. If it's not valid forward it along.
                if (Request.QueryString["i"] != null)
                    Int32.TryParse(Request.QueryString["i"].ToString(), out inviteID);

                if (inviteID > 0)
                {
                    inviteds = GetInviteInfo(inviteID);
                    if (inviteds.Tables.Count == 0)
                        Response.Redirect("~/invalid.aspx");
                }
                else
                    Response.Redirect("~/invalid.aspx");
                
                //Get Invite Information
                int eventID = 0, numPeople = 0;
                string eventName = "";
                DateTime submitDate = DateTime.MinValue;
                iso = inviteds.Tables["Invite"].Rows[0]["iso"].ToString();
                string inviteName = (string)inviteds.Tables["Invite"].Rows[0]["InviteName"];
                attending = (bool)inviteds.Tables["Invite"].Rows[0]["Attending"];
                string comment = (string)inviteds.Tables["Invite"].Rows[0]["Comment"];
                if (inviteds.Tables["Invite"].Rows[0]["SubmitDate"] != DBNull.Value)
                    submitDate = (DateTime)inviteds.Tables["Invite"].Rows[0]["SubmitDate"];
                if (attending)
                {
                    eventID = (int)inviteds.Tables["Invite"].Rows[0]["EventID"];
                    eventName = (string)inviteds.Tables["Invite"].Rows[0]["EventName"];
                    numPeople = (int)inviteds.Tables["Invite"].Rows[0]["NumPeople"];
                }

                //Populate Translations
                PopulateTranslations(iso, inviteName);
                repAMSInfo.DataSource = GetGlossaryDataset(iso, 3);
                repAMSInfo.DataBind();
                repLINInfo.DataSource = GetGlossaryDataset(iso, 4);
                repLINInfo.DataBind();

                //Populate Link Information
                string link = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + "rsvp/" + inviteID.ToString();
                lnkRSVP.NavigateUrl = link;
                lnkRSVP.Text = link;
                btnMistakeLink.NavigateUrl = link;

                //Populate Invite Information
                lblName.Text = inviteName;
                lblSubmittedResponse.Text = submitDate.ToShortDateString();
                if (attending)
                {
                    pnlEvent.Visible = true;
                    pnlNumPeople.Visible = true;
                    lblEventResponse.Text = eventName;
                    lblNumPeopleResponse.Text = numPeople.ToString();

                    if (eventID == 1 || eventID == 3)
                        pnlAMSInfo.Visible = true;
                    else
                        pnlAMSInfo.Visible = false;

                    if (eventID == 2 || eventID == 3)
                        pnlLINInfo.Visible = true;
                    else
                        pnlLINInfo.Visible = false;

                }
                else
                {
                    pnlEvent.Visible = false;
                    pnlNumPeople.Visible = false;
                    pnlLINInfo.Visible = false;
                    pnlAMSInfo.Visible = false;
                }
            }
        }

        private DataSet GetInviteInfo(int inviteID)
        {
            DataSet ds = new DataSet();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand("RSVPGetInviteInfo", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InviteID", inviteID);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        DataTable emp = new DataTable("Invite");
                        emp.Load(dr);
                        ds.Tables.Add(emp);
                    }
                }
            }
            return ds;
        }

        private void PopulateTranslations(string iso, string name)
        {
            DataSet ds = GetGlossaryDataset(iso);
            //Standard Labels
            lblThankYouHeader.Text = GetObjText(ds, lblThankYouHeader.ID.ToString());
            lblThankYou.Text = GetObjText(ds, lblThankYou.ID.ToString()) + ":";
            lblMistake.Text = GetObjText(ds, lblMistake.ID.ToString());
            lblRSVPInfo.Text = GetObjText(ds, lblRSVPInfo.ID.ToString());
            lblAttending.Text = GetObjText(ds, lblAttending.ID.ToString()) + ": ";
            lblEventConfirm.Text = GetObjText(ds, lblEventConfirm.ID.ToString()) + ": ";
            lblNumPeopleConfirm.Text = GetObjText(ds, lblNumPeopleConfirm.ID.ToString()) + ": ";
            lblSubmitted.Text = GetObjText(ds, lblSubmitted.ID.ToString()) + ": ";
            if (attending)
                lblAttendingAnswer.Text = GetObjText(ds, "lblYes");
            else
                lblAttendingAnswer.Text = GetObjText(ds, "lblNo");
            lblAMSInfoHeader.Text = GetObjText(ds, lblAMSInfoHeader.ID.ToString());
            lblLINInfoHeader.Text = GetObjText(ds, lblLINInfoHeader.ID.ToString());
        }

        private string GetObjText(DataSet glossary, string objName)
        {
            string transText = "NOT-PREPARED";

            foreach (DataRow dr in glossary.Tables["Glossary"].Rows)
            {
                if (dr["ObjName"].ToString() == objName)
                {
                    transText = dr["Transtext"].ToString();
                    break;
                }
            }

            return transText;
        }

        private DataSet GetGlossaryDataset(string iso, int grpID = 0)
        {
            DataSet ds = new DataSet();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand("gblGetGlossaryByLang", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@iso", iso);
                cmd.Parameters.AddWithValue("@GrpID", grpID);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        DataTable emp = new DataTable("Glossary");
                        emp.Load(dr);
                        ds.Tables.Add(emp);
                    }
                }
            }
            return ds;
        }
    }
}