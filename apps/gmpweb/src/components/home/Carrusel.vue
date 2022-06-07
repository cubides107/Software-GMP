<template>
  <div>

    <b-carousel class="m-3" :interval="5000" indicators controls>   
        <b-carousel-slide :key="imagesCarrusel.imagenCarruselUno" v-for="imagesCarrusel in listImagesCarrusel"
          content-visible-up="sm"
          :img-src="imagesCarrusel.imagenCarrusel"
        ></b-carousel-slide>
    </b-carousel>

  </div>
</template>

<script>
import { listImagesCarrusel } from "@/assets/enums/Enlaces.js";
export default {
  name: "Carrusel",

  computed: {
    listImagesCarrusel() {
      return listImagesCarrusel;
    },
  },
};
</script>

<style>
.carousel-indicators {
  bottom: -50px;
}
.carousel-indicators li {
  width: 10px;
  height: 10px;
  border-radius: 100%;
  background-color: grey;
}
</style>