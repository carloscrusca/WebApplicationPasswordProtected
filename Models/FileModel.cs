using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationPasswordProtected.Models
{
    public class FileModel
    {
        [Key()]
        public int Id { get; set; }

        public string Descricao { get; set; }

        [ForeignKey("Todo")]
        public int TaskID { get; set; }
        public virtual Todo Todo { get; set; }

        [Display(Name = "Attachments")]
        public string Attachment { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Attachments Name")]
        public IFormFile AttachmentsName { get; set; }
    }
}
