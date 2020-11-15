using FlatFilesConverter.Business.Config;
using FlatFilesConverter.Business.Export;
using FlatFilesConverter.Business.Import;
using FlatFilesConverter.Business.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSVMapper = FlatFilesConverter.Business.Import.CSVMapper;
using FixedWidthMapper = FlatFilesConverter.Business.Export.FixedWidthMapper;

namespace FlatFilesConverter
{
    public partial class CSVToFixedWidth : Page
    {
        private const string GENERIC_ERROR_MESSAGE = "An error has occurred. Please try again.";

        private List<ColumnLayout> Columns => (List<ColumnLayout>)(ViewState["Columns"] ?? (ViewState["Columns"] = new List<ColumnLayout>()));

        protected void ButtonAddRow_Click(object sender, EventArgs e)
        {
            ClearErrors();

            ColumnLayout columnLayout = ValidateFieldLayoutForm();

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
            string savePath = Server.MapPath("Data\\");
            string filePath = SaveFileToDisk(savePath);
            if (string.IsNullOrEmpty(filePath))
            {
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

            var outputFileName = Path.GetFileNameWithoutExtension(filePath) + ".dat";
            var outputFilePath = Server.MapPath($"Data\\{outputFileName}");
            var table = ConvertFile(filePath, outputFilePath, config);

            if (table is null)
            {
                return;
            }

            if (Session["userID"] is int userID)
            {
                SaveFileToDataBase(filePath, config, table, userID);
            }

            Response.Redirect($"DownloadFile.ashx?filePath={outputFilePath}");
        }

        private void ClearErrors()
        {
            BulletedListError.Items.Clear();
            LabelColumnsEmptyError.Text = string.Empty;
        }

        private void ResetFieldLayoutForm()
        {
            TextBoxFieldName.Focus();
            TextBoxFieldName.Text = string.Empty;
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
            Assert(columnPosition < 0, "Column Position has to be 0 or a postive number.");
            Assert(fieldLength <= 0, "Field Length has to be a positive number.");

            return new ColumnLayout
            {
                FieldName = TextBoxFieldName.Text,
                ColumnPosition = columnPosition,
                FieldLength = fieldLength
            };
        }

        private void AssertNotEmpty(string input, string errorMessage)
        {
            Assert(string.IsNullOrWhiteSpace(input), errorMessage);
        }

        private void Assert(bool isInvalid, string errorMessage)
        {
            if (!isInvalid) return;
            BulletedListError.Items.Add(new ListItem(errorMessage));
        }

        private void BindGridView()
        {
            GridViewLayout.DataSource = Columns;
            GridViewLayout.DataBind();
        }

        private static void SaveFileToDataBase(string filePath, Configuration config, DataTable table, int userID)
        {
            var JSONConfig = JsonConvert.SerializeObject(config);
            FileService fileService = new FileService();
            fileService.SaveTable(JSONConfig, userID, Path.GetFileNameWithoutExtension(filePath), table);
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
                LabelDelimiterError.Text = "Delimiter has to be a character";
                delimiter = (char)0;
            }

            return delimiter;
        }
        
        private string SaveFileToDisk(string savePath)
        {
            string errorMesssage = string.Empty;
            string filePath = string.Empty;

            if (FileUpload.HasFile)
            {
                string fileName = Server.HtmlEncode(FileUpload.FileName);
                string extension = Path.GetExtension(fileName);

                if (extension.ToLower() == ".csv")
                {
                    filePath = savePath + fileName;

                    try
                    {
                        FileUpload.SaveAs(filePath);
                    }
                    catch
                    {
                        //TODO: log exception
                        errorMesssage = GENERIC_ERROR_MESSAGE;
                    }
                }
                else
                {
                    errorMesssage = "File must have .csv extension.";
                }
            }
            else
            {
                errorMesssage = "You did not specify a file to upload.";
            }

            if (!string.IsNullOrEmpty(errorMesssage))
            {
                LabelFileUploadError.Text = errorMesssage;
                filePath = string.Empty;
            }

            return filePath;
        }
        
        protected private DataTable ConvertFile(string savePath, string outputFilePath, Configuration config)
        {
            DataTable table;

            var importer = new Importer(new CSVMapper());
            var exporter = new Exporter(new FixedWidthMapper());

            try
            {
                table = importer.Import(savePath, config);
                exporter.Export(table, outputFilePath, config);
            }
            catch
            {
                //TODO: log exception
                BulletedListError.Items.Add(new ListItem(GENERIC_ERROR_MESSAGE));
                table = null;
            }

            return table;
        }
    }
}