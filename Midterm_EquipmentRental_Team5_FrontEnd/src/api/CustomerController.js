import RequestHandler from '@/services/RequestHandler'

export async function getAllCustomers(page = 1) {
  try {
    const res = await RequestHandler.get(`customer?page=${page}`)
    return res
  } catch (error) {
    console.error(error)
  }
}

export async function getCustomer(id) {
  try {
    const res = await RequestHandler.get(`customer/${id}`)
    return res
  } catch (error) {
    console.error(error)
  }
}

export async function createCustomer(newCustomer) {
  try {
    console.log(newCustomer)
    const res = await RequestHandler.post('customer', newCustomer)
    return res
  } catch (error) {
    console.error(error)
  }
}

export async function updateCustomer(id, updatedCustomer) {
  try {
    const res = await RequestHandler.put(`customer/${id}`, updatedCustomer)
    return res
  } catch (error) {
    console.error(error)
  }
}

export async function deleteCustomer(id) {
  try {
    const res = await RequestHandler.delete(`customer/${id}`)
    return res
  } catch (error) {
    console.error(error)
  }
}

export async function getCustomerRentalHistory(id) {
  try {
    const res = await RequestHandler.get(`customer/${id}/rental-history`)
    return res
  } catch (error) {
    console.error(error)
  }
}

export async function getCustomerActiveRental(id) {
  try {
    const res = await RequestHandler.get(`customer/${id}/active-rental`)
    return res
  } catch (error) {
    console.error(error)
  }
}
