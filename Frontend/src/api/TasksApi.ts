const base: string = "http://localhost:5093/TodoApp";

const list:     string = `${base}/tasks/list`;
const create:   string = `${base}/tasks/create`;
const update:   string = `${base}/tasks/update`;
const taskDeletion =  (id: number) => `${base}/tasks/${id}`;

const login:    string = `${base}/users/login`;
const register: string = `${base}/users/register`
const logout:   string = `${base}/users/logout`
const userDeletion = (id: number) => `${base}/users/${id}`;

export default {
    todo: {
        base,
        list,
        create,
        update,
        taskDeletion,
        login,
        register,
        logout,
        userDeletion
    }
}