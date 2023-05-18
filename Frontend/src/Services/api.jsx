import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7021/',
  headers: {
    'Content-Type': 'application/json charset=utf-8',
    'Cache-Control': 'max-age=31536000',
  },
  maxBodyLength: Infinity,
});
export default api;
