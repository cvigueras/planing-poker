<template>
    <div class="results">Results</div>
    <hr />
    <div class="actions">
        <button class="btnShowVotes">Show votes</button>
        <button class="btnResetMatch">Reset match</button>
    </div>
    <table v-if="isAdmin() == true" id="players">
        <tr>
            <th>Player</th>
            <th>Actions</th>
            <th>Vote</th>
        </tr>
        <tr v-for="user in users" v-bind:key="user" v-bind:id="user.name">
            <td>{{ user.name }}</td>
            <td v-if="user.admin == false">
                <p class="btnDelete" @click="removeUser($event)" type="submit" v-bind:id="user.name" >Eliminar usuario</p>
            </td>
            <td v-if="user.admin == true">Show votes</td>
            <td v-bind:id="`vote-${user.name}`">---</td>
        </tr>
    </table>
    <table v-if="isAdmin() == false" id="players">
        <tr>
            <th>Player</th>
            <th>Actions</th>
            <th>Vote</th>
        </tr>
        <tr v-for="user in users" v-bind:key="user" v-bind:id="user.name">
            <td>{{ user.name }}</td>
            <td>No privileges</td>
            <td v-bind:id="`vote-${user.name}`">---</td>
        </tr>
    </table>
</template>

<script lang="js">

    export default {
        computed: {
            users() {
                return this.$store.state.users
            },
        },
        methods: {
            subscribeEvents() {
                this.$signalr.on('OnNotifyUserHasVoted', (user, vote) => {
                    document.getElementById(user).className = "highlight";
                    document.getElementById("vote-" + user).innerHTML = "&#10004;";
                    console.log("El usuario: " + user + "ha votado: " + vote);
                });
            },
            isAdmin() {
                let gameId = localStorage.getItem("gameid");
                let user = localStorage.getItem("username");
                return this.$store.getters.userIsAdmin(gameId, user);
            },
            removeUser(event) {
                if (event) {
                    event.preventDefault()
                }
                let userName = event.currentTarget.id;
                let gameId = localStorage.getItem("gameid");
                this.$signalr
                    .invoke('RemoveFromGroup', userName, gameId)
                    .catch(function (err) { console.error(err) })

                console.log(event.currentTarget.id);
            },
        },
        mounted() {
            if (this.$store.state.users.length == 0) {
                this.$store.state.users = JSON.parse(localStorage.getItem('users'));
            }
        },
        created() {
            this.subscribeEvents();
            this.$signalr.on('OnJoinGroup', (user, admin) => {
                const userAdded = {
                    name: user,
                    gameId: localStorage.getItem('gameid'),
                    admin: admin,
                }
                let existUser = this.$store.getters.existUser(userAdded.gameId, user);
                if (!existUser) {
                    this.$store.dispatch('addToUsers', JSON.stringify(userAdded));
                }
            });

            this.$signalr.on('OnReceiveMessage', (message) => {
                alert(message);
            });

            this.$signalr.on('OnRemoveGroup', (user, connectionId) => {
                const userAdded = {
                    name: user.name,
                    gameId: localStorage.getItem('gameid'),
                }
                let existUser = this.$store.getters.existUser(userAdded.gameId, user);
                if (!existUser) {
                    this.$store.dispatch('removeToUsers', JSON.stringify(userAdded));
                }
                if (this.isAdmin()) {

                    console.log("The connection Id of disconnected user is: " + connectionId);
                    this.$signalr
                        .invoke('SendMessageToUser', connectionId, "You has been disconnected")
                        .catch(function (err) { console.error(err) })
                }
            });
        },
    }
</script>

<style scoped>
    @import "../css/player.css";
</style>