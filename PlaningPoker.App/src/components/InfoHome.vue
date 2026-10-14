<template>
    <div class="stats">
        <span>{{ totalMatchs }} games played</span>
        <span>{{ totalUsers }} players so far</span>
    </div>
</template>

<script lang="js">
    import axios from 'axios';

    export default {
        created() {
            this.fetchTotalUsers();
        },
        data() {
            return {
                totalUsers: '–',
                totalMatchs: '–'
            };
        },
        methods: {
            fetchTotalUsers() {
                axios.get('users/GetTotalNumberUsers')
                    .then(response => {
                        this.totalUsers = response.data;
                    }).catch(error => console.log(error))
            },
            fetchTotalGame() {
                axios.get('game/GetNumberMatchs')
                    .then(response => {
                        this.totalMatchs = response.data;
                    }).catch(error => console.log(error))
            },
        },
        mounted() {
            this.fetchTotalGame();
        }
    }
</script>

<style scoped>
    @import "../css/info.css";
</style>
