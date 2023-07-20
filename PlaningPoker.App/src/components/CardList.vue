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
    import { getCurrentInstance } from "vue";

    export default {
        props: {
            id: String
        },
        setup() {
            const instance = getCurrentInstance();
            window.signalR = instance.appContext.config.globalProperties.$signalr;
        },
        data() {
            return {
                firstcards: this.firstcards,
                secondcards: this.secondcards
            };
        },
        created() {
            this.fetchData();
            this.subscribeEvents();
        },
        methods: {
            subscribeEvents() {
                this.$signalr.on('OnNotifyUserHasVoted', (user, vote) => {
                    console.log("El usuario: " + user + "ha votado: " + vote);
                });
            },
            fetchData() {
                axios.get('cards')
                    .then(response => {
                        this.firstcards = response.data.slice(0, 7);
                        this.secondcards = response.data.slice(7, 13);
                    }).catch(error => console.log(error))
            },
            insertVote(vote) {
                const votes = {
                    name: localStorage.getItem('username'),
                    group: localStorage.getItem('gameid'),
                    value: vote
                }
                axios.post('votes', votes)
                    .then(function () {
                        window.signalR
                            .invoke('NotifyUserHasVoted', votes.group, votes.name, votes.value)
                            .catch(function (err) { console.error(err) })
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

                let element = document.getElementById(event.currentTarget.id).firstElementChild;
                element.classList.add("selected");
                
                this.insertVote(event.currentTarget.textContent);
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
    @import "../css/cardlist.css";
</style>