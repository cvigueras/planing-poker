import * as signalR from '@aspnet/signalr'

const url = process.env.VUE_APP_ENVIROMENT_SIGNALR

const signal = new signalR.HubConnectionBuilder()
    .withUrl(url, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
    })
    .configureLogging(signalR.LogLevel.Information)
    .build()

signal.onclose((err) => {
    console.log("You have been disconnected", err)
})

export default {
    signal
}