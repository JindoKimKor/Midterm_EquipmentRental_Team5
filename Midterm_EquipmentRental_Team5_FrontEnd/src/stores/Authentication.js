import { ref } from 'vue'
import { defineStore } from 'pinia'
import { authMe, isUserAuthorized, logout as logoutController } from '@/api/AuthController'
import router from '@/router'

export const useAuthenticationStore = defineStore('Authentication', () => {
  const authRole = ref(null)
  const authUserName = ref(null)
  const authUserId = ref(null)

  async function checkAspNetCoreCookie() {
    const match = isUserAuthorized()
    if (match) {
      try {
        // The cookie exists — now verify the authenticated user
        const user = await authMe()

        if (user) {
          authRole.value = user.role || user.Role || 'User'
          authUserName.value = user.userName || user.UserName || 'Unknown'
          authUserId.value = user.id || user.Id || null
          return true
        }
      } catch (error) {
        console.error('Failed to validate cookie:', error)
        return false
      }
    }

    // Cookie not found or user invalid
    return false
  }

  async function logout() {
    await logoutController()
    router.go(0)
    authRole.value = null
    authUserName.value = null
    authUserId.value = null
  }

  return { checkAspNetCoreCookie, authRole, authUserName, authUserId, logout }
})
