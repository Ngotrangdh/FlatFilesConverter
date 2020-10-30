using System;
using System.Data;
using System.Collections.Generic;
using Newtonsoft;
using FlatFileConverter.Data;
using FlatFilesConverter.Business.Services;
using Newtonsoft.Json;
using FlatFilesConverter.Business.Config;
using FlatFilesConverter.Business.Export;

namespace FlatFilesConverter
{
    public partial class MyFiles : System.Web.UI.Page
    {
        private FileService FileService => new FileService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] != null)
            {
                int userID = int.Parse(Session["userID"].ToString());
                List<File> files = FileService.GetFileList(userID);

                if (files.Count != 0)
                {
                    GridViewFileList.DataSource = files;
                    GridViewFileList.DataBind();

                    if (Request.QueryString["id"] is string tableName)
                    {
                        ButtonDownloadCSV.Visible = true;
                        ButtonDownloadFixedWidth.Visible = true;
                        GridViewFileView.DataSource = FileService.GetFileTable(tableName);
                        GridViewFileView.DataBind();
                    }
                    else
                    {
                        LabelNoTableChoosen.Text = "Please select a table on the left.";
                    }
                }
                else
                {
                    LabelNoFileUploaded.Text = "You have not uploaded any files.";
                }
            }
        }

        protected void ButtonDownloadCSV_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] is string fileName)
            {
                var CSVMapper = new CSVMapper();
                string outputFilePath = Server.MapPath($"Data\\{fileName}.csv");
                GenerateDownloadedCSVFile(fileName, CSVMapper, outputFilePath);
            }
        }

        protected void ButtonDownloadFixedWidth_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] is string fileName)
            {
                var fixedWidthMapper = new FixedWidthMapper();
                string outputFilePath = Server.MapPath($"Data\\{fileName}.dat");
                GenerateDownloadedCSVFile(fileName, fixedWidthMapper, outputFilePath);
            }
        }

        protected private void GenerateDownloadedCSVFile(string fileName, IMapper mapper, string outputFilePath)
        {
            DataTable table = FileService.GetFileTable(fileName).Tables[0];
            string jSONConfig = FileService.GetFileConfiguration(fileName);
            Configuration config = JsonConvert.DeserializeObject<Configuration>(jSONConfig);

            var writer = new Writer();
            var exporter = new Exporter(mapper, writer);
            exporter.Export(table, outputFilePath, config);
            Response.Redirect($"DownloadFile.ashx?filePath={outputFilePath}");
        }

    }
}