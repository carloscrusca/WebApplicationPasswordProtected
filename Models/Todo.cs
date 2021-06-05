using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace WebApplicationPasswordProtected.Models
{
    public class Todo
    {
        public int Id { get; set; }

        [Display(Name = "Release Date")]
        public DateTime DueDate { get; set; }


        public string Summary { get; set; }


        public string Assigned { get; set; }

       
        public bool Done { get; set; }

        [Display(Name = "Archives")]
        public List<FileModel> GetFiles { get; set; }

      

          

    }
}
