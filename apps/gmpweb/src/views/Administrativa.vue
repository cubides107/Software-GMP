<template>
  <div>
    <div class="container-fluid">
      <div class="row">
        <div class="col-lg-3 col-md-6 col-sm-12 mt-3">
          <CreacionUsuarioInterno></CreacionUsuarioInterno>
        </div>
        <div class="col-lg-3 col-md-6 col-sm-12 mt-3">
          <CreacionUsuarioExterno></CreacionUsuarioExterno>
        </div>
        <div class="col-lg-3 col-md-6 col-sm-12 mt-3">
          <ConsultarUsuarios></ConsultarUsuarios>
        </div>
        <div class="col-lg-3 col-md-6 col-sm-12 mt-3">
          <PerfilUsuario> </PerfilUsuario>
        </div>
      </div>
    </div>

    <Footer></Footer>
  </div>
</template>

<script>
import Footer from "../components/basico/Footer.vue";
import CreacionUsuarioInterno from "../components/administrativo/CreacionUsuarioInterno.vue";
import { existeItemLocal } from "@/assets/localStorage/index.js";
import { localStorageKey } from "@/assets/enums/localStorageKey.js";
import { mapGetters } from "vuex";
import CreacionUsuarioExterno from "../components/administrativo/CreacionUsuarioExterno.vue";
import ConsultarUsuarios from "../components/administrativo/ConsultarUsuarios.vue";
import PerfilUsuario from "../components/administrativo/InformacionUsuario.vue";

export default {
  name: "Administrativa",

  mounted() {
    if (existeItemLocal(localStorageKey.token) == false) {
      this.irAIngreso();
    } else if (
      this.GetSession.role != "root" &&
      this.GetSession.role != "interno"
    ) {
      //tambien debe crear una alerta debe crear una alerta
      this.irAIngreso();
    }
  },

  computed: {
    ...mapGetters(["GetSession"]),
  },

  components: {
    Footer,
    CreacionUsuarioInterno,
    CreacionUsuarioExterno,
    ConsultarUsuarios,
    PerfilUsuario
  },

  methods: {
    irAIngreso() {
      this.$router.push({ name: "Ingreso" });
    },
    irAOpcionesOsuario() {
      this.$router.push({ name: "OpcionesUsuario" });
    },
  },
};
</script>

<style>
</style>