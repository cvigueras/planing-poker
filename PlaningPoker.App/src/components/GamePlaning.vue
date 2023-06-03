<template>
    <div class="post">
        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>

        <div v-if="post" class="content">
            <div>
                <h1>Planing Poker Game details:</h1>
                <p>Share game: {{ id }}</p>
                <p>Player: {{ createdBy }}</p>
                <p>Title: {{ title }}</p>
                <p>Description: {{ description }}</p>
                <p>RoundTime: {{ roundTime }}</p>
                <p>Expiration Game: {{ expiration }}</p>
                <p>This is an about page used to illustrate mapping a view to a router with Vue Router.</p>
            </div>
        </div>
    </div>
</template>


<script lang="js">
    import axios from 'axios';

    export default {
        mounted() {
            var id = localStorage.getItem("gameid");
            var game = this.$store.getters.getCurrentGame(id);
            this.id = game.id;
            this.createdBy = game.createdBy;
            this.title = game.title;
            this.description = game.description;
            this.roundTime = game.roundTime;
            this.expiration = game.expiration;
        },
        data() {
            return {
                loading: false,
                post: null
            };
        },
        created() {
            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData() {
                this.post = null;
                this.loading = true;

                axios.get('cards')
                    .then(response => {
                        console.log("El listado de cards es: " + response.data)
                        this.post = response.data;
                        this.loading = false;
                        return;
                    }).catch(error => console.log(error))
            }
        },
    };
</script>