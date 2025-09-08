import { createRouter, createWebHistory } from "vue-router";
import HomePage from "../pages/HomePage.vue";
import TasksPage from "../pages/TaskPage.vue";
import CreatePage from "../pages/CreatePage.vue";
import LoginPage from "../pages/LoginPage.vue";
import RegisterPage from "../pages/RegisterPage.vue";

const routes = [
  {
    path: "/",
    name: "Home",
    component: HomePage
  },
  {
    path: "/tasks",
    name: "Tasks",
    component: TasksPage
  },
  {
    path: "/create",
    name: "Create",
    component: CreatePage
  },
  {
    path: "/login",
    name: "Login",
    component: LoginPage
  },
  {
    path: "/register",
    name: "Register",
    component: RegisterPage
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
