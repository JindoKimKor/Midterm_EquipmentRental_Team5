<template>
  <div class="chat-window">
    <div class="chat-header">
      <div class="header-content">
        <div class="header-left">
          <v-avatar size="40" color="primary" class="font-weight-bold mr-3">
            {{ getInitials(selectedUser.name) }}
          </v-avatar>
          <div class="header-info">
            <h2 class="text-h6 mb-0 font-weight-bold">{{ selectedUser.name }}</h2>
            <p class="text-caption text-medium-emphasis mb-0">Chat History</p>
          </div>
        </div>
      </div>
    </div>
    <MessageList :selectedUser="selectedUser" />
    <MessageInput :selectedUser="selectedUser" />
  </div>
</template>

<script setup>
import MessageList from './MessageList.vue'
import MessageInput from './MessageInput.vue'

defineProps({
  selectedUser: {
    type: Object,
    required: true,
    default: () => ({ name: 'User', connectionId: null }),
  },
})

const getInitials = (name) => {
  if (!name) return '?'
  return name
    .split(' ')
    .map((word) => word[0])
    .join('')
    .toUpperCase()
    .slice(0, 2)
}
</script>

<style scoped>
.chat-window {
  display: flex;
  flex-direction: column;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.02);
}

.dark .chat-window {
  background-color: rgba(255, 255, 255, 0.01);
}

.chat-header {
  padding: 16px 24px;
  border-bottom: 1px solid rgba(0, 0, 0, 0.08);
  background: rgba(255, 255, 255, 0.5);
  backdrop-filter: blur(8px);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
}

.dark .chat-header {
  background: rgba(30, 30, 30, 0.7);
  border-bottom: 1px solid rgba(255, 255, 255, 0.08);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
}

.header-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 12px;
}

.header-info {
  display: flex;
  flex-direction: column;
}
</style>
