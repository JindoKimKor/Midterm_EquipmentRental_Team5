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

export function authMe() {
  try {
    const res = RequestHandler.get('/auth/me')
    return res
  } catch (error) {
    console.error(error)
  }
}

export async function logout() {
  try {
    const res = await RequestHandler.post('/auth/logout')
    return res
  } catch (error) {
    console.error(error)
  }
}

export async function isUserAuthorized() {
  try {
    return (await RequestHandler.get('/auth/authorized')) && true
  } catch (error) {
    console.error(error)
  }
}
