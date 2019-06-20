using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Deborah.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Inqury.Models;

namespace Deborah_Downloder
{
    public class Downloader
    {
        private readonly MyContext _context;
        
        public Downloader(MyContext context)
        {
            this._context = context;
        }

        public string Get_Inqury(IQueryable<Mst_Download> header, int _length, DateTime date1, DateTime date2, bool check, string word="")
        {
            var _result = from inq in this._context.Tra_Inqury
                            join usr in this._context.Mst_User
                            on inq.Login_Id equals usr.Id
                            join sys in this._context.Mst_System
                            on inq.System_Id equals sys.Id
                            join ty in this._context.Mst_Type
                            on inq.Type_Id equals ty.Id
                            join com in this._context.Mst_Communication
                            on inq.Com_Id equals com.Id
                            orderby inq.Start_day
                            orderby inq.Start_Time
                            orderby inq.Id
                            select new Download_List
                            {
                                Id = inq.Id,
                                System_Name = sys.System_name,
                                Com_Name = com.Com_Name,
                                Type_Name = ty.Type_Name,
                                Relation_Id = inq.Relation_Id,
                                Staff_Flag = inq.Staff_Flag,
                                Company_Name = inq.Company_Name,
                                Tan_Name = inq.Tan_Name,
                                Tel_No = inq.Tel_No,
                                User_Name = usr.User_Name,
                                Inqury = inq.Inqury,
                                Answer = inq.Answer,
                                Complate_Flag = inq.Complate_Flag,
                                Start_day = inq.Start_day,
                                Start_Time = inq.Start_Time,
                                Fin_Time = inq.Fin_Time,
                            };
            _result = check ? _result.Where(x => x.Complate_Flag == false) : _result;
            _result = date1.ToString("yyyy") == "0001" ? _result : _result.Where(x => x.Start_day >= date1);
            _result = date2.ToString("yyyy") == "0001" ? _result : _result.Where(x => x.Start_day <= date2);
            if (word != null)
            {
                _result = _result.Where(x => x.Inqury.Contains(word)
                                                || x.Answer.Contains(word)
                                                || x.Company_Name.Contains(word)
                                                || x.Tan_Name.Contains(word)
                                                || x.Tel_No.Contains(word)
                                        );
            }
            int i = 0; //列カウンタ
            StringBuilder list = new StringBuilder("");
            foreach(var item in _result)
            {
                foreach(var clm in header)
                {
                    //先頭列であれば、カンマを入れない。
                    if(i >= 1)
                    {
                        list.Append(",");
                    }
                    try
                    {
                        if (typeof(Download_List).GetProperty(clm.Set_Inqury).GetValue(item).ToString() != "")
                        {
                            if (clm.Set_Format != null) //Set_Forrmatにデータが入っている場合は、その形式を使用する。
                            {
                                list.Append(Convert.ToDateTime(typeof(Download_List).GetProperty(clm.Set_Inqury).GetValue(item)).ToString(clm.Set_Format));
                            }
                            else
                            {
                                list.Append(typeof(Download_List).GetProperty(clm.Set_Inqury).GetValue(item));
                            }
                            if (clm.Column_Name == "Staf_Flag") //受発注区分がTrueの場合は、発注者を入れる。falseなら受注者を入れる。
                            {
                                if (item.Staff_Flag)
                                {
                                    list.Append("発注者");
                                }
                                else
                                {
                                    list.Append("受注者");
                                }
                            }
                        }
                        else
                        {
                            list.Append(clm.Set_Inqury);
                        }
                    }
                    catch(NullReferenceException) //Nullの場合は、半角スぺ―スを入れる。
                    {
                        if (clm.Set_Inqury == null)
                        {
                            list.Append(" ");
                        }
                        else
                        {
                            list.Append(clm.Set_Inqury.ToString()); //Null以外だった場合は、Set_Inquryの内容をそのまま追加する。
                        }
                    }
                    i++; //列カウントを１つづつ増やす。
                }
                i = 0; //１行のCSV成形が終了後、ゼロクリアする（再度１列目から開始させる）
                list.Append("\r\n");
            }
            return list.ToString();
        }
    }

    public class Download_List
    {
        public int Id { get; set; }
        public string System_Name { get; set; }
        public string Com_Name { get; set; }
        public string Type_Name { get; set; }
        public int Relation_Id { get; set; }
        public bool Staff_Flag { get; set; }
        public string Company_Name { get; set; }
        public string Tan_Name { get; set; }
        public string Tel_No { get; set; }
        public string User_Name { get; set; }
        public string Inqury { get; set; }
        public string Answer { get; set; }
        public bool Complate_Flag { get; set; }
        public DateTime Start_day { get; set; }
        public DateTime Start_Time { get; set; }
        public DateTime Fin_Time { get; set; }
    }
}