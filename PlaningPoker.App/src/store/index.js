import { createStore } from 'vuex'

export default createStore({
    state: {
        //id: '',
        //createdBy: 'CreatedByStore',
        //title: 'TitleStore',
        //description: 'DescriptionStore',
        //roundTime: 0,
        //expiration: 0,
        games: []
    },
    mutations: {
        UPDATE_GAMES(state, payload) {
            state.games = payload
        }
    },
    actions: {
        addToGames(context, payload) {
            const games = context.state.games
            games.push(payload)
            context.commit('UPDATE_GAMES', games)
        }
    },
    getters: {
        fullGame: function (state) {
            const gameStored = {
                createdBy: state.createdBy,
                title: state.title,
                description: state.description,
                roundTime: state.roundTime,
                expiration: state.expiration
            };
            return gameStored;
        }
    }
})