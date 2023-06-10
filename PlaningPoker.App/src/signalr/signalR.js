import * as signalR from '@aspnet/signalr'

const url = "https://localhost:7096/hubs/planing"

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