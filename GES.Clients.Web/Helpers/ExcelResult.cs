using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using jqGridWeb;

namespace GES.Clients.Web.Helpers
{
    public class ExcelResult : ActionResult
    {
        private readonly DataForExcel _data;
        private readonly string _fileName;

        public ExcelResult(string[] headers, List<string[]> data, string fileName, string sheetName)
        {
            _data = new DataForExcel(headers, data, sheetName);
            _fileName = fileName;
        }

        public ExcelResult(string[] headers, DataForExcel.DataType[] colunmTypes, List<string[]> data, string fileName, string sheetName)
        {
            _data = new DataForExcel(headers, colunmTypes, data, sheetName);
            _fileName = fileName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ClearContent();
            response.ClearHeaders();
            response.Cache.SetMaxAge(new TimeSpan(0));

            using (var stream = new MemoryStream())
            {
                _data.CreateXlsxAndFillData(stream);

                //Return it to the client - strFile has been updated, so return it. 
                response.AddHeader("content-disposition", "attachment; filename=" + _fileName);

                // see http://filext.com/faq/office_mime_types.php
                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.ContentEncoding = Encoding.UTF8;
                stream.WriteTo(response.OutputStream);
            }
            response.Flush();
            response.Close();
        }
    }
}