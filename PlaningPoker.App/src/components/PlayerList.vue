<template>
    <ShowMessageModal modaltype="disconnected" v-show="showMessageModal" />
    <div class="results">Results</div>
    <hr />
    <div class="actions">
        <button class="btnShowVotes" @click="showAllVotes()">Show votes</button>
        <button class="btnResetMatch" @click="resetMatch()">Reset match</button>
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
                <p class="btnDelete" @click="removeUser($event)" type="submit" v-bind:id="user.name">Eliminar usuario</p>
            </td>
            <td v-if="user.admin == true"></td>
            <td v-bind:id="`vote-${user.name}`">{{user.value}}</td>
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
            <td></td>
            <td v-bind:id="`vote-${user.name}`">{{user.value}}</td>
        </tr>
    </table>
</template>

<script lang="js">

    import ShowMessageModal from './ShowMessageModal.vue';

    export default {
        components: {
            ShowMessageModal
        },
        computed: {
            users() {
                return this.$store.state.users
            },
        },
        data() {
            return {
                showMessageModal: false,
            };
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
                let gameId = sessionStorage.getItem("gameid");
                let user = sessionStorage.getItem("username");
                return this.$store.getters.userIsAdmin(gameId, user);
            },
            showAllVotes() {
                let gameId = sessionStorage.getItem("gameid");
                this.$signalr
                    .invoke('ReceiveAllVotes', gameId)
                    .catch(function (err) { console.error(err) })
                console.log("Showing all votes");
            },
            resetMatch() {
                let users = JSON.parse(sessionStorage.getItem("users"));
                this.$store.state.users = users;
            },
            removeUser(event) {
                if (event) {
                    event.preventDefault()
                }
                let userName = event.currentTarget.id;
                let gameId = sessionStorage.getItem("gameid");
                this.$signalr
                    .invoke('RemoveFromGroup', userName, gameId)
                    .catch(function (err) { console.error(err) })

                console.log(event.currentTarget.id);
            },
            removeStorage()
            {
                sessionStorage.removeItem('users');
                sessionStorage.removeItem('username');
                sessionStorage.removeItem('gameid');
            }
        },
        mounted() {
            if (this.$store.state.users.length == 0) {
                this.$store.state.users = JSON.parse(sessionStorage.getItem('users'));
            }
        },
        created() {
            this.subscribeEvents();
            this.$signalr.on('OnJoinGroup', (user, admin) => {
                const userAdded = {
                    name: user,
                    gameId: sessionStorage.getItem('gameid'),
                    admin: admin,
                    value: ""
                }
                let existUser = this.$store.getters.existUser(userAdded.gameId, user);
                if (!existUser) {
                    this.$store.dispatch('addToUsers', JSON.stringify(userAdded));
                    sessionStorage.setItem('users', JSON.stringify(this.$store.state.users));
                }
            });

            this.$signalr.on('OnReceiveMessage', (message) => {
                console.log(message);
                this.removeStorage();
                this.$signalr.stop();
                this.showMessageModal = true;

            });

            this.$signalr.on('OnRemoveGroup', (user, connectionId) => {
                const userAdded = {
                    name: user.name,
                    gameId: sessionStorage.getItem('gameid'),
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

            this.$signalr.on('OnReceiveAllVotes', (votes) => {
                this.$store.state.users = votes;
                console.info(votes);
            });
        },
    }
</script>

<style scoped>
    @import "../css/player.css";
</style>