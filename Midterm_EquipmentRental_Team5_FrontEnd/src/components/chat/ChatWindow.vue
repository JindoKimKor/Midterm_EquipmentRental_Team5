<template>
  <div class="d-flex flex-column h-100">
    <v-card elevation="2" rounded="0" class="flex-shrink-0">
      <v-card-text class="d-flex align-center pa-4">
        <div class="pr-4">
          <v-avatar size="40" color="primary" class="font-weight-bold">
            {{ getInitials(selectedUser.name) }}
          </v-avatar>
        </div>
        <div>
          <h2 class="text-h6 mb-0 font-weight-bold text-truncate">{{ selectedUser.name }}</h2>
        </div>
      </v-card-text>
    </v-card>
    <v-divider />
    <MessageList :selectedUser="selectedUser" class="flex-grow-1 overflow-y-auto" />
    <MessageInput :selectedUser="selectedUser" :chatId="chatId" />
  </div>
</template>

<script setup>
import MessageList from './MessageList.vue'
import MessageInput from './MessageInput.vue'
import { useChatStore } from '@/stores/ChatStore'
import { getChatHistory } from '@/api/ChatController'
import { createConnection, getConnection } from '@/services/signalr.ChatHub'
import { onBeforeMount, onBeforeUnmount, watch } from 'vue'

const chatStore = useChatStore()
let messageListener = null

const props = defineProps({
  selectedUser: {
    type: Object,
    required: true,
    default: () => ({ name: 'User', id: null }),
  },
  chatId: {
    type: Number,
    required: true,
  },
})

const loadChatHistory = async () => {
  try {
    chatStore.setConnecting(true)
    const response = await getChatHistory(props.chatId)
    chatStore.loadMessages(response || [])
    chatStore.setConnecting(false)
  } catch (error) {
    console.error('Failed to load chat history:', error)
    chatStore.setError('Failed to load chat history')
    chatStore.loadMessages([])
    chatStore.setConnecting(false)
  }
}

watch(
  () => props.chatId,
  async () => {
    console.log('Trigger')
    await loadChatHistory()
  },
  { immediate: true },
)

onBeforeMount(async () => {
  await initializeSignalR()
})

onBeforeUnmount(() => {
  const connection = getConnection()
  if (connection && messageListener) {
    connection.off('ReceiveMessage', messageListener)
  }
})

const initializeSignalR = async () => {
  try {
    const connection = createConnection()

    if (connection.state === 'Disconnected') await connection.start()

    if (messageListener) {
      connection.off('ReceiveMessage', messageListener)
    }

    messageListener = (senderId, chatId, messageContent, timestamp, senderName) => {
      chatStore.addMessage({
        senderId: senderId,
        senderName: senderName || 'Unknown',
        content: messageContent,
        chatId: chatId,
        timestamp: timestamp || new Date().toISOString(),
        isRead: false,
      })
    }

    connection.on('ReceiveMessage', messageListener)
  } catch (error) {
    console.error('Failed to initialize SignalR:', error)
    chatStore.setError('Failed to connect to chat server')
  }
}

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
