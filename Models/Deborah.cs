using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Deborah.Models
{
    public class Mst_System
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "20文字以内で登録してください。")]
        public string System_name { get; set; }
        [StringLength(15, ErrorMessage = "15文字以内で登録してください。")]
        public string OmmitName { get; set; }
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

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [StringLength(10, ErrorMessage = "3文字以上、10文字以内です。", MinimumLength = 3)]
        [RegularExpression(@"[a-zA-Z0-9]+", ErrorMessage = "半角英数字のみ入力できます。")]
        public string Login_Id {get; set;}
        [StringLength(10, ErrorMessage = "10文字以内です。", MinimumLength = 1)]
        public string User_Name { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Passwordは5文字以上、12文字以内です。", MinimumLength = 5)]
        public string Password { get; set; }
        public byte[] Password_Salt { get; set; }
        [Required]
        public string Hostname { get; set; }
        public bool DisconnectableFlag { get; set; }
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
        public bool Del_Flag { get; set; }
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
        public bool Check_Flag { get; set; }
        [Required(ErrorMessage = "この項目は必須入力です。")]
        public string Company_Name { get; set; }
        [Required(ErrorMessage = "この項目は必須入力です。")]
        public string Tan_Name { get; set; }
        [Required(ErrorMessage = "この項目は必須入力です。")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "電話番号として認識できません。")]
        public string Tel_No { get; set; }
        public int Login_Id { get; set; }
        [DataType(DataType.MultilineText)]
        public string Inqury { get; set; }
        [DataType(DataType.MultilineText)]
        public string Answer { get; set; }
        public bool Complate_Flag { get; set; }
        [Required(ErrorMessage = "この項目は必須入力です。")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Start_day { get; set; }
        [DataType(DataType.Time)]
        public DateTime Start_Time { get; set; }
        [DataType(DataType.Time)]
        public DateTime Fin_Time { get; set; }
    }

    public class Mst_Download
    {
        public int Id { get; set; }
        public string Column_Name { get; set; }
        // Tra_Inquryから取得したいカラム名を設定する。
        // null設定がある場合は、空の値となります。
        public string Set_Inqury { get; set; }
        //日付データのみ適用されるフォーマット（例： yyyy/MM/ddなど）
        public string Set_Format { get; set; }
        public int Order_No { get; set; }
    }
}
