<template>
  <div class="flex gap-4 p-4 h-screen bg-base-100">
    
    <div class="flex-1 bg-base-200 p-4 rounded-lg h-full overflow-y-auto flex flex-col">
      <h2 class="font-bold text-xl mb-4 text-center">Pending</h2>

    </div>

    <div class="flex-1 bg-base-200 p-4 rounded-lg h-full overflow-y-auto flex flex-col">
      <h2 class="font-bold text-xl mb-4 text-center">In Progress</h2>
    </div>

    <div class="flex-1 bg-base-200 p-4 rounded-lg h-full overflow-y-auto flex flex-col">
      <h2 class="font-bold text-xl mb-4 text-center">Completed</h2>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import TaskCard from "../components/TaskCard.vue";

export default defineComponent({
  name: "TaskPage",
  components: { TaskCard },
});
</script>