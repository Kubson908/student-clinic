<script setup>
    import NavBar from "./components/NavBar.vue"
    import FooterBar from "./components/FooterBar.vue"
    import AppointmentList from "./components/AppointmentList.vue"

</script>

<template>
    <v-app class="v-theme--light">
        <NavBar />
        <AppointmentList />
        <FooterBar />
    </v-app>
</template>



<style>
:root {
    
}

#app {
  font-family: Souvenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
</style>
