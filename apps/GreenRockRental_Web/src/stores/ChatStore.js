import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useChatStore = defineStore('chat', () => {
  const messages = ref([])
  const isConnecting = ref(false)
  const error = ref(null)

  const messageCount = computed(() => messages.value.length)

  /**
   * Adds a message to the messages array
   * @param {Object} message - Message object with message, timestamp, username, etc.
   */
  const addMessage = (message) => {
    messages.value.push({
      id: message.id || Date.now(),
      chatId: message.chatId || null,
      content: message.content || null,
      receiverId: message.receiverId || null,
      senderId: message.senderId || null,
      senderName: message.senderName || null,
      isRead: message.isRead || false,
      timestamp: message.timestamp || new Date().toISOString(),
    })
  }

  /**
   * Loads messages into the store (replaces current messages)
   * @param {Array} loadedMessages - Array of message objects to load
   */
  const loadMessages = (loadedMessages) => {
    messages.value = (loadedMessages || []).map((msg) => ({
      id: msg.id || msg.messageId || Date.now(),
      ...msg,
      timestamp: msg.timestamp || new Date().toISOString(),
    }))
  }

  /**
   * Clears all messages from the store
   */
  const clearMessages = () => {
    messages.value = []
    error.value = null
  }

  /**
   * Sets loading state
   * @param {boolean} isLoading - Loading state
   */
  const setConnecting = (isLoading) => {
    isConnecting.value = isLoading
  }

  /**
   * Sets error state
   * @param {string|null} errorMsg - Error message
   */
  const setError = (errorMsg) => {
    error.value = errorMsg
  }

  return {
    messages,
    isConnecting,
    error,

    messageCount,

    addMessage,
    loadMessages,
    clearMessages,
    setConnecting,
    setError,
  }
})
