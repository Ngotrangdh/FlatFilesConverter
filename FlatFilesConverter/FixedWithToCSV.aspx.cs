using FlatFilesConverter.Business.Config;
using FlatFilesConverter.Business.Export;
using FlatFilesConverter.Business.Import;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                BulletedListError.Items.Add(new ListItem("Field name required"));
            }

            if (string.IsNullOrWhiteSpace(TextBoxColumnPosition.Text))
            {
                BulletedListError.Items.Add(new ListItem("Column Position required"));
            }

            if (string.IsNullOrWhiteSpace(TextBoxFieldLength.Text))
            {
                BulletedListError.Items.Add(new ListItem("Field length required"));
            }

            if (!int.TryParse(TextBoxColumnPosition.Text, out int _columnPosition))
            {
                BulletedListError.Items.Add(new ListItem("Invalid column position input"));
            }

            if (!int.TryParse(TextBoxFieldLength.Text, out int _fieldLength))
            {
                BulletedListError.Items.Add(new ListItem("Invalid field length input"));
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
            // check and save the upload file
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

            // check if config is entered
            if (! Columns.Any())
            {
                LabelColumnsEmptyError.Text = "Please provide the field configuration.";
                return;
            }

            // get the config: isFirstLineHeader, delimiter, columnLayouts
            bool isFirstLineHeader = CheckBoxIsFirstLineHeader.Checked;
            char delimiter;
            if (string.IsNullOrWhiteSpace(TextBoxDelimiter.Text))
            {
                delimiter = ',';
            }
            else
            {
                delimiter = TextBoxDelimiter.Text[0];
            }

            var config = new Configuration
            {
                Delimiter = delimiter,
                IsFirstLineHeader = isFirstLineHeader,
                ColumnLayouts = Columns
            };

            // call business code
            var outputFileName = System.IO.Path.GetFileNameWithoutExtension(savePath) + ".csv";
            var outputFilePath = Server.MapPath($"Data\\{outputFileName}");
            var fileReader = new FileReader();
            var fixedWidthMapper = new Business.Import.FixedWidthMapper();
            var importer = new Importer(fileReader, fixedWidthMapper);

            var table = importer.Import(savePath, config);

            var CSVMapper = new Business.Export.CSVMapper();
            var writer = new Writer();
            var exporter = new Exporter(CSVMapper, writer);
            exporter.Export(table, outputFilePath, config);
            Response.Redirect($"DownloadFile.ashx?filePath={outputFilePath}");
        }
    }
}