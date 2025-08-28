<template>
  <div class="flex gap-4 p-4 h-screen bg-base-100">
    <!-- Pending -->
    <div class="flex-1 bg-base-200 p-4 rounded-lg h-full overflow-y-auto flex flex-col">
      <h2 class="font-bold text-xl mb-4 text-center">Pending</h2>
        <TaskCard 
            v-for="task in tasks.filter(t=> t.status === Status.Pending)"
            :key="task.id"
            :title="task.title"
            :priority="task.priority"/>
    </div>
    <!-- In Progress -->
    <div class="flex-1 bg-base-200 p-4 rounded-lg h-full overflow-y-auto flex flex-col">
      <h2 class="font-bold text-xl mb-4 text-center">In Progress</h2>
    </div>
    <!-- Completed -->
    <div class="flex-1 bg-base-200 p-4 rounded-lg h-full overflow-y-auto flex flex-col">
      <h2 class="font-bold text-xl mb-4 text-center">Completed</h2>
    </div>
  </div>
</template>

<script lang="ts">
import { ref, onMounted } from "vue";
import TaskCard from "../components/TaskCard.vue";
import TaskService from "../services/TaskService.ts"
import type Task from "../models/TaskInterface.ts"
import {Status, Priority} from "../models/TaskInterface.ts"

export default {
    setup() {
        const tasks = ref<Task[]>([]);

        onMounted(async () =>{
            tasks.value = await TaskService.getTasks();
        });
        return {tasks, Status, Priority};
    }
};


</script>