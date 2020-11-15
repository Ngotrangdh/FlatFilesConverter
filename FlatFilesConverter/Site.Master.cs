using System;
using System.Web.UI;

namespace FlatFilesConverter
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] is null)
            {
                Response.Redirect($"~/Login.aspx?ReturnURL={Request.Url}");
            }
            else
            {
                LinkLogin.Visible = false;
                LinkRegister.Visible = false;
                LinkLogout.Visible = true;

                switch (Request.Url.AbsolutePath)
                {
                    case "/CSVToFixedWidth":
                        LinkCSVToFixedWidth.Attributes["class"] = "active";
                        break;
                    case "/FixedWidthToCSV":
                        LinkFixedWidthToCSV.Attributes["class"] = "active";
                        break;
                    case "/MyFiles":
                        LinkMyFiles.Attributes["class"] = "active";
                        break;
                    case "/Logout":
                        LinkLogout.Attributes["class"] = "active";
                        break;
                    case "/Registration":
                        LinkRegister.Attributes["class"] = "active";
                        break;
                }
            }
        }
    }
}