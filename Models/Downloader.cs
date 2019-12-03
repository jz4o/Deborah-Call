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
using OfficeOpenXml.Utils;

namespace Deborah_Downloder
{
    public class Downloader
    {
        private readonly MyContext _context;
        private readonly System.Drawing.Color _color1; //Excelヘッダー色
        private readonly System.Drawing.Color _color2; //Excelの一番左側色
        private readonly System.Drawing.Color _color3; //Excelの一番左側色
        public Downloader(MyContext context)
        {
            this._context = context;
            this._color1 = System.Drawing.ColorTranslator.FromHtml("#31869b");
            this._color2 = System.Drawing.ColorTranslator.FromHtml("#b7dee8");
            this._color3 = System.Drawing.ColorTranslator.FromHtml("#ffffff");
        }

        public string Get_Inqury(IQueryable<Mst_Download> header, int _length, DateTime date1, DateTime date2, bool check, string word="")
        {
            var _result = (from inq in this._context.Tra_Inqury
                            join usr in this._context.Mst_User
                            on inq.Login_Id equals usr.Id
                            join sys in this._context.Mst_System
                            on inq.System_Id equals sys.Id
                            join ty in this._context.Mst_Type
                            on inq.Type_Id equals ty.Id
                            join com in this._context.Mst_Communication
                            on inq.Com_Id equals com.Id
                            where inq.Del_Flag == false
                            orderby inq.Start_day
                            orderby inq.Start_Time
                            orderby inq.Id
                            select new Download_List
                            {
                                Id = inq.Id,
                                System_Id = inq.System_Id,
                                System_Name = sys.System_name,
                                Ommit_Name = sys.OmmitName,
                                Com_Id = inq.Com_Id,
                                Com_Name = com.Com_Name,
                                Type_Id = inq.Type_Id,
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
                                Check_Flag = inq.Check_Flag,
                                Start_day = inq.Start_day,
                                Start_Time = inq.Start_Time,
                                Fin_Time = inq.Fin_Time,
                            }).Take(1000).AsNoTracking();
            _result = check ? _result.Where(x => x.Check_Flag == false) : _result;
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
                        //Console.WriteLine(typeof(Download_List).GetProperty(clm.Set_Inqury).GetValue(item).ToString());
                        if (typeof(Download_List).GetProperty(clm.Set_Inqury).GetValue(item).ToString() != "")
                        {
                            if ((clm.Set_Format != null || clm.Set_Format != "") && clm.Set_Format.Length > 0) //Set_Formatにデータが入っている場合は、その形式を使用する。
                            {
                                list.Append(Convert.ToDateTime(typeof(Download_List).GetProperty(clm.Set_Inqury).GetValue(item)).ToString(clm.Set_Format));
                            }
                            else if (clm.Set_Inqury == "Staff_Flag") //受発注区分がTrueの場合は、発注者を入れる。falseなら受注者を入れる。
                            {
                                if (item.Staff_Flag) 
                                {
                                    list.Append("職員");
                                }
                                else
                                {
                                    list.Append("業者");
                                }
                            }
                            else if (clm.Set_Inqury == "Complate_Flag")
                            {
                                if (item.Complate_Flag)
                                {
                                    list.Append("完了");
                                }
                                else
                                {
                                    list.Append("未完了");
                                }
                            }
                            else if (clm.Set_Inqury == "Relation_Id") //ゼロを表示させない
                            {
                                if (item.Relation_Id == 0)
                                {
                                    list.Append(" ");
                                }
                                else
                                {
                                    list.Append(item.Relation_Id.ToString());
                                }
                            }
                            else if (clm.Column_Name == "受付番号")
                            {
                                string str = typeof(Download_List).GetProperty(clm.Set_Inqury).GetValue(item).ToString();
                                Console.WriteLine(typeof(Download_List).GetProperty(clm.Set_Inqury).GetValue(item));
                                list.Append(String.Format("問-{0}", str));
                            }
                            else
                            {
                                list.Append(typeof(Download_List).GetProperty(clm.Set_Inqury).GetValue(item).ToString().Replace("\r", "").Replace("\n", ""));
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

        public byte[] Create_Excel(string file_name, StringBuilder csv)
        {
            var output_file = new FileInfo(file_name);
            var excel = new ExcelPackage(output_file);
            var sheet = excel.Workbook.Worksheets.Add(file_name);
            CreateFile(sheet, csv);
            return excel.GetAsByteArray();
        }
        private void CreateFile(ExcelWorksheet sheet, StringBuilder csv)
        {
            string[] del = {"\r\n"};
            int cnt = 0;
            int _x = 1; //行
            int _y = 3; //列
            Fixed_Character(sheet); //固定文字を挿入→整形する。
            String str_csv = csv.ToString();
            var csv_split_rn = str_csv.TrimEnd('\r', '\n').Split(del, StringSplitOptions.None).ToList();
            foreach (var clm in csv_split_rn) //改行で区切ってLIST化する。
            {
                var csv_split_kanma = clm.Split(",").ToList();　//更にLIST化する。（カンマで）
                cnt = csv_split_kanma.Count(); //要素数を取得
                foreach (var val in csv_split_kanma)
                {
                    var cell = sheet.Cells[_y, _x];
                    cell.Value = val;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    AddBorder(cell);
                    if (_y > 3) //先頭行以外
                    {
                        cell.Style.WrapText = true; //折り返して全体表示
                        if (_x == 1)
                        {
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(this._color2);
                        }
                        else if (_x == 2)
                        {
                            cell.Style.Font.Bold = true;
                        }
                    }
                    _x++;
                }
                if (_y > 3)
                {
                    Centering(sheet, _y);
                }
                _y = HeaderEdit(sheet, cnt, _y);
                //_y++;
                _x = 1;
            }
            WidthChange(sheet);
            PrintSetting(sheet, _x, _y);
        }

        private void PrintSetting(ExcelWorksheet sheet, int _x, int _y)
        {
            sheet.PrinterSettings.Orientation = eOrientation.Landscape; //印刷横向き
            // 余白設定 上下左右
            sheet.PrinterSettings.TopMargin = 0;
            sheet.PrinterSettings.LeftMargin = 0;
            sheet.PrinterSettings.RightMargin = 0;
            sheet.PrinterSettings.BottomMargin = 0;
            // 改ページ設定
            sheet.PrinterSettings.PrintArea = sheet.Cells[1, 1, _y, _x];
        }
        private void WidthChange(ExcelWorksheet sheet)
        {
            var _result = this._context.Mst_Download.OrderBy(x => x.Order_No).OrderBy(x => x.Id);
            int i = 1;
            foreach (var w in _result)
            {
                if (w.WidthCell < 1)
                {
                    break;
                }
                sheet.Column(i).Width = w.WidthCell;
                i++;
            }
        }



        private int HeaderEdit(ExcelWorksheet sheet, int cnt, int _y)
        {
            
            if (_y == 3)
            {
                for (int i=1; i <= cnt; i++)
                {
                    AddBorder(sheet.Cells[_y, i, _y + 1, i]);
                    sheet.Cells[_y, i, _y + 1, i].Merge = true; //セルの結合
                    sheet.Cells[_y, i, _y + 1, i].Style.WrapText = true; //ヘッダー部分の折り返し
                    sheet.Cells[_y, i, _y + 1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; //横中央ぞろえ
                    sheet.Row(_y).Height = 26.5;
                    sheet.Row(_y + 1).Height = 26.5;

                    var allcells = sheet.Cells[_y, i];
                    allcells.Style.VerticalAlignment = ExcelVerticalAlignment.Center; //縦位置の中央揃え
                    allcells.Style.Font.Color.SetColor(this._color3);
                }
                sheet.Cells[_y, 1, _y, cnt].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[_y, 1, _y, cnt].Style.Fill.BackgroundColor.SetColor(this._color1);
                return _y + 2;
            }
            return _y + 1;
        }

        //固定文字の挿入
        public void Fixed_Character(ExcelWorksheet sheet)
        {
            sheet.Cells[2,1].Value = _Year(DateTime.Today);
            sheet.Cells[2,1].Style.Font.Bold = true;
            sheet.Cells[2,1].Style.Font.UnderLine = true;
            sheet.Cells[2,1].Style.Font.Size = 14;
            sheet.Cells[2,1,2,2].Merge = true;
            sheet.Cells[2,1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; //中央寄せ
            sheet.Cells[2,1].Style.Font.Name = "ＭＳ Ｐゴシック";
            sheet.Cells[2,3].Value = "電子調達共同利用システム問い合わせ一覧";
            sheet.Cells[2,3].Style.Font.Bold = true;
            sheet.Cells[2,3].Style.Font.UnderLine = true;
            sheet.Cells[2,3].Style.Font.Size = 14;
            sheet.Cells[2,1].Style.Font.Name = "ＭＳ Ｐゴシック";
        }

        private void Centering(ExcelWorksheet sheet, int _y)
        {
            var _result = this._context.Mst_Download.AsNoTracking();
            int i = 1;
            foreach (var v in _result)
            {
                switch (v.Positon)
                {
                    case "Center":
                        sheet.Cells[_y, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                    case "Left":
                        sheet.Cells[_y, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        break;
                    case "Right":
                        sheet.Cells[_y, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        break;
                    default:
                        sheet.Cells[_y, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        break;
                }
                i++;
            }
        }

        private string _Year(DateTime nendo)
        {
            var _y = nendo.Year;
            if (nendo.Month >= 4 && nendo.Month <= 12)
            {
                return _y.ToString() + "年度";
            }
            else
            {
                return (_y - 1).ToString() + "年度";
            }
        }
        private void AddBorder(ExcelRange cells)
        {
            cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            return;
        }
    }

    public class Download_List
    {
        public int Id { get; set; }
        public int System_Id { get; set; }
        public string System_Name { get; set; }
        public string Ommit_Name { get; set; }
        public int Com_Id { get; set; }
        public string Com_Name { get; set; }
        public int Type_Id  { get; set; }
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
        public bool Check_Flag { get; set; }
        public DateTime Start_day { get; set; }
        public DateTime Start_Time { get; set; }
        public DateTime Fin_Time { get; set; }
    }
}