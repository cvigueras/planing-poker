<template>
    <div class="post">
        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>

        <div v-if="post" class="content">
            <p>Hola, soy Home</p>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';
    export default {

        data() {
            return {
                loading: false,
                post: null
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData() {
                const game = {
                    createdBy: "Carlos",
                    title: "Release1",
                    description: "Vote for Release1",
                    roundTime: 60,
                    expiration: 60
                };

                axios.post('game', game)
                    .then(response => {
                        this.getGame(response.data)
                        this.post = response;
                        this.loading = false;
                        return;
                    }).catch(error => console.log(error))
            },
            getGame(Id) {
                console.log("GameId value: " + Id);
                axios.get('game/' + Id)
                    .then(response => (this.info = console.log(response.data)))
                    .catch(error => console.log(error))
            }
        },

    }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    h3 {
        margin: 40px 0 0;
    }

    ul {
        list-style-type: none;
        padding: 0;
    }

    li {
        display: inline-block;
        margin: 0 10px;
    }

    a {
        color: #42b983;
    }
</style>