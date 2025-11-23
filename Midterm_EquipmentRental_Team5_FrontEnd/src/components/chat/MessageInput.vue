<template>
  <div class="bg-white pa-8">
    <v-form @submit.prevent="handleSend">
      <v-text-field
        v-model="messageText"
        placeholder="Type your message..."
        variant="outlined"
        density="compact"
        clearable
        @keydown.enter.exact="handleSend"
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
import { sendMessage } from '@/api/ChatController'
import { useChatStore } from '@/stores/ChatStore'

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
const chatStore = useChatStore()

const handleSend = async () => {
  if (!messageText.value.trim()) return

  try {
    isSending.value = true
    const messageContent = messageText.value.trim()

    const response = await sendMessage(props.chatId, {
      message: messageContent,
    })

    if (response) {
      chatStore.addMessage(response)
    }

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
