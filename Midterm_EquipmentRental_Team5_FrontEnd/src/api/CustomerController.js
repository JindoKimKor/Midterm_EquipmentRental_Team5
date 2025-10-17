import RequestHandler from '@/services/RequestHandler'

export async function getAllCustomers(page = 1) {
  try {
    const res = await RequestHandler.get('customer')
    return res
  } catch (error) {
    console.error(error)
  }
}

export async function getCustomer(id) {
  try {
    // ...
  } catch (error) {
    console.error(error)
  }
}

export async function createCustomer(newCustomer) {
  try {
    await RequestHandler.post('customer', { newCustomer })
  } catch (error) {
    console.error(error)
  }
}

export async function updateCustomer(id, updatedCustomer) {
  try {
    // ...
  } catch (error) {
    console.error(error)
  }
}

export async function deleteCustomer(id) {
  try {
    await RequestHandler.delete(`customer/${id}`)
  } catch (error) {
    console.error(error)
  }
}

export async function getCustomerRentalHistory(id) {
  try {
    // ...
  } catch (error) {
    // ...
  }
}

export async function getCustomerActiveRental(id) {
  try {
    // ...
  } catch (error) {
    console.error(error)
  }
}
