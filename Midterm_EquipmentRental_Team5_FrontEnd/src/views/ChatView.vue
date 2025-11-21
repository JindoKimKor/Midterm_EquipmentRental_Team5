<template>
  <div class="chat-view">
    <v-navigation-drawer permanent width="280" class="chat-sidebar" :rail="false" border>
      <div class="sidebar-title-section d-flex align-center">
        <v-btn icon size="small" @click="goBack">
          <v-icon>mdi-arrow-left</v-icon>
          <v-tooltip activator="parent" location="right">Go Back</v-tooltip>
        </v-btn>
        <h2 class="text-h6 font-weight-bold mb-0">Messages</h2>
      </div>
      <div class="search-section">
        <v-text-field
          v-model="searchQuery"
          placeholder="Search users..."
          variant="outlined"
          density="compact"
          prepend-inner-icon="mdi-magnify"
          clearable
          hide-details
          class="search-input"
        />
      </div>
      <v-list v-if="filteredUsers.length > 0" class="users-list">
        <template v-for="(user, index) in filteredUsers" :key="user.connectionId">
          <v-list-item
            class="user-item"
            @click="selectUser(user)"
            :active="selectedUser?.connectionId === user.connectionId"
          >
            <template #prepend>
              <v-avatar size="36" color="primary" class="font-weight-bold">
                {{ getInitials(user.name) }}
              </v-avatar>
            </template>
            <v-list-item-title class="font-weight-500">{{ user.name }}</v-list-item-title>
            <template #append>
              <v-icon
                v-if="selectedUser?.connectionId === user.connectionId"
                size="20"
                color="primary"
                class="active-indicator"
              >
                mdi-check-circle
              </v-icon>
            </template>
          </v-list-item>
          <v-divider v-if="index < filteredUsers.length - 1" class="my-1" />
        </template>
      </v-list>

      <div v-else class="empty-users-state">
        <v-icon size="48" color="disabled" class="mb-3">mdi-account-multiple-outline</v-icon>
        <p class="text-caption text-disabled text-center">
          {{ searchQuery ? 'No users found' : 'No users available' }}
        </p>
      </div>
    </v-navigation-drawer>
    <div class="chat-main">
      <transition name="chat-transition" mode="out-in">
        <div v-if="selectedUser" key="chat" class="chat-container">
          <ChatWindow :selectedUser="selectedUser" />
        </div>

        <div v-else key="empty" class="empty-state">
          <v-icon size="96" color="disabled" class="mb-4">mdi-chat-outline</v-icon>
          <h3 class="text-h6 text-disabled mb-2">Select a user to start chatting</h3>
          <p class="text-caption text-disabled">Choose a user from the list to view chat history</p>
        </div>
      </transition>
    </div>
  </div>
</template>

<script setup>
import { ref, onBeforeMount, computed } from 'vue'
import { useRouter } from 'vue-router'
import ChatWindow from '@/components/chat/ChatWindow.vue'
import { getAllCustomers } from '@/api/CustomerController'

const router = useRouter()
const users = ref([])
const selectedUser = ref(null)
const searchQuery = ref('')

onBeforeMount(async () => {
  try {
    users.value = await getAllCustomers()
  } catch (error) {
    console.error('Failed to load users:', error)
  }
})

const filteredUsers = computed(() => {
  const query = searchQuery.value.toLowerCase().trim()
  if (!query) return users.value
  return users.value.filter((user) =>
    user.name.toLowerCase().includes(query)
  )
})

const selectUser = (user) => {
  selectedUser.value = user
}

const goBack = () => {
  router.back()
}

const getInitials = (name) => {
  if (!name) return '?'
  return name
    .split(' ')
    .map((word) => word[0])
    .join('')
    .toUpperCase()
    .slice(0, 2)
}
</script>

<style scoped>
/* Main Layout */
.chat-view {
  display: flex;
  height: calc(100vh - 64px);
  background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
}

