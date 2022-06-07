<template>
  <div>
    <CabeceraSolicitud></CabeceraSolicitud>
    <FormularioSolicitud></FormularioSolicitud>
  </div>
</template>

<script>
import CabeceraSolicitud from "../components/solicitud/CabeceraSolicitud.vue";
import FormularioSolicitud from "../components/solicitud/FormularioSolicitud.vue";
import { existeItemLocal } from "@/assets/localStorage/index.js";
import { localStorageKey } from "@/assets/enums/localStorageKey.js";

export default {
  name: "Solicitud",

  mounted(){
    if(existeItemLocal(localStorageKey.token) == false){
      this.irAIngreso();
    }
  },

  components: {
    CabeceraSolicitud,
    FormularioSolicitud,
  },

  computed: {
  },

  methods: {
    irAIngreso() {
      this.$router.push({ name: "Ingreso" });
    },
  },
};
</script>

<style>
</style>