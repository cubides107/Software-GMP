<template>
  <div>
    <div class="row mb-4">
      <div class="col-6">
        <b-form-checkbox
          id="checkbox-mapa"
          v-model="statusActivarMapa"
          name="checkbox-mapa"
          value="Activada"
          unchecked-value="Desactivada"
        >
          habilitar el mapa para el envio de la solicitud
        </b-form-checkbox>
      </div>

      <div class="col-6">
        Estado: <strong>{{ statusActivarMapa }}</strong>
      </div>
    </div>

    <div class="row">
      <div class="col-md-12">

        <label for="FileMapa">Ingrese el mapa: </label>

        <input
          type="file"
          id="FileMapa"
          name="Mapa"
          :disabled="activarCampo"
          @change="CargarMapa"
          accept="application/pdf"
        />

        <div class="form-text">
          <span class="badge badge-pill badge-warning" v-if="MapaCorrecto">Campo invalido</span>
        </div>

      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "ImputsCamposMapa",

  components: {},

  data() {
    return {
      fileMapa: null,
      statusActivarMapa: "Desactivada",
    };
  },

  computed: {
    MapaCorrecto() {
      return this.fileMapa == null ? true : false;
    },
    activarCampo() {
      if (this.statusActivarMapa == "Activada") {
        return false;
      }

      return true;
    },
  },

  methods: {
    imputMapa({ newValue }) {
      this.fileMapa = newValue;
    },
    CargarMapa(e){
      this.fileMapa = e.target.files[0];
    },

    //retorna stado 0: si los campos estan desabilitados
    //retorna stado 1: si los campos estan incompletos
    //retorna stado 2: si los campos estan completos
    GetCamposMapa() {
      if (this.statusActivarMapa == "Desactivada") {
        return {
          stado: 0,
          File: null,
        };
      } else if (this.fileMapa == null) {
        return {
          stado: 1,
          File: null,
        };
      } else {
        return {
          stado: 2,
          File: this.fileMapa,
        };
      }
    },
    ResetearCampos(){
      this.fileMapa= null
      this.statusActivarMapa= "Desactivada"
    }
  },
};
</script>

<style>
</style>