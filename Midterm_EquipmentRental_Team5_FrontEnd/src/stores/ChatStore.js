import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import {
  createConnection,
  getConnection,
  joinRoom,
  leaveRoom,
  sendMessage,
  getOnlineUsers,
  onReceiveMessage,
  onUserJoined,
  onUserLeft,
  onUserDisconnected,
  onOnlineUsers,
  isConnected,
} from '@/services/signalr'

export const useChatStore = defineStore('chat', () => {
  // State
  const messages = ref([])
  const onlineUsers = ref([])
  const currentRoom = ref('general')
  const currentUsername = ref('')
  const isConnecting = ref(false)
  const error = ref(null)

  // Computed
  const messageCount = computed(() => messages.value.length)
  const userCount = computed(() => onlineUsers.value.length)
  const isReady = computed(() => isConnected() && currentUsername.value)

  // Actions
  const initializeConnection = async (room, username) => {
    try {
      isConnecting.value = true
      error.value = null

      // Create connection
      const conn = createConnection()

      // Set up event listeners before starting
      setupEventListeners()

      // Start connection
      if (conn.state === 'Disconnected') {
        await conn.start()
      }

      // Store current room and username
      currentRoom.value = room
      currentUsername.value = username

      // Join the room
      await joinRoom(room, username)

      isConnecting.value = false
    } catch (err) {
      error.value = err.message
      isConnecting.value = false
      console.error('Failed to initialize chat connection:', err)
    }
  }

  const setupEventListeners = () => {
    onReceiveMessage((data) => {
      addMessage({
        ...data,
        isCurrentUser: data.username === currentUsername.value,
      })
    })

    onUserJoined((data) => {
      addSystemMessage(data)
      requestOnlineUsers()
    })

    onUserLeft((data) => {
      addSystemMessage(data)
      requestOnlineUsers()
    })

    onUserDisconnected((data) => {
      addSystemMessage(data)
      requestOnlineUsers()
    })

    onOnlineUsers((users) => {
      onlineUsers.value = users
    })
  }

  const addMessage = (message) => {
    messages.value.push({
      id: Date.now(),
      ...message,
      timestamp: message.timestamp || new Date().toISOString(),
    })
  }

  const addSystemMessage = (data) => {
    addMessage({
      username: 'System',
      message: data.message,
      timestamp: data.timestamp,
      isSystem: true,
    })
  }

  const sendChatMessage = async (text) => {
    if (!text.trim()) return

    try {
      // Add message optimistically (will be sent back by server)
      addMessage({
        username: currentUsername.value,
        message: text,
        timestamp: new Date().toISOString(),
        isCurrentUser: true,
      })

      // Send to server
      await sendMessage(currentRoom.value, currentUsername.value, text)
    } catch (err) {
      error.value = err.message
      console.error('Failed to send message:', err)
    }
  }

  const requestOnlineUsers = async () => {
    try {
      await getOnlineUsers(currentRoom.value)
    } catch (err) {
      console.error('Failed to get online users:', err)
    }
  }

  const disconnect = async () => {
    try {
      if (currentRoom.value && currentUsername.value) {
        await leaveRoom(currentRoom.value, currentUsername.value)
      }
      clearChat()
    } catch (err) {
      console.error('Failed to disconnect:', err)
    }
  }

  const clearChat = () => {
    messages.value = []
    onlineUsers.value = []
    currentRoom.value = 'general'
    currentUsername.value = ''
    error.value = null
  }

  const changeRoom = async (newRoom) => {
    try {
      if (currentRoom.value && currentUsername.value) {
        await leaveRoom(currentRoom.value, currentUsername.value)
      }

      currentRoom.value = newRoom
      messages.value = [] // Clear messages when switching rooms

      await joinRoom(newRoom, currentUsername.value)
      requestOnlineUsers()
    } catch (err) {
      error.value = err.message
      console.error('Failed to change room:', err)
    }
  }

  return {
    // State
    messages,
    onlineUsers,
    currentRoom,
    currentUsername,
    isConnecting,
    error,

    // Computed
    messageCount,
    userCount,
    isReady,

    // Actions
    initializeConnection,
    setupEventListeners,
    addMessage,
    addSystemMessage,
    sendChatMessage,
    requestOnlineUsers,
    disconnect,
    clearChat,
    changeRoom,
  }
})
