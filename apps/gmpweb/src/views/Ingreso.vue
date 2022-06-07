<template>
  <div>

    <CintaLogo :enlace="ImagenLogo"></CintaLogo>

    <div class="container-fluid">
      <div class="row">

        <div class="col-md-12 col-lg-5">
          <Imagen :enlace="ImagenIngresoUsuario"></Imagen>
        </div>

        <div class="col-md-12 col-lg-7">
          <Logeo @irAHome="irAHome()" @verificarAdministrativo="abrirModalVerificacionAdmin()"></Logeo>
        </div>
        
      </div>
    </div>

    <VerificarAccesoAdmin ref="VerificarAccesoAdmin" @irAdministrativa="irAdministrativa()"></VerificarAccesoAdmin>

    <Footer></Footer>
  </div>
</template>

<script>
import { ImagenEnlace } from "@/assets/enums/Enlaces.js";
import Logeo from "../components/usuario/Logeo.vue";
import Imagen from "../components/basico/Imagen.vue";
import Footer from "../components/basico/Footer.vue";
import CintaLogo from "../components/basico/CintaLogo.vue";
import VerificarAccesoAdmin from "../components/administrativo/VerificarAccesoAdmin.vue"

export default {
  name: "Ingreso",

  components: {
    Logeo,
    Imagen,
    Footer,
    CintaLogo,
    VerificarAccesoAdmin
  },

  //metodos
  methods:{
    irAHome(){
      this.$router.push({ name: "Home" });
    },
    irAdministrativa(){
      this.$router.push({ name: "Administrativa" });
    },
    abrirModalVerificacionAdmin(){
      this.$refs.VerificarAccesoAdmin.abrirModal();
    }
  },

  //propiedades computadas
  computed: {
    ImagenIngresoUsuario() {
      return ImagenEnlace.ImagenIngresoUsuario;
    },
    ImagenLogo() {
      return ImagenEnlace.ImagenLogo;
    },
  },
};
</script>

<style>
</style>