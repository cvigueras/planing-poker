<template>
    <div class="head">
        <div class="logo"><img style=" width: 100px;" alt="Vue logo" src="../assets/logo.png"></div>
        <div class="headText">Planing Poker</div>
    </div>
</template>

<script lang="js">
        export default {}
</script>

<style scoped>

    .logo {
        position: relative;
        float: left;
        width: 120px;
        height: 100%;
    }

    .headText {
        font-family: auto;
        font-size: 47px;
        color: #04AA6D;
        -webkit-text-stroke: 1px #3377ff;
    }

    .head {
        position: fixed;
        width: 100%;
        border-bottom: 1px solid gray;
        height: 100px;
        line-height: 100px;
        background-color: #FAF5F5;
        margin: -8px 0 20px -8px;
        padding: 0 8px 0 8px;
        z-index: 999999999;
    }
</style>