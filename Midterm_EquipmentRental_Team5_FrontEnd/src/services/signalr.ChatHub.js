import * as signalR from '@microsoft/signalr'

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

/**
 * Gets the current SignalR connection
 * @returns {signalR.HubConnection|null} The connection or null if not initialized
 */
export function getConnection() {
  return connection
}

/**
 * Closes the SignalR connection
 */
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

/**
 * Checks if the SignalR connection is active
 * @returns {boolean} True if connected, false otherwise
 */
export function isConnected() {
  return connection?.state === signalR.HubConnectionState.Connected
}

/**
 * Sends a message through SignalR
 * @param {string} room - The chat room
 * @param {string} username - The sender's username
 * @param {string} message - The message content
 * @returns {Promise<void>}
 */
export async function sendMessage(room, username, message) {
  if (!connection) {
    throw new Error('SignalR connection not initialized')
  }
  return await connection.invoke('SendMessage', room, username, message)
}

/**
 * Registers a callback for receiving messages
 * @param {Function} callback - Function to call when a message is received
 */
export function onReceiveMessage(callback) {
  if (!connection) {
    throw new Error('SignalR connection not initialized')
  }
  connection.on('ReceiveMessage', callback)
}