.dark .chat-view {
  background: linear-gradient(135deg, #2a2d31 0%, #1e1f23 100%);
}

/* Sidebar */
.chat-sidebar {
  border-right: 1px solid rgba(0, 0, 0, 0.08);
  background-color: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(10px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
}

.dark .chat-sidebar {
  border-right: 1px solid rgba(255, 255, 255, 0.08);
  background-color: rgba(30, 30, 30, 0.95);
}

/* Main Chat Area */
.chat-main {
  flex: 1;
  display: flex;
  flex-direction: column;
  min-width: 0;
}

.chat-container {
  flex: 1;
  display: flex;
  flex-direction: column;
  min-width: 0;
}

.empty-state {
  flex: 1;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 40px;
  text-align: center;
}

/* Sidebar Header */
.sidebar-title-section {
  padding: 12px 12px;
  border-bottom: 1px solid rgba(0, 0, 0, 0.06);
  gap: 12px;
}

.dark .sidebar-title-section {
  border-bottom: 1px solid rgba(255, 255, 255, 0.06);
}

.sidebar-title-section h2 {
  flex: 1;
  white-space: nowrap;
}

/* Search Section */
.search-section {
  padding: 12px 12px;
  background: linear-gradient(180deg, transparent 0%, rgba(0, 0, 0, 0.02) 100%);
  border-bottom: 1px solid rgba(0, 0, 0, 0.04);
}

.dark .search-section {
  background: linear-gradient(180deg, transparent 0%, rgba(255, 255, 255, 0.02) 100%);
  border-bottom: 1px solid rgba(255, 255, 255, 0.04);
}

.search-input :deep(.v-field) {
  border-radius: 8px;
}

/* Users List */
.users-list {
  padding: 8px 0;
  max-height: calc(100vh - 360px);
  overflow-y: auto;
}

/* Custom Scrollbar */
.users-list::-webkit-scrollbar {
  width: 6px;
}

.users-list::-webkit-scrollbar-track {
  background: transparent;
}

.users-list::-webkit-scrollbar-thumb {
  background: rgba(0, 0, 0, 0.2);
  border-radius: 3px;
}

.users-list::-webkit-scrollbar-thumb:hover {
  background: rgba(0, 0, 0, 0.3);
}

.dark .users-list::-webkit-scrollbar-thumb {
  background: rgba(255, 255, 255, 0.2);
}

.dark .users-list::-webkit-scrollbar-thumb:hover {
  background: rgba(255, 255, 255, 0.3);
}

/* User List Item */
.user-item {
  padding: 12px 16px;
  border-radius: 0;
  transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
  margin: 0;
  cursor: pointer;
  border-left: 3px solid transparent;
  user-select: none;
}

.user-item:hover {
  background-color: rgba(0, 0, 0, 0.04);
}

.dark .user-item:hover {
  background-color: rgba(255, 255, 255, 0.05);
}

.user-item.v-list-item--active {
  background-color: rgba(33, 150, 243, 0.08);
  border-left: 3px solid rgb(33, 150, 243);
}

.dark .user-item.v-list-item--active {
  background-color: rgba(33, 150, 243, 0.1);
  border-left: 3px solid rgb(66, 165, 245);
}

/* Active Indicator Animation */
.active-indicator {
  animation: scale-in 0.3s cubic-bezier(0.34, 1.56, 0.64, 1);
}

@keyframes scale-in {
  0% {
    transform: scale(0.6);
    opacity: 0;
  }
  100% {
    transform: scale(1);
    opacity: 1;
  }
}

/* Empty State */
.empty-users-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 40px 20px;
  text-align: center;
  min-height: 250px;
}

/* Chat Window Transitions */
.chat-transition-enter-active {
  transition: all 0.4s cubic-bezier(0.34, 1.56, 0.64, 1);
}

.chat-transition-leave-active {
  transition: all 0.3s cubic-bezier(0.4, 0, 1, 1);
}

.chat-transition-enter-from {
  opacity: 0;
  transform: translateX(30px) scale(0.95);
}

.chat-transition-leave-to {
  opacity: 0;
  transform: translateX(-20px) scale(0.95);
}

.chat-transition-enter-to,
.chat-transition-leave-from {
  opacity: 1;
  transform: translateX(0) scale(1);
}
</style>
