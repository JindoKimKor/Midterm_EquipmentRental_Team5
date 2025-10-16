using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Midterm_EquipmentRental_Team5.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string HashedPassword { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}