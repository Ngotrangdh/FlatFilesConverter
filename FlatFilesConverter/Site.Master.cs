using System;
using System.Web.UI;

namespace FlatFilesConverter
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isUserLoggedIn = Session["userID"] != null;
            LinkMyFiles.Visible = isUserLoggedIn;
            LinkLogin.Visible = !isUserLoggedIn;
            LinkRegister.Visible = !isUserLoggedIn;
            LinkLogout.Visible = isUserLoggedIn;

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