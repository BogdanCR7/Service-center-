using Exam.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Models
{
    public class OrderCreationViewModel
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string Category { get; set; }
        public int YearOfIssue { get; set; }
        [Required]
        public string Brand { get; set; }
        public double Price { get; set; }
        [Required]
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        [Required]
        public string PassportNumber { get; set; }
        [Required]
        public string Name { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public string YearBirth { get; set; }
        [Required]
        public int Wage { get; set; }
        public bool Completed { get; set; } = false;
    }
}
