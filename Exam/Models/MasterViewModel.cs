using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Models
{
    public class MasterViewModel
    {
        public string Name { get; set; }
        public string password { get; set; }
       
        public string DateofBirth { get; set; }
        public string Relationship { get; set; }
        public string City { get; set; }
        public string Language { get; set; }
        public string Education { get; set; }
        public string Phone { get; set; }

        public string email { get; set; }

        [Required(ErrorMessage = "Вы должны выбрать один файл")]
        [Display(Name = "Выбор файлов")]
        public IFormFileCollection files { get; set; }
    }
}
