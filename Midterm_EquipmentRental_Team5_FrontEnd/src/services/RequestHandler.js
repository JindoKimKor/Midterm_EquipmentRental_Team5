import axios from 'axios'
import router from '@/router'

const baseURL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5115/api/'

const api = axios.create({
  baseURL,
  headers: {
    'Content-Type': 'application/json',
  },
  withCredentials: true,
})

const handleError = (error) => {
  console.error('API Error:', error)
  if (error.status === 401) {
    router.push('/')
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
