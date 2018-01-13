using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Adam_and_Christine_C
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Button btnLogin = (Button)lgnForm.FindControl("LoginButton");
            TextBox txtUsername = (TextBox)lgnForm.FindControl("UserName");

            if (btnLogin != null)
                pnlLoginForm.DefaultButton = btnLogin.UniqueID;
            if (txtUsername != null)
                txtUsername.Focus();
        }
    }
}