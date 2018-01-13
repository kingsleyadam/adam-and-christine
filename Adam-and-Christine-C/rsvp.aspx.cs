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
using System.Net.Mail;
using System.Net;

namespace Adam_and_Christine_C
{
    public partial class rsvp : System.Web.UI.Page
    {
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

                //Get ISO Information
                if (Request.QueryString["l"] != null)
                    iso = Request.QueryString["l"].ToUpper();

                //Get Invite Information
                int eventID = 0, numPeople = 0;
                DateTime submitDate = DateTime.MinValue;
                iso = inviteds.Tables["Invite"].Rows[0]["iso"].ToString();
                string inviteName = (string)inviteds.Tables["Invite"].Rows[0]["InviteName"];
                bool attending = (bool)inviteds.Tables["Invite"].Rows[0]["Attending"];
                string comment = (string)inviteds.Tables["Invite"].Rows[0]["Comment"];
                if (inviteds.Tables["Invite"].Rows[0]["SubmitDate"] != DBNull.Value)
                    submitDate = (DateTime)inviteds.Tables["Invite"].Rows[0]["SubmitDate"];
                if (attending)
                {
                    eventID = (int)inviteds.Tables["Invite"].Rows[0]["EventID"];
                    numPeople = (int)inviteds.Tables["Invite"].Rows[0]["NumPeople"];
                    
                }

                //Set The ViewState for the values we need later
                ViewState["inviteName"] = inviteName;
                ViewState["inviteID"] = inviteID;
                ViewState["iso"] = iso;

                //Correct the Language If Needed
                if (iso != "US" && iso != "NL" && iso != "IT" && iso != "DE")
                    iso = "NA";

                //Populate Drop Down Boxes
                PopulateDropDowns();
                //Populate Text For Our Translations
                //If NA then we need the user to select a language
                if (iso == "NA")
                {
                    imgDE.CssClass = "nofilter";
                    imgUS.CssClass = "nofilter";
                    imgIT.CssClass = "nofilter";
                    imgNL.CssClass = "nofilter";
                    lblHeader.Text = "Hello ||Name||, Please select your language.".Replace("||Name||", inviteName);

                    pnlRSVPForm.Visible = false;
                    pnlEvent.Visible = false;
                    pnlNumPeople.Visible = false;
                    pnlComment.Visible = false;
                    pnlSubmitRow.Visible = false;
                }
                else
                {
                    PopulateTranslations(iso, inviteName);
                    UpdateFlags(iso);
                }

                if (submitDate != DateTime.MinValue)
                {
                    if (attending)
                    {
                        ddYesNo.SelectedIndex = 1;
                        ddEvent.SelectedValue = eventID.ToString();
                        ddNumPeople.SelectedValue = numPeople.ToString();
                        if (comment.Length > 0)
                        {
                            txtComment.Text = comment;
                        }
                        pnlRSVPForm.Visible = true;
                        pnlEvent.Visible = true;
                        pnlNumPeople.Visible = true;
                        pnlComment.Visible = true;
                        pnlAlreadySubmitted.Visible = true;
                    }
                    else
                    {
                        ddYesNo.SelectedIndex = 2;
                        if (comment.Length > 0)
                        {
                            txtComment.Text = comment;
                        }
                        pnlRSVPForm.Visible = true;
                        pnlComment.Visible = true;
                        pnlEvent.Visible = false;
                        pnlNumPeople.Visible = false;
                        pnlAlreadySubmitted.Visible = true;
                    }
                }
                else
                {
                    pnlEvent.Visible = false;
                    pnlNumPeople.Visible = false;
                    pnlComment.Visible = false;
                    pnlSubmitRow.Visible = false;
                    pnlAlreadySubmitted.Visible = false;
                }

