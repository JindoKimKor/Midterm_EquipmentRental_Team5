import { ref } from 'vue'
import { defineStore } from 'pinia'

const useAuthenticationStore = defineStore('Authentication', () => {
  const authRole = ref(null)
  const authToken = ref(null)

  function getAuthToken() {
    const match = document.cookie.match(/(?:^|; )auth_token=([^;]*)/)
    if (match) {
      return { key: 'auth_token', value: decodeURIComponent(match[1]) }
    }
    return null
  }

  function getAuthRole() {
    const match = document.cookie.match(/(?:^|; )auth_role=([^;]*)/)
    if (match) {
      return { key: 'auth_role', value: decodeURIComponent(match[1]) }
    }
    return null
  }

  function checkToken() {
    const token = getAuthToken()
    if (!token) return false
    return true
  }

  function checkRole() {
    const role = getAuthRole()
    if (!role) return false
    return true
  }

  function checkAuthToken() {
    if (!authToken.value) {
      if (!checkToken()) return false
      setToken(getAuthToken().value)
    }
    return true
  }

  function checkAuthRole() {
    if (!authRole.value) {
      if (!checkRole()) return false
      setRole(getAuthRole().value)
    }
    return true
  }

  function setToken(token) {
    authToken.value = token
    document.cookie = `auth_token=${token}; path=/; max-age=3600; Secure; SameSite=Strict;`
  }

  function setRole(newRole) {
    authRole.value = newRole
    document.cookie = `auth_role=${newRole}; path=/; max-age=3600; Secure; SameSite=Strict;`
  }

  function logout() {
    document.cookie = 'auth_role=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;'
    document.cookie = 'auth_token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;'
  }

  return { checkAuthToken, checkAuthRole, setToken, setRole, authRole, authToken, logout }
})

export default useAuthenticationStore
