<template>
    <section class="panelCard deck">
        <div class="deckHead">
            <h2>Pick your estimate</h2>
            <p>{{ selectedCard ? 'Your vote is in. You can change it until the votes are shown.' : 'Votes stay hidden until the votes are shown.' }}</p>
        </div>
        <div class="deckGrid">
            <button v-for="card in cards" :key="card.value" type="button" class="deckCard"
                    :class="[getCardClass(card), { selected: selectedCard === card.value }]"
                    :aria-pressed="selectedCard === card.value"
                    :aria-label="card.value === 'coffee' ? 'Coffee break' : 'Estimate ' + card.value"
                    @click="getCard(card)">
                <svg v-if="card.value === 'coffee'" width="30" height="30" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M4 9h12v6a5 5 0 0 1-5 5h-2a5 5 0 0 1-5-5V9z" /><path d="M16 11h2a2.5 2.5 0 0 1 0 5h-2" /><path d="M8 3v2M12 3v2" /></svg>
                <span v-else>{{ card.value }}</span>
            </button>
        </div>
    </section>
</template>

<script lang="js">
    import axios from 'axios';

    export default {
        props: {
            id: String
        },
        data() {
            return {
                cards: []
            };
        },
        computed: {
            selectedCard() {
                return this.$store.state.selectedCard;
            }
        },
        created() {
            this.fetchData();
        },
        methods: {
            fetchData() {
                axios.get('cards')
                    .then(response => {
                        // Numbers first, then the special cards ("?" and "coffee").
                        const isSpecial = card => card.value === '?' || card.value === 'coffee';
                        const numbers = response.data.filter(card => !isSpecial(card));
                        const specials = response.data.filter(isSpecial).sort((a, b) => (a.value === '?' ? -1 : 1) - (b.value === '?' ? -1 : 1));
                        this.cards = numbers.concat(specials);
                    }).catch(error => console.log(error))
            },
            insertVote(vote) {
                const votes = {
                    name: sessionStorage.getItem('username'),
                    group: sessionStorage.getItem('gameid'),
                    value: vote
                }
                axios.post('votes', votes)
                    .then(() => {
                        this.$signalr
                            .invoke('NotifyUserHasVoted', votes.group, votes.name, votes.value)
                            .catch(function (err) { console.error(err) })
                    }).catch(error => console.log(error))
            },
            getCard(card) {
                this.$store.commit('SET_SELECTED_CARD', card.value);
                this.insertVote(card.value);
            },
            getCardClass(card) {
                switch (card.value) {
                    case "?":
                        return "interrogant";
                    case "coffee":
                        return "coffee";
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
