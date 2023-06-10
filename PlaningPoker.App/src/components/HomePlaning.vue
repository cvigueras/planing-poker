<template>
    <HeaderPlaning />

    <div class="divCreate">
        <input v-model="username" type="text" placeholder="Username" name="username" class="inputHome" required>
        <input v-model="gamename" type="text" placeholder="GameName" name="gamename" class="inputHome" required>
        <input v-model="description" type="text" placeholder="Description" name="description" class="inputHome" required>
        <button class="btnCreate" @click="createGame" type="submit">
            Create game
        </button>
    </div>
    <div class="divJoin">
        <input v-model="username" type="text" placeholder="Username" name="username" class="inputHome" required>
        <input v-model="gameId" type="text" placeholder="Game Id" name="gameId" class="inputHome" required>
        <button class="btnJoin" @click="joinGame" type="submit">
            Join game
        </button>
    </div>
</template>

<script>
    import axios from 'axios';
    import HeaderPlaning from './HeaderPlaning.vue';
    import gameFactory from '../factories/game'

    export default {
        components: {
            HeaderPlaning
        },
        mounted() {
            localStorage.clear();
        },
        methods: {
            createGame() {
                var gameCreated = gameFactory.create(this.username, this.gamename, this.description);
                this.fetchData(gameCreated);
            },
            fetchData(game) {
                axios.post('game', game)
                    .then(response => {
                        game.id = response.data;
                        var user = this.buildUser(game.createdBy, game.id);
                        this.$store.dispatch('addToUsers', JSON.stringify(user));
                        this.$store.dispatch('addToGames', JSON.stringify(game));
                        localStorage.setItem("users", JSON.stringify([user]));
                        localStorage.setItem("gameid", game.id);
                        localStorage.setItem("username", game.createdBy);
                        this.$router.push('/planing');
                    }).catch(error => console.log(error))
            },
            joinGame() {
                var user = this.buildUser(this.username, this.gameId);
                axios.put('game/' + this.gameId, user)
                    .then(response => {
                        var game = response.data;
                        this.$store.state.users = game.users;
                        this.$store.dispatch('addToGames', JSON.stringify(game))
                        localStorage.setItem("users", JSON.stringify(game.users));
                        localStorage.setItem("gameid", game.id);
                        localStorage.setItem("username", user.name);
                        this.$router.push('/planing');
                    }).catch(error => console.log(error))
            },
            buildUser(userName, gameId) {
                return {
                    name: userName,
                    gameId: gameId
                }
            },
            //setLocalData(user, game)
            //{

            //}
        },
    }
</script>

<style scoped>
    @import "../css/home.css";
</style>