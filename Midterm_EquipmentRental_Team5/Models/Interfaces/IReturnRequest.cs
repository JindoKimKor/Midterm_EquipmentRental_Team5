using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Midterm_EquipmentRental_Team5.Models.Interfaces
{
    public interface IReturnRequest
    {
        int RentalId { get; set; }
        string Condition { get; set; }
        string? Notes { get; set; }
    }
}