using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Deborah.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Inqury.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Sammarys.Models
{
    class Sammary
    {
        private readonly MyContext _context;
        private readonly IQueryable<Tra_Inqury> _inqury;
        public Sammary(MyContext context)
        {
            this._context = context;
            string today = DateTime.Today.ToString("yyyy-MM-dd");
            this._inqury = this._context.Tra_Inqury
                            .Where(x => x.Start_day.ToString("yyyy-MM-dd") == today)
                            .Where(x => x.Del_Flag == false).AsNoTracking();
        }
        public IQueryable<SammaryStyle> get_sammary_system()
        {
            var _result = from sys in this._context.Mst_System
                            orderby sys.Id
                            select new SammaryStyle
                            {
                                Id = sys.Id,
                                System = sys.OmmitName,
                                Counts = this._inqury.Where(x => x.System_Id == sys.Id).Count()
                            };
            //if (_result != null) this._context.Entry(_result).Reload();
            return _result;
        }
        public int get_sammary_total()
        {
            string today = DateTime.Today.ToString("yyyy-MM-dd");
            var _result = this._inqury.Count();
            //if (_result > 0) this._context.Entry(_result).Reload();
            return _result;
        }

        public int get_sammary_Staff()
        {
            string today = DateTime.Today.ToString("yyyy-MM-dd");
            var _result = this._inqury.Where(x => x.Staff_Flag == true).Count();
            //if (_result > 0) this._context.Entry(_result).Reload();
            return _result;
        }
    }
    public class SammaryStyle
    {
        public int Id { get; set; }
        public string System { get; set; }
        public int Counts { get; set; }
    }
}
