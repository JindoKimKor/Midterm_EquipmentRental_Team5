<template>
  <div class="d-flex h-100 bg-secondary">
    <v-navigation-drawer permanent width="280" :rail="false" border class="bg-surface elevation-2">
      <template #prepend>
        <div class="d-flex align-center gap-2 pa-3 border-b">
          <v-btn icon size="small" @click="goBack">
            <v-icon>mdi-arrow-left</v-icon>
            <v-tooltip activator="parent" location="right">Go Back</v-tooltip>
          </v-btn>
          <h2 class="text-h6 font-weight-bold mb-0 flex-grow-1">Messages</h2>
        </div>
        <div class="pa-3 border-b">
          <v-text-field
            v-model="searchQuery"
            placeholder="Search chats..."
            variant="outlined"
            density="compact"
            prepend-inner-icon="mdi-magnify"
            clearable
            hide-details
            rounded="sm"
          />
        </div>
      </template>

      <v-list v-if="filteredChats.length > 0" class="flex-grow-1 overflow-y-auto">
        <template v-for="(chat, index) in filteredChats" :key="chat.chatId">
          <v-list-item
            @click="selectChat(chat)"
            :active="selectedChat?.chatId === chat.chatId"
            rounded="0"
          >
            <template #prepend>
              <v-avatar size="36" color="primary" class="font-weight-bold">
                {{ getInitials(getChatName(chat)) }}
              </v-avatar>
            </template>
            <v-list-item-title class="font-weight-500">{{ getChatName(chat) }}</v-list-item-title>
            <template #append>
              <v-icon v-if="selectedChat?.chatId === chat.chatId" size="20" color="primary">
                mdi-check-circle
              </v-icon>
            </template>
          </v-list-item>
          <v-divider v-if="index < filteredChats.length - 1" class="my-1" />
        </template>
      </v-list>

      <div v-else class="d-flex flex-column align-center justify-center pa-8 flex-grow-1">
        <v-icon size="48" color="disabled" class="mb-3">mdi-account-multiple-outline</v-icon>
        <p class="text-caption text-disabled text-center">
          {{ searchQuery ? 'No chats found' : 'No chats available' }}
        </p>
      </div>
    </v-navigation-drawer>

    <div class="d-flex flex-column flex-grow-1 min-width-0">
      <transition name="fade">
        <div v-if="selectedChat" key="chat" class="d-flex flex-column flex-grow-1 min-width-0">
          <ChatWindow :selectedUser="getChatUser(selectedChat)" :chatId="selectedChat.chatId" />
        </div>

        <div
          v-else
          key="empty"
          class="d-flex flex-column align-center justify-center flex-grow-1 pa-8 text-center bg-white"
        >
          <v-icon size="96" class="mb-4">mdi-chat-outline</v-icon>
          <h3 class="text-h6 mb-2">Select a chat to start messaging</h3>
          <p class="text-caption">Choose a chat from the list to view history</p>
        </div>
      </transition>
    </div>
  </div>
</template>

<script setup>
import { ref, onBeforeMount, computed } from 'vue'
import { useRouter } from 'vue-router'
import { getUserChats } from '@/api/ChatController'
import ChatWindow from '@/components/chat/ChatWindow.vue'

const router = useRouter()
const chats = ref([])
const selectedChat = ref(null)
const searchQuery = ref('')

onBeforeMount(async () => {
  try {
    const response = await getUserChats()
    chats.value = response || []
  } catch (error) {
    console.error('Failed to load chats:', error)
  }
})

const filteredChats = computed(() => {
  const query = searchQuery.value.toLowerCase().trim()
  if (!query) return chats.value
  return chats.value.filter((chat) => getChatName(chat).toLowerCase().includes(query))
})

const selectChat = (chat) => {
  selectedChat.value = chat
}

const goBack = () => {
  router.back()
}

/**
 * Gets the display name for a chat
 * Shows the sender or receiver name depending on the context
 */
const getChatName = (chat) => {
  if (!chat) return '?'
  // Show sender name if receiver is Admin, otherwise show receiver name
  return chat.sender?.name || chat.receiver?.name || 'Unknown Chat'
}

/**
 * Gets the user object for the ChatWindow
 * Returns the sender or receiver object
 */
const getChatUser = (chat) => {
  if (!chat) return null
  return chat.sender || chat.receiver
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
