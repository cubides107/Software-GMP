<template>
  <div>
   <Carrusel></Carrusel>
   <CartasInformativas></CartasInformativas> 
   <MapaHome></MapaHome>
   <Footer></Footer>
  </div>
</template>

<script>
import { existeItemLocal } from "@/assets/localStorage/index.js";
import { localStorageKey } from "@/assets/enums/localStorageKey.js";
import Carrusel from '../components/home/Carrusel.vue';
import CartasInformativas from  '../components/home/CartasInformativas.vue';
import MapaHome from '../components/home/MapaHome.vue';
import Footer from '../components/basico/Footer.vue';

export default {
  name: "Home",

  mounted() {
    if (existeItemLocal(localStorageKey.token) == false) {
      this.irAIngreso();
    }
  },

  components: {
    Carrusel,
    CartasInformativas,
    MapaHome,
    Footer
  },

  methods:{
    irAIngreso(){
      this.$router.push({ name: "Ingreso" });
    },
  }
};
</script>
