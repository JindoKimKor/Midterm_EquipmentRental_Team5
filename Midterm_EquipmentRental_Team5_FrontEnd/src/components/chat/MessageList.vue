<template>
  <v-card flat border="none" class="d-flex flex-column h-100 rounded-0">
    <!-- Loading state -->
    <div
      v-if="chatStore.isConnecting"
      class="d-flex flex-column align-center justify-center flex-grow-1 gap-4"
    >
      <v-progress-circular indeterminate color="primary" />
      <p class="text-body2">Connecting to chat...</p>
    </div>

    <!-- Empty state -->
    <div
      v-else-if="chatStore.messages.length === 0"
      class="d-flex flex-column align-center justify-center flex-grow-1 gap-4"
    >
      <v-icon size="48" color="medium-emphasis">mdi-chat-outline</v-icon>
      <p class="text-body2 text-medium-emphasis">No messages yet. Say hello!</p>
    </div>

    <!-- Messages -->
    <v-card-text v-else class="d-flex flex-column gap-2 overflow-y-auto flex-grow-1 pa-6">
      <template v-for="msg in chatStore.messages" :key="msg.id">
        <!-- System message -->
        <div v-if="msg.isSystem" class="d-flex align-center gap-3 my-2">
          <v-divider class="flex-grow-1" />
          <p class="text-caption text-medium-emphasis flex-shrink-0">{{ msg.message }}</p>
          <v-divider class="flex-grow-1" />
        </div>

        <!-- User message -->
        <div v-else :class="['d-flex', msg.isCurrentUser ? 'justify-end' : 'justify-start']">
          <v-card
            flat
            :color="msg.isCurrentUser ? 'primary' : 'surface-variant'"
            class="pa-3 message-card"
          >
            <div class="d-flex justify-space-between align-center gap-2 mb-1">
              <span class="text-subtitle2 font-weight-500">{{ msg.username }}</span>
              <span class="text-caption text-medium-emphasis">{{ formatTime(msg.timestamp) }}</span>
            </div>
            <p class="text-body2 mb-0">{{ msg.message }}</p>
          </v-card>
        </div>
      </template>
    </v-card-text>
  </v-card>
</template>

<script setup>
import { useChatStore } from '@/stores/ChatStore'
import { watch, nextTick } from 'vue'

defineProps({
  selectedUser: {
    type: Object,
    required: true,
  },
})

const chatStore = useChatStore()

watch(
  () => chatStore.messages.length,
  async () => {
    await nextTick()
    const container = document.querySelector('.messages-content')
    if (container) {
      container.scrollTop = container.scrollHeight
    }
  },
)

const formatTime = (timestamp) => {
  if (!timestamp) return ''
  const date = new Date(timestamp)
  return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
}
</script>

<style scoped>
.message-card {
  max-width: 70%;
  word-break: break-word;
}

@media (max-width: 600px) {
  .message-card {
    max-width: 85%;
  }
}
</style>
