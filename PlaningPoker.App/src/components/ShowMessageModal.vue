<template>
    <div class="modal-overlay">
        <div class="modal">
            <div v-if="modaltype === 'disconnected'">
                <img class="check" src="../assets/disconnected.png" alt="" />
                <h6>Disconnected!</h6>
                <p>Your have been disconnected from match!!</p>
                <button @click="goHome" class="btnModalOk">Go Home</button>
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