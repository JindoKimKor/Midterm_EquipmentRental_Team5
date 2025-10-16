import { ref, computed } from 'vue'
import { defineStore } from 'pinia'

export const useCounterStore = defineStore('counter', () => {
  const role = ref(null)
  const AuthToken = ref(0)

  function getAuthToken() {}

  function logout() {}

  function login() {}

  return { count, doubleCount, increment }
})
