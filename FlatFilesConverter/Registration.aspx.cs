using System;
using FlatFilesConverter.Data;
using FlatFilesConverter.Business.Services;

namespace FlatFilesConverter
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonRegister_Click(object sender, EventArgs e)
        {
            UserService UserService = new UserService();
            User user = new User() { Username = TextBoxRegisterUsername.Text, Password = TextBoxRegisterPassword.Text };
            bool isRegistered = UserService.RegisterUser(user);

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