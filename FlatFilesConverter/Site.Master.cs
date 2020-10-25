using FlatFilesConverter.Business.Authentication;
using System;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace FlatFilesConverter
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] != null)
            {
                LinkLogin.Visible = false;
                LinkRegister.Visible = false;
                LinkLogout.Visible = true;
                return;
            }
            else if (Request.Cookies["username"] is HttpCookie usernameCookie)
            {
                string username = Unprotect(usernameCookie.Value, "identity");
                if (!string.IsNullOrEmpty(username))
                {
                    if (UserService.HasUser(username))
                    {
                        Session["username"] = username;
                        return;
                    }
                }
            }

            Response.Redirect($"~/Login.aspx?ReturnURL={Request.Url}");
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