using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Deborah.Models;
using System.Diagnostics;
using Inqury.Models;

namespace Search
{
    public class SearchInqury
    {

        public IEnumerable<MyList> _inqury;
        private readonly DateTime _date1;
        private readonly DateTime _date2;
        private readonly bool _check;
        private readonly string _word;

        public SearchInqury(IEnumerable<MyList> inqury, DateTime date1, DateTime date2, bool check, string word)
        {
            this._inqury = inqury;
            this._date1 = date1;
            this._date2 = date2;
            this._check = check;
            this._word = word;
        }

        public IEnumerable<MyList> Search_start()
        {
            IEnumerable<MyList> _result = this._inqury;
            _result = (this._check) ? _result.Where(x => x.Check_Flag == false) : _result;
            _result = (this._date1.ToString("yyyy") == "0001") ? _result : _result.Where(x => x.Start_day >= (this._date1));
            _result = (this._date2.ToString("yyyy") == "0001") ? _result : _result.Where(x => x.Start_day <= (this._date2));
            if (this._word != null)
            {
                _result = _result.Where(x => x.Inqury.Contains(this._word)
                                                || x.Answer.Contains(this._word)
                                                || x.Company_Name.Contains(this._word)
                                                || x.Tan_Name.Contains(this._word)
                                                || x.Tel_No.Contains(this._word)
                                        );
            }
            return _result;
        }
    }
}