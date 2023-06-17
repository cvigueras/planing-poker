<template>
    <table v-if="isAdmin() == true" id="players">
        <tr>
            <th>Player</th>
            <th>Actions</th>
            <th>Vote</th>
        </tr>
        <tr v-for="user in users" v-bind:key="user">
            <td>{{ user.name }}</td>
            <td v-if="user.admin == false">
                <button class="btnCreate" @click="removeUser($event)" type="submit" v-bind:id="user.name" />
            </td>
            <td v-if="user.admin == true">---</td>
            <td>---</td>
        </tr>
    </table>
    <table v-if="isAdmin() == false" id="players">
        <tr>
            <th>Player</th>
            <th>Actions</th>
            <th>Vote</th>
        </tr>
        <tr v-for="user in users" v-bind:key="user">
            <td>{{ user.name }}</td>
            <td>No privileges</td>
            <td>---</td>
        </tr>
    </table>
</template>

<script lang="js">
    export default {
        computed: {
            users() {
                return this.$store.state.users
            },
        },
        methods: {
            isAdmin() {
                var gameId = localStorage.getItem("gameid");
                var user = localStorage.getItem("username");
                return this.$store.getters.userIsAdmin(gameId, user);
            },
            removeUser(event) {
                if (event) {
                    event.preventDefault()
                }
                var userName = event.currentTarget.id;
                var gameId = localStorage.getItem("gameid");
                this.$signalr
                    .invoke('RemoveFromGroup', userName, gameId)
                    .catch(function (err) { console.error(err) })
                
                console.log(event.currentTarget.id);
            },
        },
        mounted() {
            if (this.$store.state.users.length == 0) {
                this.$store.state.users = JSON.parse(localStorage.getItem('users'));
            }
        },
        created() {
            this.$signalr.on('OnJoinGroup', (user, admin) => {
                const userAdded = {
                    name: user,
                    gameId: localStorage.getItem('gameid'),
                    admin: admin,
                }
                var existUser = this.$store.getters.existUser(userAdded.gameId, user);
                if (existUser == false) {
                    this.$store.dispatch('addToUsers', JSON.stringify(userAdded));
                }
            });

            this.$signalr.on('OnRemoveGroup', (user) => {
                const userAdded = {
                    name: user,
                    gameId: localStorage.getItem('gameid'),
                }
                var existUser = this.$store.getters.existUser(userAdded.gameId, user);
                if (existUser == false) {
                    this.$store.dispatch('removeToUsers', JSON.stringify(userAdded));
                }
            });
        },
    }
</script>

<style scoped>
    @import "../css/player.css";
</style>