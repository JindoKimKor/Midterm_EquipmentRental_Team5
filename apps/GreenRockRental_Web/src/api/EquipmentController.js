import RequestHandler from '@/services/RequestHandler'

export async function getAllEquipment(page = 1) {
  try {
    const res = await RequestHandler.get(`equipment?page=${page}`)
    return res
  } catch (error) {
    console.error('Error fetching all equipment:', error)
  }
}

export async function getEquipment(id) {
  try {
    const res = await RequestHandler.get(`equipment/${id}`)
    return res
  } catch (error) {
    console.error(`Error fetching equipment with ID ${id}:`, error)
  }
}

export async function addEquipment(newEquipment) {
  try {
    const res = await RequestHandler.post('equipment', newEquipment)
    return res
  } catch (error) {
    console.error('Error adding new equipment:', error)
  }
}

export async function getEquipmentRentalHistory(equipmentId) {
  try {
    const res = await RequestHandler.get(`equipment/${equipmentId}/rental-history`)
    return res
  } catch (error) {
    console.error(`Error fetching rental history for equipment ID ${equipmentId}:`, error)
    throw error
  }
}

export async function updateEquipment(id, updatedEquipment) {
  try {
    const res = await RequestHandler.put(`equipment/${id}`, updatedEquipment)
    return res
  } catch (error) {
    console.error(`Error updating equipment with ID ${id}:`, error)
  }
}

export async function deleteEquipment(id) {
  try {
    const res = await RequestHandler.delete(`equipment/${id}`)
    return res
  } catch (error) {
    console.error(`Error deleting equipment with ID ${id}:`, error)
  }
}

export async function getAvailableEquipment() {
  try {
    const res = await RequestHandler.get('equipment/available')
    return res
  } catch (error) {
    console.error('Error fetching available equipment:', error)
  }
}

export async function getRentedEquipmentSummary() {
  try {
    const res = await RequestHandler.get('equipment/rented')
    return res
  } catch (error) {
    console.error('Error fetching rented equipment summary:', error)
  }
}
