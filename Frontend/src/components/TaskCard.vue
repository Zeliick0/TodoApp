<template>
  <div class="card w-80 bg-base-100 shadow-sm">
    <div class="card-body p-4">
        
      <h2 class="card-title text-lg font-bold">{{ title }}</h2>

      <span
        :class="['badge badge-outline mt-2', priorityColor]"
      >
        {{ priority }}
      </span>

      <div class="card-actions justify-end mt-4">
        <button class="btn btn-sm btn-primary">Edit</button>
        <button class="btn btn-sm btn-error">Delete</button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, computed } from "vue";

export default defineComponent({
  name: "TaskCard",
  props: {
    title: { type: String, required: true },
    priority: { type: String, required: true },
  },
  setup(props) {
    const priorityColor = computed(() => {
      switch (props.priority.toLowerCase()) {
        case "high":
          return "badge-error";
        case "medium":
          return "badge-warning";
        case "low":
          return "badge-success";
        default:
          return "badge-neutral";
      }
    });

    return { priorityColor };
  },
});
</script>