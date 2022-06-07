import Vue from 'vue'
import App from './App.vue'
import {BootstrapVue, IconsPlugin, BootstrapVueIcons} from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import {UserStore} from './assets/stores/UserStore.js'
import router from './assets/router/index.js'

Vue.config.productionTip = false
Vue.use(BootstrapVue)
Vue.use(BootstrapVueIcons)
Vue.use(IconsPlugin)

new Vue({
  render: h => h(App),
  router,

  //falta agregar varios stores
  store: UserStore
}).$mount('#app')
