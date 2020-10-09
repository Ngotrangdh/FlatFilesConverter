using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FlatFilesConverter
{
    public partial class CSVToFixedWidth : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SubmitFile_Click(object sender, EventArgs e)
        {
            HttpPostedFile uploadedFile = FileUpload.PostedFile;

            if (uploadedFile.ContentLength > 0 && uploadedFile.FileName.ToLower().EndsWith(".csv"))
            {
                string saveLocation = Server.MapPath($"Data\\{uploadedFile.FileName}");
                try
                {
                    uploadedFile.SaveAs(saveLocation);
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
            else
            {
                Response.Write("Please select a file to upload.");
            }
        }
    }
}