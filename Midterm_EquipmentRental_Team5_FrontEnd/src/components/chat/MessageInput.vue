<template>
  <div class="bg-white pa-8">
    <v-form @submit.prevent="handleSend">
      <v-text-field
        v-model="messageText"
        placeholder="Type your message..."
        variant="outlined"
        density="compact"
        clearable
        hide-details
      >
        <template #append-inner>
          <v-btn
            icon="mdi-send"
            variant="text"
            size="small"
            :disabled="!messageText.trim() || isSending"
            :loading="isSending"
            @click="handleSend"
          />
        </template>
      </v-text-field>
      <v-fade-transition>
        <v-alert
          v-if="error"
          type="error"
          variant="tonal"
          class="mt-3"
          closable
          @click:close="clearError"
        >
          {{ error }}
        </v-alert>
      </v-fade-transition>
    </v-form>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { sendMessage, isConnected as isSignalRConnected } from '@/services/signalr.ChatHub'
import { useAuthenticationStore } from '@/stores/Authentication'

const props = defineProps({
  selectedUser: {
    type: Object,
    required: true,
  },
  chatId: {
    type: Number,
    required: true,
  },
})

const messageText = ref('')
const error = ref(null)
const isSending = ref(false)
const authStore = useAuthenticationStore()

const handleSend = async () => {
  if (!messageText.value.trim()) return

  if (!isSignalRConnected()) {
    error.value = 'Not connected to chat server. Please refresh the page.'
    console.error('SignalR not connected')
    return
  }

  if (!authStore.authUserId || !props.selectedUser?.id) {
    error.value = 'User information is missing'
    console.error('Missing userId or selectedUser.id')
    return
  }

  try {
    isSending.value = true
    const messageContent = messageText.value.trim()
    await sendMessage(props.selectedUser.id, props.chatId, messageContent)
    messageText.value = ''
    error.value = null
  } catch (err) {
    error.value = err.message || 'Failed to send message'
    console.error('Message send error:', err)
  } finally {
    isSending.value = false
  }
}

const clearError = () => {
  error.value = null
}
</script>
