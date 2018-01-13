using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Adam_and_Christine_C
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int inviteID = 0;
                string redirectUrl, directoryListing, iso = Request.QueryString["l"];

                if (Request.QueryString["i"] != null)
                    Int32.TryParse(Request.QueryString["i"].ToString(), out inviteID);

                directoryListing = Request.Path.ToString().Replace(".aspx", "");
                directoryListing = directoryListing.Replace("/", "");

                if (!Request.IsLocal && !Request.IsSecureConnection)
                {
                    if (inviteID > 0)
                    {
                        if (iso != null)
                            redirectUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + directoryListing + "/" + inviteID.ToString() + "-" + iso;
                        else
                            redirectUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + directoryListing + "/" + inviteID.ToString();
                    }
                    else
                    {
                        if (directoryListing == "default")
                            redirectUrl = Request.Url.GetLeftPart(UriPartial.Authority);
                        else
                            redirectUrl = Request.Url.ToString();
                    }
                    redirectUrl = redirectUrl.ToString().Replace("http:", "https:");
                    Response.Redirect(redirectUrl);
                }
            }
        }
    }
}