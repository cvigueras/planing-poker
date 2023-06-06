<template>
    <div class="post">
        <div v-if="loading" class="loading">
            Loading... CardList
        </div>
        <div v-for="card in cards" v-bind:key="card.value" class="grid-container" @click="getCard($event)" v-bind:id="card.id">
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
    </div>
    <h4>{{ id }}</h4>
</template>

<script lang="js">
    import axios from 'axios';
    export default {
        props: {
            id: String
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
            getCard(event) {
                if (event) {
                    event.preventDefault()
                }

                const allElements = document.querySelectorAll('*');
                allElements.forEach((element) => {
                    element.classList.remove('selected');
                });

                var element = document.getElementById(event.currentTarget.id).firstElementChild;
                element.classList.add("selected");
                console.log(event.currentTarget.id);
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

    .selected {
        box-shadow: 0 0px 3px 11px rgba(4, 170, 109, 0.6) !important;
        position: relative !important;
        z-index: 9999;
        pointer-events: auto;
        transition: all 0.5s ease;
        transform: scale(1.2, 1.2);
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
</style>