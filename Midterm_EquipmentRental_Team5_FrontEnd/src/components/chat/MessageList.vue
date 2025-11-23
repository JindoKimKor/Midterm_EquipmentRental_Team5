<template>
  <v-card flat border="none" class="d-flex flex-column h-100 rounded-0 bg-background">
    <!-- Loading State -->
    <div
      v-if="chatStore.isConnecting"
      class="d-flex flex-column align-center justify-center flex-grow-1 gap-4"
    >
      <v-progress-circular indeterminate color="primary" size="48" />
      <div class="text-center">
        <p class="text-body2 font-weight-500">Loading messages...</p>
        <p class="text-caption text-medium-emphasis mt-2">Please wait a moment</p>
      </div>
    </div>

    <!-- Empty State -->
    <div
      v-else-if="chatStore.messages.length === 0"
      class="d-flex flex-column align-center justify-center flex-grow-1 gap-3 pa-8"
    >
      <div class="empty-state-icon">
        <v-icon size="64" color="primary" opacity="0.3">mdi-chat-outline</v-icon>
      </div>
      <div class="text-center">
        <h3 class="text-h6 font-weight-600 mb-2">No messages yet</h3>
        <p class="text-body2 text-medium-emphasis">Start a conversation with {{ props.selectedUser?.name }}</p>
      </div>
    </div>

    <!-- Messages List -->
    <v-card-text v-else class="d-flex flex-column gap-3 overflow-y-auto flex-grow-1 pa-6 messages-container">
      <template v-for="msg in chatStore.messages" :key="msg.id">
        <!-- System message (divider with text) -->
        <div v-if="msg.isSystem" class="d-flex align-center gap-3 my-2">
          <v-divider class="flex-grow-1" />
          <p class="text-caption text-medium-emphasis flex-shrink-0">{{ msg.message }}</p>
          <v-divider class="flex-grow-1" />
        </div>

        <!-- Regular message bubbles -->
        <div
          v-else
          :class="['message-row', isCurrentUserMessage(msg) ? 'sent' : 'received']"
        >
          <v-card
            flat
            :color="isCurrentUserMessage(msg) ? 'primary' : 'surface-variant'"
            :text-color="isCurrentUserMessage(msg) ? 'white' : 'on-surface-variant'"
            class="message-card"
          >
            <v-card-text class="pa-3">
              <div class="d-flex align-center justify-space-between gap-2 mb-2">
                <span class="text-subtitle2 font-weight-600">{{ getSenderName(msg) }}</span>
                <span
                  :class="isCurrentUserMessage(msg) ? 'text-white' : 'text-medium-emphasis'"
                  class="text-caption timestamp"
                >
                  {{ formatTime(msg.timestamp) }}
                </span>
              </div>
              <p class="text-body2 mb-0 message-content">{{ msg.content }}</p>
            </v-card-text>
          </v-card>
        </div>
      </template>
    </v-card-text>
  </v-card>
</template>

<script setup>
import { useChatStore } from '@/stores/ChatStore'
import { useAuthenticationStore } from '@/stores/Authentication'
import { watch, nextTick } from 'vue'

const chatStore = useChatStore()
const authStore = useAuthenticationStore()

const props = defineProps({
  selectedUser: {
    type: Object,
    required: true,
  },
})

/**
 * Determines if a message was sent by the current user
 * @param {Object} msg - Message object
 * @returns {boolean} True if message is from current user
 */
const isCurrentUserMessage = (msg) => {
  return String(msg.senderId) === String(authStore.authUserId)
}

/**
 * Gets the display name for the message sender
 * @param {Object} msg - Message object
 * @returns {string} Sender's name
 */
const getSenderName = (msg) => {
  if (isCurrentUserMessage(msg)) {
    return 'You'
  }
  return props.selectedUser?.name || 'Unknown'
}

watch(
  () => chatStore.messages.length,
  async () => {
    await nextTick()
    const cardText = document.querySelector('.v-card-text')
    if (cardText) {
      cardText.scrollTop = cardText.scrollHeight
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
.messages-container {
  background: linear-gradient(to bottom, transparent 0%, transparent 90%, rgba(0, 0, 0, 0.02) 100%);
}

.message-row {
  display: flex;
  gap: 1rem;
  margin-bottom: 0.5rem;
  animation: slideUp 0.3s ease-out;
}

.message-row.sent {
  justify-content: flex-end;
}

.message-row.received {
  justify-content: flex-start;
}

.message-card {
  max-width: 70%;
  word-break: break-word;
  transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
  border-radius: 16px !important;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.08);
}

.message-card:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  transform: translateY(-2px);
}

.message-content {
  word-wrap: break-word;
  line-height: 1.4;
  letter-spacing: 0.3px;
}

.timestamp {
  opacity: 0.85;
  font-size: 0.75rem !important;
}

.empty-state-icon {
  animation: float 3s ease-in-out infinite;
  margin-bottom: 1rem;
}

/* Animations */
@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(12px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes float {
  0%, 100% {
    transform: translateY(0px);
  }
  50% {
    transform: translateY(-8px);
  }
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

/* Responsive Design */
@media (max-width: 768px) {
  .message-card {
    max-width: 80%;
  }
}

@media (max-width: 600px) {
  .message-card {
    max-width: 85%;
  }

  .message-row {
    gap: 0.5rem;
  }
}

@media (max-width: 480px) {
  .message-card {
    max-width: 90%;
  }

  .messages-container {
    padding: 1rem !important;
  }
}
</style>
