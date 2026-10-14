import * as signalR from '@aspnet/signalr'

const url = process.env.VUE_APP_ENVIROMENT_SIGNALR

const signal = new signalR.HubConnectionBuilder()
    .withUrl(url, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
    })
    .configureLogging(signalR.LogLevel.Information)
    .build()

const retryDelays = [0, 2000, 5000, 10000, 30000]

let startPromise = null
let stoppedByUser = false

signal.onclose((err) => {
    startPromise = null
    console.log("You have been disconnected", err)
    if (!stoppedByUser) {
        reconnect()
    }
})

// Joins again the game stored in the session, so the server links the new connection id to the user.
const rejoin = () => {
    const gameId = sessionStorage.getItem("gameid")
    const userName = sessionStorage.getItem("username")
    if (gameId && userName) {
        return signal.invoke('JoinGroup', gameId, userName)
    }
}

const reconnect = async (attempt = 0) => {
    try {
        await start()
        await rejoin()
    } catch (err) {
        if (stoppedByUser) {
            return
        }
        const delay = retryDelays[Math.min(attempt, retryDelays.length - 1)]
        console.log(`Reconnection failed, retrying in ${delay} ms`, err)
        setTimeout(() => reconnect(attempt + 1), delay)
    }
}

// Starts the connection only once; later calls reuse the same (pending or finished) start.
const start = () => {
    if (!startPromise) {
        stoppedByUser = false
        startPromise = signal.start().catch((err) => {
            startPromise = null
            throw err
        })
    }
    return startPromise
}

// Stops the connection on purpose, so it is not reconnected automatically.
const stop = () => {
    stoppedByUser = true
    return signal.stop()
}

export default {
    signal,
    start,
    stop
}