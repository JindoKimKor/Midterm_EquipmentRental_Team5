<template>
  <div class="message-list-container">
    <v-card class="message-list-card">
      <v-card-text class="messages-content">
        <!-- Loading state -->
        <div v-if="chatStore.isConnecting" class="text-center py-8">
          <v-progress-circular indeterminate color="primary" class="mb-4" />
          <p>Connecting to chat...</p>
        </div>

        <!-- Empty state -->
        <div v-else-if="chatStore.messages.length === 0" class="text-center py-8 text-grey">
          <v-icon size="48" class="mb-4">mdi-chat-outline</v-icon>
          <p>No messages yet. Say hello!</p>
        </div>

        <!-- Messages -->
        <div v-else class="messages-wrapper">
          <div
            v-for="msg in chatStore.messages"
            :key="msg.id"
            :class="['message-group', { 'system-message': msg.isSystem }]"
          >
            <!-- System message -->
            <div v-if="msg.isSystem" class="system-msg">
              <v-divider class="my-2" />
              <p class="text-caption text-grey text-center">{{ msg.message }}</p>
              <v-divider class="my-2" />
            </div>

            <!-- User message -->
            <div v-else :class="['message', { 'message-current': msg.isCurrentUser }]">
              <div class="message-bubble">
                <div class="message-header">
                  <strong class="message-username">{{ msg.username }}</strong>
                  <span class="message-time">{{ formatTime(msg.timestamp) }}</span>
                </div>
                <div class="message-text">{{ msg.message }}</div>
              </div>
            </div>
          </div>
        </div>
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup>
import { useChatStore } from '@/stores/ChatStore'
import { watch, nextTick } from 'vue'

defineProps({
  selectedUser: {
    type: Object,
    required: true
  }
})

const chatStore = useChatStore()

// Auto-scroll to bottom when new messages arrive
watch(
  () => chatStore.messages.length,
  async () => {
    await nextTick()
    const container = document.querySelector('.messages-content')
    if (container) {
      container.scrollTop = container.scrollHeight
    }
  }
)

const formatTime = (timestamp) => {
  if (!timestamp) return ''
  const date = new Date(timestamp)
  return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
}
</script>

<style scoped>
.message-list-container {
  flex: 1;
  display: flex;
  min-height: 0;
  overflow: hidden;
}

.message-list-card {
  width: 100%;
  border-radius: 0;
  border: none;
  box-shadow: none;
}

.messages-content {
  height: 100%;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
  padding: 20px 24px;
}

.messages-wrapper {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.message-group {
  display: flex;
  flex-direction: column;
  margin: 4px 0;
}

.system-message {
  text-align: center;
}

.message {
  display: flex;
  margin: 4px 0;
}

.message-current {
  justify-content: flex-end;
}

.message-bubble {
  max-width: 70%;
  padding: 10px 14px;
  border-radius: 12px;
  background-color: rgba(0, 0, 0, 0.06);
  word-wrap: break-word;
  box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
}

.dark .message-bubble {
  background-color: rgba(255, 255, 255, 0.08);
}

.message-current .message-bubble {
  background-color: rgba(33, 150, 243, 0.15);
  box-shadow: 0 1px 2px rgba(33, 150, 243, 0.1);
}

.dark .message-current .message-bubble {
  background-color: rgba(33, 150, 243, 0.2);
}

.message-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 8px;
  margin-bottom: 4px;
  font-size: 0.875rem;
}

.message-username {
  color: rgba(0, 0, 0, 0.87);
}

.dark .message-username {
  color: rgba(255, 255, 255, 0.87);
}

.message-time {
  font-size: 0.75rem;
  color: rgba(0, 0, 0, 0.54);
}

.dark .message-time {
  color: rgba(255, 255, 255, 0.54);
}

.message-text {
  word-break: break-word;
  line-height: 1.4;
}

.text-grey {
  color: rgba(0, 0, 0, 0.54);
}

.dark .text-grey {
  color: rgba(255, 255, 255, 0.54);
}
</style>
