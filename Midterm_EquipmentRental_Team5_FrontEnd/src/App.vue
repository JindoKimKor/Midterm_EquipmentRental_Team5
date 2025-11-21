<template>
  <v-app>
    <v-main>
      <RouterView />
    </v-main>
  </v-app>
</template>

<script setup>
import { RouterView } from 'vue-router'
import { onMounted } from 'vue'
import { createConnection } from './services/signalr'

onMounted(async () => {
  const conn = createConnection()

  conn.on('ReceiveMessage', (user, message) => {
    console.log('Message:', user, message)
  })

  try {
    await conn.start()
    console.log('SignalR connected!')
  } catch (err) {
    console.error('Connection failed:', err)
  }
})
</script>

<style scoped></style>
