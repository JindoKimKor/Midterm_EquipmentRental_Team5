using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenRockRental_Api.Domain.Interfaces
{
    public interface IReturnRequest
    {
        int RentalId { get; set; }
        string Condition { get; set; }
        string? Notes { get; set; }
    }
}