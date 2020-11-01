using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FlatFilesConverter
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();

            if (exc != null)
            {
                Error.Text = exc.InnerException.Message;
            }
        }
    }
}