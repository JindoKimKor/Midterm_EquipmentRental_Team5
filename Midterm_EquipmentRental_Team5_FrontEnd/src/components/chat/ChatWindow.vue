<template>
  <div class="chat-window">
    <!-- Header -->
    <v-card class="chat-header" flat>
      <v-card-text class="pa-4">
        <div class="header-content">
          <div class="header-left">
            <v-btn icon size="small" @click="goBack" class="mr-2">
              <v-icon>mdi-arrow-left</v-icon>
              <v-tooltip activator="parent" location="bottom">Go Back</v-tooltip>
            </v-btn>
            <v-icon class="mr-2">mdi-chat</v-icon>
            <div>
              <h2 class="text-h6 mb-0">{{ chatStore.currentRoom }}</h2>
              <p class="text-caption mb-0">
                {{ chatStore.userCount }} user{{ chatStore.userCount !== 1 ? 's' : '' }} online
              </p>
            </div>
          </div>
          <div class="header-right">
            <v-menu>
              <template #activator="{ props }">
                <v-btn icon v-bind="props">
                  <v-icon>mdi-dots-vertical</v-icon>
                </v-btn>
              </template>
              <v-list>
                <v-list-item @click="showOnlineUsers = true">
                  <template #prepend>
                    <v-icon>mdi-account-multiple</v-icon>
                  </template>
                  <v-list-item-title>View Members</v-list-item-title>
                </v-list-item>
                <v-divider />
                <v-list-item @click="handleDisconnect">
                  <template #prepend>
                    <v-icon>mdi-logout</v-icon>
                  </template>
                  <v-list-item-title>Leave Chat</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
          </div>
        </div>
      </v-card-text>
    </v-card>

    <!-- Messages -->
    <MessageList />

    <!-- Input -->
    <MessageInput />

    <!-- Online Users Dialog -->
    <v-dialog v-model="showOnlineUsers" max-width="400">
      <v-card>
        <v-card-title>Online Members</v-card-title>
        <v-divider />
        <v-card-text class="pa-0">
          <v-list>
            <v-list-item v-for="user in chatStore.onlineUsers" :key="user.connectionId">
              <template #prepend>
                <v-icon class="mr-2">mdi-account-circle</v-icon>
              </template>
              <v-list-item-title>{{ user.username }}</v-list-item-title>
              <template #append>
                <v-icon color="success" size="small">mdi-circle</v-icon>
              </template>
            </v-list-item>
          </v-list>
        </v-card-text>
        <v-divider />
        <v-card-actions>
          <v-spacer />
          <v-btn color="primary" @click="showOnlineUsers = false">Close</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useChatStore } from '@/stores/ChatStore'
import MessageList from './MessageList.vue'
import MessageInput from './MessageInput.vue'

const router = useRouter()
const chatStore = useChatStore()
const showOnlineUsers = ref(false)

const handleDisconnect = async () => {
  await chatStore.disconnect()
  // Optionally redirect or handle disconnect
}

const goBack = async () => {
  await chatStore.disconnect()
  router.back()
}
</script>

<style scoped>
.chat-window {
  display: flex;
  flex-direction: column;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.02);
}

.chat-header {
  border-bottom: 1px solid rgba(0, 0, 0, 0.12);
}

.dark .chat-header {
  border-bottom: 1px solid rgba(255, 255, 255, 0.12);
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

.header-right {
  display: flex;
  gap: 8px;
}
</style>
