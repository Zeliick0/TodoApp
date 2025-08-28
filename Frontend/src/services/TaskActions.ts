import TaskService from "./TaskService.ts";
import type Tasks from "../models/TaskInterface.ts"
import TaskDto from "../models/TasksDto.ts";



export default class TaskActions {

     private static updateLocal(task: Tasks, tasksList: Tasks[]) {
        const index = tasksList.findIndex(t => t.id === task.id);
        if (index !== -1) {
            tasksList.splice(index, 1, task);
        }
    }

   public static async startTask(task: Tasks, tasksList: Tasks[]): Promise<void> {
        const dto = new TaskDto({
            ...task,
            status: 1,
        });

        await TaskService.updateTask(dto);

       const updatedTask = { ...task, status: 1 };
        this.updateLocal(updatedTask, tasksList);
    }

    public static async completeTask(task: Tasks, tasksList: Tasks[]): Promise<void> {
        const dto = new TaskDto({
            ...task,
            status: 2,
        });

        await TaskService.updateTask(dto);

        const updatedTask = { ...task, status: 2 };
        this.updateLocal(updatedTask, tasksList);
    }

    public static async deleteTask(task: Tasks, tasksList: Tasks[]): Promise<void> {
        await TaskService.deleteTask(task.id);

        const index = tasksList.findIndex(t => t.id === task.id);
        if (index !== -1) {
            tasksList.splice(index, 1);
        }
    }

    public static async updateTask(task: Tasks, tasksList: Tasks[]): Promise<void> {
        const dto = new TaskDto(task);
        await TaskService.updateTask(dto); 

        const updatedTask = { ...task };
        this.updateLocal(updatedTask, tasksList);
    }
}