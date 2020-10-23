using System;
using System.Web.UI;

namespace FlatFilesConverter
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["Username"] == null)
            {
                Response.Redirect("Login.aspx?ReturnURL=About.aspx");
            }
        }
    }
}