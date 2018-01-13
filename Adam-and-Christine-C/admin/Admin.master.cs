using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Adam_and_Christine_C.admin
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string site = Path.GetFileName(Request.Url.AbsolutePath);

            switch (site)
            {
                case "invitations.aspx":
                    liInvitations.Attributes["class"] = "active lbl";
                    liTranslations.Attributes["class"] = "lbl";
                    break;
                case "translations.aspx":
                    liInvitations.Attributes["class"] = "lbl";
                    liTranslations.Attributes["class"] = "active lbl";
                    break;
            }
        }
    }
}