<template>
    <img alt="Vue logo" src="../assets/logo.png">
    <div class="divContainer">
        <label for="uname"><b>Planing Poker</b></label>
        <br /><br />
        <input v-model="username" type="text" placeholder="Username" name="uname" required>
        <input v-model="gamename" type="text" placeholder="GameName" name="psw" required>
        <button @click="getValues" type="submit">
            <router-link to="/planing"> Create game</router-link>
        </button>
    </div>
</template>

<script>
    import axios from 'axios';

    export default {

        data() {
            return {
                loading: false,
                post: null
            };
        },
        created() {
            this.fetchData();
        },
        watch: {
            '$route': 'fetchData'
        },
        methods: {
            getValues() {
                const gameCreated = {
                    createdBy: this.username,
                    title: this.gamename,
                    description: "Vote for Release1",
                    roundTime: 60,
                    expiration: 60
                };
                console.log(this.username);
                console.log(this.gamename);
                this.fetchData(gameCreated);
            },
            fetchData(game) {
                axios.post('game', game)
                    .then(response => {
                        this.getGame(response.data)
                        this.post = response;
                        this.$store.dispatch('addToGames', response.data)
                        this.loading = false;
                        this.$router.push('/planing'); 
                        return;
                    }).catch(error => console.log(error))
            },
            getGame(Id) {
                console.log("GameId value: " + Id);
                axios.get('game/' + Id)
                    .then(response => {

                        this.info = console.log(response.data);
                    })
                    .catch(error => console.log(error))
            }
        },

    }
</script>

<style scoped>
    .divContainer {
        margin: auto;
        width: 50%;
        border: 1px solid green;
        padding: 30px;
        border-style: dotted;
    }

    input[type=text], input[type=password] {
        width: 100%;
        padding: 12px 20px;
        margin: 8px 0;
        display: inline-block;
        border: 1px solid #ccc;
        box-sizing: border-box;
    }

    button {
        background-color: #04AA6D;
        color: white;
        padding: 14px 20px;
        margin: 8px 0;
        border: none;
        cursor: pointer;
        width: 100%;
    }

        button:hover {
            opacity: 0.8;
        }
</style>