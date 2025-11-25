import RequestHandler from '@/services/RequestHandler'

/**
 * Gets all chats for the authenticated user
 * @returns {Promise} List of chats for the current user
 */
export async function getUserChats() {
  try {
    const res = await RequestHandler.get('chat')
    return res
  } catch (error) {
    console.error('Failed to fetch user chats:', error)
    throw error
  }
}

/**
 * Gets the message history for a specific chat
 * @param {number} chatId - The ID of the chat
 * @returns {Promise} List of messages in the chat
 */
export async function getChatHistory(chatId) {
  try {
    const res = await RequestHandler.get(`chat/${chatId}`)
    return res
  } catch (error) {
    console.error(`Failed to fetch chat history for chat ${chatId}:`, error)
    throw error
  }
}

/**
 * Sends a message in a chat
 * @param {number} chatId - The ID of the chat
 * @param {SendMessageDto} dto - The message data including sender, receiver, and content
 * @returns {Promise} The sent message details
 */
export async function sendMessage(chatId, dto) {
  try {
    if (!dto.content || !dto.content.trim()) {
      throw new Error('Message content cannot be empty')
    }
    const res = await RequestHandler.post(`chat/${chatId}/message`, dto)
    return res
  } catch (error) {
    console.error(`Failed to send message in chat ${chatId}:`, error)
    throw error
  }
}

/**
 * Deletes a specific message from a chat
 * @param {number} chatId - The ID of the chat
 * @param {number} messageId - The ID of the message to delete
 * @returns {Promise} Success message
 */
export async function deleteMessage(chatId, messageId) {
  try {
    const res = await RequestHandler.delete(`chat/${chatId}/message/${messageId}`)
    return res
  } catch (error) {
    console.error(`Failed to delete message ${messageId} from chat ${chatId}:`, error)
    throw error
  }
}
