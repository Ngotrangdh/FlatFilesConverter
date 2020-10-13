using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FlatFilesConverter.Business.Config;
using FlatFilesConverter.Business.Export;
using FlatFilesConverter.Business.Import;

namespace FlatFilesConverter
{
    public partial class FixedWidthToCSV : System.Web.UI.Page
    {
        private List<ColumnLayout> Columns => (List<ColumnLayout>)(ViewState["ColumnLayouts"] ?? (ViewState["ColumnLayouts"] = new List<ColumnLayout>()));
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonAddRow_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(TextBoxColumnPosition.Text, out int _columnPosition))
            {
                Response.Write("Invalid Column Position Input");
                return;
            }

            if (!int.TryParse(TextBoxFieldLength.Text, out int _fieldLength))
            {
                Response.Write("Invalid Field Length Input");
                return;
            }

            if (string.IsNullOrWhiteSpace(TextBoxFieldName.Text))
            {
                Response.Write("Invalid Field Name Input");
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
            // save the upload file
            var savePath = Server.MapPath("Data\\");
            string fileName;
            
            if (FileUpload.HasFile)
            {
                fileName = Server.HtmlEncode(FileUpload.FileName);
                savePath += FileUpload.FileName;
                FileUpload.SaveAs(savePath);
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
            ViewState["OutputFilePath"] = outputFilePath;
        }

        protected void ButtonDownload_Click(object sender, EventArgs e)
        {
            var outputFilePath = ViewState["OutputFilePath"];
            Response.Redirect($"DownloadFile.ashx?filePath={outputFilePath}");
        }
    }
}