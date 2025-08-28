<template>
  <div class="min-h-screen bg-base-100 flex justify-center items-start pt-20 px-4">
    <div class="bg-base-200 rounded-xl shadow-lg p-10 max-w-md w-full text-center">
      <h1 class="text-4xl font-bold mb-4">TodoApp</h1>
      <p class="text-gray-600 mb-6">
            Simple TodoApp that i made to practice ASP.NET,C# and some frontend tools.
      </p>

      <div class="flex flex-col sm:flex-row gap-15 justify-center">
        <button
          class="btn btn-primary w-full sm:w-auto"
          @click="goToTasks">
          Task List
        </button>
        <button
          class="btn btn-accent w-full sm:w-auto"
          @click="goToCreate">
          Create Task
        </button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { useRouter } from "vue-router";

export default defineComponent({
  name: "Home",
  setup() {
    const router = useRouter();

    const goToTasks = () => router.push({ name: "Tasks" });
    const goToCreate = () => router.push({ name: "Create" });

    return { goToTasks, goToCreate };
  },
});
</script>

<style scoped>
.btn:hover {
  transform: translateY(-2px);
  transition: transform 0.2s ease;
}
</style>