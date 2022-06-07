<template>
  <div>
    <CardOpcion
      :title="administracionTitulo"
      :description="administracionDescripcion"
      :imgsrc="administracionEnlace"
      @ejecutarOpcion="irAdministracion()"
    >
    </CardOpcion>
  </div>
</template>

<script>
import CardOpcion from './CardOpcion.vue'
import {TituloOpcionesUsuarioHelp, DescripcionOpcionesUsuarioHelp} from "@/assets/enums/HelpText.js";
import { ImagenEnlace } from "@/assets/enums/Enlaces.js";

export default {
  name: "OpcionAdministrativa",
  components:{
      CardOpcion,
  },
  computed:{
    administracionTitulo(){
      return TituloOpcionesUsuarioHelp.administracionTitulo;
    },

    administracionDescripcion(){
      return DescripcionOpcionesUsuarioHelp.administracionDescripcion;
    },

    administracionEnlace(){
      return ImagenEnlace.administracionEnlace;
    }
  },
  methods:{
    irAdministracion(){
      this.$router.push({name: "Administrativa"});
    },
    irAIngreso(){
      this.$router.push({ name: "Ingreso" });
    },
  }
};
</script>

<style>
</style>