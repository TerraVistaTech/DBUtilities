using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ProcessLogin(object sender, EventArgs e)
    {
        if(FormsAuthentication.Authenticate(txtUser.Text, txtPassword.Text))
        {
            Utils.LogGlobal(Request.UserHostAddress + " logged in.");
            FormsAuthentication.RedirectFromLoginPage(txtUser.Text, chkPersistLogin.Checked);
        }
        else
        {
            ErrorMessage.InnerHtml = "<b>Something went wrong...</b> please re-enter your credentials...";
        }
    }
}