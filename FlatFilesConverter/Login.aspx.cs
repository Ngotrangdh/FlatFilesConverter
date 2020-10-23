using FlatFileConverter.Data;
using FlatFilesConverter.Business.Authentication;
using System;

namespace FlatFilesConverter
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] != null)
            {
                Response.Redirect("Default.aspx");
            }
        }

        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            var username = TextBoxLoginUsername.Text;
            var password = TextBoxLoginPassword.Text;
            User user = new User() { Username = username, Password = password };

            string returnPath = Request.QueryString["ReturnURL"];

            
            if (UserService.IsAuthenticated(user))
            {
                Session["Username"] = username;
            }
            else
            {
                DivLoginError.Visible = true;
                LoginErrorMessages.Text = ("Incorrect username or password");
                return;  
            }

            if (returnPath == null)
            {

                Response.Redirect("Default.aspx");
            }
            else
            {
                Response.Redirect(returnPath);
            }
        }
    }
}