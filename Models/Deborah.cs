using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Deborah.Models
{
    public class Mst_System
    {
        [Key]
        public int Id {  get; set; }
        [Required]
        [StringLength(30)]
        public string System_name { get; set; }
    }

    public class Mst_Status
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Status_Name { get; set; }
    }

    public class Mst_Communication
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        public string Com_Name { get; set; }
    }

    public class Mst_Type
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Type_Name { get; set; }
    }

    public class Mst_User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        public string Login_Id {get; set;}
        [Required]
        public string User_Name { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Hostname { get; set; }
    }

    public class Tra_Entry
    {
        [Key]
        public int Id { get; set; }
        public string Company_Name { get; set; }
        public string Tan_Name { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Tel_No { get; set; }
        public string Hostname { get; set; }
        [DataType(DataType.Time)]
        public DateTime Entry_Time { get; set; }
        [DataType(DataType.Time)]
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
        [DataType(DataType.PhoneNumber)]
        public string Tel_No { get; set; }
        public int Login_Id { get; set; }
        [DataType(DataType.MultilineText)]
        public string Inqury { get; set; }
        [DataType(DataType.MultilineText)]
        public string Answer { get; set; }
        public bool Complate_Flag { get; set; }
        [DataType(DataType.Date)]
         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Start_day { get; set; }
        [DataType(DataType.Time)]
        public DateTime Start_Time { get; set; }
        [DataType(DataType.Time)]
        public DateTime Fin_Time { get; set; }
    }
}
