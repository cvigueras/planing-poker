<template>
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
</template>

<script lang="js">
    export default {
        computed: {
            users() {
                return this.$store.state.users
            },
        },
        mounted() {
            if (this.$store.state.users.length == 0) {
                this.$store.state.users = JSON.parse(localStorage.getItem('users'));
            }
        },
        created() {
            this.$signalr.on('OnJoinGroup', (user) => {
                const userAdded = {
                    name: user,
                    gameId: localStorage.getItem('gameid')
                }
                var existUser = this.$store.getters.existUser(userAdded.gameId, user);
                if (existUser == false) {
                    this.$store.dispatch('addToUsers', JSON.stringify(userAdded));
                }
            })
        },
    }
</script>

<style scoped>
    @import "../css/player.css";
</style>