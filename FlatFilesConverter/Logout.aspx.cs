using System;
using System.Web;

namespace FlatFilesConverter
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Abandon();

            if (Request.Cookies["username"] is HttpCookie usernameCookie)
            {
                usernameCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(usernameCookie);
            }

            Response.Redirect("~/Default.aspx");
        }
    }
}