using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FlatFilesConverter
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            var username = TextBoxLoginUsername.Text;
            var password = TextBoxLoginPassword.Text;
            int userID = 0;

            string connectionString = "Server=localhost\\SQLEXPRESS;Database=FlatFilesConverter;Trusted_Connection=True;";
            string selectCommand = $"Select UserID from dbo.Login where UserName='{username}' and Password='{password}'";
            string returnPath = Request.QueryString["ReturnURL"];

            // try catch any exception during connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(selectCommand, conn))
                {
                    conn.Open();
                    var lookUpError = comm.ExecuteScalar();
                    if (lookUpError == null)
                    {
                        return;
                    }
                    else
                    {
                        userID = (int)lookUpError;
                    }
                }
            }
            if (userID != 0)
            {
                Session["UserID"] = userID;
            }

            if (returnPath == null)
            {

                Response.Redirect("Default.aspx");
            }
            else
            {
                Response.Redirect(returnPath);
            }

        }

    }
}