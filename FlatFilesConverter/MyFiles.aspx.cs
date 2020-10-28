using System;
using System.Collections.Generic;
using FlatFileConverter.Data;
using FlatFilesConverter.Business.Services;

namespace FlatFilesConverter
{
    public partial class MyFiles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] != null)
            {
                int userID = int.Parse(Session["userID"].ToString());
                List<File> files = FileService.GetFileList(userID);
                GridViewFileList.DataSource = files;
                GridViewFileList.DataBind();
            }
        }
    }
}