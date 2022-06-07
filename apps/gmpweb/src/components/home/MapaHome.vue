<template>
  <div class="m-3">
    <b-embed
      type="iframe"
      aspect="21by9"
      :src="urlMapaHome"
      class="google-maps"
      allowfullscreen
    ></b-embed>
  </div>
</template>

<script>
import { ImagenEnlace } from "@/assets/enums/Enlaces.js";
export default {
  name: "MapaHome",
  computed: {
    urlMapaHome() {
      return ImagenEnlace.mapaHome;
    },
  },
};
</script>

<style>
</style>