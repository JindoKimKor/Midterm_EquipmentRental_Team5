import axios from 'axios'
import useAuthenticationStore from '@/stores/Authentication'
import router from '@/router'

const authStore = useAuthenticationStore()

const baseURL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5115/api/'

const api = axios.create({
  baseURL,
  headers: {
    'Content-Type': 'application/json',
  },
})

api.interceptors.request.use((config) => {
  const token = authStore.authToken
  console.log(token)
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

const handleError = (error) => {
  console.error('API Error:', message)
  if (error.status === 401) {
    router.push('/')
  } else {
    throw new Error(message)
  }
}

export const get = async (url, params = {}) => {
  try {
    const res = await api.get(url, { params })
    return res.data
  } catch (err) {
    handleError(err)
  }
}

export const post = async (url, data = {}) => {
  try {
    const res = await api.post(url, data)
    return res.data
  } catch (err) {
    handleError(err)
  }
}

export const put = async (url, data = {}) => {
  try {
    const res = await api.put(url, data)
    return res.data
  } catch (err) {
    handleError(err)
  }
}

export const del = async (url, data = {}) => {
  try {
    const res = await api.delete(url, data)
    return res.data
  } catch (err) {
    handleError(err)
  }
}

export default {
  get,
  post,
  put,
  delete: del,
}
