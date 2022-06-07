<template>
  <div>
      <b-card
      :title="title"
      :img-src="imgsrc"
      img-alt="Image"
      img-top
      tag="article"
      class="m-3"
      style="max-width: 40rem;"
    >
      <b-card-text>
        {{description}}
      </b-card-text>

      <b-button pill variant="outline-secondary" @click="ir()">Ir</b-button>
    </b-card>
  </div>
</template>

<script>
export default {
    name: 'CardOpcion',
    props:{
        title: String,
        imgsrc: String,
        description: String
    },
    methods:{
      ir(){
        this.$emit("ejecutarOpcion");
      }
    }
}
</script>

<style>

</style>