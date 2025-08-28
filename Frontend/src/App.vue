<script setup lang="ts">
import Navbar from "./components/Navbar.vue"
</script>

<template>
<div class="min-h-screen flex flex-col">
    <Navbar />

    <main class="flex-1 p-4">
      <router-view />
    </main>
  </div>
</template>

<style scoped>

</style>
