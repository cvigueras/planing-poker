<template>
    <HeaderPlaning />

    <img v-if="loading" src="../assets/loading.gif" alt="Loading" class="loading" />

    <div v-else class="content">
        <table id="customers">
            <tr>
                <th>Admin</th>
                <th>Title</th>
                <th>Description</th>
                <th>Share Game Id</th>
            </tr>
            <tr>
                <td>{{ createdBy }}</td>
                <td>{{ title }}</td>
                <td>{{ description }}</td>
                <td>
                    <input type="text" id="urlShare" name="urlValue" :value="urlValue" class="inputGame">
                    <input type="button" @click="copyUrl" value="Copy id" class="btnGame">
                </td>
            </tr>
        </table>

        <CardList :id="this.id" />

        <PlayerList />
    </div>
</template>


<script lang="js">
    import axios from 'axios';
    import CardList from './CardList.vue'
    import PlayerList from './PlayerList.vue'
    import HeaderPlaning from './HeaderPlaning.vue';

    export default {
        components: {
            CardList, HeaderPlaning, PlayerList
        },
        computed: {
            urlValue() {
                return this.id;
            }
        },
        data() {
            return {
                loading: false,
                game: this.game
            };
        },
        created() {
            this.getGame();
            this.onDisconnect();
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
                let admin = false;
                if (JSON.parse(sessionStorage.getItem("users")).length == 1) {
                    admin = true;
                }
                this.$signalr.start().then(() => {
                    this.$signalr
                        .invoke('JoinGroup', gameId, userName, admin)
                        .catch(function (err) { console.error(err) })
                });
            },
            buildGame(game) {
                this.id = game.id;
                this.createdBy = game.createdBy;
                this.title = game.title;
                this.description = game.description;
                this.roundTime = game.roundTime;
                this.expiration = game.expiration;
                sessionStorage.setItem('users', JSON.stringify(game.users));
                setTimeout(() => {
                    this.game = game;
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
                let copyText = document.getElementById("urlShare");
                copyText.select();
                copyText.setSelectionRange(0, 99999);
                navigator.clipboard.writeText(copyText.value);
            },
            onDisconnect() {
                this.$signalr.on('UserDisconnected', (connectionId) => {
                    console.log(connectionId);
                }); 
            }
        },
    }
</script>

<style scoped>
    @import "../css/game.css";
</style>