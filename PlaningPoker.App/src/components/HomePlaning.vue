<template>
    <HeaderPlaning />
    <ShowMessageModal modaltype="info" v-show="showMessageModal" @close-modal="showMessageModal = false" />
    <div v-if="loading" class="ring">
        Loading
        <span></span>
    </div>
    <div class="divCreate">
        <input v-model="username" type="text" placeholder="user name" name="username" class="inputHome" required>
        <input v-model="gamename" type="text" placeholder="game name" name="gamename" class="inputHome" required>
        <input v-model="description" type="text" placeholder="description" name="description" class="inputHome" required>
        <button class="btnCreate" @click="createGame" type="submit">
            create game
        </button>
    </div>
    <div class="divJoin">
        <input v-model="name" type="text" placeholder="user name" name="name" class="inputHome" required>
        <input v-model="gameId" type="text" placeholder="game id" name="gameId" class="inputHome" required>
        <button class="btnJoin" @click="joinGame" type="submit">
            join game
        </button>
    </div>

    <InfoHome />

</template>

<script>
    import axios from 'axios';
    import HeaderPlaning from './HeaderPlaning.vue';
    import InfoHome from './InfoHome.vue';
    import gameFactory from '../factories/game'
    import ShowMessageModal from './ShowMessageModal.vue';

    export default {
        components: {
            HeaderPlaning,
            InfoHome,
            ShowMessageModal
        },
        data() {
            return {
                showMessageModal: false,
                loading: false,
            };
        },
        mounted() {
            sessionStorage.clear();
        },
        methods: {
            createGame() {
                this.loading = true;
                let gameCreated = gameFactory.create(this.username, this.gamename, this.description);
                this.fetchData(gameCreated);
            },
            fetchData(game) {
                let user = this.buildUser(this.username, this.gameId);
                axios.post('game', game)
                    .then(response => {
                        this.persistLocalData(response.data, user);
                        this.loading = false;
                    }).catch(error => { this.showMessageModal = true; console.log(error); this.loading = false; })
            },
            joinGame() {
                this.loading = true;
                let user = this.buildUser(this.name, this.gameId);
                axios.put('game/' + this.gameId, user)
                    .then(response => {
                        this.persistLocalData(response.data, user);
                        this.loading = false;
                    }).catch(error => { this.showMessageModal = true; console.log(error); this.loading = false; })
            },
            buildUser(userName, gameId) {
                return {
                    name: userName,
                    gameId: gameId
                }
            },
            persistLocalData(game, user) {
                this.$store.state.users = game.users;
                this.$store.dispatch('addToGames', JSON.stringify(game))
                sessionStorage.setItem("users", JSON.stringify(game.users));
                sessionStorage.setItem("gameid", game.id);
                sessionStorage.setItem("username", user.name);
                this.$router.push('/planing');
            }
        },
    }
</script>

<style scoped>
    @import "../css/home.css";
    @import "../css/loader.css";
</style>