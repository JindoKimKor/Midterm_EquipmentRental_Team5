using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Midterm_EquipmentRental_Team5.Models;

namespace Midterm_EquipmentRental_Team5.Services.Interfaces
{
    public interface IEquipmentServices
    {   
        Task<IEnumerable<Equipment>> GetAllEquipmentAsync(int page = 1);
        Task<Equipment> GetEquipmentByIdAsync(int id);
        Task AddEquipmentAsync(Equipment newEquipment);
        Task UpdateEquipmentAsync(int id, Equipment updatedEquipment);
        Task DeleteEquipmentAsync(int id);
        Task<IEnumerable<Equipment>> GetAvailableEquipmentAsync();
        Task<IEnumerable<Equipment>> GetRentedEquipmentAsync();

    }
}