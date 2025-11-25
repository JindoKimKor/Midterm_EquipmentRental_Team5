using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenRockRental_Api.Domain.Interfaces
{
    public interface IIssueRequest
    {
        int EquipmentId { get; set; }
        int CustomerId { get; set; }
        DateTime? DueDate { get; set; }
    }
}