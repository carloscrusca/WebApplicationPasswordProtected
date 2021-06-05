using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using WebApplicationPasswordProtected.Models;

namespace WebApplicationPasswordProtected.ViewModels
{
    public class FilelViewModel
    {

        public string Descricao { get; set; }

        public int TaskID { get; set; }

        

        [Required(ErrorMessage = "Please upload a file")]
        [Display(Name = "Attachments")]
        public IFormFile AttachmentName { get; set; }
    }
}
