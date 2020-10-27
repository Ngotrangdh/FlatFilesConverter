using FlatFilesConverter.Business.Config;
using FlatFilesConverter.Business.Export;
using FlatFilesConverter.Business.Import;
using FlatFileConverter.Data;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.IO;

namespace FlatFilesConverter
{
    public partial class CSVToFixedWidth : Page
    {
        private List<ColumnLayout> Columns => (List<ColumnLayout>)(ViewState["Columns"] ?? (ViewState["Columns"] = new List<ColumnLayout>()));

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonAddRow_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(TextBoxColumnPosition.Text, out int columnPosition))
            {
                Response.Write("Invalid Column Position Input");
                return;
            }

            if (!int.TryParse(TextBoxFieldLength.Text, out int fieldLength))
            {
                Response.Write("Invalid Field Length Input");
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBoxFieldName.Text))
            {
                Response.Write("Invalid Field Name Input");
                return;
            }

            ColumnLayout column = new ColumnLayout
            {
                ColumnPosition = columnPosition,
                FieldLength = fieldLength,
                FieldName = TextBoxFieldName.Text
            };
            Columns.Add(column);
            BindGridView();
        }

        protected void GridViewLayout_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Columns.RemoveAt(e.RowIndex);
            BindGridView();
        }

        private void BindGridView()
        {
            GridViewLayout.DataSource = Columns;
            GridViewLayout.DataBind();
        }

        protected void ButtonConvert_Click(object sender, EventArgs e)
        {
            string savePath = Server.MapPath("Data\\");
            if (FileUpload.HasFile)
            {
                string fileName = Server.HtmlEncode(FileUpload.FileName);
                string extension = System.IO.Path.GetExtension(fileName);

                if (extension.ToLower() == ".csv")
                {
                    savePath += fileName;
                    try
                    {
                        FileUpload.SaveAs(savePath);
                    }
                    catch (Exception ex)
                    {

                        UpLoadStatusLabel.Text = ex.Message;
                    }
                }
                else
                {
                    UpLoadStatusLabel.Text = "Your file was not uploaded because it does not have .csv extension.";
                }

            }
            else
            {
                UpLoadStatusLabel.Text = "You did not specify a file to upload.";
                return;
            }


            char delimiter;

            if (string.IsNullOrWhiteSpace(TextBoxDelimiter.Text))
            {
                delimiter = ',';
            }
            else
            {
                delimiter = TextBoxDelimiter.Text[0];
            }

            bool isFirstLineHeader = CheckBoxIsFirstLineHeader.Checked;

            var Configuration = new Configuration
            {
                Delimiter = delimiter,
                IsFirstLineHeader = isFirstLineHeader,
                ColumnLayouts = Columns
            };

            //call business code to convert file having configuration and filepath
            //import
            var CSVReader = new FileReader();
            var CSVMapper = new Business.Import.CSVMapper(); 
            //can i use Imapper
            var importer = new Importer(CSVReader, CSVMapper);
            var table = importer.Import(savePath, Configuration);
            var userID = int.Parse((string)Session["userID"]);
            UserDAO.SaveDataTable(userID, Path.GetFileNameWithoutExtension(savePath), table);

            //export
            var OutputFilePath = savePath.Replace("csv", "dat");
            var FixedWidthMapper = new Business.Export.FixedWidthMapper();
            var FixedWidthWriter = new Business.Export.Writer();
            var Exporter = new Business.Export.Exporter(FixedWidthMapper, FixedWidthWriter);
            Exporter.Export(table, OutputFilePath, Configuration);
            ViewState["OutputFilepath"] = OutputFilePath;
        }

        protected void ButtonDownload_Click(object sender, EventArgs e)
        {
            var OutputFilePath = ViewState["OutputFilepath"];
            Response.Redirect($"DownloadFile.ashx?filePath={OutputFilePath}");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var lines = UserDAO.ReadDB();
            foreach (var item in lines)
            {
                BulletedListTable.Items.Add(item);
            } ;
        }
    }
}