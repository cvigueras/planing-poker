<template>
    <HeaderPlaning />

    <div class="divCreate">
        <input v-model="username" type="text" placeholder="Username" name="username" class="inputHome" required>
        <input v-model="gamename" type="text" placeholder="GameName" name="gamename" class="inputHome" required>
        <input v-model="description" type="text" placeholder="Description" name="description" class="inputHome" required>
        <input v-model="roundTime" type="number" placeholder="Round Time" name="roundTime" class="btnNumber" required>
        <input v-model="expiration" type="number" placeholder="Expiration Game" name="expiration" class="btnNumber" required>
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

    export default {
        components: {
            HeaderPlaning
        },
        methods: {
            createGame() {
                const gameCreated = {
                    id: '',
                    createdBy: this.username,
                    title: this.gamename,
                    description: this.description,
                    roundTime: this.roundTime,
                    expiration: this.expiration
                };
                this.fetchData(gameCreated);
            },
            fetchData(game) {
                axios.post('game', game)
                    .then(response => {
                        game.id = response.data;
                        this.$store.dispatch('addToGames', JSON.stringify(game));
                        const user =
                        {
                            name: game.createdBy,
                            gameId: game.id
                        };
                        this.$store.dispatch('addToUsers', JSON.stringify(user));
                        localStorage.setItem("gameid", game.id);
                        this.$signalr
                            .invoke('JoinGroup', game.id, game.createdBy)
                            .catch(function (err) { return console.error(err) })
                        this.$router.push('/planing'); 
                    }).catch(error => console.log(error))
            },
            joinGame() {
                const user = {
                    name: this.username,
                    gameId: this.gameId
                }
                axios.put('game/' + this.gameId, user)
                    .then(response => {
                        var game = response.data;
                        this.$store.state.users = game.users;
                        this.$store.dispatch('addToGames', JSON.stringify(game))
                        localStorage.setItem("gameid", game.id);
                        this.$signalr
                            .invoke('JoinGroup', this.gameId, user.name)
                            .catch(function (err) { return console.error(err) })
                        this.$router.push('/planing');
                    }).catch(error => console.log(error))
            },
        },
    }
</script>

<style scoped>
    @import "../css/home.css";
</style>