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
        },
    }
</script>

<style scoped>
    @import "../css/player.css";
</style>