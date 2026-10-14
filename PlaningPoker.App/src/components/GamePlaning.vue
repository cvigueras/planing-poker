<template>
    <div v-if="!mobile" class="room">
        <HeaderPlaning @leave="askToLeave" />

        <LoadingPlaning v-if="loading" />

        <div v-else class="roomBody">
            <div class="mainCol">
                <section class="panelCard gameInfo">
                    <div class="gameText">
                        <span class="gameAdmin">Admin: {{ adminName }}</span>
                        <h1>{{ title }}</h1>
                        <p>{{ description }}</p>
                    </div>
                    <div class="shareBox">
                        <div class="shareText">
                            <span class="shareLabel">Share this link with your team</span>
                            <span class="shareId">{{ shareLink }}</span>
                        </div>
                        <button type="button" class="btnCopy" @click="copyUrl">
                            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><rect x="9" y="9" width="12" height="12" rx="2" /><path d="M5 15V5a2 2 0 0 1 2-2h8" /></svg>
                            {{ copied ? 'Copied' : 'Copy link' }}
                        </button>
                    </div>
                </section>

                <CardList :id="id" />
            </div>

            <PlayerList />
        </div>

        <ShowMessageModal v-if="confirmLeaveOpen" modaltype="leave" :busy="leaving" @confirm="leaveGame" @close-modal="cancelLeave" />
    </div>

    <div v-else class="mRoom">
        <LoadingPlaning v-if="loading" />

        <template v-else>
            <header class="mHead">
                <div class="mHeadTop">
                    <svg width="28" height="28" viewBox="0 0 36 36" fill="none" aria-hidden="true" class="mLogo">
                        <rect x="3" y="7" width="20" height="26" rx="4" fill="#0B5A43" stroke="#FFFFFF" stroke-width="2" transform="rotate(-10 13 20)" />
                        <rect x="12" y="3" width="20" height="26" rx="4" fill="#FFFFFF" />
                        <circle cx="22" cy="16" r="4" fill="#027A4F" />
                    </svg>
                    <div class="mTitle">
                        <h1>{{ title }}</h1>
                        <span>Admin: {{ adminName }}</span>
                    </div>
                    <button type="button" class="mInvite" aria-label="Invite players" @click="openSheet">
                        <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2" /><circle cx="9" cy="7" r="4" /><path d="M19 8v6M22 11h-6" /></svg>
                    </button>
                    <button type="button" class="mInvite" aria-label="Leave game" @click="askToLeave">
                        <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4" /><path d="M16 17l5-5-5-5" /><path d="M21 12H9" /></svg>
                    </button>
                </div>
                <div class="mProgress">
                    <span>{{ votedCount }} of {{ totalPlayers }} voted</span>
                    <div class="mBar"><div class="mBarFill" :style="{ width: progress + '%' }"></div></div>
                </div>
            </header>

            <main class="mMain">
                <p v-if="description" class="mDesc">{{ description }}</p>
                <PlayerList mobile />
            </main>

            <footer class="mDock">
                <GameActions compact />
                <CardList mobile :id="id" />
            </footer>

            <div v-if="sheetOpen" class="sheetLayer">
                <button type="button" class="sheetBackdrop" aria-label="Close" @click="closeSheet"></button>
                <div class="sheet" role="dialog" aria-modal="true" aria-labelledby="invite-title">
                    <span class="sheetHandle"></span>
                    <div class="sheetText">
                        <h2 id="invite-title">Invite your team</h2>
                        <p>Share this link. Players only need to enter their name to join.</p>
                    </div>
                    <div class="sheetId">{{ shareLink }}</div>
                    <button type="button" class="btnSheetCopy" @click="copyUrl">{{ copied ? 'Copied' : 'Copy link' }}</button>
                    <button type="button" class="btnSheetDone" @click="closeSheet">Done</button>
                </div>
            </div>
        </template>

        <ShowMessageModal v-if="confirmLeaveOpen" modaltype="leave" :busy="leaving" @confirm="leaveGame" @close-modal="cancelLeave" />
    </div>

    <div class="toasts" aria-live="polite">
        <div v-for="toast in toasts" :key="toast.id" class="toast" role="status">
            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><circle cx="12" cy="12" r="10" /><path d="M12 16v-4M12 8h.01" /></svg>
            <span>{{ toast.text }}</span>
        </div>
    </div>
</template>


