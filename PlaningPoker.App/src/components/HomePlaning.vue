<template>
    <div class="login">
        <section class="hero">
            <div class="brand">
                <svg width="36" height="36" viewBox="0 0 36 36" fill="none" aria-hidden="true">
                    <rect x="3" y="7" width="20" height="26" rx="4" fill="#0B5A43" stroke="#FFFFFF" stroke-width="2" transform="rotate(-10 13 20)" />
                    <rect x="12" y="3" width="20" height="26" rx="4" fill="#FFFFFF" />
                    <circle cx="22" cy="16" r="4" fill="#027A4F" />
                </svg>
                <span class="brandName">Planing Poker</span>
            </div>

            <div class="pitch">
                <div class="fan" aria-hidden="true">
                    <div class="fanCard fanCard1">3</div>
                    <div class="fanCard fanCard2">5</div>
                    <div class="fanCard fanCard3">8</div>
                    <div class="fanCard fanCard4">
                        <svg width="44" height="44" viewBox="0 0 24 24" fill="none" stroke="#10201B" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round"><path d="M4 9h12v6a5 5 0 0 1-5 5h-2a5 5 0 0 1-5-5V9z" /><path d="M16 11h2a2.5 2.5 0 0 1 0 5h-2" /><path d="M8 3v2M12 3v2" /></svg>
                    </div>
                </div>
                <h1>Estimate together, without the noise.</h1>
                <p>Create a game, share its ID and everyone votes at the same time. Votes stay hidden until the admin reveals them. No account needed.</p>
            </div>

            <InfoHome />
        </section>

        <main class="panel">
            <form class="card" @submit.prevent="submit" novalidate>
                <template v-if="isInvite">
                    <div v-if="inviteChecking" class="intro">
                        <h2>Checking your invite…</h2>
                    </div>

                    <template v-else-if="inviteInvalid">
                        <div class="intro">
                            <h2>This link is not valid</h2>
                            <p>The game does not exist or the link is incomplete. Ask the admin for a new link.</p>
                        </div>
                        <router-link to="/" class="linkBtn">Create or join another game</router-link>
                    </template>

                    <template v-else-if="inviteGame">
                        <div class="intro">
                            <span class="inviteBy">{{ inviteGame.createdBy }} invited you to</span>
                            <h2>{{ inviteGame.title }}</h2>
                            <p>Enter your name to join the game. No account needed.</p>
                        </div>

                        <div class="fields">
                            <div class="field">
                                <label for="invite-username">Your name</label>
                                <input id="invite-username" v-model.trim="username" type="text" autocomplete="nickname" maxlength="19" placeholder="e.g. Carlos" class="inputHome" required>
                                <span class="hint">3 to 19 characters. Others will see this name.</span>
                            </div>
                        </div>

                        <div v-if="formError" class="errorBox" role="alert">
                            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><circle cx="12" cy="12" r="10" /><path d="M12 8v5M12 16.5v.01" /></svg>
                            <span>{{ formError }}</span>
                        </div>

                        <button type="submit" class="btnJoin" :disabled="loading">{{ loading ? 'Joining…' : 'Join game' }}</button>
                        <router-link to="/" class="linkBtn">Create your own game instead</router-link>
                    </template>
                </template>

                <template v-else>
                <div class="tabs" role="group" aria-label="Choose what you want to do">
                    <button type="button" :class="['tab', { active: isCreate }]" :aria-pressed="isCreate" @click="setMode('create')">
                        <span class="dot dotCreate"></span>Create a game
                    </button>
                    <button type="button" :class="['tab', { active: !isCreate }]" :aria-pressed="!isCreate" @click="setMode('join')">
                        <span class="dot dotJoin"></span>Join a game
                    </button>
                </div>

                <div class="intro">
                    <h2 v-if="isCreate">Start a new game</h2>
                    <h2 v-else>Join your team</h2>
                    <p v-if="isCreate">You will be the admin. After creating it you get a link to share with your team.</p>
                    <p v-else>Paste the link the admin shared with you, or its game ID.</p>
                </div>

                <div class="fields">
                    <div class="field">
                        <label for="username">Your name</label>
                        <input id="username" v-model.trim="username" type="text" autocomplete="nickname" maxlength="19" placeholder="e.g. Carlos" class="inputHome" required>
                        <span class="hint">3 to 19 characters. Others will see this name.</span>
                    </div>

                    <template v-if="isCreate">
                        <div class="field">
                            <label for="gamename">Game name</label>
                            <input id="gamename" v-model.trim="gamename" type="text" placeholder="e.g. Sprint 42 planning" class="inputHome" required>
                        </div>
                        <div class="field">
                            <label for="description">Description</label>
                            <input id="description" v-model.trim="description" type="text" placeholder="What are you estimating?" class="inputHome" required>
                        </div>
                    </template>

                    <div v-else class="field">
                        <label for="gameid">Game link or ID</label>
                        <input id="gameid" v-model.trim="gameId" type="text" autocomplete="off" spellcheck="false" placeholder="Paste the link here" class="inputHome inputMono"
                               :class="{ inputInvalid: joinError }" :aria-invalid="joinError ? 'true' : 'false'" aria-describedby="gameid-error" required>
                        <div v-if="joinError" id="gameid-error" class="errorBox" role="alert">
                            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><circle cx="12" cy="12" r="10" /><path d="M12 8v5M12 16.5v.01" /></svg>
                            <span>{{ joinError }}</span>
                        </div>
                    </div>
                </div>

                <div v-if="formError" class="errorBox" role="alert">
                    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><circle cx="12" cy="12" r="10" /><path d="M12 8v5M12 16.5v.01" /></svg>
                    <span>{{ formError }}</span>
                </div>

                <button type="submit" :class="isCreate ? 'btnCreate' : 'btnJoin'" :disabled="loading">
                    <template v-if="loading">{{ isCreate ? 'Creating…' : 'Joining…' }}</template>
                    <template v-else>{{ isCreate ? 'Create game' : 'Join game' }}</template>
                </button>
                </template>
            </form>
        </main>
    </div>
