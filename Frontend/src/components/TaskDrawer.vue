<template>
  <!-- Backdrop -->
  <transition name="fade">
    <div
      v-if="visible"
      class="fixed inset-0 bg-black/40 z-40"
      @click="closeDrawer"
    ></div>
  </transition>

  <!-- Centered Pop-up Card -->
  <transition name="scale-fade">
    <div
      v-if="visible"
      class="fixed inset-0 flex items-center justify-center z-50"
    >
      <div
        class="bg-base-100 rounded-lg shadow-lg p-6 max-w-md w-full relative"
        @click.stop
      >
        <!-- Header -->
        <div class="flex justify-between items-center mb-4">
          <h2 v-if="!editing" class="text-xl font-bold">{{ task?.title }}</h2>
          <input v-else v-model="editableTask!.title" class="input input-bordered w-full" />
          <button class="btn btn-sm btn-ghost" @click="closeDrawer">✕</button>
        </div>

        <!-- Content -->
        <div class="space-y-4">
          <!-- Priority & Status -->
          <div class="flex items-center gap-2">
            <span v-if="!editing" :class="['badge font-bold', priorityColor]">{{ priorityText }}</span>
            <select v-else v-model="editableTask!.priority" class="select select-bordered">
              <option :value="0">Low</option>
              <option :value="1">Medium</option>
              <option :value="2">High</option>
            </select>
             <span class="text-sm text-gray-500">{{ editableTask!.status === 0 ? 'Pending' : editableTask!.status === 1 ? 'In Progress' : 'Completed' }}</span>
          </div>

          <!-- Description -->
          <div>
            <p v-if="!editing">{{ editableTask!.description }}</p>
            <textarea v-else v-model="editableTask!.description" class="textarea textarea-bordered w-full"></textarea>
          </div>

          <!-- CreatedAt -->
          <p class="text-xs text-gray-400">Created: {{ formattedCreatedAt }}</p>

          <!-- Action buttons -->
          <div class="flex gap-2 mt-4">
            <button v-if="!editing" class="btn btn-primary btn-sm" @click="editing = true">Edit</button>
            <button v-else class="btn btn-success btn-sm" @click="confirmEdit">Confirm</button>
            <button class="btn btn-error btn-sm" @click="deleteTask">Delete</button>
          </div>
        </div>
      </div>
    </div>
  </transition>
</template>

<script lang="ts">
import { defineComponent, ref, computed, watch } from "vue";
import type Tasks from "../models/TaskInterface.ts";
import TaskActions from "../services/TaskActions.ts";
import { Priority } from "../models/TaskInterface.ts";

export default defineComponent({
  name: "TaskDrawer",
  props: {
    task: { type: Object as () => Tasks | null, required: false },
    visible: { type: Boolean, required: true },
    tasksList: { type: Array as () => Tasks[], required: true } // Pass main tasks array
  },
  emits: ["close"],
  setup(props, { emit }) {
    const editing = ref(false);
    const editableTask = ref<Tasks | null>(null);

    watch(
      () => props.task,
      (newTask) => {
        if (newTask) {
          editableTask.value = { ...newTask }; // shallow copy
          editing.value = false;
        }
      },
      { immediate: true }
    );

    const closeDrawer = () => emit("close");

    // Handling priority displaying
    const priorityMap = {
      [Priority.Low]: { text: "Low", color: "badge-success" },
      [Priority.Medium]: { text: "Medium", color: "badge-warning" },
      [Priority.High]: { text: "High", color: "badge-error" },
    };

    const priorityColor = computed(
      () => priorityMap[editableTask.value?.priority ?? 0]?.color ?? "badge-neutral"
    );

    const priorityText = computed(
      () => priorityMap[editableTask.value?.priority ?? 0]?.text ?? "Not specified"
    );

    const formattedCreatedAt = computed(() =>
      editableTask.value?.createdAt ? new Date(editableTask.value.createdAt).toLocaleString() : ""
    );

    // Handling button actions
    const confirmEdit = async () => {
      if (!editableTask.value) return;
      await TaskActions.updateTask(editableTask.value, props.tasksList);
      editing.value = false;
      emit("close");
    };

    const deleteTask = async () => {
      if (!editableTask.value) return;
      console.log("Deleting task:", editableTask.value.id);
      await TaskActions.deleteTask(editableTask.value, props.tasksList);
      emit("close");
    };

    return {
      editing,
      editableTask,
      priorityColor,
      priorityText,
      formattedCreatedAt,
      confirmEdit,
      deleteTask,
      closeDrawer,
    };
  },
});
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

.scale-fade-enter-active,
.scale-fade-leave-active {
  transition: transform 0.3s ease, opacity 0.3s ease;
}
.scale-fade-enter-from,
.scale-fade-leave-to {
  transform: scale(0.8);
  opacity: 0;
}
</style>