<script lang="js">
    import axios from 'axios';
    import CardList from './CardList.vue'
    import PlayerList from './PlayerList.vue'
    import GameActions from './GameActions.vue';
    import LoadingPlaning from './LoadingPlaning.vue';
    import ShowMessageModal from './ShowMessageModal.vue';
    import HeaderPlaning from './HeaderPlaning.vue';
    import signalr from '../signalr/signalR.js';
    import { buildGameLink } from '../utils/gameLink';

    const MOBILE_QUERY = '(max-width: 700px)';
    const TOAST_DURATION = 4000;
    let toastCounter = 0;

    export default {
        components: {
            CardList, HeaderPlaning, PlayerList, GameActions, LoadingPlaning, ShowMessageModal
        },
        data() {
            return {
                loading: false,
                copied: false,
                mobile: false,
                sheetOpen: false,
                confirmLeaveOpen: false,
                leaving: false,
                toasts: [],
                id: '',
                createdBy: '',
                title: '',
                description: ''
            };
        },
        computed: {
            adminName() {
                const admin = this.$store.state.users.find(user => user.admin == true);
                return admin ? admin.name : this.createdBy;
            },
            shareLink() {
                return this.id ? buildGameLink(this.id) : '';
            },
            votedCount() {
                return this.$store.getters.votedCount;
            },
            totalPlayers() {
                return this.$store.state.users.length;
            },
            progress() {
                return this.totalPlayers ? Math.round((this.votedCount / this.totalPlayers) * 100) : 0;
            }
        },
        created() {
            this.watchViewport();
            // After a refresh the votes stay shown if they were shown before.
            this.$store.commit('SET_REVEALED', sessionStorage.getItem('revealed') === 'true');
            this.$store.commit('SET_SELECTED_CARD', null);
            this.getGame();
            this.onDisconnect();
        },
        beforeUnmount() {
            this.$signalr.off('UserDisconnected', this.onUserDisconnected);
            this.$signalr.off('OnUserLeft', this.onUserLeft);
            this.viewport.removeEventListener('change', this.onViewportChange);
        },
        methods: {
            watchViewport() {
                this.viewport = window.matchMedia(MOBILE_QUERY);
                this.mobile = this.viewport.matches;
                this.viewport.addEventListener('change', this.onViewportChange);
            },
            onViewportChange(event) {
                this.mobile = event.matches;
                this.sheetOpen = false;
            },
            getGame() {
                this.loading = true;
                let gameid = sessionStorage.getItem("gameid");
                this.fetchGame(gameid);
                this.joinGroup();
            },
            joinGroup() {
                let gameId = sessionStorage.getItem("gameid");
                let userName = sessionStorage.getItem("username");
                signalr.start()
                    .then(() => this.$signalr.invoke('JoinGroup', gameId, userName))
                    .catch(function (err) { console.error(err) });
            },
            buildGame(game) {
                this.id = game.id;
                this.createdBy = game.createdBy;
                this.title = game.title;
                this.description = game.description;
                sessionStorage.setItem('users', JSON.stringify(game.users));
                setTimeout(() => {
                    this.loading = false;
                }, 1000);
            },
            fetchGame(gameId) {
                axios.get('game/' + gameId)
                    .then(response => {
                        // The server keeps the votes of the current round: whoever has one has already voted.
                        const users = response.data.users.map(user => ({ ...user, hasVoted: !!user.value }));
                        this.$store.commit('UPDATE_USERS', users);
                        const me = users.find(user => user.name === sessionStorage.getItem('username'));
                        this.$store.commit('SET_SELECTED_CARD', me && me.value ? me.value : null);
                        this.buildGame(response.data);
                    }).catch(error => console.log(error))
            },
            copyUrl() {
                navigator.clipboard.writeText(this.shareLink).then(() => {
                    this.copied = true;
                    setTimeout(() => { this.copied = false; }, 2000);
                }).catch(error => console.log(error));
            },
            askToLeave() {
                this.sheetOpen = false;
                this.confirmLeaveOpen = true;
            },
            cancelLeave() {
                this.confirmLeaveOpen = false;
            },
            async leaveGame() {
                this.leaving = true;
                const gameId = sessionStorage.getItem('gameid');
                try {
                    // The server removes the player from the game and from its SignalR group.
                    await this.$signalr.invoke('LeaveGame', gameId);
                } catch (err) {
                    console.error(err);
                }
                try {
                    // Stops the connection on purpose, so it is not reconnected automatically.
                    await signalr.stop();
                } catch (err) {
                    console.error(err);
                }
                ['users', 'username', 'gameid', 'revealed'].forEach(key => sessionStorage.removeItem(key));
                this.$store.commit('RESET_GAME');
                this.$router.push('/');
            },
            openSheet() {
                this.copied = false;
                this.sheetOpen = true;
            },
            closeSheet() {
                this.sheetOpen = false;
            },
            onDisconnect() {
                this.$signalr.on('UserDisconnected', this.onUserDisconnected);
                this.$signalr.on('OnUserLeft', this.onUserLeft);
            },
            onUserDisconnected(userName) {
                console.log(userName + " has been disconnected");
            },
            onUserLeft(userName) {
                this.showToast(userName + ' has left the game');
            },
            showToast(text) {
                const toast = { id: ++toastCounter, text };
                this.toasts.push(toast);
                setTimeout(() => {
                    this.toasts = this.toasts.filter(item => item.id !== toast.id);
                }, TOAST_DURATION);
            }
        },
    }
</script>

<style scoped>
    @import "../css/game.css";
</style>
