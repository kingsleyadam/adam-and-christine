using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace Adam_and_Christine_C.admin
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Roles.IsUserInRole("Admin"))
                    Response.Redirect("~/admin/invitations.aspx");
                else if (Roles.IsUserInRole("translations"))
                    Response.Redirect("~/admin/translations.aspx");
            }
        }
    }
}