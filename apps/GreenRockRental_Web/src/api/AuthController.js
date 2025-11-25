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
    throw error
  }
}

export function googleAuthentication() {
  try {
    window.location.href = 'http://localhost:5115/api/auth/google-login'
  } catch (error) {
    console.error(error)
  }
}

export async function authMe() {
  try {
    const res = await RequestHandler.get('/auth/me')
    return res
  } catch (error) {
    // Silent fail - user is not authenticated
    return null
  }
}

export async function logout() {
  try {
    const res = await RequestHandler.post('/auth/logout')
    return res
  } catch (error) {
    console.error('Logout failed:', error)
  }
}

export async function isUserAuthorized() {
  try {
    return (await RequestHandler.get('/auth/authorized')) && true
  } catch (error) {
    // Silent fail - expected when user is not authenticated
    return false
  }
}
