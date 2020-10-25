using FlatFileConverter.Data;
using FlatFilesConverter.Business.Authentication;
using System.Web;
using System;
using System.Web.Security;
using System.Text;

namespace FlatFilesConverter
{
    public partial class Login : System.Web.UI.Page
    {
        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            var username = TextBoxLoginUsername.Text;
            var password = TextBoxLoginPassword.Text;
            bool isPersistentCookie = CheckBoxRememberMe.Checked;
            User user = new User() { Username = username, Password = password };

            string returnPath = Request.QueryString["ReturnURL"];

            if (UserService.IsAuthenticated(user))
            {
                Session["username"] = username;

                if (isPersistentCookie)
                {
                    string protectedUsername = Protect(username, "identity");
                    HttpCookie cookie = new HttpCookie("username", protectedUsername)
                    {
                        Expires = DateTime.Now.AddDays(2)
                    };
                    Response.Cookies.Add(cookie);
                }
            }
            else
            {
                DivLoginError.Visible = true;
                LoginErrorMessages.Text = ("Incorrect username or password");
                return;
            }

            if (returnPath == null)
            {

                Response.Redirect("~/Default.aspx");
            }
            else
            {
                Response.Redirect(returnPath);
            }
        }

        public static string Protect(string text, string purpose)
        {
            if (string.IsNullOrEmpty(text)) return null;

            byte[] stream = Encoding.UTF8.GetBytes(text);
            byte[] encodedValue = MachineKey.Protect(stream, purpose);
            return Convert.ToBase64String(encodedValue);
        }
    }
}