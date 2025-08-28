<template>
  <div class="bg-base-200 rounded-lg shadow-lg p-6 max-w-lg mx-auto mt-10">
    <h1 class="text-2xl font-bold mb-4">Create New Task</h1>

    <form @submit.prevent="submitTask" class="space-y-4">
      <!-- Title -->
      <div>
        <label class="block font-medium mb-1">Title</label>
        <input v-model="title" type="text" class="input input-bordered w-full" required />
      </div>

      <!-- Description -->
      <div>
        <label class="block font-medium mb-1">Description</label>
        <textarea v-model="description" class="textarea textarea-bordered w-full" required></textarea>
      </div>

      <!-- Priority -->
      <div>
        <label class="block font-medium mb-1">Priority</label>
        <select v-model="priority" class="select select-bordered w-full">
          <option :value="0">Low</option>
          <option :value="1">Medium</option>
          <option :value="2">High</option>
        </select>
      </div>
      <!-- Submit -->
      <button type="submit" class="btn btn-primary w-full">Create Task</button>
    </form>
  </div>
</template>

<script lang="ts">
import { ref } from "vue";
import TaskService from "../services/TaskService.ts";
import TaskDto from "../models/TasksDto.ts";
import { useRouter } from "vue-router";

export default {
  setup() {
    const router = useRouter();
    const title = ref("");
    const description = ref("");
    const status = ref(0);
    const priority = ref(0);

    const submitTask = async () => {
      const dto = new TaskDto({
        title: title.value,
        description: description.value,
        status: 0,
        priority: priority.value,
      });

      try {
        await TaskService.createTask(dto);
        alert("Task created successfully!");
        title.value = "";
        description.value = "";
        status.value = 0;
        priority.value = 0;
      } catch (err) {
        console.error(err);
        alert("Failed to create task.");
      }
      router.push({ name: "Tasks" });
    };

    return { title, description, priority, submitTask };
  },
};
</script>