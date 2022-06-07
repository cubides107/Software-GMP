<template>
  <div>
    <div class="mb-3">
      <label :for="imputId" class="form-label">{{ labelName }}</label>
      <input
        class="form-control"
        :type="imputType"
        :id="imputId"
        :aria-describedby="HelpId"
        v-model="campo"
        :disabled="desactivado"
      />
      <div class="form-text" :id="HelpId">
        {{HelpTxt}} 
        <span class="badge badge-pill badge-warning" v-if="campoCorrecto">Campo invalido</span>
        <span class="badge badge-pill badge-success" v-if="campoOpcional">Campo opcional</span>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "ImputCampo",

  components: {
    
  },

  data() {
    return {
      campo: ""
    }
  },
  
  props: {
    labelName: String,
    imputId: String,
    imputType: String,
    HelpId: String,
    HelpTxt: String,
    campoCorrecto: Boolean,
    campoOpcional: Boolean,
    desactivado: {
      type: Boolean,
      default: false
    }
  },

  watch:{
    campo(newValue, oldValue){
      this.$emit("GetValorCampoImput", {newValue, oldValue});
    }
  },

  methods: {
    ResetearCampo(){
      this.campo = "";
    }
  },
};
</script>

<style>
</style>