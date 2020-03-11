using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KisaanMart.ViewModels
{
    public class LoginViewModel
    {
        [Key]
        public int Id { get; set; }
        public string UserPhoneNo { get; set; }
        public int  OTP { get; set; }
        public string Role { get; set; }
    }
}
