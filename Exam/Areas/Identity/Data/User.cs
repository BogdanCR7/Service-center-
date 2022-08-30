using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Exam.Models;
using Microsoft.AspNetCore.Identity;

namespace Exam.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the User class
    public class User : IdentityUser
    {   [Key]
        public int UserId { get; set; }
      
        public string Name { get; set; }
       
        public string ImagePath { get; set; }
      
        public string DateofBirth { get; set; }
        public string Relationship { get; set; }
        public string City { get; set; }
      
        public string Language { get; set; }
        public string Education { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        public List<Order> orders { get; set; } = new List<Order>();
    }
}
