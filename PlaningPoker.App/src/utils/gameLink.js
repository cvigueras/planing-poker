// A game is shared as a link to the start screen: <origin><base>?game=<gameId>.
// A query string is used (instead of a path) so it works on any static hosting without server rewrites.

export function buildGameLink(gameId) {
    return `${window.location.origin}${process.env.BASE_URL}?game=${encodeURIComponent(gameId)}`
}

// Accepts either a game link or a bare game id and returns the game id.
export function parseGameId(text) {
    const value = (text || '').trim()
    try {
        const id = new URL(value).searchParams.get('game')
        if (id) {
            return id.trim()
        }
    } catch (error) {
        // Not a link: it is a bare game id.
    }
    return value
}
