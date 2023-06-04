<template>
    <div class="post">
        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>

        <div v-if="cards" class="content">
            <label for="urlShare">Share the game</label>
            <input type="text" id="urlShare" name="urlValue" :value="urlValue">
            <button @click="copyUrl">Copy url</button>

            <table id="customers">
                <tr>
                    <th>Scrum master</th>
                    <th>Title</th>
                    <th>Description</th>
                    <th>RoundTime</th>
                    <th>Expiration</th>
                </tr>
                <tr>
                    <td>{{ createdBy }}</td>
                    <td>{{ title }}</td>
                    <td>{{ description }}</td>
                    <td>{{ roundTime }}</td>
                    <td>{{ expiration }}</td>
                </tr>
            </table>
            <div class="cards">
                <div v-for="card in cards" v-bind:key="card.value" :class="getCardClass(card)">
                    {{ card.value }}
                </div>
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
        },
        data() {
            return {
                loading: false,
                cards: this.cards
            };
        },
        created() {
            this.fetchData();
        },
        computed: {
            urlValue() {
                return this.id;
            }
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData() {
                this.cards = null;
                this.loading = true;

                axios.get('cards')
                    .then(response => {
                        this.cards = response.data;
                        this.loading = false;
                        return;
                    }).catch(error => console.log(error))
            },
            copyUrl() {
                var copyText = document.getElementById("urlShare");
                copyText.select();
                copyText.setSelectionRange(0, 99999);
                navigator.clipboard.writeText(copyText.value);
            },
            getCardClass(card) {
                switch (card.value) {
                    case "?":
                        return "card interrogant";
                    case "coffee":
                        return "card coffee";
                    case "0":
                        return "card cero";
                    case "0,5":
                        return "card cerocinco"
                    case "1":
                        return "card uno";
                    case "2":
                        return "card dos";
                    case "3":
                        return "card tres";
                    case "5":
                        return "card cinco";
                    case "8":
                        return "card ocho";
                    case "13":
                        return "card trece";
                    case "20":
                        return "card veinte";
                    case "40":
                        return "card cuarenta";
                    case "100":
                        return "card cien";
                    default:
                        return "card interrogant";
                }
            }
        },
    }
</script>

<style scoped>

    .interrogant {
        background-color: #FF5733;
    }

    .coffee {
        background-color: #CEFF33;
    }

    .cero {
        background-color: #A233FF;
    }

    .cerocinco {
        background-color: #FF337D;
    }

    .uno {
        background-color: #D4FF33;
    }

    .dos {
        background-color: #E0FF33;
    }

    .tres {
        background-color: #33FFB5;
    }

    .cinco {
        background-color: #FFB233;
    }

    .ocho {
        background-color: #33E3FF;
    }

    .trece {
        background-color: #3361FF;
    }

    .veinte {
        background-color: #FC33FF;
    }

    .cuarenta {
        background-color: #4FFF33;
    }

    .cien {
        background-color: #FFFF33;
    }

    .cards {
        position: relative;
        float: left;
        width: 100%;
    }

    .card {
        position: relative;
        float: left;
        border: 1px solid black;
        width: 65px;
        height: 100px;
        margin: 2% 0 0 4%;
        line-height: 100px;
        font-weight: bold;
        font-size: 21px;
        color: white;
        border-style: dotted;
    }

    .infoGame {
        position: relative;
        float: left;
        width: 80%;
        border: 1px solid #04AA6D;
        margin-left: 10%;
    }

    #customers {
        font-family: Arial, Helvetica, sans-serif;
        border-collapse: collapse;
        width: 100%;
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
        }
</style>