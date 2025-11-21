import * as signalR from '@microsoft/signalr'

let connection = null

export function createConnection() {
  if (connection) {
    return connection
  }

  connection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:5115/chathub', {
      withCredentials: true,
    })
    .withAutomaticReconnect()
    .build()

  // Handle connection state changes
  connection.onreconnecting((error) => {
    console.log('Reconnecting to SignalR:', error)
  })

  connection.onreconnected((connectionId) => {
    console.log('Reconnected to SignalR:', connectionId)
  })

  connection.onclose((error) => {
    console.log('SignalR connection closed:', error)
    connection = null
  })

  return connection
}

export function getConnection() {
  return connection
}

export function closeConnection() {
  if (connection) {
    connection.stop()
    connection = null
  }
}

export function isConnected() {
  return connection && connection.state === signalR.HubConnectionState.Connected
}
