<template>
    <div class="head">
        Header
    </div>
    <div class="post">
        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>

        <div v-if="cards" class="content">
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
                        <input type="button" @click="copyUrl" value="Copy url">
                    </td>
                </tr>
            </table>
            <div v-for="card in cards" v-bind:key="card.value" class="grid-container">
                <div class="grid-item">
                    <div></div>
                    <div></div>
                    <div></div>
                    <div></div>
                    <div v-if="card.value == 'coffee'" :class="getCardClass(card)">☕︎</div>
                    <div v-else :class="getCardClass(card)">{{ card.value }}</div>
                    <div></div>
                    <div></div>
                </div>
            </div>
            <div class="content">
                <table id="players">
                    <tr>
                        <th>Player</th>
                        <th>Vote</th>
                    </tr>
                    <tr>
                        <td>{{ createdBy }}</td>
                        <td>---</td>
                    </tr>
                </table>
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
                        return "interrogant";
                    case "coffee":
                        return "coffee";
                    case "0":
                        return "cero";
                    case "0,5":
                        return "cerocinco"
                    default:
                        return "cardNumber";
                }
            }
        },
    }
</script>

<style scoped>

    .head {
        position: relative;
        float: left;
        width: 100%;
        background-color: #F1EDED;
        border-bottom: 1px solid gray;
        height: 40px;
        line-height: 40px;
        margin-bottom: 20px;
    }

    .grid-container {
        position: relative;
        float: left;
        height: auto;
        padding: 20px;
    }

    .grid-item {
        display: grid;
        grid-template-columns: 1fr 1fr 1fr;
        grid-template-rows: 1fr 2fr 1fr;
        grid-gap: 5px;
        background-color: white;
        height: 120px;
        line-height: 120px;
        position: relative;
        float: left;
        width: 80px;
        color: black;
        font-size: 2.9em;
        padding: 10px;
        border-radius: 5%;
        box-shadow: 0px 5px 15px grey;
        margin-left: 20px;
    }

    .heart {
        text-align: center;
        align-self: center;
        font-size: 2em;
        color: red;
    }

    /*Cards CSS*/
    .interrogant {
        color: #FF5733;
    }

    .coffee {
        color: #FF337D;
    }

    .cero {
        color: #A233FF;
    }

    .cerocinco {
        color: #FF337D;
    }


    .cardNumber {
        color: #3361FF;
    }


    .content {
        position: relative;
        float: left;
        width: 100%;
    }


    .infoGame {
        position: relative;
        float: left;
        width: 80%;
        border: 1px solid #04AA6D;
        margin-left: 10%;
    }

    /*Customers CSS*/
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
        text-align: center;
    }

    /*Player css*/
    #players {
        font-family: Arial, Helvetica, sans-serif;
        border-collapse: collapse;
        width: 80%;
        margin-left: 10%;
        margin-top: 100px;
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