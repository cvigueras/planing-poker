<template>
    <HeaderPlaning />

    <img v-if="loading" src="../assets/loading.gif" alt="Loading" class="loading" />

    <div v-else class="content">
        <table id="customers">
            <tr>
                <th>Admin</th>
                <th>Title</th>
                <th>Description</th>
                <th>RoundTime</th>
                <th>Expiration</th>
                <th>Share Game Id</th>
            </tr>
            <tr>
                <td>{{ createdBy }}</td>
                <td>{{ title }}</td>
                <td>{{ description }}</td>
                <td>{{ roundTime }}</td>
                <td>{{ expiration }}</td>
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
            this.fetchData();
        },
        methods: {
            fetchData() {
                this.loading = true;
                var id = localStorage.getItem("gameid");
                var game = this.$store.getters.getCurrentGame(id);
                if (game != undefined) {

                    this.id = game.id;
                }
                else {
                    this.id = id;
                }
                this.createdBy = game.createdBy;
                this.title = game.title;
                this.description = game.description;
                this.roundTime = game.roundTime;
                this.expiration = game.expiration;
                setTimeout(() => {
                    this.game = game;
                    this.loading = false;
                }, "1000");

            },
            signalRTest() {

                //this.$signalr
                //    .invoke('SendMessageToAll', "hmy", "Hola a todos")
                //    .catch(function (err) { return console.error(err) })

                this.$signalr
                    .invoke('JoinGroup', this.id, this.createdBy)
                    .catch(function (err) { return console.error(err) })

                //this.$signalr
                //    .invoke('SendMessageToGroup', this.id, "Hola a todo el grupo")
                //    .catch(function (err) { return console.error(err) })

                //this.$signalr.on('OnReceiveMessage', (user) => {
                //    console.log(user)
                //})
            },
            copyUrl() {
                var copyText = document.getElementById("urlShare");
                copyText.select();
                copyText.setSelectionRange(0, 99999);
                navigator.clipboard.writeText(copyText.value);
            },
        },
    }
</script>

<style scoped>
    @import "../css/game.css";
</style>