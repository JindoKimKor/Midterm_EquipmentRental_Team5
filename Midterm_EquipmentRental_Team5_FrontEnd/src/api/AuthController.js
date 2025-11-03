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

export function googleAuthentication() {
  try {
    window.location.href = 'http://localhost:5115/api/auth/google-login'
  } catch (error) {
    console.error(error)
  }
}
