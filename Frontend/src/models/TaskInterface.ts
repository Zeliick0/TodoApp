export enum Status {
    Pending = 0,
    InProgress = 1,
    Completed = 2
}

export enum Priority {
    Low = 0,
    Medium = 1,
    High = 2
}

export default interface Tasks {
    id: number;
    title: string;
    description: string;
    status: Status;
    priority: Priority;
    createdAt: string;
}