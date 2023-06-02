<template>
    <div>
        <h1>About</h1>
        <p> {{ $store.getters.fullGame }} {{ $store.state.games }}</p>
        <p>This is an about page used to illustrate mapping a view to a router with Vue Router.</p>
    </div>
</template>

<script>
    export default {
        name: "GamePlaning",
        mounted() {

        },
    };
 </script>