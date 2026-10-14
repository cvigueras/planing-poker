import * as signalR from '@aspnet/signalr'

const url = process.env.VUE_APP_ENVIROMENT_SIGNALR

const signal = new signalR.HubConnectionBuilder()
    .withUrl(url, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
    })
    .configureLogging(signalR.LogLevel.Information)
    .build()

let startPromise = null

signal.onclose((err) => {
    startPromise = null
    console.log("You have been disconnected", err)
})

// Starts the connection only once; later calls reuse the same (pending or finished) start.
const start = () => {
    if (!startPromise) {
        startPromise = signal.start().catch((err) => {
            startPromise = null
            throw err
        })
    }
    return startPromise
}

export default {
    signal,
    start
}