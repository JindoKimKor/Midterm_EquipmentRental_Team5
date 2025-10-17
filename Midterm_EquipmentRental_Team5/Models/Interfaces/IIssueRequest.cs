using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Midterm_EquipmentRental_Team5.Models.Interfaces
{
    public interface IIssueRequest
    {
        int EquipmentId { get; set; }
        int CustomerId { get; set; }
        DateTime? DueDate { get; set; }
    }
}