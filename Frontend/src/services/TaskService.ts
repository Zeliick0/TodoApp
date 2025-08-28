import REST from "./REST.ts";
import TasksApi from "../api/TasksApi";
import type Tasks from "../models/TaskInterface.ts"
import TaskDto from "../models/TasksDto.ts"

export default class TaskService {

    public static async getTasks(): Promise<Tasks[]> {
        return REST.get<Tasks[]>(TasksApi.task.list);
    }

    public static async getTasksWithQuery(query: Record<string, string>): Promise<Tasks[]> {
        return REST.getWithQuery<Tasks[]>(TasksApi.task.list, query);
    }

    public static async createTask(task: TaskDto): Promise<Tasks> {
        return REST.post(TasksApi.task.create, task);
    }

    public static async updateTask(task: TaskDto): Promise<Tasks> {
        return REST.post(TasksApi.task.update, task);
    }

    public static async deleteTask(id: number): Promise<boolean> {
        const url= TasksApi.task.deletion(id);
        return REST.delete(url);
    }
}