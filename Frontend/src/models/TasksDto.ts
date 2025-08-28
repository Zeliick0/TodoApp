import { Status, Priority } from "./TaskInterface";
import type Tasks from "./TaskInterface";

export default class TasksDto implements Tasks {
    constructor (
        public id: number,
        public title: string,
        public description: string,
        public status: Status,
        public priority: Priority,
    ) {}
}