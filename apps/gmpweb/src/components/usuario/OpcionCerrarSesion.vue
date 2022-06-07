<template>
  <div>
    <CardOpcion
      :title="loqoutTitulo"
      :description="logoutDescripcion"
      :imgsrc="logoutEnlace"
      @ejecutarOpcion="abrirModal()"
    >
    </CardOpcion>

    <b-modal
      ref="modal-ingresarConOtroUsuario"
      hide-footer
      title="Ingrese con otro usuario">

      <p class="text-center">¿Desea cerrar la sesión?</p>

      <b-button class="mt-3" variant="outline-primary" block @click="cerrarModal()">Cancelar</b-button>
      <b-button class="mt-2" variant="outline-warning" block @click="volverALoquearse()">Cerrar sesión</b-button>
    </b-modal>
    
  </div>
</template>

<script>
import CardOpcion from "./CardOpcion.vue";
import { TituloOpcionesUsuarioHelp, DescripcionOpcionesUsuarioHelp } from "@/assets/enums/HelpText.js";
import { ImagenEnlace } from "@/assets/enums/Enlaces.js";
import { mapActions } from "vuex";

import { eliminarItemLocal, obtenerItemLocal } from '@/assets/localStorage/index.js'
import { localStorageKey } from '@/assets/enums/localStorageKey.js'

export default {
  name: "OpcionCerrarSesion",
  components: {
    CardOpcion,
  },
  computed: {

    loqoutTitulo() {
      return TituloOpcionesUsuarioHelp.loqoutTitulo;
    },

    logoutDescripcion() {
      return DescripcionOpcionesUsuarioHelp.logoutDescripcion;
    },

    logoutEnlace() {
      return ImagenEnlace.logoutEnlace;
    },
  },

  methods: {
    ...mapActions(["LogoutAction"]),

    abrirModal() {
      this.$refs["modal-ingresarConOtroUsuario"].show();
    },

    async volverALoquearse() {
      //obtenemos el token del local
      const token = obtenerItemLocal(localStorageKey.token);

      //se desloquea del sistema
      await this.LogoutAction({token: token});

      //elimina el token del local
      eliminarItemLocal(localStorageKey.token);

      //retornamos al ingreso
      this.$emit("irAIngreso");
    },

    cerrarModal(){
      this.$refs["modal-ingresarConOtroUsuario"].hide();
    },
  },
};
</script>

<style>
</style>