</template>

<script>
    import axios from 'axios';
    import InfoHome from './InfoHome.vue';
    import gameFactory from '../factories/game'
    import { parseGameId } from '../utils/gameLink'

    export default {
        components: {
            InfoHome
        },
        data() {
            return {
                mode: 'create',
                loading: false,
                username: '',
                gamename: '',
                description: '',
                gameId: '',
                formError: '',
                joinError: '',
                inviteGame: null,
                inviteChecking: false,
                inviteInvalid: false,
            };
        },
        computed: {
            isCreate() {
                return this.mode === 'create';
            },
            inviteId() {
                const game = this.$route.query.game;
                return typeof game === 'string' ? game.trim() : '';
            },
            isInvite() {
                return this.inviteId !== '';
            }
        },
        watch: {
            inviteId() {
                this.loadInvite();
            }
        },
        created() {
            // Before the first render, so the invite is already "checking" when the page is painted.
            this.loadInvite();
        },
        mounted() {
            sessionStorage.clear();
        },
        methods: {
            loadInvite() {
                this.inviteGame = null;
                this.inviteInvalid = false;
                this.formError = '';
                if (!this.isInvite) {
                    return;
                }
                this.inviteChecking = true;
                axios.get('game/' + encodeURIComponent(this.inviteId))
                    .then(response => {
                        this.inviteGame = { title: response.data.title, createdBy: response.data.createdBy };
                    }).catch(error => {
                        console.log(error);
                        this.inviteInvalid = true;
                    }).finally(() => {
                        this.inviteChecking = false;
                    })
            },
            setMode(mode) {
                this.mode = mode;
                this.formError = '';
                this.joinError = '';
            },
            submit() {
                this.formError = '';
                this.joinError = '';
                if (this.username.length < 3 || this.username.length > 19) {
                    this.formError = 'Your name must have between 3 and 19 characters.';
                    return;
                }
                if (this.isInvite) {
                    this.joinGame(this.inviteId);
                } else if (this.isCreate) {
                    this.createGame();
                } else {
                    this.joinFromForm();
                }
            },
            createGame() {
                if (!this.gamename || !this.description) {
                    this.formError = 'Please fill in the game name and the description.';
                    return;
                }
                this.loading = true;
                let gameCreated = gameFactory.create(this.username, this.gamename, this.description);
                this.fetchData(gameCreated);
            },
            fetchData(game) {
                let user = this.buildUser(this.username, this.gameId);
                axios.post('game', game)
                    .then(response => {
                        this.persistLocalData(response.data, user);
                        this.loading = false;
                    }).catch(error => {
                        console.log(error);
                        this.formError = 'We could not create the game. Try again later.';
                        this.loading = false;
                    })
            },
            joinFromForm() {
                const gameId = parseGameId(this.gameId);
                if (!gameId) {
                    this.joinError = 'Please enter the game link or ID.';
                    return;
                }
                this.joinGame(gameId);
            },
            joinGame(gameId) {
                this.loading = true;
                let user = this.buildUser(this.username, gameId);
                axios.put('game/' + encodeURIComponent(gameId), user)
                    .then(response => {
                        this.persistLocalData(response.data, user);
                        this.loading = false;
                    }).catch(error => {
                        console.log(error);
                        const message = 'We could not join that game. Check the link with the admin and try again.';
                        if (this.isInvite) {
                            this.formError = message;
                        } else {
                            this.joinError = message;
                        }
                        this.loading = false;
                    })
            },
            buildUser(userName, gameId) {
                return {
                    name: userName,
                    gameId: gameId
                }
            },
            persistLocalData(game, user) {
                this.$store.state.users = game.users;
                this.$store.dispatch('addToGames', JSON.stringify(game))
                sessionStorage.setItem("users", JSON.stringify(game.users));
                sessionStorage.setItem("gameid", game.id);
                sessionStorage.setItem("username", user.name);
                this.$router.push('/planing');
            }
        },
    }
</script>

<style scoped>
    @import "../css/home.css";
</style>
