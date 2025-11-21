<template>
  <div class="message-input-container">
    <v-card class="message-input-card">
      <v-card-text class="pa-4">
        <v-form @submit.prevent="handleSend">
          <div class="input-wrapper">
            <v-text-field
              v-model="messageText"
              placeholder="Type a message..."
              variant="outlined"
              density="compact"
              clearable
              :disabled="!isReady"
              @keydown.enter.exact="handleSend"
              hide-details
            >
              <template #append-inner>
                <v-btn
                  icon="mdi-send"
                  variant="text"
                  size="small"
                  :disabled="!messageText.trim() || !isReady"
                  @click="handleSend"
                />
              </template>
            </v-text-field>
          </div>
          <v-fade-transition>
            <v-alert
              v-if="error"
              type="error"
              variant="tonal"
              class="mt-2"
              closable
              @click:close="clearError"
            >
              {{ error }}
            </v-alert>
          </v-fade-transition>
        </v-form>
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useChatStore } from '@/stores/ChatStore'

const chatStore = useChatStore()
const messageText = ref('')
const error = ref(null)

const isReady = computed(() => chatStore.isReady)

const handleSend = async () => {
  if (!messageText.value.trim()) return

  try {
    await chatStore.sendChatMessage(messageText.value)
    messageText.value = ''
    error.value = null
  } catch (err) {
    error.value = err.message || 'Failed to send message'
  }
}

const clearError = () => {
  error.value = null
}
</script>

<style scoped>
.message-input-container {
  padding: 16px;
  background: transparent;
}

.message-input-card {
  box-shadow: 0 -1px 3px rgba(0, 0, 0, 0.1);
}

.input-wrapper {
  display: flex;
  gap: 8px;
  align-items: center;
}
</style>
