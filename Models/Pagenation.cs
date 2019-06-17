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
        //private readonly int _bet_separate;

        public Pagenation(IQueryable<MyList> _result, int size)
        {
            this._inqury = _result;
            //this._bet_separate = 5;
            this._page_size = size; //１ページあたりの表示件数
        }

        public List<int> Separate_count()
        {
            List<int> separate_page = new List<int>();
            int full_page = this._inqury.Count();
            int _rlt = full_page / this._page_size;
            int _i = 1;
            while (_rlt > 0)
            {
                separate_page.Add(_i);
                _i++;
                _rlt--;
            }
            return separate_page;
        }
        public IQueryable<MyList> Pager(int separate_now)
        {
            var _result = this._inqury.Where(x => x.Id <= separate_now * this._page_size);
            _result = _result.Where(x => x.Id > (separate_now * this._page_size - this._page_size));
            return _result;
        }
        public List<int> Separate_now(int now_page)
        {
            var _list = Separate_count();
            return _list;
        }
    }
}