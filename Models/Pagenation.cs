using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Deborah.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Inqury.Models;

namespace Pagenations
{
    public class Pagenation
    {
        private readonly IQueryable<MyList> _inqury;
        public readonly int _page_size;

        public Pagenation(IQueryable<MyList> _result)
        {
            this._inqury = _result;
            this._page_size = 2; //１ページあたりの表示件数
        }

        public List<int> Separate_count()
        {
            List<int> separate_page = new List<int>();
            int full_page = this._inqury.Count();
            int _rlt = full_page / this._page_size;
            int _i = 0;
            while (_rlt > 0)
            {
                separate_page.Append(_i);
                _i++;
                _rlt--;
            }
            return separate_page;
        }

        public IQueryable<MyList> Start_List()
        {
            IQueryable<MyList> _r = this._inqury.Where(x => x.Id <= this._page_size);
            return _r;
        }
        public IQueryable<MyList> Next(int _last_number)
        {
            IQueryable<MyList> _r = this._inqury.Where(x => x.Id > _last_number && x.Id <= (_last_number + this._page_size + 1));
            return _r;
        }
        public IQueryable<MyList> Prev(int _last_number)
        {
            IQueryable<MyList> _r = this._inqury.Where(x => x.Id < _last_number && x.Id <= (_last_number - this._page_size + 1));
            return _r;
        }
        public IQueryable<MyList> ActionSelect(string actions, int last_page)
        {
            IQueryable<MyList> _result = null;
            if (actions == "next")
            {
                _result = Next(last_page);
            }
            else if (actions == "prev")
            {
                _result = Prev(last_page);
            }
            else
            {
                return this._inqury;
            }
            return _result;
        }
        public List<string> Pager_Area(int last_page)
        {
            var _area = new List<string>();
            var full_separate = Separate_count().Count();
            if (last_page / this._page_size < 1)
            {
                if (this._inqury.Count() > this._page_size)
                {
                    _area.Add("Next");
                }
            }
            else
            {
                _area.Add("Prev");
                _area.Add("Next");
            }
            return _area;
        }
    }
}