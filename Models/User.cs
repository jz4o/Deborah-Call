using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Deborah.Models;


namespace Deborah_User
{
    public class User
    {
        public int Id { get; set; }
        public string User_Name { get; set; }
        public bool DisconnectableFlag { get; set; }
        public int InquryCount { get; set; }
    }
}