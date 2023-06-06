<template>
    <div class="cardlist">
        <div v-for="card in firstcards" v-bind:key="card.value" class="grid-container" @click="getCard($event)" v-bind:id="card.id">
            <div class="grid-item">
                <div v-if="card.value == 'coffee'" :class="getCardClass(card)">☕︎</div>
                <div v-else :class="getCardClass(card)">{{ card.value }}</div>
            </div>
        </div>
    </div>
    <div class="cardlist">
        <div v-for="card in secondcards" v-bind:key="card.value" class="grid-container" @click="getCard($event)" v-bind:id="card.id">
            <div class="grid-item">
                <div v-if="card.value == 'coffee'" :class="getCardClass(card)">☕︎</div>
                <div v-else :class="getCardClass(card)">{{ card.value }}</div>
            </div>
        </div>
    </div>
    
    <!--<h4>{{ id }}</h4>-->
</template>

<script lang="js">
    import axios from 'axios';
    export default {
        props: {
            id: String
        },
        data() {
            return {
                firstcards: this.firstcards,
                secondcards: this.secondcards
            };
        },
        created() {
            this.fetchData();
        },
        methods: {
            fetchData() {
                axios.get('cards')
                    .then(response => {
                        this.firstcards = response.data.slice(0, 7);
                        this.secondcards = response.data.slice(7, 13);
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

    .cardlist {
        position: relative;
        float: left;
        width: 80%;
        text-align: center;
        margin-left: 10%;
        margin-right: 10%;
        background-color: aliceblue;
    }

    .selected {
        box-shadow: 0 0px 3px 6px rgba(4, 170, 109, 0.6) !important;
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
        padding: 14px;
    }

    .grid-item {
        background-color: white;
        height: 80px;
        line-height: 80px;
        position: relative;
        float: left;
        width: 40px;
        color: black;
        font-size: 1.5em;
        padding: 10px;
        border-radius: 5%;
        box-shadow: 0px 5px 15px grey;
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