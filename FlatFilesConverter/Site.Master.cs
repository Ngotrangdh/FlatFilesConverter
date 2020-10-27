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
            if (Page.Title.Contains("CSV To Fixed Width"))
            {
                LinkCSVToFixedWidth.Attributes["class"] = "active";
            }
            if (Page.Title.Contains("Fixed Width To CSV"))
            {
                LinkFixedWithToCSV.Attributes["class"] = "active";
            }

            if (Session["userID"] != null)
            {
                LinkLogin.Visible = false;
                LinkRegister.Visible = false;
                LinkLogout.Visible = true;
                return;
            }
            else if (Request.Cookies["username"] is HttpCookie usernameCookie)
            {
                string username = Unprotect(usernameCookie.Value, "identity");
                int userID = UserService.HasUser(username);
                if (!string.IsNullOrEmpty(username))
                {
                    if (userID != 0)
                    {
                        Session["userID"] = userID;
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