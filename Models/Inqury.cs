using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inqury.Models
{
    public class MyList
    {
        public int Id { get; set; }
        public DateTime Start_Time { get; set; }
        public string Company_Name { get; set; }
        public string Tel_No { get; set; }
        public string User_Name { get; set; }
        public string Inqury { get; set; }
        public bool Staff_Flag { get; set; }
        public bool Complate_Flag { get; set; }
    }

    public class Entry
    {
        public int Id { get; set; }
        public string Tel_No { get; set; }
        public DateTime Entry_Time { get; set; }
        public DateTime End_Time { get; set; }
        public string Login_Id { get; set; }
        public string Company_Name { get; set; }
        public string Tan_Name { get; set; }
    }
    public class Show_List
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
        public string  Answer { get; set; }
        public bool Complate_Flag { get; set; }
        public DateTime Start_day { get; set; }
        public DateTime Start_Time { get; set; }
        public DateTime Fin_Time { get; set; }
    }
}
