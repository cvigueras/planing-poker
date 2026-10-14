<template>
    <div class="actions" :class="{ compact }">
        <button v-if="!(compact && revealed)" type="button" class="btnShowVotes" @click="showAllVotes()">Show votes</button>
        <button type="button" class="btnResetMatch" :class="{ btnPrimary: compact && revealed }" @click="resetMatch()">Reset match</button>
    </div>
</template>

<script lang="js">
    export default {
        props: {
            compact: Boolean
        },
        computed: {
            revealed() {
                return this.$store.state.revealed;
            }
        },
        methods: {
            showAllVotes() {
                let gameId = sessionStorage.getItem("gameid");
                this.$signalr
                    .invoke('ReceiveAllVotes', gameId)
                    .catch(function (err) { console.error(err) })
                console.log("Showing all votes");
            },
            resetMatch() {
                let users = JSON.parse(sessionStorage.getItem("users"))
                    .map(user => ({ ...user, hasVoted: false }));
                this.$store.commit('UPDATE_USERS', users);
                this.$store.commit('SET_SELECTED_CARD', null);
                this.$store.commit('SET_REVEALED', false);
            }
        }
    }
</script>

<style scoped>
    .actions {
        display: flex;
        flex-wrap: wrap;
        gap: 10px;
    }

    .compact {
        flex-wrap: nowrap;
        padding: 0 16px;
    }

    .btnShowVotes,
    .btnResetMatch {
        height: 52px;
        border-radius: 12px;
        font: 700 16px 'Figtree', system-ui, sans-serif;
        cursor: pointer;
    }

    .btnShowVotes {
        flex: 1 1 160px;
        border: 0;
        background: #027A4F;
        color: #FFFFFF;
    }

    .btnShowVotes:hover {
        filter: brightness(1.1);
    }

    .btnResetMatch {
        flex: 1 1 140px;
        border: 1.5px solid #B8C4BD;
        background: #FFFFFF;
        color: #10201B;
    }

    .btnResetMatch:hover {
        background: #F0F4F1;
    }

    .compact .btnShowVotes {
        flex: 1.3 1 0;
    }

    .compact .btnResetMatch {
        flex: 1 1 0;
    }

    .btnResetMatch.btnPrimary {
        border: 0;
        background: #027A4F;
        color: #FFFFFF;
    }

    .btnShowVotes:focus-visible,
    .btnResetMatch:focus-visible {
        outline: 3px solid rgba(36, 87, 214, .45);
        outline-offset: 2px;
    }
</style>
