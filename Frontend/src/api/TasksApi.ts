const base: string = "http://localhost:5093/TodoApp";

const list:     string = `${base}/list`;
const create:   string = `${base}/create`;
const update:   string = `${base}/update`;
const deletion =  (id: number) => `${base}/${id}`;

export default {
    task: {
        base,
        list,
        create,
        update,
        deletion
    }
}