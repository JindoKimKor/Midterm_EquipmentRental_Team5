import RequestHandler from '@/services/RequestHandler'

export async function getAllRentals(page = 1) {
  try {
    const res = await RequestHandler.get(`rental?page=${page}`)
    return res
  } catch (error) {
    console.error('Error fetching all rentals:', error)
  }
}

export async function getRentalDetails(id) {
  try {
    const res = await RequestHandler.get(`rental/${id}`)
    return res
  } catch (error) {
    console.error(`Error fetching rental details for ID ${id}:`, error)
  }
}

export async function issueEquipment(issueRequest) {
  try {
    const res = await RequestHandler.post('rental/issue', issueRequest)
    return res
  } catch (error) {
    console.error('Error issuing equipment:', error)
  }
}

export async function returnEquipment(returnRequest) {
  try {
    const res = await RequestHandler.post('rental/return', returnRequest)
    return res
  } catch (error) {
    console.error('Error returning equipment:', error)
  }
}

export async function getActiveRentals() {
  try {
    const res = await RequestHandler.get('rental/active')
    return res
  } catch (error) {
    console.error('Error fetching active rentals:', error)
  }
}

export async function getCompletedRentals() {
  try {
    const res = await RequestHandler.get('rental/completed')
    return res
  } catch (error) {
    console.error('Error fetching completed rentals:', error)
  }
}

export async function getOverdueRentals() {
  try {
    const res = await RequestHandler.get('rental/overdue')
    return res
  } catch (error) {
    console.error('Error fetching overdue rentals:', error)
  }
}

export async function getEquipmentRentalHistory(equipmentId) {
  try {
    const res = await RequestHandler.get(`rental/equipment/${equipmentId}/history`)
    return res
  } catch (error) {
    console.error(`Error fetching rental history for equipment ID ${equipmentId}:`, error)
  }
}

export async function extendRental(id, extensionRequest) {
  try {
    const res = await RequestHandler.post(`rental/${id}/extend`, extensionRequest)
    return res
  } catch (error) {
    console.error(`Error extending rental with ID ${id}:`, error)
  }
}

export async function cancelRental(id) {
  try {
    const res = await RequestHandler.post(`rental/${id}/cancel`)
    return res
  } catch (error) {
    console.error(`Error cancelling rental with ID ${id}:`, error)
  }
}
