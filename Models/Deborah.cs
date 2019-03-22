using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Deborah.Models
{
    public class Mst_System
    {
        public int Id {  get; set; }
        [Required]
        [StringLength(30)]
        public string System_name { get; set; }
    }

    public class Mst_Status
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Status_Name { get; set; }
    }

    public class Mst_Communication
    {
        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        public string Com_Name { get; set; }
    }

    public class Mst_Type
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Type_Name { get; set; }
    }

    public class Mst_User
    {
        public int Id { get; set; }
        [StringLength(10)]
        public string Login_Id {get; set;}
        public string User_Name { get; set; }
        public string Password { get; set; }
        public string Hostname { get; set; }
    }

    public class Tra_Entry
    {
        public int Id { get; set; }
        [Required]
        [Phone]
        public string Tel_No { get; set; }
        public string Hostname { get; set; }
        public DateTime Entry_Time { get; set; }
        public DateTime End_Time { get; set; }
    }

    public class Tra_Inqury
    {
        public int Id { get; set; }
        public int Entry_Id { get; set; }
        public int System_Id { get; set; }
        public int Com_Id { get; set; }
        public int Type_Id { get; set; }
        public int Relation_Id { get; set; }
        public bool Staff_Flag { get; set; }
        public string Company_Name { get; set; }
        public string Tan_Name { get; set; }
        public string Tel_No { get; set; }
        public int Login_Id { get; set; }
        public long Inqury { get; set; }
        public long Answer { get; set; }
        public bool Complate_Flag { get; set; }
        public DateTime Entry_Time { get; set; }
        public DateTime End_Time { get; set; }
    }
}
