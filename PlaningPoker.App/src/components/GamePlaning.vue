<template>
    <div class="room">
        <HeaderPlaning />

        <img v-if="loading" src="../assets/loading.gif" alt="Loading" class="loading" />

        <div v-else class="roomBody">
            <div class="mainCol">
                <section class="panelCard gameInfo">
                    <div class="gameText">
                        <span class="gameAdmin">Admin: {{ createdBy }}</span>
                        <h1>{{ title }}</h1>
                        <p>{{ description }}</p>
                    </div>
                    <div class="shareBox">
                        <div class="shareText">
                            <span class="shareLabel">Share this game ID with your team</span>
                            <span class="shareId">{{ id }}</span>
                        </div>
                        <button type="button" class="btnCopy" @click="copyUrl">
                            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><rect x="9" y="9" width="12" height="12" rx="2" /><path d="M5 15V5a2 2 0 0 1 2-2h8" /></svg>
                            {{ copied ? 'Copied' : 'Copy ID' }}
                        </button>
                    </div>
                </section>

                <CardList :id="id" />
            </div>

            <PlayerList />
        </div>
    </div>
</template>


<script lang="js">
    import axios from 'axios';
    import CardList from './CardList.vue'
    import PlayerList from './PlayerList.vue'
    import HeaderPlaning from './HeaderPlaning.vue';
    import signalr from '../signalr/signalR.js';

    export default {
        components: {
            CardList, HeaderPlaning, PlayerList
        },
        data() {
            return {
                loading: false,
                copied: false,
                id: '',
                createdBy: '',
                title: '',
                description: ''
            };
        },
        created() {
            this.getGame();
            this.onDisconnect();
        },
        beforeUnmount() {
            this.$signalr.off('UserDisconnected', this.onUserDisconnected);
        },
        methods: {
            getGame() {
                this.loading = true;
                let gameid = sessionStorage.getItem("gameid");
                this.fetchGame(gameid);
                this.joinGroup();
            },
            joinGroup() {
                let gameId = sessionStorage.getItem("gameid");
                let userName = sessionStorage.getItem("username");
                signalr.start()
                    .then(() => this.$signalr.invoke('JoinGroup', gameId, userName))
                    .catch(function (err) { console.error(err) });
            },
            buildGame(game) {
                this.id = game.id;
                this.createdBy = game.createdBy;
                this.title = game.title;
                this.description = game.description;
                sessionStorage.setItem('users', JSON.stringify(game.users));
                setTimeout(() => {
                    this.loading = false;
                }, 1000);
            },
            fetchGame(gameId) {
                axios.get('game/' + gameId)
                    .then(response => {
                        this.$store.state.users = response.data.users;
                        this.buildGame(response.data);
                    }).catch(error => console.log(error))
            },
            copyUrl() {
                navigator.clipboard.writeText(this.id).then(() => {
                    this.copied = true;
                    setTimeout(() => { this.copied = false; }, 2000);
                }).catch(error => console.log(error));
            },
            onDisconnect() {
                this.$signalr.on('UserDisconnected', this.onUserDisconnected);
            },
            onUserDisconnected(userName) {
                console.log(userName + " has been disconnected");
            }
        },
    }
</script>

<style scoped>
    @import "../css/game.css";
</style>
