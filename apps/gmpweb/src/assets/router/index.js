import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '@/views/Home.vue'
import About from '@/views/About.vue'
import Ingreso from '@/views/Ingreso.vue'
import Solicitud from '@/views/Solicitud.vue'
import OpcionesUsuario from '@/views/OpcionesUsuario.vue'
import Administrativa from '@/views/Administrativa'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Ingreso',
    component: Ingreso
  },
  {
    path: '/Home',
    name: 'Home',
    component: Home
  },
  {
    path: '/About',
    name: 'About',
    component: About
  },
  {
    path: '/Solicitud',
    name: 'Solicitud',
    component: Solicitud
  },
  {
    path: '/OpcionesUsuario',
    name: 'OpcionesUsuario',
    component: OpcionesUsuario
  },
  {
    path: '/Administrativa',
    name: 'Administrativa',
    component: Administrativa
  }
]

const router = new VueRouter({
  routes
})

export default router
