<template>
  <div class="d-flex flex-column h-100">
    <v-card elevation="2" rounded="0" class="flex-shrink-0">
      <v-card-text class="d-flex align-center gap-3 pa-4">
        <v-avatar size="40" color="primary" class="font-weight-bold">
          {{ getInitials(selectedUser.name) }}
        </v-avatar>
        <div class="d-flex flex-column min-width-0">
          <h2 class="text-h6 mb-0 font-weight-bold text-truncate">{{ selectedUser.name }}</h2>
          <p class="text-caption text-medium-emphasis mb-0">Chat History</p>
        </div>
      </v-card-text>
    </v-card>
    <MessageList :selectedUser="selectedUser" class="flex-grow-1 overflow-y-auto" />
    <v-divider />
    <MessageInput :selectedUser="selectedUser" class="flex-shrink-0 pa-4" />
  </div>
</template>

<script setup>
import MessageList from './MessageList.vue'
import MessageInput from './MessageInput.vue'

defineProps({
  selectedUser: {
    type: Object,
    required: true,
    default: () => ({ name: 'User', connectionId: null }),
  },
})

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
