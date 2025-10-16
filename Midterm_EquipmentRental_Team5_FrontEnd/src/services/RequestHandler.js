import axios from 'axios'

const baseURL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api'

const api = axios.create({
  baseURL,
  headers: {
    'Content-Type': 'application/json',
  },
})

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

const handleError = (error) => {
  const message = error.response?.data?.message || error.message || 'Something went wrong'
  console.error('API Error:', message)
  throw new Error(message)
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
