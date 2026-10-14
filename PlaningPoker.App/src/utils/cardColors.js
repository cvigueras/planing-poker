// One color per card, going from cool to warm as the estimate grows. All of them are dark enough
// to be read on the white card.
const CARD_COLORS = {
    '0': '#64748B',
    '0,5': '#6D5BD0',
    '1': '#2457D6',
    '2': '#0B7FAB',
    '3': '#0E8A7D',
    '5': '#027A4F',
    '8': '#5E8C16',
    '13': '#B07A00',
    '20': '#C2570C',
    '40': '#C2362F',
    '100': '#B0286B',
    '?': '#7B3FA0',
    'coffee': '#6B4A32'
}

export function cardColor(value) {
    return CARD_COLORS[value] || '#4B5B54'
}
