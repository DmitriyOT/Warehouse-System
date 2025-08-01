import axios from 'axios'

const baseUrlApi = import.meta.env.VITE_APP_API_URL

const $host = axios.create({
    baseURL: baseUrlApi
})

export { $host }