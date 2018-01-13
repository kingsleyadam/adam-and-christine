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
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Adam_and_Christine_C.admin
{
    public partial class invitations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Set Default ViewState Values
                ViewState["SearchFor"] = "";
                ViewState["SearchValue"] = "";
                ViewState["SortExpr"] = "FullName ASC";

                //Set Default Page Size
                grdInvites.PageSize = Convert.ToInt32(ddRows.Items[0].Value);

                //Populate Objects
                PopulateInviteGrid();
                PopulateSearchFlds();
                PopulateFreqData();

                //Initialize The Invite Form
                InitializeForm();

                //Hide Objects
                pnlInviteInfo.Visible = false;
            }
            txtSearch.Focus();

        }

        private void PopulateInviteGrid()
        {
            string searchFor = (string)ViewState["SearchFor"];
            string searchValue = (string)ViewState["SearchValue"];
            string sortExpression = (string)ViewState["SortExpr"];
            DataSet ds = GetInviteDataSet("admGetAllInvites", searchFor, searchValue, sortExpression);
            if (ds.Tables.Count > 0)
            {
                grdInvites.DataSource = ds;
                if (ds.Tables["Invites"].Rows.Count > grdInvites.PageSize)
                    grdInvites.CssClass = "tablehovernoLast table table-hover";
                else
                    grdInvites.CssClass = "table table-hover";
            }
            else
            {
                grdInvites.DataSource = null;
                grdInvites.CssClass = "table";
            }
            
            grdInvites.DataBind();
        }

        private DataSet GetInviteDataSet(string spName, string searchFor, string searchValue, string sortExpression)
        {
            DataSet ds = new DataSet();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand(spName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SearchFor", searchFor);
                cmd.Parameters.AddWithValue("@SearchValue", searchValue);
                cmd.Parameters.AddWithValue("@SortExpression", sortExpression);
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

        private void PopulateFreqData()
        {
            DataSet ds = GetFreqDataset();
            lblAMS.Text = ds.Tables["Freq"].Rows[0]["AMS"].ToString();
            lblNEB.Text = ds.Tables["Freq"].Rows[0]["NEB"].ToString();
            lblNA.Text = ds.Tables["Freq"].Rows[0]["NoResponse"].ToString();
        }

        private DataSet GetFreqDataset()
        {
            DataSet ds = new DataSet();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand("admGetInviteFrequency", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        DataTable emp = new DataTable("Freq");
                        emp.Load(dr);
                        ds.Tables.Add(emp);
                    }
                }
            }
            return ds;
        }

        private void PopulateSearchFlds() 
        {
            repSearchFields.DataSource = GetSearchFldsDataset();
            repSearchFields.DataBind();
        }

        private DataSet GetSearchFldsDataset()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("SearchFlds");
            dt.Columns.Add("FldNum");
            dt.Columns.Add("Display");

            dt.Rows.Add("All", "All Fields");
            ds.Tables.Add(dt);

            int z = 0;
            while (z < grdInvites.Columns.Count) 
            {
                if (grdInvites.Columns[z].Visible)
                {
                    string fldType = grdInvites.Columns[z].GetType().ToString();
                    if (fldType == "System.Web.UI.WebControls.TemplateField")
                    {
                        TemplateField tf = (TemplateField)grdInvites.Columns[z];
                        if (tf.SortExpression.Length > 0)
                        {
                            dt.Rows.Add(tf.SortExpression, tf.HeaderText);
                        }
                    }
                    else
                    {
                        BoundField bf = (BoundField)grdInvites.Columns[z];
                        if (bf.SortExpression.Length > 0)
                        {
                            dt.Rows.Add(bf.SortExpression, bf.HeaderText);
                        }
                    }
                }
                z++;
            }

            return ds;
        }

        protected void grdInvites_PreRender(object sender, EventArgs e)
        {
            if (grdInvites.Rows.Count > 0) 
            {
                grdInvites.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void grdInvites_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button selectButton = (Button)e.Row.FindControl("SelectButton");
                int colCount = grdInvites.Columns.Count;
                for (int i = 3; i < colCount; i++)
                {
                    e.Row.Cells[i].Attributes["OnClick"] = ClientScript.GetPostBackEventReference(selectButton, "");
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            
            ViewState["SearchFor"] = btnSearch.CommandArgument;
            ViewState["SearchValue"] = txtSearch.Text;
            PopulateInviteGrid();
            
            if (grdInvites.Rows.Count == 1)
            {
                grdInvites.SelectedIndex = 0;
                int inviteID = (int)grdInvites.SelectedDataKey.Value;
                PopulateInviteInfo(inviteID);
            } 
            else
            {
                grdInvites.SelectedIndex = -1;
                pnlInviteInfo.Visible = false;
            }
        }

        protected void UpdateSearch_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            if (lb.CommandName == "UpdateSearchFld")
            {
                btnSearch.CommandArgument = lb.CommandArgument;
                btnSearch.Text = "Search in: " + lb.Text;

                ViewState["SearchFor"] = btnSearch.CommandArgument;
                ViewState["SearchValue"] = txtSearch.Text;
                PopulateInviteGrid();

                if (grdInvites.Rows.Count == 1)
                {
                    grdInvites.SelectedIndex = 0;
                    int inviteID = (int)grdInvites.SelectedDataKey.Value;
                    PopulateInviteInfo(inviteID);
                }
                else
                {
                    grdInvites.SelectedIndex = -1;
                    pnlInviteInfo.Visible = false;
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            btnSearch.CommandArgument = "All";
            btnSearch.Text = "Search in: All Fields";
            ViewState["SearchFor"] = btnSearch.CommandArgument;
            ViewState["SearchValue"] = "";
            txtSearch.Text = "";
            PopulateInviteGrid();
            pnlInviteInfo.Visible = false;
            grdInvites.SelectedIndex = -1;
        }

        private void InitializeForm()
        {
            lblFullName.Attributes.Add("for", txtFullName.ClientID.ToString());
            lblInviteName.Attributes.Add("for", txtInviteName.ClientID.ToString());
            lblEmail.Attributes.Add("for", txtEmail.ClientID.ToString());

            PopulateDropDown(ddEvent, "RSVPGetEvents");
            PopulateDropDown(ddLangName, "tmGetLang");

            for (int i = 0; i <= 10; i++)
            {
                if (i == 0)
                    ddNumPeople.Items.Add(new ListItem("Please Select...", i.ToString()));
                else
                    ddNumPeople.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            ddAttending.Items.Add(new ListItem("Please Select...", "0"));
            ddAttending.Items.Add(new ListItem("Yes", "1"));
            ddAttending.Items.Add(new ListItem("No", "2"));
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
                            dd.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
                        }
                    }
                }
            }
        }

        private DataSet GetInviteInfo(int inviteID)
        {
            DataSet ds = new DataSet();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand("admGetInviteInfo", con))
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

        protected void grdInvites_SelectedIndexChanged(object sender, EventArgs e)
        {
            int inviteID = (int)grdInvites.SelectedDataKey.Value;
            PopulateInviteInfo(inviteID);
        }

        private void PopulateInviteInfo(int inviteID)
        {
            DataSet ds = GetInviteInfo(inviteID);
            //Populate Information into Variables
            DateTime submitDate = DateTime.MinValue;
            string iso = "", fullName = "", inviteName = "", email = "", comment = "";
            int eventID = 0, numPeople = 0;
            bool attending = false;
            if (ds.Tables["Invite"].Rows[0]["FullName"] != DBNull.Value)
                fullName = (string)ds.Tables["Invite"].Rows[0]["FullName"];
            if (ds.Tables["Invite"].Rows[0]["InviteName"] != DBNull.Value)
                inviteName = (string)ds.Tables["Invite"].Rows[0]["InviteName"];
            if (ds.Tables["Invite"].Rows[0]["Email"] != DBNull.Value)
                email = (string)ds.Tables["Invite"].Rows[0]["Email"];
            if (ds.Tables["Invite"].Rows[0]["EventID"] != DBNull.Value)
                eventID = (int)ds.Tables["Invite"].Rows[0]["EventID"];
            if (ds.Tables["Invite"].Rows[0]["NumPeople"] != DBNull.Value)
                numPeople = (int)ds.Tables["Invite"].Rows[0]["NumPeople"];
            if (ds.Tables["Invite"].Rows[0]["SubmitDate"] != DBNull.Value)
                submitDate = (DateTime)ds.Tables["Invite"].Rows[0]["SubmitDate"];
            if (ds.Tables["Invite"].Rows[0]["iso"] != DBNull.Value)
                iso = (string)ds.Tables["Invite"].Rows[0]["iso"];
            if (ds.Tables["Invite"].Rows[0]["Attending"] != DBNull.Value)
                attending = (bool)ds.Tables["Invite"].Rows[0]["Attending"];
            if (ds.Tables["Invite"].Rows[0]["Comment"] != DBNull.Value)
                comment = (string)ds.Tables["Invite"].Rows[0]["Comment"];
            string rsvpLink = (string)ds.Tables["Invite"].Rows[0]["RSVPLink"];

            //Text Boxes
            txtFullName.Text = fullName;
            txtInviteName.Text = inviteName;
            txtEmail.Text = email;

            //Drop Downs
            if (attending == true && submitDate != DateTime.MinValue)
                ddAttending.SelectedIndex = 1;
            else if (attending == false && submitDate != DateTime.MinValue)
                ddAttending.SelectedIndex = 2;
            else
                ddAttending.SelectedIndex = 0;

            if (attending)
                pnlEventNumPeople.Visible = true;
            else
                pnlEventNumPeople.Visible = false;

            ListItem liEvent = ddEvent.Items.FindByValue(eventID.ToString());
            ddEvent.SelectedIndex = ddEvent.Items.IndexOf(liEvent);

            ListItem liNumPeople = ddNumPeople.Items.FindByValue(numPeople.ToString());
            ddNumPeople.SelectedIndex = ddNumPeople.Items.IndexOf(liNumPeople);

            ListItem liLang = ddLangName.Items.FindByValue(iso);
            ddLangName.SelectedIndex = ddLangName.Items.IndexOf(liLang);

            //Labels
            if (submitDate != DateTime.MinValue)
                lblSubmitDate.Text = submitDate.ToShortDateString();
            else
                lblSubmitDate.Text = "Not Submitted";
            lblComment.Text = comment;
            lblRSVPLink.Text = rsvpLink;
            lnkRSVPLink.NavigateUrl = rsvpLink;

            //QR Code
            imgQRCode.ImageUrl = "~/img/qrcodes/" + inviteID.ToString() + ".jpg";

            //Show The Form
            pnlInviteInfo.Visible = true;
        }

        protected void ddAttending_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddAttending.SelectedIndex == 1)
                pnlEventNumPeople.Visible = true;
            else
            {
                pnlEventNumPeople.Visible = false;
                ddEvent.SelectedIndex = 0;
                ddNumPeople.SelectedIndex = 0;
            }
                
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlInviteInfo.Visible = false;
            grdInvites.SelectedIndex = -1;
            btnUpdate.Text = "Update";
            btnUpdate.CommandArgument = "Update";
            pnlGridViewInfoHeader.Visible = true;
            pnlGridViewInvites.Visible = true;
            pnlSearch.Visible = true;
            pnlLabels.Visible = true;
            btnGenQRCode.Visible = true;
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            pnlInviteInfo.Visible = true;
            grdInvites.SelectedIndex = -1;
            btnUpdate.Text = "Submit";
            btnUpdate.CommandArgument = "Insert";
            pnlGridViewInfoHeader.Visible = false;
            pnlGridViewInvites.Visible = false;
            pnlSearch.Visible = false;
            pnlLabels.Visible = false;
            btnGenQRCode.Visible = false;

            ddNumPeople.SelectedIndex = 0;
            ddEvent.SelectedIndex = 0;
            ddLangName.SelectedIndex = 0;
            ddAttending.SelectedIndex = 0;
            txtFullName.Text = "";
            txtInviteName.Text = "";
            txtEmail.Text = "";

            txtFullName.Focus();
        }

        private void UpdateInvite(int inviteID, string fullName, string inviteName, string email, int eventID, int numPeople, string iso)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand("admUpdateInvite", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InviteID", inviteID);
                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@InviteName", inviteName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@EventID", eventID);
                cmd.Parameters.AddWithValue("@NumPeople", numPeople);
                cmd.Parameters.AddWithValue("@iso", iso);
                if (ddAttending.SelectedIndex == 1)
                    cmd.Parameters.AddWithValue("@Attending", true);
                else
                    cmd.Parameters.AddWithValue("@Attending", false);

                if (ddAttending.SelectedIndex > 0) 
                {
                    cmd.Parameters.AddWithValue("@UpdateSubmit", true);
                    lblSubmitDate.Text = DateTime.Now.ToShortDateString();
                }
                else
                {
                    cmd.Parameters.AddWithValue("@UpdateSubmit", false);
                    lblSubmitDate.Text = "Not Submitted";
                }
                    
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        private void AddInvite(string fullName, string inviteName, string email, int eventID, int numPeople, string iso, ref int inviteID)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand("admAddInvite", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@InviteName", inviteName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@EventID", eventID);
                cmd.Parameters.AddWithValue("@NumPeople", numPeople);
                cmd.Parameters.AddWithValue("@iso", iso);
                if (eventID == 1)
                    cmd.Parameters.AddWithValue("@Attending", true);
                else
                    cmd.Parameters.AddWithValue("@Attending", false);

                if (ddAttending.SelectedIndex > 0)
                {
                    cmd.Parameters.AddWithValue("@UpdateSubmit", true);
                    lblSubmitDate.Text = DateTime.Now.ToShortDateString();
                }
                else
                {
                    cmd.Parameters.AddWithValue("@UpdateSubmit", false);
                    lblSubmitDate.Text = "Not Submitted";
                }
                cmd.Parameters.Add("@InviteID",SqlDbType.Int);
                cmd.Parameters["@InviteID"].Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();
                inviteID = (int)cmd.Parameters["@InviteID"].Value;
                con.Close();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text, inviteName = txtInviteName.Text, email = txtEmail.Text, iso = ddLangName.SelectedValue;
            int eventID = Convert.ToInt32(ddEvent.SelectedValue), numPeople = Convert.ToInt32(ddNumPeople.SelectedValue);

            if (btnUpdate.CommandArgument == "Update")
            {
                int inviteID = (int)grdInvites.SelectedDataKey.Value;
                UpdateInvite(inviteID, fullName, inviteName, email, eventID, numPeople, iso);

                SetPageAndIndexByInvite(inviteID);
            }
            else if (btnUpdate.CommandArgument == "Insert")
            {
                int inviteID = 0;
                AddInvite(fullName, inviteName, email, eventID, numPeople, iso, ref inviteID);

                if (inviteID > 0)
                {
                    string inviteURL = "http://Adam-and-Christine.com/rsvp/" + inviteID.ToString();
                    GenerateQRCodeImg(inviteID, inviteURL);
                }
                pnlInviteInfo.Visible = false;
                grdInvites.SelectedIndex = -1;
                btnUpdate.Text = "Update";
                btnUpdate.CommandArgument = "Update";
                pnlGridViewInfoHeader.Visible = true;
                pnlGridViewInvites.Visible = true;
                pnlSearch.Visible = true;
                pnlLabels.Visible = true;
                btnGenQRCode.Visible = true;
                
            }
            PopulateFreqData();
            PopulateInviteGrid();
        }

        private void DeleteInvite(int inviteID)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            using (SqlCommand cmd = new SqlCommand("admDeleteInvite", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InviteID", inviteID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        protected void grdInvites_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int inviteID = (int)grdInvites.DataKeys[e.RowIndex].Value;
            if (grdInvites.SelectedIndex == e.RowIndex)
                grdInvites.SelectedIndex = -1;

            DeleteInvite(inviteID);
            PopulateInviteGrid();
            PopulateFreqData();
            pnlInviteInfo.Visible = false;
        }

        protected void grdInvites_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdInvites.PageIndex = e.NewPageIndex;
            grdInvites.SelectedIndex = -1;
            PopulateInviteGrid();
        }

        protected void grdInvites_PageIndexChanged(object sender, EventArgs e)
        {
            pnlInviteInfo.Visible = false;
        }

        protected void ddRows_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dd = (DropDownList)sender;
            grdInvites.PageSize = Convert.ToInt32(dd.SelectedValue);
            if (grdInvites.SelectedIndex != -1) 
            {
                int inviteID = (int)grdInvites.SelectedDataKey.Value;
                SetPageAndIndexByInvite(inviteID);
            }
            else
                grdInvites.PageIndex = 0;

            PopulateInviteGrid();
        }

        private void SetPageAndIndexByInvite (int inviteID)
        {
            bool exit = false;
            for (int i = 0; i < grdInvites.PageCount; i++)
                {
                    grdInvites.PageIndex = i;
                    PopulateInviteGrid();

                    foreach (GridViewRow r in grdInvites.Rows)
                    {
                        if (inviteID == (int)grdInvites.DataKeys[r.RowIndex].Value)
                        {
                            grdInvites.SelectedIndex = r.RowIndex;
                            exit = true;
                            break;
                        }
                    }
                    if (exit) break;
                }
        }

        protected void btnGenQRCode_Click(object sender, EventArgs e)
        {
            int inviteID = (int)grdInvites.SelectedDataKey.Value;
            string inviteURL = lnkRSVPLink.NavigateUrl;
            GenerateQRCodeImg(inviteID, inviteURL);
            imgQRCode.ImageUrl = "~/img/qrcodes/" + inviteID.ToString() + ".jpg";
        }

        private void GenerateQRCodeImg(int inviteID, string qrInfo)
        {
            QRCodeEncoder qrEncode = new QRCodeEncoder();
            Bitmap img = qrEncode.Encode(qrInfo);
            string fullPath = Server.MapPath("~/img/qrcodes/") + inviteID.ToString() + ".jpg";
            img.Save(fullPath, ImageFormat.Jpeg);
        }

        protected void btnGenQRCodesAll_Click(object sender, EventArgs e)
        {
            DataSet inviteDS = GetInviteDataSet("admGetAllInvites", "", "", "");

            try
            {
                foreach (DataRow row in inviteDS.Tables[0].Rows)
                {
                    int inviteID = Convert.ToInt32(row["InviteID"]);
                    string rsvpLink = row["RSVPLink"].ToString();
                    GenerateQRCodeImg(inviteID, rsvpLink);
                }
                lblQRCodesMessage.Text = "Success!";
            }
            catch (Exception ex)
            {
                lblQRCodesMessage.Text = "Error! " + ex.Message;
                lblQRCodesMessage.ForeColor = System.Drawing.Color.Red;
            }
            finally
            {
                lblQRCodesMessage.Visible = true;
            }

        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin/invitations_printmulti.aspx");
        }

        protected void grdInvites_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Print")
            {
                int inviteID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("~/admin/print.aspx?i=" + inviteID.ToString());
            }
        }

        protected void grdInvites_Sorting(object sender, GridViewSortEventArgs e)
        {
            int inviteID = 0;
            string[] sortOrder = ViewState["SortExpr"].ToString().Split(' ');
            if (sortOrder[0] == e.SortExpression)
            {
                if (sortOrder[1] == "ASC")
                    ViewState["SortExpr"] = e.SortExpression + " DESC";
                else
                    ViewState["SortExpr"] = e.SortExpression + " ASC";
            }
            else
            {
                ViewState["SortExpr"] = e.SortExpression + " ASC";
            }

            if (grdInvites.SelectedIndex != -1)
                inviteID = (int)grdInvites.SelectedDataKey.Value;

            PopulateInviteGrid();

            if (inviteID > 0)
            {
                SetPageAndIndexByInvite(inviteID);
                PopulateInviteGrid();
            }
                
        }

        protected void btnExcelDownload_Click(object sender, EventArgs e)
        {
            string searchFor = (string)ViewState["SearchFor"];
            string searchValue = (string)ViewState["SearchValue"];
            string sortExpression = (string)ViewState["SortExpr"];
            string excelName = "Invites-" + DateTime.Now.ToLocalTime() + ".xls";
            DataSet ds = GetInviteDataSet("admGetAllInvitesDownload", searchFor, searchValue, sortExpression);
            GridView gv = new GridView();
            if (ds.Tables.Count > 0)
            {
                gv.DataSource = ds;
                gv.DataBind();
                ExportToExcel(excelName, gv);
            }

            grdInvites.DataBind();
        }

        private void ExportToExcel(string fileName, GridView gv)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write("<head><meta http-equiv=Content-Type content=:" + '"' + "text/html; charset=utf-8" + '"' + "></head>");


            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    Table table = new Table();

                    if (gv.HeaderRow != null)
                    {
                        PrepareControlForExport(gv.HeaderRow);
                        table.Rows.Add(gv.HeaderRow);
                    }

                    foreach (GridViewRow row in gv.Rows)
                    {
                        PrepareControlForExport(row);
                        table.Rows.Add(row);
                    }

                    if (gv.FooterRow != null)
                    {
                        PrepareControlForExport(gv.FooterRow);
                        table.Rows.Add(gv.FooterRow);
                    }
                    table.GridLines = gv.GridLines;
                    table.RenderControl(htw);

                    HttpContext.Current.Response.Write(sw.ToString());
                    HttpContext.Current.Response.End();
                }
            }
        }

        private void PrepareControlForExport(Control control)
        {
            for (int i = 0; i <= control.Controls.Count - 1; i++)
            {
                Control current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                }

                if (current.HasControls())
                {
                    PrepareControlForExport(current);
                }
            }
        }
    }
}