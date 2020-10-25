using System;
using System.Web;

namespace FlatFilesConverter
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["username"] is HttpCookie usernameCookie)
            {
                Session.Abandon();
                usernameCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(usernameCookie);
                Response.Redirect("~/Login.aspx", true);
            }
            
            Response.Redirect("~/Default.aspx");
        }
    }
}