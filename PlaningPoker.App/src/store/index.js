import { createStore } from 'vuex'

export default createStore({
    state: {
        games: []
    },
    mutations: {
        UPDATE_GAMES(state, payload) {
            state.games = payload
        }
    },
    actions: {
        addToGames(context, payload) {
            //games.push(payload);
            var game = context.state.games.concat(JSON.parse(payload));
            context.commit('UPDATE_GAMES', game)
        }
    },
    getters: {
        getCurrentGame: (state) => (id) => {
            return state.games.find(game => game.id === id);
        }
    },
})