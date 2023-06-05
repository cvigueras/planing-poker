import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import signalr from './signalr/signalR.js'

const app = createApp(App).use(router).use(store)
app.config.globalProperties.$signalr = signalr.signal;
app.mount('#app')