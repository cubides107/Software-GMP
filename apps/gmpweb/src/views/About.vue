<template>
  <div>
    <Acercade></Acercade>
    <Footer></Footer>
  </div>
</template>

<script>
import { existeItemLocal } from "@/assets/localStorage/index.js";
import { localStorageKey } from "@/assets/enums/localStorageKey.js";
import Acercade from '../components/about/Acercade.vue';
import Footer from '../components/basico/Footer.vue';

export default {
  name: "About",
  mounted() {
    if (existeItemLocal(localStorageKey.token) == false) {
      this.irAIngreso();
    } 
  },
  components: {
      Acercade,
      Footer
   }
}
</script>

<style>

</style>
