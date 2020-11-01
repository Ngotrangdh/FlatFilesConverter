using System.Web;

namespace FlatFilesConverter
{
    /// <summary>
    /// Summary description for DownloadFile
    /// </summary>
    public class DownloadFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var filePath = request.QueryString["filePath"];
            var fileName = System.IO.Path.GetFileName(filePath);
            var response = context.Response;
            response.Clear();
            response.ContentType = "application/octet-stream";
            response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
            response.TransmitFile(filePath);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}