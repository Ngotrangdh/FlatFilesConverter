using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using FlatFilesConverter.Business.Config;
using FlatFilesConverter.Business.Export;
using FlatFilesConverter.Business.Import;
using FlatFilesConverter.Business.Services;
using System.IO;
using FixedWidthMapper = FlatFilesConverter.Business.Import.FixedWidthMapper;
using CSVMapper = FlatFilesConverter.Business.Export.CSVMapper;

namespace FlatFilesConverter
{
    public partial class FixedWidthToCSV : Page
    {
        private const string COLUMN_LAYOUTS = "ColumnLayouts";
        private const string GENERIC_ERROR_MESSAGE = "An error has occurred. Please try again.";
        private List<ColumnLayout> Columns => (List<ColumnLayout>)(ViewState[COLUMN_LAYOUTS] ?? (ViewState[COLUMN_LAYOUTS] = new List<ColumnLayout>()));

        protected void ButtonAddRow_Click(object sender, EventArgs e)
        {
            ClearErrors();

            var columnLayout = ValidateFieldLayoutForm();

            if (BulletedListError.Items.Count > 0)
            {
                return;
            }

            Columns.Add(columnLayout);
            BindGridView();
            ResetFieldLayoutForm();
        }

        protected void GridViewLayout_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Columns.RemoveAt(e.RowIndex);
            BindGridView();
        }

        protected void ButtonConvert_Click(object sender, EventArgs e)
        {
            var savePath = Server.MapPath("Data\\");

            var filePath = SaveFileToDisk(savePath);

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (!Columns.Any())
            {
                LabelColumnsEmptyError.Text = "Please provide the field configuration.";
                return;
            }

            char delimiter = GetDelimiter();

            if (delimiter == 0)
            {
                return;
            }

            var config = new Configuration
            {
                Delimiter = delimiter,
                IsFirstLineHeader = CheckBoxIsFirstLineHeader.Checked,
                ColumnLayouts = Columns
            };

            var outputFileName = Path.GetFileNameWithoutExtension(filePath) + ".csv";
            var outputFilePath = Server.MapPath($"Data\\{outputFileName}");

            var table = ConvertFile(filePath, config, outputFilePath);

            if (table is null)
            {
                return;
            }

            if (Session["userID"] is int userID)
            {
                SaveFileToDatabase(filePath, config, table, userID);
            }

            Response.Redirect($"DownloadFile.ashx?filePath={outputFilePath}");
        }

        private char GetDelimiter()
        {
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
                delimiter = (char)0;
            }
            return delimiter;
        }

        private void ResetFieldLayoutForm()
        {
            TextBoxFieldName.Text = string.Empty;
            TextBoxFieldName.Focus();
            TextBoxColumnPosition.Text = string.Empty;
            TextBoxFieldLength.Text = string.Empty;
        }

        private ColumnLayout ValidateFieldLayoutForm()
        {
            AssertNotEmpty(TextBoxFieldName.Text, "Field Name required.");
            AssertNotEmpty(TextBoxColumnPosition.Text, "Column Position required.");
            AssertNotEmpty(TextBoxFieldLength.Text, "Field Length required.");
            Assert(!int.TryParse(TextBoxColumnPosition.Text, out int columnPosition), "Column Position has to be 0 or greater.");
            Assert(!int.TryParse(TextBoxFieldLength.Text, out int fieldLength), "Field Length has to be greater than 0.");
            Assert(columnPosition < 0, "Column Position has to be 0 or greater.");
            Assert(fieldLength < 0, "Field Length has to be greater than 0.");

            return new ColumnLayout
            {
                ColumnPosition = columnPosition,
                FieldLength = fieldLength,
                FieldName = TextBoxFieldName.Text
            };
        }

        private void AssertNotEmpty(string fieldInput, string error)
        {
            Assert(string.IsNullOrWhiteSpace(fieldInput), error);
            Assert(string.IsNullOrWhiteSpace(TextBoxFieldName.Text), "Field name required.");
        }

        private void Assert(bool predicate, string error)
        {
            if (predicate)
            {
                BulletedListError.Items.Add(new ListItem(error));
            }
        }

        private void ClearErrors()
        {
            BulletedListError.Items.Clear();
            LabelColumnsEmptyError.Text = string.Empty;
        }

        private void BindGridView()
        {
            GridViewLayout.DataSource = Columns;
            GridViewLayout.DataBind();
        }

        protected private DataTable ConvertFile(string savePath, Configuration config, string outputFilePath)
        {
            DataTable table;

            var importer = new Importer(new FixedWidthMapper());
            var exporter = new Exporter(new CSVMapper());

            try
            {
                table = importer.Import(savePath, config);
                exporter.Export(table, outputFilePath, config);
            }
            catch
            {
                BulletedListError.Items.Add(new ListItem(GENERIC_ERROR_MESSAGE));
                table = null;
            }
            return table;
        }
        private static void SaveFileToDatabase(string savePath, Configuration config, DataTable table, int userID)
        {
            string JSONConfig = JsonConvert.SerializeObject(config);
            FileService fileService = new FileService();
            fileService.SaveTable(JSONConfig, userID, Path.GetFileNameWithoutExtension(savePath), table);
        }

        private string SaveFileToDisk(string savePath)
        {
            string errorMessage = string.Empty;
            string filePath = string.Empty;
            if (FileUpload.HasFile)
            {
                filePath = savePath + Server.HtmlEncode(FileUpload.FileName);
                try
                {
                    FileUpload.SaveAs(filePath);
                }
                catch
                {
                    errorMessage = GENERIC_ERROR_MESSAGE;
                }
            }
            else
            {
                errorMessage = "You did not specify a file to upload";
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                LabelFileUploadError.Text = errorMessage;
                filePath = string.Empty;
            }

            return filePath;
        }

    }
}