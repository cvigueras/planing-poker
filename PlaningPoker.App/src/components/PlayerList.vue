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
</style>