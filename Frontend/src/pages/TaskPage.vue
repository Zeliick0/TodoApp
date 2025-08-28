<template>
  <div class="flex gap-4 p-4 h-screen bg-base-100">
    <!-- Pending -->
    <div class="flex-1 bg-base-200 p-4 rounded-lg h-full overflow-y-auto flex flex-col gap-4">
      <h2 class="font-bold text-xl mb-2 text-center">Pending</h2>
        <TaskCard 
            v-for="task in tasks.filter(t=> t.status === Status.Pending)"
            :key="task.id"
            :task="task"
            @start="startTask"
            @select="openDrawer"
            />
    </div>
    <!-- In Progress -->
    <div class="flex-1 bg-base-200 p-4 rounded-lg h-full overflow-y-auto flex flex-col gap-4">
      <h2 class="font-bold text-xl mb-2 text-center">In Progress</h2>
      <TaskCard 
            v-for="task in tasks.filter(t=> t.status === Status.InProgress)"
            :key="task.id"
            :task="task"
            @complete="completeTask"
            @select="openDrawer"
            />
    </div>
    <!-- Completed -->
    <div class="flex-1 bg-base-200 p-4 rounded-lg h-full overflow-y-auto flex flex-col gap-4">
      <h2 class="font-bold text-xl mb-2 text-center">Completed</h2>
      <TaskCard 
            v-for="task in tasks.filter(t=> t.status === Status.Completed)"
            :key="task.id"
            :task="task"
            @select="openDrawer"
            />
    </div>

    <TaskDrawer
      :task="selectedTask"
      :visible="drawerVisible"
      :tasks-list="tasks"
      @close="drawerVisible = false"
      />

  </div>
</template>

<script lang="ts">
import { ref, onMounted } from "vue";
import TaskCard from "../components/TaskCard.vue";
import TaskDrawer from "../components/TaskDrawer.vue"
import TaskService from "../services/TaskService.ts"
import TaskAction from "../services/TaskActions.ts"
import type Tasks from "../models/TaskInterface.ts"
import {Status, Priority} from "../models/TaskInterface.ts"

export default {
    components: { TaskCard, TaskDrawer },
    setup() {
        const tasks = ref<Tasks[]>([]);

        const selectedTask = ref<Tasks | null>(null);
        const drawerVisible = ref(false);


        onMounted(async () =>{
            tasks.value = await TaskService.getTasks();
        });

        const openDrawer = (task: Tasks) => {
          selectedTask.value = task;
          drawerVisible.value = true;
        };

        const startTask = async (task: Tasks) => {
          await TaskAction.startTask(task, tasks.value);
        };

        const completeTask = async (task: Tasks) => {
          await TaskAction.completeTask(task, tasks.value);
        }

        return { tasks, Status, Priority, selectedTask, drawerVisible, openDrawer, startTask, completeTask, };
    }
};

</script>