import RequestHandler from '@/services/RequestHandler'

export async function login(request) {
  try {
    const res = await RequestHandler.post('/auth/login', {
      Username: request.Username,
      Password: request.Password,
    })
    return res
  } catch (error) {
    console.error(error)
  }
}
