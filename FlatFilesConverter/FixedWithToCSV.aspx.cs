using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft;
using FlatFilesConverter.Business.Config;
using FlatFilesConverter.Business.Export;
using FlatFilesConverter.Business.Import;
using FlatFilesConverter.Business.Services;
using Newtonsoft.Json;
using System.Data;

namespace FlatFilesConverter
{
    public partial class FixedWidthToCSV : Page
    {
        private const string COLUMN_LAYOUTS = "ColumnLayouts";

        private List<ColumnLayout> Columns => (List<ColumnLayout>)(ViewState[COLUMN_LAYOUTS] ?? (ViewState[COLUMN_LAYOUTS] = new List<ColumnLayout>()));

        protected void ButtonAddRow_Click(object sender, EventArgs e)
        {
            BulletedListError.Items.Clear();
            LabelColumnsEmptyError.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(TextBoxFieldName.Text))
            {
                BulletedListError.Items.Add(new ListItem("Field name required."));
            }

            if (string.IsNullOrWhiteSpace(TextBoxColumnPosition.Text))
            {
                BulletedListError.Items.Add(new ListItem("Column Position required."));
            }

            if (string.IsNullOrWhiteSpace(TextBoxFieldLength.Text))
            {
                BulletedListError.Items.Add(new ListItem("Field length required."));
            }

            if (!int.TryParse(TextBoxColumnPosition.Text, out int _columnPosition))
            {
                BulletedListError.Items.Add(new ListItem("Column position has to be 0 or a postive number."));
            }

            if (!int.TryParse(TextBoxFieldLength.Text, out int _fieldLength))
            {
                BulletedListError.Items.Add(new ListItem("Field length has to be a positive number."));
            }

            if (_columnPosition < 0 )
            {
                BulletedListError.Items.Add(new ListItem("Column position has to be 0 or a postive number."));
            }

            if ( _fieldLength < 0)
            {
                BulletedListError.Items.Add(new ListItem("Field length has to be a positive number."));

            }

            if (BulletedListError.Items.Count > 0)
            {
                return;
            }

            var ColumnLayout = new ColumnLayout
            {
                FieldName = TextBoxFieldName.Text,
                ColumnPosition = _columnPosition,
                FieldLength = _fieldLength
            };

            Columns.Add(ColumnLayout);
            BindGridView();
            TextBoxFieldName.Text = string.Empty;
            TextBoxFieldName.Focus();
            TextBoxColumnPosition.Text = string.Empty;
            TextBoxFieldLength.Text = string.Empty;
        }

        private void BindGridView()
        {
            GridViewLayout.DataSource = Columns;
            GridViewLayout.DataBind();
        }

        protected void GridViewLayout_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Columns.RemoveAt(e.RowIndex);
            BindGridView();
        }

        protected void ButtonConvert_Click(object sender, EventArgs e)
        {
            var savePath = Server.MapPath("Data\\");
            
            if (FileUpload.HasFile)
            {
                savePath += Server.HtmlEncode(FileUpload.FileName);
                FileUpload.SaveAs(savePath);
            }
            else
            {
                LabelFileUploadError.Text = "Please upload a file.";
                return;
            }

            if (! Columns.Any())
            {
                LabelColumnsEmptyError.Text = "Please provide the field configuration.";
                return;
            }

            bool isFirstLineHeader = CheckBoxIsFirstLineHeader.Checked;

            char delimiter;
            if (string.IsNullOrWhiteSpace(TextBoxDelimiter.Text))
            {
                delimiter = ',';
            }
            else if (TextBoxDelimiter.Text.Length == 1)
            {
                delimiter = TextBoxDelimiter.Text[0];
            }
            else
            {
                LabelDelimiterError.Text = "Delimiter has to be a character.";
                return;
            }

            var config = new Configuration
            {
                Delimiter = delimiter,
                IsFirstLineHeader = isFirstLineHeader,
                ColumnLayouts = Columns
            };

            var outputFileName = System.IO.Path.GetFileNameWithoutExtension(savePath) + ".csv";
            var outputFilePath = Server.MapPath($"Data\\{outputFileName}");
            var importMapper = new Business.Import.FixedWidthMapper();
            var exportMapper = new Business.Export.CSVMapper();
            var table = ConvertFile(importMapper, savePath, config, outputFilePath, exportMapper);

            var userID = int.Parse(Session["userID"].ToString());
            string JSONConfig = JsonConvert.SerializeObject(config);
            FileService fileService = new FileService();
            fileService.SaveTable(JSONConfig, userID, System.IO.Path.GetFileNameWithoutExtension(savePath), table);

            Response.Redirect($"DownloadFile.ashx?filePath={outputFilePath}");
        }

        protected private DataTable ConvertFile(Business.Import.IMapper importMapper, string savePath, Configuration config, string outputFilePath, Business.Export.IMapper exportMapper)
        {
            var reader = new FileReader();
            var writer = new Writer();
            var importer = new Importer(reader, importMapper);
            var exporter = new Exporter(exportMapper, writer);
            var table = importer.Import(savePath, config);
            exporter.Export(table, outputFilePath, config);
            return table;
        }
    }
}