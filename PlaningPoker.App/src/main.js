import { createApp } from 'vue'
import axios from 'axios'
import App from './App.vue'
import router from './router'
import store from './store'
import signalr from './signalr/signalR.js'

// In development the requests go through the dev server proxy; in production they go straight to the API.
if (process.env.NODE_ENV === 'production') {
    axios.defaults.baseURL = process.env.VUE_APP_ENVIROMENT
}

const app = createApp(App).use(router).use(store)
app.config.globalProperties.$signalr = signalr.signal;
app.mount('#app')