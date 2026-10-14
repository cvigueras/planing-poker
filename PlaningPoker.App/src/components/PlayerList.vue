<template>
    <aside class="players" :class="{ mobileList: mobile }">
        <ShowMessageModal modaltype="disconnected" v-show="showMessageModal" />

        <div v-if="!mobile" class="playersHead">
            <div class="playersTitle">
                <h2>Players</h2>
                <span class="count">{{ votedCount }} of {{ users.length }} voted</span>
            </div>
            <div class="progress"><div class="progressBar" :style="{ width: progress + '%' }"></div></div>
        </div>

        <div class="rows">
            <div v-for="user in users" :key="user.name" class="row" :class="{ self: isSelf(user) }">
                <span class="avatar" :class="{ avatarAdmin: user.admin }">{{ user.name.charAt(0).toUpperCase() }}</span>
                <div class="who">
                    <span class="name">{{ user.name }}</span>
                    <span class="tag">{{ tagFor(user) }}</span>
                </div>

                <span v-if="revealed" class="valueBox">
                    <svg v-if="user.value === 'coffee'" width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="#10201B" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M4 9h12v6a5 5 0 0 1-5 5h-2a5 5 0 0 1-5-5V9z" /><path d="M16 11h2a2.5 2.5 0 0 1 0 5h-2" /><path d="M8 3v2M12 3v2" /></svg>
                    <template v-else>{{ user.value || '–' }}</template>
                </span>
                <span v-else-if="user.hasVoted" class="chip chipVoted">
                    <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M5 12.5l4.5 4.5L19 7.5" /></svg>
                    Voted
                </span>
                <span v-else class="chip chipWaiting">Waiting</span>

                <button v-if="isAdmin && !user.admin" type="button" class="btnRemove" :aria-label="'Remove ' + user.name + ' from the game'" @click="removeUser($event, user.name)">
                    <svg v-if="mobile" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M18 6L6 18M6 6l12 12" /></svg>
                    <template v-else>Remove</template>
                </button>
            </div>
        </div>

        <GameActions v-if="!mobile" />
    </aside>
</template>

<script lang="js">

    import ShowMessageModal from './ShowMessageModal.vue';
    import GameActions from './GameActions.vue';
    import signalr from '../signalr/signalR.js';

    export default {
        components: {
            ShowMessageModal, GameActions
        },
        props: {
            mobile: Boolean
        },
        computed: {
            revealed() {
                return this.$store.state.revealed;
            },
            users() {
                return this.$store.state.users
            },
            isAdmin() {
                let gameId = sessionStorage.getItem("gameid");
                let user = sessionStorage.getItem("username");
                return this.$store.getters.userIsAdmin(gameId, user);
            },
            votedCount() {
                return this.$store.getters.votedCount;
            },
            progress() {
                return this.users.length ? Math.round((this.votedCount / this.users.length) * 100) : 0;
            }
        },
        data() {
            return {
                showMessageModal: false,
            };
        },
        methods: {
            isSelf(user) {
                return user.name === sessionStorage.getItem("username");
            },
            tagFor(user) {
                if (user.admin) {
                    return this.isSelf(user) ? 'Admin · You' : 'Admin';
                }
                return this.isSelf(user) ? 'You' : 'Player';
            },
            subscribeEvents() {
                this.signalHandlers = {
                    OnNotifyUserHasVoted: (user, vote) => {
                        this.$store.commit('SET_USER_VOTED', {
                            name: user,
                            gameId: sessionStorage.getItem('gameid'),
                            value: vote
                        });
                    },
                    OnResetVotes: () => {
                        this.$store.commit('RESET_VOTES');
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
                        signalr.stop();
                        this.showMessageModal = true;
                    },
                    OnRemoveGroup: (userName) => {
                        const userRemoved = {
                            name: userName,
                            gameId: sessionStorage.getItem('gameid'),
                        }
                        this.$store.dispatch('removeToUsers', JSON.stringify(userRemoved));
                    },
                    OnReceiveAllVotes: (votes) => {
                        this.$store.commit('UPDATE_USERS', votes);
                        this.$store.commit('SET_REVEALED', true);
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
                sessionStorage.removeItem('revealed');
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
