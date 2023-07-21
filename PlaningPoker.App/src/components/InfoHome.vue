<template>

    <img v-if="loading" src="../assets/loading.gif" alt="Loading" class="loading" />

    <div v-else>
        <div class="divInfoMatch">
            <div class="textInfo">{{ totalMatchs }} partidas jugadas hasta hoy.</div>
            <div class="iconInfoMatch">&#9731;</div>
        </div>

        <div class="divInfoUser">
            <div class="textInfo">{{ totalUsers }} jugadores han pasado por aquí</div>
            <div class="iconInfoUser">&#9879;</div>
        </div>
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
                loading: false
            };
        },
        methods: {
            fetchTotalUsers() {
                this.loading = true;
                axios.get('users/GetTotalNumberUsers')
                    .then(response => {
                        this.totalUsers = response.data;
                        this.fetchTotalGame();
                    }).catch(error => console.log(error))
            },
            fetchTotalGame() {
                axios.get('game/GetNumberMatchs')
                    .then(response => {
                        this.totalMatchs = response.data;
                        setTimeout(() => {
                            this.loading = false;
                        }, 1000);
                    }).catch(error => console.log(error))
            },
        }
    }
</script>

<style scoped>
    @import "../css/info.css";
</style>