<template>
  <div>
    <div class="d-flex justify-content-center">
      <b-form-checkbox v-model="checked" name="check-button" switch>
        Acepta la política de tratamiento de datos
      </b-form-checkbox>
    </div>

    <b-container class="mt-1" fluid v-show="checked">
      <b-row>
        <b-col cols="4">
          <b-list-group v-b-scrollspy:listgroup-ex>
            <b-list-group-item href="#list-item-1">Item 1</b-list-group-item>
            <b-list-group-item href="#list-item-2">Item 2</b-list-group-item>
            <b-list-group-item href="#list-item-3">Item 3</b-list-group-item>
            <b-list-group-item href="#list-item-4">Item 4</b-list-group-item>
            <b-list-group-item href="#list-item-5">Item 5</b-list-group-item>
          </b-list-group>
        </b-col>

        <b-col cols="8">
          <div
            id="listgroup-ex"
            style="position: relative; overflow-y: auto; height: 300px"
          >
            <h4 id="list-item-1">Item 1</h4>
            <p>{{ text }}</p>
            <h4 id="list-item-2">Item 2</h4>
            <p>{{ text }}</p>
            <h4 id="list-item-3">Item 3</h4>
            <p>{{ text }}</p>
            <h4 id="list-item-4">Item 4</h4>
            <p>{{ text }}</p>
            <h4 id="list-item-5">Item 5</h4>
            <p>{{ text }}</p>
          </div>
        </b-col>
      </b-row>
    </b-container>
  </div>
</template>

<script>
export default {
  name: "AceptaPoliticaDatos",
  data() {
    return {
      checked: false,
      text: `
          Quis magna Lorem anim amet ipsum do mollit sit cillum voluptate ex nulla
          tempor. Laborum consequat non elit enim exercitation cillum aliqua
          consequat id aliqua. Esse ex consectetur mollit voluptate est in duis
          laboris ad sit ipsum anim Lorem. Incididunt veniam velit elit elit veniam
          Lorem aliqua quis ullamco deserunt sit enim elit aliqua esse irure. Laborum
          nisi sit est tempor laborum mollit labore officia laborum excepteur
          commodo non commodo dolor excepteur commodo. Ipsum fugiat ex est consectetur
          ipsum commodo tempor sunt in proident.
        `,
    };
  },
  watch:{
      checked(newData){
          this.$emit("AceptaPoliticasDeTratamientoDeDatos", newData);
      }
  },
  methods: {
    ResetearCampos(){
      this.checked = false;
    }
  },
};
</script>

<style>
</style>