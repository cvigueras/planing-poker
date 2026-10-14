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
        <tr v-for="user in users" v-bind:key="user.name" :class="{ highlight: user.hasVoted }">
            <td>{{ user.name }}</td>
            <td v-if="user.admin == false">
                <p class="btnDelete" @click="removeUser($event, user.name)" type="submit">Eliminar usuario</p>
            </td>
            <td v-if="user.admin == true"></td>
            <td v-bind:id="`vote-${user.name}`">{{ voteText(user) }}</td>
        </tr>
    </table>
    <table v-if="isAdmin() == false" id="players">
        <tr>
            <th>Player</th>
            <th>Actions</th>
            <th>Vote</th>
        </tr>
        <tr v-for="user in users" v-bind:key="user.name" :class="{ highlight: user.hasVoted }">
            <td>{{ user.name }}</td>
            <td></td>
            <td v-bind:id="`vote-${user.name}`">{{ voteText(user) }}</td>
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
            voteText(user) {
                if (user.value) {
                    return user.value;
                }
                return user.hasVoted ? "✔" : "";
            },
            subscribeEvents() {
                this.signalHandlers = {
                    OnNotifyUserHasVoted: (user) => {
                        this.$store.commit('SET_USER_VOTED', {
                            name: user,
                            gameId: sessionStorage.getItem('gameid')
                        });
                    },
                    OnJoinGroup: (user, admin) => {
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
                    },
                    OnReceiveMessage: (message) => {
                        console.log(message);
                        this.removeStorage();
                        this.$signalr.stop();
                        this.showMessageModal = true;
                    },
                    OnRemoveGroup: (user, connectionId) => {
                        const userRemoved = {
                            name: user.name,
                            gameId: sessionStorage.getItem('gameid'),
                        }
                        this.$store.dispatch('removeToUsers', JSON.stringify(userRemoved));
                        if (this.isAdmin()) {
                            console.log("The connection Id of disconnected user is: " + connectionId);
                            this.$signalr
                                .invoke('SendMessageToUser', connectionId, "You has been disconnected")
                                .catch(function (err) { console.error(err) })
                        }
                    },
                    OnReceiveAllVotes: (votes) => {
                        this.$store.commit('UPDATE_USERS', votes);
                        console.info(votes);
                    },
                };
                Object.entries(this.signalHandlers).forEach(([event, handler]) => {
                    this.$signalr.on(event, handler);
                });
            },
            unsubscribeEvents() {
                Object.entries(this.signalHandlers).forEach(([event, handler]) => {
                    this.$signalr.off(event, handler);
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
                let users = JSON.parse(sessionStorage.getItem("users"))
                    .map(user => ({ ...user, hasVoted: false }));
                this.$store.commit('UPDATE_USERS', users);
            },
            removeUser(event, userName) {
                if (event) {
                    event.preventDefault()
                }
                let gameId = sessionStorage.getItem("gameid");
                this.$signalr
                    .invoke('RemoveFromGroup', userName, gameId)
                    .catch(function (err) { console.error(err) })

                console.log(userName);
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
        },
        beforeUnmount() {
            this.unsubscribeEvents();
        },
    }
</script>

<style scoped>
    @import "../css/player.css";
</style>