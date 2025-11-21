<template>
  <v-card elevation="3" rounded="lg" class="pa-4">
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
            :disabled="!messageText.trim()"
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
  </v-card>
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
