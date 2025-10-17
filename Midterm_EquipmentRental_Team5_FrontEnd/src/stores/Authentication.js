import { ref } from 'vue'
import { defineStore } from 'pinia'

const useAuthenticationStore = defineStore('Authentication', () => {
  const role = ref(null)
  const AuthToken = ref(null)

  function getCookie(name) {
    const match = document.cookie.match(name)
    if (match) {
      const [key, value] = match.input.split('=')
      return { key, value }
    }
    return null
  }

  function checkCookie() {
    const token = getCookie('auth_token')
    if (!token) return false
    return true
  }

  function checkAuthToken() {
    if (!AuthToken.value) {
      if (!checkCookie()) return false
      setToken(getCookie('auth_token').value)
    }
    return true
  }

  function setToken(token) {
    AuthToken.value = token
    if (!checkCookie('auth_token')) {
      document.cookie = `auth_token=${token}; path=/; max-age=${7 * 24 * 60 * 60}`
    }
  }

  return { checkAuthToken, setToken }
})

export default useAuthenticationStore
