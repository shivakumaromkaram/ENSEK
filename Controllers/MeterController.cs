
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ENSEK.Services;
using ENSEK.Models;
namespace ENSEK.Controllers
{
    public class MeterController : ApiController
    {
        [HttpPost]
        [Route("meter-reading-uploads")]
        public HttpResponseMessage UploadMeterReadings()
        {
            var httpRequest = HttpContext.Current.Request;
            MeterReadingResponce objResponce = new MeterReadingResponce();
            DataTable dt = new DataTable();
          
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    string path = HttpContext.Current.Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var postedFile = httpRequest.Files[file];
                    string extension = Path.GetExtension(postedFile.FileName);
                    if (extension.ToLower().Contains("csv"))
                    {
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {

                            string UploadCSVFile = postedFile.FileName;
                            var filePath = HttpContext.Current.Server.MapPath("~/Uploads/" + UploadCSVFile);
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                            postedFile.SaveAs(filePath);
                            dt = MeterReadingService.CreateDataTableForCSVFile(filePath);
                            objResponce = MeterReadingService.MeterReadingProcess(dt);
                            var JSONRespone = new { MeterReadingsData = objResponce };
                            return Request.CreateResponse(HttpStatusCode.OK, JSONRespone, Configuration.Formatters.JsonFormatter);
                        }
                    }
                    else
                    {
                            var JSONRespone = new { Error = "Invalid File",Message="Upload CSV Format File" };
                            return Request.CreateResponse(HttpStatusCode.OK, JSONRespone, Configuration.Formatters.JsonFormatter);
                    }
                }
            }
            else
            {
                var JSONRespone = new { Error = "File Not Found" };
                return Request.CreateResponse(HttpStatusCode.OK, JSONRespone, Configuration.Formatters.JsonFormatter);
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Oops! Something happened", Configuration.Formatters.JsonFormatter);
        }
    }
}