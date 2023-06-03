<script lang="ts" setup></script>

<template>
  <router-view v-slot="{ Component }">
      <v-fade-transition mode="out-in">
        <component :is="Component"
      /></v-fade-transition>
    </router-view>
</template>

<style></style>
