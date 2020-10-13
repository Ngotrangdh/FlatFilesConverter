using System;
using System.Collections.Generic;
using System.Linq;
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
            var response = context.Response;
            response.Clear();
            response.ContentType = "application/octet-stream";
            response.AddHeader("Content-Disposition", $"attachment; filename={filePath}");
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