using System;
using System.Web.UI;

namespace FlatFilesConverter
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx?ReturnURL=About.aspx");
            }
        }
    }
}