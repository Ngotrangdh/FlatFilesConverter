using System;
using FlatFileConverter.Data;
using FlatFilesConverter.Business.Authentication;

namespace FlatFilesConverter
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonRegister_Click(object sender, EventArgs e)
        {
            var user = new User() { Username = TextBoxRegisterUsername.Text, Password = TextBoxRegisterPassword.Text };
            var isRegistered = UserService.RegisterUser(user);
            if (isRegistered)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                DivRegisterError.Visible = true;
                RegisterErrorMessages.Text = "Username has already taken!";
                return;
            }
        }
    }
}