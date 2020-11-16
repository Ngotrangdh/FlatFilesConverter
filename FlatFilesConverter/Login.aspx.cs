using System;
using System.Text;
using System.Web;
using System.Web.Security;
using FlatFilesConverter.Data;
using FlatFilesConverter.Business.Services;

namespace FlatFilesConverter
{
    public partial class Login : System.Web.UI.Page
    {
        private UserService UserService => new UserService();
        private string ReturnPath => Request.QueryString["ReturnURL"] ?? "/";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] is null && Request.Cookies["username"] is HttpCookie usernameCookie)
            {
                string username = Unprotect(usernameCookie.Value, "identity");
                if (!string.IsNullOrEmpty(username))
                {
                    User user = UserService.GetUser(username);
                    if (user != null)
                    {
                        Session["userID"] = user.UserID;
                        Response.Redirect(ReturnPath);
                    }
                }
            }
        }

        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            string username = TextBoxLoginUsername.Text;
            string password = TextBoxLoginPassword.Text;
            User user = new User { Username = username, Password = password };
            int userID = UserService.AuthenticateUser(user);

            if (userID > 0)
            {
                RememberUser(username, userID);
            }
            else
            {
                DivLoginError.Visible = true;
                LoginErrorMessages.Text = "Incorrect username or password";
                return;
            }

            Response.Redirect(ReturnPath);
        }

        private void RememberUser(string username, int userID)
        {
            Session["userID"] = userID;

            if (CheckBoxRememberMe.Checked)
            {
                string protectedUserID = Protect(username, "identity");
                HttpCookie cookie = new HttpCookie("username", protectedUserID)
                {
                    Expires = DateTime.Now.AddDays(2)
                };
                Response.Cookies.Add(cookie);
            }
        }

        public static string Protect(string text, string purpose)
        {
            if (string.IsNullOrEmpty(text)) return null;

            byte[] stream = Encoding.UTF8.GetBytes(text);
            byte[] encodedValue = MachineKey.Protect(stream, purpose);
            return Convert.ToBase64String(encodedValue);
        }

        public static string Unprotect(string text, string purpose)
        {
            if (string.IsNullOrEmpty(text)) return null;

            try
            {
                byte[] stream = Convert.FromBase64String(text);
                byte[] decodedValue = MachineKey.Unprotect(stream, purpose);
                return Encoding.UTF8.GetString(decodedValue);
            }
            catch
            {
                return null;
            }
        }
    }
}