                //Set Already Submitted Link
                if (pnlAlreadySubmitted.Visible == true)
                {
                    string link = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + "confirm/" + inviteID.ToString();
                    lnkConfirmFwd.NavigateUrl = link;
                }

            }
        }

        private void PopulateTranslations(string iso, string name)
        {
            DataSet ds = GetGlossaryDataset(iso);
            //Standard Labels
            lblHeader.Text = GetObjText(ds, lblHeader.ID.ToString()).Replace("||Name||", name);
            lblFillForm.Text = GetObjText(ds, lblFillForm.ID.ToString());
            lblYesNo.Text = GetObjText(ds, lblYesNo.ID.ToString());
            lblEvent.Text = GetObjText(ds, lblEvent.ID.ToString());
            lblNumPeople.Text = GetObjText(ds, lblNumPeople.ID.ToString());
            lblComment.Text = GetObjText(ds, lblComment.ID.ToString());
            lnkText.Text = GetObjText(ds, lnkText.ID.ToString());
            lblAlreadySubmitText.Text = GetObjText(ds, lblAlreadySubmitText.ID.ToString());
            lnkConfirmFwd.Text = GetObjText(ds, lnkConfirmFwd.ID.ToString());
            //YesNo Drop Down
            ddYesNo.Items[0].Text = GetObjText(ds, "lblPleaseSelect");
            ddYesNo.Items[1].Text = GetObjText(ds, "lblYes");
            ddYesNo.Items[2].Text = GetObjText(ds, "lblNo");
            //Num People Drop Down
            ddNumPeople.Items[0].Text = GetObjText(ds, "lblPleaseSelect");
            //Event Drop Down
            ddEvent.Items[0].Text = GetObjText(ds, "lblPleaseSelect");
            ddEvent.Items[1].Text = GetObjText(ds, "ddEventAMS");
            ddEvent.Items[2].Text = GetObjText(ds, "ddEventLIN");
            ddEvent.Items[3].Text = GetObjText(ds, "ddEventBOTH");
            //Error Messages
            reqEvent.ErrorMessage = GetObjText(ds, "lblErrorEvent");
            reqNumPeople.ErrorMessage = GetObjText(ds, "lblErrorNumPeople");
            //Loading Text
            Label lblLoading = (Label)updateProgress.FindControl("lblLoading");
            if (lblLoading != null)
                lblLoading.Text = GetObjText(ds, lblLoading.ID.ToString());
        }

        private void PopulateDropDowns()
        {
            //Num People
            if (ddNumPeople.Items.Count == 0)
            {
                int n = -1;
                while (n++ < 10)
                    ddNumPeople.Items.Add(new ListItem(n.ToString(), n.ToString()));
            }
            //YesNo
            if (ddYesNo.Items.Count == 0) 
            {
                int n = -1;
                while (n++ < 2)
                    ddYesNo.Items.Add(new ListItem(n.ToString(), n.ToString()));
            }
            //Event
            ddEvent.Items.Clear();
            DataSet ds = GetEventsDropDown("US");
            foreach (DataRow dr in ds.Tables["Events"].Rows)
                ddEvent.Items.Add(new ListItem(dr["EventName"].ToString(), dr["EventID"].ToString()));

        }

        private DataSet GetEventsDropDown(string iso)
        {
            DataSet ds = new DataSet();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand("RSVPGetEvents", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@iso", iso);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        DataTable emp = new DataTable("Events");
                        emp.Load(dr);
                        ds.Tables.Add(emp);
                    }
                }
            }
            return ds;
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

        private DataSet GetGlossaryDataset(string iso)
        {
            DataSet ds = new DataSet();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand("gblGetGlossaryByLang", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@iso", iso);
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

        private bool fncIsValidInvite(int inviteID)
        {
            bool isValid = false;
            SqlDataReader dr = null;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand("select dbo.fncGetRSVPIsValidInvite(@InviteID) as functionresult", con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("InviteID", inviteID);

                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    isValid = Convert.ToBoolean(dr["functionresult"].ToString());
                }
                con.Close();
                dr.Close();
            }

            return isValid;
        }

        private void UpdateFlags(string iso)
        {
            switch (iso)
            {
                case "US":
                    imgUS.CssClass = "nofiltersmall";
                    imgNL.CssClass = "grayscale";
                    imgIT.CssClass = "grayscale";
                    imgDE.CssClass = "grayscale";
                    break;
                case "NL":
                    imgUS.CssClass = "grayscale";
                    imgNL.CssClass = "nofiltersmall";
                    imgIT.CssClass = "grayscale";
                    imgDE.CssClass = "grayscale";
                    break;
                case "IT":
                    imgUS.CssClass = "grayscale";
                    imgNL.CssClass = "grayscale";
                    imgIT.CssClass = "nofiltersmall";
                    imgDE.CssClass = "grayscale";
                    break;
                case "DE":
                    imgUS.CssClass = "grayscale";
                    imgNL.CssClass = "grayscale";
                    imgIT.CssClass = "grayscale";
                    imgDE.CssClass = "nofiltersmall";
                    break;
            }
        }

        protected void lnkFlag_Click(object sender, EventArgs e)
        {
            LinkButton lnkBtn = (LinkButton)sender;
            string iso = lnkBtn.CommandArgument;
            ViewState["iso"] = iso;
            UpdateFlags(iso);
            PopulateTranslations(iso, (string)ViewState["inviteName"]);
            //Turn things back on
            pnlRSVPForm.Visible = true;
        }

        protected void ddYesNo_SelectedIndexChanged(object sender, EventArgs e = null)
        {
            switch (Convert.ToInt32(ddYesNo.SelectedValue))
            {
                case 0:
                    pnlComment.Visible = false;
                    pnlEvent.Visible = false;
                    pnlNumPeople.Visible = false;
                    pnlSubmitRow.Visible = false;
                    break;
                case 1:
                    pnlComment.Visible = true;
                    pnlEvent.Visible = true;
                    pnlNumPeople.Visible = true;
                    pnlSubmitRow.Visible = true;
                    break;
                case 2:
                    pnlComment.Visible = true;
                    pnlEvent.Visible = false;
                    pnlNumPeople.Visible = false;
                    pnlSubmitRow.Visible = true;
                    break;
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

        private void SubmitInvite(int inviteID, int eventid, int numPeople, string iso, bool attending, string comment)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand("RSVPUpdateInvite", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InviteID", inviteID);
                cmd.Parameters.AddWithValue("@EventID", eventid);
                cmd.Parameters.AddWithValue("@NumPeople", numPeople);
                cmd.Parameters.AddWithValue("@iso", iso);
                cmd.Parameters.AddWithValue("@Attending", attending);
                cmd.Parameters.AddWithValue("@Comment", comment);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void SendEmail()
        {
            //Get Config Values
            string fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            string fromDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
            string fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
            string smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            int smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
            //Set Mail Settings
            SmtpClient client = new SmtpClient(smtpHost, smtpPort);
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
            MailAddress from = new MailAddress(fromEmailAddress, fromDisplayName);
            MailAddress toAdam = new MailAddress("kingsleyadam@msn.com");
            MailAddress toChristine = new MailAddress("christine.terhall@gmail.com");
            MailMessage message = new MailMessage();
            message.From = from;
            message.To.Add(toAdam);
            message.To.Add(toChristine);
            message.Subject = "Someone has RSVP'd for the wedding!";
            message.Body = GetEmailBody();
            message.IsBodyHtml = true;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message + ex.StackTrace;
            }
        }

        private string GetEmailBody()
        {
            string body = "";

            body += ViewState["inviteName"] + " has submited their RSVP for the wedding. Here is the information:<br /><br />";
            body += "RSVP Response: " + ddYesNo.SelectedItem.ToString() + "<br />";
            if (ddYesNo.SelectedIndex == 1)
            {
                body += "Event: " + ddEvent.SelectedItem.ToString() + "<br />";
                body += "Number Of People: " + ddNumPeople.SelectedItem.ToString() + "<br />";
            }
            if (txtComment.Text.Length > 0)
            {
                body += "Comment: " + txtComment.Text;
            }
            return body;
        }

        protected void lnkText_Click(object sender, EventArgs e)
        {
            int inviteID = (int)ViewState["inviteID"], eventID = Convert.ToInt32(ddEvent.SelectedValue), numPeople = Convert.ToInt32(ddNumPeople.SelectedValue);
            string iso = (string)ViewState["iso"], comment = txtComment.Text;
            bool attending = false;
            if (ddYesNo.SelectedValue == "1")
                attending = true;
            else
                attending = false;

            SubmitInvite(inviteID, eventID, numPeople, iso, attending, comment);
            try
            {
                SendEmail();
            } 
            catch (Exception ex) 
            {
                lblError.Text = ex.Message + ex.StackTrace;
            }

            Response.Redirect("~/confirm/" + inviteID.ToString());
        }
    }
}