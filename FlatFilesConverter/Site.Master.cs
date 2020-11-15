using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

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
                    ActivateLink(LinkCSVToFixedWidth);
                    break;
                case "/FixedWidthToCSV":
                    ActivateLink(LinkFixedWidthToCSV);
                    break;
                case "/MyFiles":
                    ActivateLink(LinkMyFiles);
                    break;
            }
        }

        private void ActivateLink(HtmlGenericControl link)
        {
            link.Attributes["class"] = "active";
        }
    }
}