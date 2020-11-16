using System;
using System.Web.UI;

namespace FlatFilesConverter
{
    public partial class ErrorPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();

            if (exc != null)
            {
                ServerError.Text = "An error has occured. Please try again.";
            }
        }
    }
}