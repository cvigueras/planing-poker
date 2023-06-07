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
                    <input type="text" id="urlShare" name="urlValue" :value="urlValue">
                    <input type="button" @click="copyUrl" value="Copy id">
                </td>
            </tr>
        </table>
        <CardList :id="this.id" />

        <table id="players">
            <tr>
                <th>Player</th>
                <th>Vote</th>
            </tr>
            <tr v-for="user in users" v-bind:key="user">
                <td>{{ user.name }}</td>
                <td>---</td>
            </tr>
        </table>
    </div>
</template>


<script lang="js">
    import CardList from './CardList.vue'
    import HeaderPlaning from './HeaderPlaning.vue';

    export default {
        components: {
            CardList, HeaderPlaning
        },
        computed: {
            users() {
                return this.$store.state.users
            },
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
            this.$signalr.on('OnJoinGroup', (user) => {
                const userAdded = {
                    name: user,
                    gameId: this.id
                }
                var existUser = this.$store.getters.existUser(this.id, user);
                if (existUser == false) {
                    this.$store.dispatch('addToUsers', JSON.stringify(userAdded));
                }
            })
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
                }, "1500");

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

    .loading{
        margin-top: 110px;
    }

    .content {
        position: relative;
        float: left;
        height: auto;
        width: 100%;
        margin-top: 110px;
    }

    /*Customers CSS*/
    #customers {
        font-family: Arial, Helvetica, sans-serif;
        border-collapse: collapse;
        width: 100%;
        margin-bottom: 30px;
    }

    #customers td, #customers th {
        border: 1px solid #ddd;
        padding: 8px;
    }

    #customers tr:nth-child(even) {
        background-color: #f2f2f2;
    }

    #customers tr:hover {
        background-color: #ddd;
    }

    #customers th {
        padding-top: 12px;
        padding-bottom: 12px;
        text-align: left;
        background-color: #04AA6D;
        color: white;
        text-align: center;
    }

    /*Player css*/
    #players {
        font-family: Arial, Helvetica, sans-serif;
        border-collapse: collapse;
        width: 80%;
        margin-left: 10%;
        margin-top: 50px;
        position: relative;
        float: left;
    }

    #players td, #customers th {
        border: 1px solid #ddd;
        padding: 8px;
    }

    #players tr:nth-child(even) {
        background-color: #f2f2f2;
    }

    #players tr:hover {
        background-color: #ddd;
    }

    #players th {
        padding-top: 12px;
        padding-bottom: 12px;
        text-align: center;
        background-color: #3377FF;
        color: white;
    }

    input[type=button] {
        background-color: #B2B1B1;
        border: none;
        color: white;
        padding: 3px 30px;
        text-decoration: none;
        margin: 4px 2px;
        cursor: pointer;
        margin-left: 12px;
    }

    input[type=text] {
        background-color: #B2B1B1;
        border: none;
        color: white;
        padding: 3px 30px;
        text-decoration: none;
        margin: 4px 2px;
        margin-left: 12px;
        width: 260px;
    }
</style>