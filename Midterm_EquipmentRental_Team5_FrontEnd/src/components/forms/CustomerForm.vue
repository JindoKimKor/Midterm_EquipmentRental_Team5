<template>
  <v-card>
    <v-card-title class="text-h5 pa-4">
      {{ isEditMode ? 'Edit Profile' : 'Add Customer' }}
    </v-card-title>

    <v-divider />

    <v-card-text class="pa-6">
      <v-form ref="formRef" v-model="isFormValid">
        <v-row dense>
          <!-- Username -->
          <v-col cols="12">
            <v-text-field
              v-model="form.userName"
              label="Username"
              variant="outlined"
              density="comfortable"
              :readonly="isEditMode"
              :disabled="isEditMode"
              :rules="isEditMode ? [] : [rules.required]"
            />
          </v-col>

          <!-- Password -->
          <v-col cols="12">
            <v-text-field
              v-model="form.password"
              label="Password"
              type="password"
              variant="outlined"
              density="comfortable"
              :rules="isEditMode ? [] : [rules.required, rules.minLength]"
              :placeholder="isEditMode ? 'Leave blank to keep current password' : ''"
            />
          </v-col>

          <!-- Name -->
          <v-col cols="12">
            <v-text-field
              v-model="form.name"
              label="Name"
              variant="outlined"
              density="comfortable"
              :rules="[rules.required]"
            />
          </v-col>

          <!-- Email -->
          <v-col cols="12">
            <v-text-field
              v-model="form.email"
              label="Email"
              type="email"
              variant="outlined"
              density="comfortable"
              :rules="[rules.required, rules.email]"
            />
          </v-col>

          <!-- Role (only show for new customers, not in edit mode) -->
          <v-col cols="12" v-if="!isEditMode">
            <v-select
              v-model="form.role"
              :items="['User', 'Admin']"
              label="Role"
              variant="outlined"
              density="comfortable"
              :rules="[rules.required]"
            />
          </v-col>
        </v-row>
      </v-form>
    </v-card-text>

    <v-divider />

    <v-card-actions class="pa-4">
      <v-spacer />
      <v-btn variant="text" @click="closeDialog">
        Cancel
      </v-btn>
      <v-btn
        color="primary"
        variant="elevated"
        @click="saveCustomer"
        :loading="loading"
        :disabled="!isFormValid"
      >
        {{ isEditMode ? 'SAVE' : 'SUBMIT' }}
      </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script setup>
import { ref, watch, onMounted } from 'vue'
import { createCustomer, updateCustomer } from '@/api/CustomerController'

const props = defineProps({
  customer: {
    type: Object,
    default: null
  },
  isEditMode: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['customer-saved'])

const formRef = ref(null)
const isFormValid = ref(false)
const loading = ref(false)

const form = ref({
  userName: '',
  password: '',
  name: '',
  email: '',
  role: 'User'
})

// Validation rules
const rules = {
  required: (value) => !!value || 'This field is required',
  email: (value) => {
    const pattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    return pattern.test(value) || 'Invalid email address'
  },
  minLength: (value) => {
    if (!value && props.isEditMode) return true
    return value.length >= 5 || 'Password must be at least 5 characters'
  }
}

// ✅ Define functions BEFORE watch
const loadCustomerData = () => {
  if (props.customer && props.customer.id) {
    form.value = {
      userName: props.customer.userName || '',
      password: '',
      name: props.customer.name || '',
      email: props.customer.email || '',
      role: props.customer.role || 'User'
    }
  }
}

const resetForm = () => {
  form.value = {
    userName: '',
    password: '',
    name: '',
    email: '',
    role: 'User'
  }
  formRef.value?.reset()
}

const closeDialog = () => {
  resetForm()
  emit('customer-saved')
}

const saveCustomer = async () => {
  const { valid } = await formRef.value.validate()

  if (!valid) return

  loading.value = true

  try {
    const customerData = {
      name: form.value.name,
      email: form.value.email,
      userName: form.value.userName,
      role: form.value.role
    }

    if (form.value.password) {
      customerData.password = form.value.password
    }

    if (props.isEditMode && props.customer?.id) {
      await updateCustomer(props.customer.id, customerData)
      alert('Profile updated successfully!')
    } else {
      customerData.password = form.value.password
      await createCustomer(customerData)
      alert('Customer added successfully!')
    }

    emit('customer-saved')
    resetForm()
  } catch (error) {
    console.error('Error saving customer:', error)
    alert(`Failed to ${props.isEditMode ? 'update' : 'add'} customer. Please try again.`)
  } finally {
    loading.value = false
  }
}

// Watch comes AFTER function definitions
watch(() => props.customer, (newCustomer) => {
  if (newCustomer && newCustomer.id) {
    loadCustomerData()
  }
}, { immediate: true })

// load on mount for safety
onMounted(() => {
  if (props.customer && props.customer.id) {
    loadCustomerData()
  }
})
</script>
