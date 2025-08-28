<template>
  <div class="card w-full max-w-[95%] mx-auto bg-base-100 shadow-md mb-2">
    <div class="card-body p-4">
      <!-- Card attributes -->
      <div class="flex justify-between items-center">
        <h2
          class="card-title text-lg font-bold cursor-pointer hover:underline flex items-center"
          @click="$emit('select', task)"
        >
          <span>{{ task.title }}</span>
        </h2>

        <span :class="['badge font-bold', priorityColor]">
          {{ priorityText }}
        </span>
      </div>

      <!-- Button for starting the task -->
      <div v-if="task.status === Status.Pending" class="mt-2">
        <button class="btn btn-sm btn-accent" @click="$emit('start', task)">
          Start
        </button>
      </div>

      <!-- Button for completing the task -->
      <div v-if="task.status === Status.InProgress" class="mt-2">
        <button class="btn btn-sm btn-success" @click="$emit('complete', task)">
          Complete
        </button>
      </div>

    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, computed } from "vue";
import { Priority, Status} from "../models/TaskInterface.ts";
import type Tasks from "../models/TaskInterface.ts";

export default defineComponent({
  name: "TaskCard",
  props: {
    task: { type: Object as () => Tasks, required: true },
  },
  emits: ["start", "select", "complete"],
  setup(props) {

    const priorityMap = {
      [Priority.Low]: { text: "Low", color: "badge-success" },
      [Priority.Medium]: { text: "Medium", color: "badge-warning" },
      [Priority.High]: { text: "High", color: "badge-error" },
    };

    const priorityColor = computed(
      () => priorityMap[props.task.priority]?.color ?? "badge-neutral"
    );

    const priorityText = computed(
      () => priorityMap[props.task.priority]?.text ?? "Not specified"
    );

    return {
      priorityColor,
      priorityText,
      Status,
    };
  },
});
</script>