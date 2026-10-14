<template>
    <div class="loader" role="status" aria-live="polite">
        <div class="dealing" aria-hidden="true">
            <div class="dealCard dealCard1" :style="{ color: cardColor('3') }">3</div>
            <div class="dealCard dealCard2" :style="{ color: cardColor('5') }">5</div>
            <div class="dealCard dealCard3" :style="{ color: cardColor('8') }">8</div>
        </div>
        <span class="loaderText">{{ text }}</span>
    </div>
</template>

<script lang="js">
    import { cardColor } from '../utils/cardColors';

    export default {
        props: {
            text: {
                type: String,
                default: 'Getting the game ready…'
            }
        },
        methods: {
            cardColor
        }
    }
</script>

<style scoped>
    .loader {
        flex: 1;
        min-height: 60vh;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        gap: 28px;
        font-family: 'Figtree', system-ui, sans-serif;
        color: #4B5B54;
    }

    .dealing {
        position: relative;
        width: 150px;
        height: 110px;
    }

    .dealCard {
        position: absolute;
        top: 14px;
        width: 62px;
        height: 86px;
        box-sizing: border-box;
        border: 1.5px solid #D5DDD8;
        border-radius: 11px;
        background: #FFFFFF;
        box-shadow: 0 8px 18px rgba(16, 32, 27, .16);
        display: flex;
        align-items: center;
        justify-content: center;
        font-family: 'Bricolage Grotesque', sans-serif;
        font-weight: 800;
        font-size: 30px;
        animation: deal 1.4s ease-in-out infinite;
    }

    .dealCard1 { left: 6px; --tilt: -14deg; animation-delay: 0s; }
    .dealCard2 { left: 44px; --tilt: -2deg; animation-delay: .18s; }
    .dealCard3 { left: 82px; --tilt: 10deg; animation-delay: .36s; }

    @keyframes deal {
        0%, 100% {
            transform: translateY(0) rotate(var(--tilt));
        }
        45% {
            transform: translateY(-16px) rotate(calc(var(--tilt) * 0.4));
        }
    }

    .loaderText {
        font-size: 15px;
        font-weight: 600;
        letter-spacing: .01em;
    }

    @media (prefers-reduced-motion: reduce) {
        .dealCard {
            animation: none;
            transform: rotate(var(--tilt));
        }
    }
</style>
