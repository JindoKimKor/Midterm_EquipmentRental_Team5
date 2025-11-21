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
    .withAutomaticReconnect([0, 0, 1000, 3000, 5000, 10000])
    .build()

  // Handle connection state changes
  connection.onreconnecting((error) => {
    console.log('Reconnecting to SignalR:', error)
  })

  connection.onreconnected((connectionId) => {
    console.log('Reconnected to SignalR with connection ID:', connectionId)
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

// Chat Hub specific methods
export async function joinRoom(room, username) {
  const conn = getConnection()
  if (conn && conn.state === signalR.HubConnectionState.Connected) {
    await conn.invoke('JoinRoom', room, username)
  }
}

export async function leaveRoom(room, username) {
  const conn = getConnection()
  if (conn && conn.state === signalR.HubConnectionState.Connected) {
    await conn.invoke('LeaveRoom', room, username)
  }
}

export async function sendMessage(room, username, message) {
  const conn = getConnection()
  if (conn && conn.state === signalR.HubConnectionState.Connected) {
    await conn.invoke('SendMessage', room, username, message)
  }
}

export async function getOnlineUsers(room) {
  const conn = getConnection()
  if (conn && conn.state === signalR.HubConnectionState.Connected) {
    await conn.invoke('GetOnlineUsers', room)
  }
}

// Message handlers
export function onReceiveMessage(callback) {
  const conn = getConnection()
  if (conn) {
    conn.on('ReceiveMessage', callback)
  }
}

export function onUserJoined(callback) {
  const conn = getConnection()
  if (conn) {
    conn.on('UserJoined', callback)
  }
}

export function onUserLeft(callback) {
  const conn = getConnection()
  if (conn) {
    conn.on('UserLeft', callback)
  }
}

export function onUserDisconnected(callback) {
  const conn = getConnection()
  if (conn) {
    conn.on('UserDisconnected', callback)
  }
}

export function onOnlineUsers(callback) {
  const conn = getConnection()
  if (conn) {
    conn.on('OnlineUsers', callback)
  }
}