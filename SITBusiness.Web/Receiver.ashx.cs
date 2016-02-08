using System;
using System.Web;
using System.IO;

namespace SITBusiness.Web
{
    /// <summary>
    /// Сводное описание для Receiver
    /// </summary>
    public class Receiver : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string filename = context.Request.QueryString["filename"].ToString();

            using (FileStream fs = File.Create(context.Server.MapPath("~/items_pics/" + filename)))
            //using (FileStream fs = File.Create(context.Server.MapPath("~/" + filename)))
            {
                SaveFile(context.Request.InputStream, fs);
            }
        }

        private void SaveFile(Stream stream, FileStream fs)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                fs.Write(buffer, 0, bytesRead);
            }
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