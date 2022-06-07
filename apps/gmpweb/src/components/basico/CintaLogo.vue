<template>
  <div class="image-fondo mt-3">
    <b-img center :src="enlace" :height="tamano" alt="cinta de logo" rounded="circle"></b-img>
  </div>
</template>

<script>
export default {
  name: "CintaLogo",

  //modelo
  data() {
    return {
      tamano: 70
    }
  },

  //propiedades
  props: {
    enlace: String,
  }
};
</script>

<style scoped>
.image-fondo{
  background-color:rgba(248, 247, 234, 0.952)
}
</style>