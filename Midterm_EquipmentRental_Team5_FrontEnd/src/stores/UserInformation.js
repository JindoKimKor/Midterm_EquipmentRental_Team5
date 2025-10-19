import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useUserInformationStore = defineStore(
  'UserInformation',
  () => {
    const id = ref(null)
    const name = ref('')
    const email = ref('')
    const userName = ref('')
    const password = ref('')
    const role = ref('')

    function setUser(data) {
      console.log(data)
      id.value = data.id ?? null
      name.value = data.name ?? ''
      email.value = data.email ?? ''
      userName.value = data.userName ?? ''
      password.value = data.password ?? ''
      role.value = data.role ?? ''
    }

    function clearUser() {
      id.value = null
      name.value = ''
      email.value = ''
      userName.value = ''
      password.value = ''
      role.value = ''
    }

    return {
      id,
      name,
      email,
      userName,
      password,
      role,
      setUser,
      clearUser,
    }
  },
  {
    persist: true,
  },
)
