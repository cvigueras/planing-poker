import { createStore } from 'vuex'

export default createStore({
    state: {
        games: [],
        users: []
    },
    mutations: {
        UPDATE_GAMES(state, payload) {
            state.games = payload
        },
        UPDATE_USERS(state, payload) {
            state.users = payload
        }
    },
    actions: {
        addToGames(context, payload) {
            var game = context.state.games.concat(JSON.parse(payload));
            context.commit('UPDATE_GAMES', game)
        },
        addToUsers(context, payload) {
            var user = context.state.users.concat(JSON.parse(payload));
            context.commit('UPDATE_USERS', user)
        }
    },
    getters: {
        getCurrentGame: (state) => (id) => {
            return state.games.find(game => game.id === id);
        },
        existUser: (state) => (gameId, name) => {
            var exist = false;
            state.users.forEach(function (user) {

                if (user.name == name && user.gameId == gameId) {
                    exist = true;
                }
            });
            return exist;
        },
        userIsAdmin: (state) => (gameId, name) => {
            var isAdmin = false;
            state.users.forEach(function (user) {

                if (user.name == name && user.gameId == gameId && user.admin == true) {
                    isAdmin = true;
                }
            });
            return isAdmin;
        },
    },
})