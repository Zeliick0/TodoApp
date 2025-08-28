
export default class TaskDto {
  id?: number;
  title: string;
  description: string;
  status: number;
  priority: number;

  constructor(task: any) {
    this.id = task.id ?? task.Id;
    this.title = task.title ?? task.Title;
    this.description = task.description ?? task.Description;
    this.status = task.status ?? task.Status;
    this.priority = task.priority ?? task.Priority;
  }
}
