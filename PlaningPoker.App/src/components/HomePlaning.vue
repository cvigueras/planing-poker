<template>
    <HeaderPlaning />

    <div class="divCreate">
        <input v-model="username" type="text" placeholder="Username" name="username" required>
        <input v-model="gamename" type="text" placeholder="GameName" name="gamename" required>
        <input v-model="description" type="text" placeholder="Description" name="description" required>
        <input v-model="roundTime" type="number" placeholder="Round Time" name="roundTime" required>
        <input v-model="expiration" type="number" placeholder="Expiration Game" name="expiration" required>
        <button class="btnCreate" @click="createGame" type="submit">
            Create game
        </button>
    </div>
    <div class="divJoin">
        <input v-model="username" type="text" placeholder="Username" name="username" required>
        <input v-model="gameId" type="text" placeholder="Game Id" name="gameId" required>
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
                        this.$store.dispatch('addToGames', JSON.stringify(game))
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

    .divCreate {
        margin: auto;
        margin-top: 110px;
        width: 50%;
        border: 1.5px solid #04AA6D;
        padding: 20px;
        border-style: dotted;
        position: relative;
        float: left;
        margin-left: 25%;
    }

    .divJoin {
        margin: auto;
        width: 30%;
        border: 1.5px solid #3377FF;
        padding: 20px;
        border-style: dotted;
        margin-top: 30px;
        position: relative;
        float: left;
        margin-left: 35%;
    }

    input[type=text] {
        width: 100%;
        padding: 12px 20px;
        margin: 8px 0;
        display: inline-block;
        border: 1px solid #ccc;
        box-sizing: border-box;
    }

    input[type=number] {
        width: 40%;
        padding: 12px 20px;
        margin: 8px 24px;
        display: inline-block;
        border: 1px solid #04AA6D;
        box-sizing: border-box;
    }

    .btnCreate {
        background-color: #04AA6D;
        color: white;
        padding: 14px 20px;
        margin: 8px 0;
        border: none;
        cursor: pointer;
        width: 100%;
    }

        .btnCreate:hover {
            opacity: 0.8;
        }

    .btnJoin {
        background-color: #3377FF;
        color: white;
        padding: 14px 20px;
        margin: 8px 0;
        border: none;
        cursor: pointer;
        width: 100%;
    }

    .btnJoin:hover {
        opacity: 0.8;
    }
</style>