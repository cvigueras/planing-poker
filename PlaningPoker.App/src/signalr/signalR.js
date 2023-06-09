import * as signalR from '@aspnet/signalr'

const url = "https://localhost:7096/hubs/planing"

const signal = new signalR.HubConnectionBuilder()
    .withUrl(url, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
    })
    .configureLogging(signalR.LogLevel.Information)
    .build()

//signal.on('SendAll', (res) => {
//    console.log(res, 'Notication sent!')
//})

//signal.start().then(() => {
//    if (window.Notification) {
//        if (Notification.permission === 'granted') {
//            return console.log('Notification granted!')
//        } else if (Notification.permission !== 'denied') {
//            console.log('Notification permission requered')
//            return Notification.requestPermission((permission) => { console.log("Permission notification", permission) })
//        } else if (Notification.permission === 'denied') {
//            return console.log('Permission denied!')
//        }
//    } else {
//        return console.error('Browser not compatible')
//    }
//    return console.log('Connected!')
//})

signal.onclose((err) => {
    console.log("You have been disconnected", err)
})

export default {
    signal
}