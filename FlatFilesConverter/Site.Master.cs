using System;
using System.Web.UI;

namespace FlatFilesConverter
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isUserLoggedIn = Session["userID"] != null;
            if (isUserLoggedIn)
            {
                LinkMyFiles.Visible = true;
                LinkLogin.Visible = false;
                LinkRegister.Visible = false;
                LinkLogout.Visible = true;
            }
            else
            {
                LinkMyFiles.Visible = false;
                LinkLogin.Visible = true;
                LinkRegister.Visible = true;
                LinkLogout.Visible = false;
            }

            switch (Request.Url.AbsolutePath)
            {
                case "/CSVToFixedWidth":
                    LinkCSVToFixedWidth.Attributes["class"] = "active";
                    break;
                case "/FixedWithToCSV":
                    LinkFixedWithToCSV.Attributes["class"] = "active";
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