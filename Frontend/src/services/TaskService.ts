import REST from "./REST.ts";
import TasksApi from "../api/TasksApi";
import type Tasks from "../models/TaskInterface.ts"
import TaskDto from "../models/TasksDto.ts"

export default class TaskService {

    public static async getTasks(): Promise<Tasks[]> {
        return REST.get<Tasks[]>(TasksApi.todo.list);
    }

    public static async getTasksWithQuery(query: Record<string, string>): Promise<Tasks[]> {
        return REST.getWithQuery<Tasks[]>(TasksApi.todo.list, query);
    }

    public static async createTask(task: TaskDto): Promise<Tasks> {
        return REST.post(TasksApi.todo.create, task);
    }

    public static async updateTask(task: TaskDto): Promise<Tasks> {
        return REST.post(TasksApi.todo.update, task);
    }

    public static async deleteTask(id: number): Promise<boolean> {
        const url= TasksApi.todo.taskDeletion(id);
        return REST.delete(url);
    }
}