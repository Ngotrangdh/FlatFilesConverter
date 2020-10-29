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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] != null)
            {
                int userID = int.Parse(Session["userID"].ToString());
                List<File> files = FileService.GetFileList(userID);
                GridViewFileList.DataSource = files;
                GridViewFileList.DataBind();

                if(Request.QueryString["id"] is string tableName)
                {
                    ButtonDownloadCSV.Visible = true;
                    ButtonDownloadFixedWidth.Visible = true;
                    GridViewFileView.DataSource = FileService.GetFileTable(tableName);
                    GridViewFileView.DataBind();
                }
                else
                {
                    LabelNoTableChoosen.Visible = true;
                    LabelNoTableChoosen.Text = "Please select a table on the left";
                }


                //BulletedList1.DataSource = files.Select(file => new
                //{
                //    Text = $"{file.FileName} - {file.CreatedDate}",
                //    URL = $"MyFiles?id={file.FileName}"
                //});

                //BulletedList1.DataTextField = "Text";
                //BulletedList1.DataValueField = "URL";
                //BulletedList1.DisplayMode = BulletedListDisplayMode.HyperLink;
                //BulletedList1.CssClass = "files-list";
                //BulletedList1.DataBind();

                //var item = new ListItem("AAA BBB CCC");
                //item.Attributes.Add("class", "list-group-item");

                //BulletedList1.CssClass = "list-group";
                //BulletedList1.Items.AddRange(new[]
                //{
                //    item
                //});
            }
        }


        protected void ButtonDownloadCSV_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] is string fileName)
            {
                DataTable table = FileService.GetFileTable(fileName).Tables[0];
                string jSONConfig = FileService.GetFileConfiguration(fileName);
                Configuration config = JsonConvert.DeserializeObject<Configuration>(jSONConfig);
                string outputFilePath = Server.MapPath($"Data\\{fileName}.csv");

                var CSVMapper = new CSVMapper();
                var CSVWriter = new Writer();
                var exporter = new Exporter(CSVMapper, CSVWriter);
                exporter.Export(table, outputFilePath, config);
                Response.Redirect($"DownloadFile.ashx?filePath={outputFilePath}");

            }
        }
    }
}