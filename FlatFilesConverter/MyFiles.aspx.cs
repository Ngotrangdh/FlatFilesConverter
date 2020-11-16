using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using FlatFilesConverter.Data;
using FlatFilesConverter.Business.Config;
using FlatFilesConverter.Business.Export;
using FlatFilesConverter.Business.Services;
using System.Linq;
using System.Web.UI;

namespace FlatFilesConverter
{
    public partial class MyFiles : Page
    {
        private FileService FileService => new FileService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] is null)
            {
                Response.Redirect($"~/Login?ReturnURL={Request.Url}");
            }

            if (!IsPostBack)
            {
                GetData();
            }
        }

        protected void ButtonDownloadCSV_Click(object sender, EventArgs e)
        {
            DownloadFile(new CSVMapper(), "csv");
        }

        protected void ButtonDownloadFixedWidth_Click(object sender, EventArgs e)
        {
            DownloadFile(new FixedWidthMapper(), "dat");
        }

        protected void GridViewFileList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            List<File> files = GetFiles();
            File file = files.ElementAt(e.RowIndex);
            files.RemoveAt(e.RowIndex);
            FileService.DeleteFile(file.FileName);
            Response.Redirect("~/MyFiles.aspx");
        }

        private void DownloadFile(IMapper mapper, string extension)
        {
            if (Request.QueryString["id"] is string fileName)
            {
                string outputFilePath = Server.MapPath($"Data\\{fileName}.{extension}");
                GenerateFile(fileName, outputFilePath, mapper);
            }
        }

        private void GetData()
        {
            List<File> files = GetFiles();

            if (files.Any())
            {
                GridViewFileList.DataSource = files;
                GridViewFileList.DataBind();

                if (Request.QueryString["id"] is string fileName)
                {
                    ShowFileDetails(fileName);
                }
                else
                {
                    LabelNoTableChoosen.Text = "Please select a file";
                }
            }
            else
            {
                LabelNoFileUploaded.Text = "You have no files.";
            }
        }

        private void ShowFileDetails(string fileName)
        {
            ButtonDownloadCSV.Visible = true;
            ButtonDownloadFixedWidth.Visible = true;
            GridViewFileView.DataSource = FileService.GetFileTable(fileName);
            GridViewFileView.DataBind();
        }

        private List<File> GetFiles()
        {
            int userID = int.Parse(Session["userID"].ToString());
            List<File> files = FileService.GetFileList(userID);
            return files;
        }

        private void GenerateFile(string fileName, string outputFilePath, IMapper mapper)
        {
            DataTable table = FileService.GetFileTable(fileName).Tables[0];
            string jsonConfig = FileService.GetFileConfiguration(fileName);
            Configuration config = JsonConvert.DeserializeObject<Configuration>(jsonConfig);
            var exporter = new Exporter(mapper);
            exporter.Export(table, outputFilePath, config);
            Response.Redirect($"DownloadFile.ashx?filePath={outputFilePath}");
        }

    }
}