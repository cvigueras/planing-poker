<template>
    <div class="modal-overlay">
        <div class="modal" role="dialog" aria-modal="true" aria-labelledby="modal-title">
            <div class="modalIcon">
                <svg v-if="modaltype === 'disconnected'" width="34" height="34" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2" /><circle cx="9" cy="7" r="4" /><path d="M17 8l5 5M22 8l-5 5" /></svg>
                <svg v-else width="34" height="34" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><circle cx="12" cy="12" r="10" /><path d="M12 8v5M12 16.5v.01" /></svg>
            </div>
            <div v-if="modaltype === 'disconnected'" class="modalText">
                <h2 id="modal-title">You were removed from the game</h2>
                <p>The admin removed you from this match. You can join again with the game ID, or start a new game.</p>
            </div>
            <div v-else class="modalText">
                <h2 id="modal-title">An error has occurred</h2>
                <p>Try again later.</p>
            </div>
            <button v-if="modaltype === 'disconnected'" @click="goHome" class="btnModalOk">Go to the start screen</button>
            <button v-else @click="$emit('close-modal')" class="btnModalOk">Close</button>
        </div>
    </div>
</template>

<script>
    export default {
        props: {
            modaltype: String
        },
        emits: ['close-modal'],
        methods: {
            goHome() {
                this.$router.push('/');
            },
        }
    }
</script>

<style scoped>
    @import "../css/modal.css";
</style>
