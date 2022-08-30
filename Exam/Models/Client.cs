using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Models
{
    public class Client
    {
        [Key]
        public int ClientsId { get; set; }
        [Required]
        public string PassportNumber { get; set; }
        [Required]
        public string Name { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
       
        public string YearBirth { get; set; }
        public List<Order> orders { get; set; } = new List<Order>();

        public bool Same(Client another)
        {
            if (Name == another.Name)
                if (PassportNumber == another.PassportNumber)
                    if (Adress == another.Adress)
                        if (PhoneNumber == another.PhoneNumber)
                            if (YearBirth == another.YearBirth) return true;
            return false;
        }
    }
}
