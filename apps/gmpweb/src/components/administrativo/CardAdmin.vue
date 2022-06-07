<template>
  <div>
    <b-card :bg-variant="tipo" :header="titulo" text-variant="white" class="text-center">
        <b-card-text>{{descripcion}}</b-card-text>
        <b-button pill variant="outline-light" @click="ir()">Ver</b-button>
    </b-card>
  </div>
</template>

<script>
export default {
    name: "CardAdmin",
    props:{
        tipo: String,
        titulo: String,
        descripcion: String
    },
    methods:{
        ir(){
            this.$emit("ejecutarOpcionAdmin")
        }
    }

}
</script>

<style>

</style>