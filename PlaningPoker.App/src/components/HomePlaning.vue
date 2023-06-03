<template>
    <img alt="Vue logo" src="../assets/logo.png">
    <div class="divContainer">
        <label for="uname"><b>Planing Poker</b></label>
        <br /><br />
        <input v-model="username" type="text" placeholder="Username" name="username" required>
        <input v-model="gamename" type="text" placeholder="GameName" name="gamename" required>
        <input v-model="description" type="text" placeholder="Description" name="description" required>
        <input v-model="roundTime" type="number" placeholder="Round Time" name="roundTime" required>
        <input v-model="expiration" type="number" placeholder="Expiration Game" name="expiration" required>
        <button @click="getValues" type="submit">
            Create game
        </button>
    </div>
</template>

<script>
    import axios from 'axios';

    export default {
        methods: {
            getValues() {
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
                        this.$router.push('/planing'); 
                    }).catch(error => console.log(error))
            },
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