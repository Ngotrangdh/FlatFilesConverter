﻿using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Newtonsoft.Json;
using FlatFilesConverter.Business.Config;
using FlatFilesConverter.Business.Export;
using FlatFilesConverter.Business.Import;
using FlatFilesConverter.Business.Services;
using System.Data;

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
            if (_columnPosition < 0)
            {
                BulletedListError.Items.Add(new ListItem("Column position has to be 0 or a postive number."));
            }

            if (_fieldLength <= 0)
            {
                BulletedListError.Items.Add(new ListItem("Field length has to be a positive number."));

            }

            if (BulletedListError.Items.Count > 0)
            {
                return;
            }

            ColumnLayout column = new ColumnLayout
            {
                FieldName = TextBoxFieldName.Text,
                ColumnPosition = _columnPosition,
                FieldLength = _fieldLength
            };
            Columns.Add(column);
            BindGridView();
            TextBoxFieldName.Text = string.Empty;
            TextBoxFieldName.Focus();
            TextBoxColumnPosition.Text = string.Empty;
            TextBoxFieldLength.Text = string.Empty;
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
                        LabelFileUploadError.Text = ex.Message;
                    }
                }
                else
                {
                    LabelFileUploadError.Text = "Your file was not uploaded because it does not have .csv extension.";
                }
            }
            else
            {
                LabelFileUploadError.Text = "You did not specify a file to upload.";
                return;
            }


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
                return;
            }

            bool isFirstLineHeader = CheckBoxIsFirstLineHeader.Checked;

            var config = new Configuration
            {
                Delimiter = delimiter,
                IsFirstLineHeader = isFirstLineHeader,
                ColumnLayouts = Columns
            };

            var importMapper = new Business.Import.CSVMapper();
            var exportMapper = new Business.Export.FixedWidthMapper();
            var outputFileName = System.IO.Path.GetFileNameWithoutExtension(savePath) + ".dat";
            var outputFilePath = Server.MapPath($"Data\\{outputFileName}");

            var table = ConvertFile(importMapper, savePath, config, outputFilePath, exportMapper);

            if (table is null)
            {
                return;
            }

            if (Session["userID"] is int userID)
            {
                var JSONConfig = JsonConvert.SerializeObject(config);
                FileService fileService = new FileService();
                fileService.SaveTable(JSONConfig, userID, Path.GetFileNameWithoutExtension(savePath), table);
            }

            Response.Redirect($"DownloadFile.ashx?filePath={outputFilePath}");
        }

        protected private DataTable ConvertFile(Business.Import.IMapper importMapper, string savePath, Configuration config, string outputFilePath, Business.Export.IMapper exportMapper)
        {
            var reader = new FileReader();
            var writer = new Writer();
            var importer = new Importer(reader, importMapper);
            var exporter = new Exporter(exportMapper, writer);

            DataTable table = new DataTable();

            try
            {
                table = importer.Import(savePath, config);
            }
            catch (Exception ex)
            {
                BulletedListError.Items.Add(new ListItem(ex.Message));
                return null;
            }
            
            exporter.Export(table, outputFilePath, config);

            return table;
        }
    }
}