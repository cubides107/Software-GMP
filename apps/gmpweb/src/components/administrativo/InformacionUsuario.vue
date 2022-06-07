<template>
  <div>
    <CardAdmin
      tipo="secondary"
      :titulo="tituloCardOpcionInformacionUser"
      :descripcion="descripcionCardOpcionInformacionUser"
      @ejecutarOpcionAdmin="abrirModalMostrarInformacionUsuario()"
    >
    </CardAdmin>

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
import CardAdmin from "./CardAdmin.vue";
import { MostrarInformacionUsuarionMixin } from "@/assets/mixins/InformacionUsuarioMixin.js";

export default {
  name: "InformacionUsuario",

  components: {
    CardAdmin,
  },

  //compartidos
  mixins: [MostrarInformacionUsuarionMixin],

};
</script>

<style>
</style>