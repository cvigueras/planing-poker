<template>
    <div class="modal-overlay">
        <div class="modal">
            <div v-if="modaltype === 'disconnected'">
                <img class="check" src="../assets/disconnected.png" alt="" />
                <h6>Disconnected!</h6>
                <p>Your have been disconnected from match!!</p>
                <button @click="goHome" class="btnModalOk">Go Home</button>
            </div>            
            <div v-if="modaltype === 'info'">
                <img class="check" src="../assets/error.png" alt="" />
                <h6>An error has ocurred!</h6>
                <p>Try again later!!</p>
                <button @click="$emit('close-modal')" class="btnModalOk">Close</button>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        props: {
            modaltype: String
        },
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