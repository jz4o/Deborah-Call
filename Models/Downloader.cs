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

        public IQueryable Get_Column()
        {
            var _result = from dwn in this._context.Mst_Download
                            orderby dwn.Order_No
                            orderby dwn.Id
                            select new Mst_Download
                            {
                                Id = dwn.Id,
                                Column_Name = dwn.Column_Name,
                                Set_Inqury = dwn.Set_Inqury,
                                Set_Format = dwn.Set_Format
                            };
            return _result;
        }

        public string Get_Inqury()
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
            StringBuilder list = new StringBuilder("");
            foreach(var item in _result)
            {
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