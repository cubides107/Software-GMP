<template>
  <div>
    <NavBar></NavBar>
  </div>
</template>

<script>
import NavBar from "./components/basico/NavBar.vue";
import { existeItemLocal, obtenerItemLocal } from "@/assets/localStorage/index.js";
import { localStorageKey } from "@/assets/enums/localStorageKey.js";
import { descifrarToken } from "@/assets/JWT/index.js";
import { mapMutations } from "vuex";

export default {
  name: "App",

  mounted() {
    this.obtenerDatosSession();
  },

  components: {
    NavBar,
  },

  methods: {
    ...mapMutations(["ModificarSession", "ModificarToken"]),

    obtenerDatosSession() {
      //cargamos el estado en caso de que recargen la pagina o sea la primera vez
      if (existeItemLocal(localStorageKey.token) == true) {
        //optenemos los valores
        const token = obtenerItemLocal(localStorageKey.token);
        var datosSession = descifrarToken(token);

        //actualizamos el estado de la app
        this.ModificarToken(token);
        this.ModificarSession(datosSession);
      }
    },
  },
};
</script>


<style>
</style>
