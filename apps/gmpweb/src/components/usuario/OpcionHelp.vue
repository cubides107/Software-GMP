<template>
  <div>
    <CardOpcion
      :title="helpTitulo"
      :description="helpDescripcion"
      :imgsrc="helpEnlace"
      @ejecutarOpcion="irAbout()"
    >
    </CardOpcion>

  </div>
</template>

<script>
import CardOpcion from "./CardOpcion.vue";
import { TituloOpcionesUsuarioHelp, DescripcionOpcionesUsuarioHelp } from "@/assets/enums/HelpText.js";
import { ImagenEnlace } from "@/assets/enums/Enlaces.js";

export default {
  name: "OpcionHelp",

  components: {
    CardOpcion,
  },

  computed: {
    helpTitulo() {
      return TituloOpcionesUsuarioHelp.helpTitulo;
    },

    helpDescripcion() {
      return DescripcionOpcionesUsuarioHelp.helpDescripcion;
    },

    helpEnlace() {
      return ImagenEnlace.helpEnlace;
    },
  },

  methods:{
    irAbout(){
      this.$emit("irAbout");
    },
  }
};
</script>

<style>
</style>