<template>
    <section v-if="mobile" class="dockDeck">
        <div class="estimateRow">
            <span class="estimateLabel">Your estimate</span>
            <span class="estimateChip" :class="{ estimateVoted: selectedCard }">{{ estimateText }}</span>
        </div>
        <div class="railWrap">
            <div ref="rail" class="rail" @scroll="updateRail">
                <button v-for="card in cards" :key="card.value" type="button" class="railCard"
                        :class="{ selected: selectedCard === card.value }"
                        :style="{ color: cardColor(card.value) }"
                        :aria-pressed="selectedCard === card.value"
                        :aria-label="card.value === 'coffee' ? 'Coffee break' : 'Estimate ' + card.value"
                        @click="getCard(card)">
                    <SuitIcon class="corner cornerTop" />
                    <svg v-if="card.value === 'coffee'" width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M4 9h12v6a5 5 0 0 1-5 5h-2a5 5 0 0 1-5-5V9z" /><path d="M16 11h2a2.5 2.5 0 0 1 0 5h-2" /><path d="M8 3v2M12 3v2" /></svg>
                    <span v-else>{{ card.value }}</span>
                    <SuitIcon class="corner cornerBottom" />
                </button>
            </div>
            <button v-if="showLeft" type="button" class="railArrow railArrowLeft" aria-label="Show previous cards" @click="scrollRail(-1)">
                <span class="railArrowIcon">
                    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M15 5l-7 7 7 7" /></svg>
                </span>
            </button>
            <button v-if="showRight" type="button" class="railArrow railArrowRight" aria-label="Show more cards" @click="scrollRail(1)">
                <span class="railArrowIcon">
                    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M9 5l7 7-7 7" /></svg>
                </span>
            </button>
        </div>
    </section>

    <section v-else class="panelCard deck">
        <div class="deckHead">
            <h2>Pick your estimate</h2>
            <p>{{ selectedCard ? 'Your vote is in. You can change it until the votes are shown.' : 'Votes stay hidden until the votes are shown.' }}</p>
        </div>
        <div class="deckGrid">
            <button v-for="card in cards" :key="card.value" type="button" class="deckCard"
                    :class="{ selected: selectedCard === card.value }"
                    :style="{ color: cardColor(card.value) }"
                    :aria-pressed="selectedCard === card.value"
                    :aria-label="card.value === 'coffee' ? 'Coffee break' : 'Estimate ' + card.value"
                    @click="getCard(card)">
                <SuitIcon class="corner cornerTop" />
                <svg v-if="card.value === 'coffee'" class="coffeeIcon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M4 9h12v6a5 5 0 0 1-5 5h-2a5 5 0 0 1-5-5V9z" /><path d="M16 11h2a2.5 2.5 0 0 1 0 5h-2" /><path d="M8 3v2M12 3v2" /></svg>
                <span v-else>{{ card.value }}</span>
                <SuitIcon class="corner cornerBottom" />
            </button>
        </div>
    </section>
</template>

<script lang="js">
    import axios from 'axios';
    import SuitIcon from './SuitIcon.vue';
    import { cardColor } from '../utils/cardColors';

    const RAIL_STEP = 216;

    export default {
        components: {
            SuitIcon
        },
        props: {
            id: String,
            mobile: Boolean
        },
        data() {
            return {
                cards: [],
                showLeft: false,
                showRight: false
            };
        },
        computed: {
            selectedCard() {
                return this.$store.state.selectedCard;
            },
            estimateText() {
                if (!this.selectedCard) {
                    return 'Not voted yet';
                }
                return this.selectedCard === 'coffee' ? 'Coffee break' : this.selectedCard;
            }
        },
        created() {
            this.fetchData();
        },
        mounted() {
            window.addEventListener('resize', this.updateRail);
        },
        beforeUnmount() {
            window.removeEventListener('resize', this.updateRail);
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
                        this.$nextTick(this.updateRail);
                    }).catch(error => console.log(error))
            },
            updateRail() {
                const rail = this.$refs.rail;
                if (!rail) {
                    return;
                }
                this.showLeft = rail.scrollLeft > 4;
                this.showRight = rail.scrollLeft + rail.clientWidth < rail.scrollWidth - 4;
            },
            scrollRail(direction) {
                const rail = this.$refs.rail;
                if (rail) {
                    rail.scrollBy({ left: direction * RAIL_STEP, behavior: 'smooth' });
                }
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
            cardColor
        },
    }
</script>

<style scoped>
    @import "../css/cardlist.css";
</style>
