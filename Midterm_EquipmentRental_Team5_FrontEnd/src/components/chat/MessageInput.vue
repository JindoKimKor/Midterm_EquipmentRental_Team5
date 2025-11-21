<template>
  <div class="message-input-container">
    <v-card class="message-input-card">
      <v-card-text class="pa-4">
        <v-form @submit.prevent="handleSend">
          <div class="input-wrapper">
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
                  :disabled="!messageText.trim()"
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
import { ref } from 'vue'

const props = defineProps({
  selectedUser: {
    type: Object,
    required: true
  }
})

const messageText = ref('')
const error = ref(null)

const handleSend = async () => {
  if (!messageText.value.trim()) return

  try {
    // TODO: Implement message sending logic for individual user chat
    console.log(`Message to ${props.selectedUser.name}:`, messageText.value)
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
  padding: 16px 20px;
  background: transparent;
}

.message-input-card {
  box-shadow: 0 -2px 8px rgba(0, 0, 0, 0.08);
  border-radius: 12px;
  border: 1px solid rgba(0, 0, 0, 0.06);
}

.dark .message-input-card {
  box-shadow: 0 -2px 8px rgba(0, 0, 0, 0.3);
  border: 1px solid rgba(255, 255, 255, 0.08);
}

.input-wrapper {
  display: flex;
  gap: 8px;
  align-items: center;
}
</style>
