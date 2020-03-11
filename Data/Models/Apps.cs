using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KisaanMart.Data.Models
{
    public class Apps
    {
        public int Id { get; set; }
        [Display(Name="Name")]
        public string AppName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string AppDescription { get; set; }

        public string AppUrl { get; set; }

        [Display(Name = "Image")]
        public byte[] AppImage { get; set; }

    }
}
