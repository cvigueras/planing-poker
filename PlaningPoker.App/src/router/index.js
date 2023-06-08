import { createWebHistory, createRouter } from "vue-router"
import HomePlaning from "@/components/HomePlaning.vue"
import PageNotFound from '@/components/PageNotFound.vue'
import GamePlaning from '@/components/GamePlaning.vue'

const routes = [
    {
        path: "/",
        name: "Home",
        component: HomePlaning,
    },
    {
        path: "/planing",
        name: "GamePlaning",
        component: GamePlaning,
    },
    {
        path: '/:pathMatch(.*)*',
        name: "PageNotFound",
        component: PageNotFound,
    },
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router