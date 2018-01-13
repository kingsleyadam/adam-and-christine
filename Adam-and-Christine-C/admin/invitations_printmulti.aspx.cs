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
    public partial class invitations_printmulti : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Set Default ViewState Values
                ViewState["SearchFor"] = "";
                ViewState["SearchValue"] = "";
                ViewState["SortExpr"] = "FullName ASC";

                //Populate Objects
                PopulateInviteGrid();
            }
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
                grdInvites.CssClass = "table";
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

        protected void grdInvites_PreRender(object sender, EventArgs e)
        {
            if (grdInvites.Rows.Count > 0)
            {
                grdInvites.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void grdInvites_Sorting(object sender, GridViewSortEventArgs e)
        {
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

            PopulateInviteGrid();
        }

        protected void btnPrintAll_Click(object sender, EventArgs e)
        {
            string inviteIDs ="";
            foreach (GridViewRow dr in grdInvites.Rows)
            {
                CheckBox cb = (CheckBox)dr.Cells[0].FindControl("chkPrint");
                if (cb != null)
                {
                    if (cb.Checked)
                    {
                        inviteIDs = inviteIDs + grdInvites.DataKeys[dr.RowIndex].Value.ToString() + ",";
                    }
                }
            }

            
            if (inviteIDs.Length > 0)
            {
                inviteIDs = inviteIDs.Substring(0, inviteIDs.Length - 1);
                Response.Redirect("~/admin/print.aspx?i=" + inviteIDs);
            }
            else
            {
                Response.Redirect("~/admin/print.aspx");
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            btnSearch.CommandArgument = "All";
            ViewState["SearchFor"] = btnSearch.CommandArgument;
            ViewState["SearchValue"] = "";
            txtSearch.Text = "";
            PopulateInviteGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            ViewState["SearchFor"] = btnSearch.CommandArgument;
            ViewState["SearchValue"] = txtSearch.Text;
            PopulateInviteGrid();
        }

        protected void chkAll_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow dr in grdInvites.Rows)
            {
                CheckBox cb = (CheckBox)dr.Cells[0].FindControl("chkPrint");
                if (cb != null)
                {
                    if (cb.Checked)
                        cb.Checked = false;
                    else
                        cb.Checked = true;
                }
            }
        }
    }
}