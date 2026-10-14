<template>
    <header class="head">
        <div class="brand">
            <svg width="30" height="30" viewBox="0 0 36 36" fill="none" aria-hidden="true">
                <rect x="3" y="7" width="20" height="26" rx="4" fill="#0B5A43" stroke="#FFFFFF" stroke-width="2" transform="rotate(-10 13 20)" />
                <rect x="12" y="3" width="20" height="26" rx="4" fill="#FFFFFF" />
                <circle cx="22" cy="16" r="4" fill="#027A4F" />
            </svg>
            <span class="brandName">Planing Poker</span>
        </div>
        <div v-if="userName" class="user">
            <span class="avatar">{{ userName.charAt(0).toUpperCase() }}</span>
            <span>{{ userName }}</span>
            <span v-if="isAdmin" class="badge">Admin</span>
            <button type="button" class="btnLeave" @click="$emit('leave')">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4" /><path d="M16 17l5-5-5-5" /><path d="M21 12H9" /></svg>
                Leave game
            </button>
        </div>
    </header>
</template>

<script lang="js">
    export default {
        emits: ['leave'],
        computed: {
            userName() {
                return sessionStorage.getItem("username");
            },
            isAdmin() {
                const gameId = sessionStorage.getItem("gameid");
                return this.$store.getters.userIsAdmin(gameId, this.userName);
            }
        }
    }
</script>

<style scoped>
    @import "../css/header.css";
</style>
