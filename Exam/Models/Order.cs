using Exam.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public Good goods { get; set; }
        [Required]
        public User user { get; set; }
        [Required]
        public Client client { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? End { get; set; }
        [Required]
        public int Wage { get; set; }
        public bool Completed { get; set; }
    }
}
