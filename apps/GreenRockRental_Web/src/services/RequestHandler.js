import axios from 'axios'
import router from '@/router'

const baseURL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5115/api/'

const api = axios.create({
  baseURL,
  headers: {
    'Content-Type': 'application/json',
  },
  withCredentials: true,
  maxRedirects: 0, // Prevent following redirects - let frontend handle auth
})

const handleError = (error) => {
  // Suppress logging for expected auth failures (redirects, 401s)
  const status = error.response?.status
  const code = error.code

  if (status === 401 || (status >= 300 && status < 400)) {
    // Expected: user is not authenticated, don't log as error
    // Silently treat as auth failure
  } else if (code !== 'ERR_NETWORK') {
    // Only log actual unexpected errors
    console.error('API Error:', error)
  }

  // Redirect to login on 401 or redirect responses
  if ((status === 401 || (status >= 300 && status < 400)) && code !== 'ERR_NETWORK') {
    router.push('/')
  }

  throw error
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
