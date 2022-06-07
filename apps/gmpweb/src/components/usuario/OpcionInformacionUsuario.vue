<template>
  <div>
    <CardOpcion
      :title="tituloCardOpcionInformacionUser"
      :description="descripcionCardOpcionInformacionUser"
      :imgsrc="imagenInformacionUsuarionCardOpcion"
      @ejecutarOpcion="abrirModalMostrarInformacionUsuario()"
    >
    </CardOpcion>

    <b-modal
      ref="modal-informacionUsuario"
      hide-footer
      :title="tituloModalInformacionUser"
    >
      <div class="mb-3">
        <b-img center :src="imagenInformacionUsuario" alt="image user"></b-img>
      </div>

      <b-list-group>
        <b-list-group-item> {{ nombreUsuario }} </b-list-group-item>
        <b-list-group-item> {{ apellidoUsuario }} </b-list-group-item>
        <b-list-group-item> {{ correoUsuario }} </b-list-group-item>
        <b-list-group-item> {{ telefonoUsuario }} </b-list-group-item>
        <b-list-group-item> {{ direccionUsuario }} </b-list-group-item>
      </b-list-group>
    </b-modal>
  </div>
</template>

<script>
import CardOpcion from "./CardOpcion.vue";
import { MostrarInformacionUsuarionMixin } from "@/assets/mixins/InformacionUsuarioMixin.js";

export default {
  name: "OpcionInformacionUsuario",

  components: {
    CardOpcion,
  },

  //compartidos
  mixins: [MostrarInformacionUsuarionMixin],
};
</script>

<style>
</style>