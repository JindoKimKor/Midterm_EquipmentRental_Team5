<template>
  <div class="chat-view">
    <!-- Sidebar with room list -->
    <v-navigation-drawer
      permanent
      width="250"
      class="chat-sidebar"
      :rail="false"
      border
    >
      <div class="sidebar-header">
        <v-btn icon size="small" @click="goBack" class="mb-2">
          <v-icon>mdi-arrow-left</v-icon>
          <v-tooltip activator="parent" location="right">Go Back</v-tooltip>
        </v-btn>
      </div>

      <v-card flat>
        <v-card-title class="d-flex align-center gap-2">
          <v-icon>mdi-chat-multiple</v-icon>
          <span>Rooms</span>
        </v-card-title>
        <v-divider />
      </v-card>

      <v-list class="rooms-list">
        <v-list-item
          v-for="room in availableRooms"
          :key="room"
          :active="chatStore.currentRoom === room"
          @click="handleRoomChange(room)"
        >
          <template #prepend>
            <v-icon>mdi-door-open</v-icon>
          </template>
          <v-list-item-title>{{ room }}</v-list-item-title>
        </v-list-item>
      </v-list>

      <v-divider />

      <v-card flat class="user-info-card">
        <v-card-text class="text-center pa-4">
          <div class="mb-2">
            <v-icon size="32" class="text-primary">mdi-account</v-icon>
          </div>
          <p class="text-caption font-weight-bold mb-1">{{ chatStore.currentUsername }}</p>
          <v-chip size="small" label>Online</v-chip>
        </v-card-text>
      </v-card>
    </v-navigation-drawer>

    <!-- Main chat area -->
    <div class="chat-main">
      <v-fade-transition>
        <div v-if="!chatStore.isReady && !chatStore.isConnecting" class="join-form">
          <v-card max-width="400" class="mx-auto">
            <v-card-title>Join Chat</v-card-title>
            <v-card-text>
              <v-form @submit.prevent="handleJoinChat">
                <v-text-field
                  v-model="joinUsername"
                  label="Username"
                  placeholder="Enter your username"
                  variant="outlined"
                  class="mb-4"
                  :rules="[v => !!v || 'Username is required']"
                  @keydown.enter="handleJoinChat"
                />

                <v-select
                  v-model="joinRoom"
                  label="Select Room"
                  :items="availableRooms"
                  variant="outlined"
                  class="mb-4"
                />

                <v-btn
                  type="submit"
                  color="primary"
                  block
                  :loading="isJoining"
                  :disabled="!joinUsername.trim()"
                >
                  Join Chat
                </v-btn>
              </v-form>

              <v-alert v-if="joinError" type="error" variant="tonal" class="mt-4" closable>
                {{ joinError }}
              </v-alert>
            </v-card-text>
          </v-card>
        </div>

        <ChatWindow v-else />
      </v-fade-transition>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useChatStore } from '@/stores/ChatStore'
import ChatWindow from '@/components/chat/ChatWindow.vue'

const router = useRouter()
const chatStore = useChatStore()
const joinUsername = ref('')
const joinRoom = ref('general')
const availableRooms = ref(['general', 'equipment', 'rentals', 'support'])
const isJoining = ref(false)
const joinError = ref(null)

const handleJoinChat = async () => {
  if (!joinUsername.value.trim()) return

  try {
    isJoining.value = true
    joinError.value = null

    await chatStore.initializeConnection(joinRoom.value, joinUsername.value)
    // Clear form
    joinUsername.value = ''
  } catch (err) {
    joinError.value = err.message || 'Failed to join chat'
    console.error('Failed to join chat:', err)
  } finally {
    isJoining.value = false
  }
}

const handleRoomChange = async (room) => {
  try {
    await chatStore.changeRoom(room)
  } catch (err) {
    joinError.value = err.message || 'Failed to change room'
  }
}

const goBack = () => {
  router.back()
}
</script>

<style scoped>
.chat-view {
  display: flex;
  height: calc(100vh - 64px); /* Account for navbar */
  background-color: #f5f5f5;
}

.dark .chat-view {
  background-color: #1e1e1e;
}

.chat-sidebar {
  background-color: background;
  border-right: 1px solid rgba(0, 0, 0, 0.12);
}

.dark .chat-sidebar {
  border-right: 1px solid rgba(255, 255, 255, 0.12);
}

.chat-main {
  flex: 1;
  display: flex;
  flex-direction: column;
  min-width: 0;
}

.join-form {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 24px;
}

.sidebar-header {
  padding: 12px 8px;
  display: flex;
  justify-content: flex-start;
  border-bottom: 1px solid rgba(0, 0, 0, 0.12);
}

.dark .sidebar-header {
  border-bottom: 1px solid rgba(255, 255, 255, 0.12);
}

.rooms-list {
  padding: 8px 0;
}

.user-info-card {
  margin: auto 8px 8px 8px;
  border: 1px solid rgba(0, 0, 0, 0.12);
}

.dark .user-info-card {
  border: 1px solid rgba(255, 255, 255, 0.12);
}
</style>
