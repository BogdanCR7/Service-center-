using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Models
{
    public class Good
    {
        [Key]
        public int GoodsId { get; set; }
        [Required]
        public Category Category { get; set; }

        public int YearOfIssue { get; set; }
        [Required]
        public string Brand { get; set; }

        public double Price { get; set; }
        [Required]
        public string Model { get; set; }
        public string SerialNumber { get; set; }

        public bool Same(Good another)
        {
            if (SerialNumber == another.SerialNumber && Model == another.Model
                && Price == another.Price && Brand == another.Brand
                && YearOfIssue == another.YearOfIssue
                && Category.CategoryId == another.Category.CategoryId) return true;
            else return false;
        }
    }
}
