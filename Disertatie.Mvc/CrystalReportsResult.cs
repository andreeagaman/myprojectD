using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Data;
using Disertatie.CrystalReports;
using CrystalDecisions.Shared;
using System.Web;
using System.IO;

namespace Disertatie.Mvc
{
    public class CrystalReportsResult : ActionResult
    {
       
        private readonly DataSet _dataSource;
        public DataSet DataSource { get { return _dataSource; } }

        private readonly string _filename;
        public string FileName { get { return _filename; } }

        public CrystalReportsResult(DataSet dataSource) : this(dataSource, GetRandomFilename()) { }

        public CrystalReportsResult(DataSet dataSource, string fileName) {
            this._dataSource = dataSource;
            this._filename = fileName;
        }
        public string _report = "RaportAvizare";
        public CrystalReportsResult(DataSet dataSource, string fileName,string rep)
        {
            this._dataSource = dataSource;
            this._filename = fileName;
            this._report = rep;
        }
        public override void ExecuteResult(ControllerContext context) {
            //if (string.Compare(_report, "DosareNoi") == 0)
            //{
            //    using (var report = new RaportAvizareDosareNoi())
            //    using (_dataSource)
            //    {
            //        report.SetDataSource(_dataSource);
            //        var response = HttpContext.Current.Response;
            //        report.ExportToHttpResponse(ExportFormatType.PortableDocFormat, response, true, _filename);
            //    }
            //}
            //else 
            //{
            //    using (var report = new RaportAvizare())
            //    using (_dataSource)
            //    {
            //        report.SetDataSource(_dataSource);
            //        var response = HttpContext.Current.Response;
            //        report.ExportToHttpResponse(ExportFormatType.PortableDocFormat, response, true, _filename);
            //    }
            
            //}

            if (string.Compare(_report, "DosareNoi") == 0)
            {
                //using (var report = new RaportAvizareDosareNoi()) asa era la daune
                //using (_dataSource)
                //{
                //    report.SetDataSource(_dataSource);
                //    var response = HttpContext.Current.Response;
                //    report.ExportToHttpResponse(ExportFormatType.PortableDocFormat, response, true, _filename);
                //}
            }
            else
            {
                //using (var report = new RaportAvizare())
                //using (_dataSource)
                //{
                //    report.SetDataSource(_dataSource);
                //    var response = HttpContext.Current.Response;
                //    report.ExportToHttpResponse(ExportFormatType.PortableDocFormat, response, true, _filename);
                //}

            }
        }

        private static string GetRandomFilename() {
            return Path.ChangeExtension(Path.GetRandomFileName(), "pdf");
        }
    }
}
