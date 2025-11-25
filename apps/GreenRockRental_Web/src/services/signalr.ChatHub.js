import * as signalR from '@microsoft/signalr'
import { useChatStore } from '@/stores/ChatStore'

const chatStore = useChatStore()

const HUB_URL = 'http://localhost:5115/chathub'
const RECONNECT_DELAYS = [0, 0, 1000, 3000, 5000, 10000]

let connection = null

/**
 * Creates and initializes SignalR connection
 * @returns {signalR.HubConnection} The SignalR hub connection
 */
export function createConnection() {
  if (connection) {
    return connection
  }

  connection = new signalR.HubConnectionBuilder()
    .withUrl(HUB_URL, {
      withCredentials: true,
    })
    .withAutomaticReconnect(RECONNECT_DELAYS)
    .build()

  setupConnectionHandlers()
  onClearMessages()

  return connection
}

/**
 * Sets up connection state change handlers
 */
function setupConnectionHandlers() {
  connection.onreconnecting((error) => {
    if (import.meta.env.DEV) {
      console.log('Attempting to reconnect to SignalR...', error)
    }
  })

  connection.onreconnected((connectionId) => {
    if (import.meta.env.DEV) {
      console.log('Reconnected to SignalR:', connectionId)
    }
  })

  connection.onclose((error) => {
    if (import.meta.env.DEV) {
      console.log('SignalR connection closed:', error)
    }
    connection = null
  })
}

export function getConnection() {
  return connection
}

export async function closeConnection() {
  if (connection) {
    try {
      await connection.stop()
    } catch (error) {
      if (import.meta.env.DEV) {
        console.error('Error closing SignalR connection:', error)
      }
    }
    connection = null
  }
}

export function isConnected() {
  return connection?.state === signalR.HubConnectionState.Connected
}

export async function sendMessage(receiverId, chatId, message) {
  try {
    if (!connection) throw new Error('SignalR connection not initialized')
    return await connection.invoke('SendMessage', receiverId, chatId, message)
  } catch (error) {
    console.error(error)
  }
}

export function onReceiveMessage(callback) {
  if (!connection) {
    throw new Error('SignalR connection not initialized')
  }
  connection.on('ReceiveMessage', callback)
}

export function onClearMessages() {
  if (!connection) {
    throw new Error('SignalR connection not initialized')
  }
  connection.on('ClearAllMessages', () => {
    chatStore.messages = []
  })
}
