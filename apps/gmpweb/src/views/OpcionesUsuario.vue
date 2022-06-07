<template>
  <div>
    <div class="container-fluid">
      <div class="row">
        <div class="col-lg-4 col-md-6">
         <OpcionHelp @irAbout="irAbout()"></OpcionHelp>
        </div>
        <div class="col-lg-4 col-md-6">
          <OpcionCerrarSesion @irAIngreso="irAIngreso()"></OpcionCerrarSesion>
        </div>
        <div class="col-lg-4 col-md-6">
          <OpcionAdministrativa v-if="tienePermisos"></OpcionAdministrativa>
          <OpcionInformacionUsuario v-else></OpcionInformacionUsuario>
        </div>
      </div>
    </div>

    <Footer></Footer>
  </div>
</template>

<script>
import OpcionHelp from '../components/usuario/OpcionHelp.vue';
import OpcionCerrarSesion from '../components/usuario/OpcionCerrarSesion.vue'
import OpcionAdministrativa from '../components/usuario/OpcionAdministrativa.vue'
import OpcionInformacionUsuario from '../components/usuario/OpcionInformacionUsuario.vue'
import Footer from "../components/basico/Footer.vue"
import {mapGetters } from "vuex";
import { existeItemLocal } from "@/assets/localStorage/index.js";
import { localStorageKey } from "@/assets/enums/localStorageKey.js";

export default {
  name: "OpcionesUsuario",

  mounted(){
    if(existeItemLocal(localStorageKey.token) == false){
      this.irAIngreso();
    }
  },

  components: {
    Footer,
    OpcionHelp,
    OpcionCerrarSesion,
    OpcionAdministrativa,
    OpcionInformacionUsuario
  },

  computed:{
    ...mapGetters(["GetSession"]),

    tienePermisos(){
      if(this.GetSession.role === "root" || this.GetSession.role === "interno"){
        return true
      }
      else{
        return false
      }
    }
  },

  methods:{
    irAIngreso(){
      this.$router.push({ name: "Ingreso" });
    },
    irAbout(){
      this.$router.push({ name: "About" });
    }
  }
};
</script>

<style>
</style>