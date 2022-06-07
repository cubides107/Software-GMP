<template>
  <div>
    <div class="row mb-4">

      <div class="col-6">
        <b-form-checkbox
          id="checkbox-1"
          v-model="statusActivarDireccion"
          name="checkbox-1"
          value="Activada"
          unchecked-value="Desactivada"
        >
          habilitar la dirección para el envio de la solicitud
        </b-form-checkbox>
      </div>

      <div class="col-6">
        Estado: <strong>{{ statusActivarDireccion }}</strong>
      </div>
    </div>

    <div class="row">
      <div :class="CambiarTamanoColumnaSinCiudad">
        <label for="imputPais" class="form-label">Pais</label>

        <b-form-select
          id="imputPais"
          v-model="Pais"
          :options="optionsPais"
          :disabled="activarCampo"
        ></b-form-select>

        <div class="form-text" id="HelpId">
          <span class="badge badge-pill badge-warning" v-if="PaisCorrecto"
            >Campo invalido</span
          >
        </div>
      </div>

      <div class="col-md-4" v-if="HabilitarCiudad">
        <label for="imputCiudad" class="form-label">Ciudad</label>

        <b-form-select
          id="imputCiudad"
          v-model="Ciudad"
          :options="optionsCiudad"
          :disabled="activarCampo"
        ></b-form-select>

        <div class="form-text" id="HelpId">
          <span class="badge badge-pill badge-warning" v-if="CiudadCorracta"
            >Campo invalido</span
          >
        </div>
      </div>

      <div :class="CambiarTamanoColumnaSinCiudad">
        <ImputCampo
          labelName="Barrio"
          imputId="imputBarrio"
          imputType="text"
          HelpId="helpBarrio"
          HelpTxt="Ingrese el Barrio"
          :campoCorrecto="BarrioCorrecto"
          :desactivado="activarCampo"
          @GetValorCampoImput="imputBarrio"
          ref="resetBarrio"
        >
        </ImputCampo>
      </div>

      <div class="col-md-12">
        <b-input-group prepend="Descripción dirección" class="mb-2">
          <b-form-input
            aria-label="First name"
            placeholder="Calle"
            v-model="Calle"
            :disabled="activarCampo"
          ></b-form-input>
          <b-form-input
            aria-label="Last name"
            placeholder="Carrera"
            v-model="Carrera"
            :disabled="activarCampo"
          ></b-form-input>
          <b-form-input
            aria-label="Last name"
            placeholder="Trasversal"
            v-model="Trasversal"
            :disabled="activarCampo"
          ></b-form-input>
          <b-form-input
            aria-label="Last name"
            placeholder="localidad"
            v-model="Localidad"
            :disabled="activarCampo"
          ></b-form-input>
          <b-form-input
            aria-label="Last name"
            placeholder="N residencia"
            v-model="residencia"
            :disabled="activarCampo"
          ></b-form-input>
          <b-form-input
            aria-label="Last name"
            placeholder="Datos adicionales"
            v-model="adicionales"
            :disabled="activarCampo"
          ></b-form-input>
        </b-input-group>

        <div class="form-text" id="HelpId">
          <span class="badge badge-pill badge-warning" v-if="DireccionCorrecta"
            >Campo invalido</span
          >
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { GetCiudades } from "@/assets/apisTerceros/DivisionPoliticaColombiaApi.js";
import ImputCampo from "../basico/ImputCampo.vue";

export default {
  name: "ImputsCamposDireccion",

  async mounted() {
    this.optionsCiudad = await GetCiudades();
  },

  components: {
    ImputCampo,
  },

  data() {
    return {
      Calle: "",
      Carrera: "",
      Trasversal: "",
      Localidad: "",
      residencia: "",
      adicionales: "",

      Pais: "",
      optionsPais: [
        { value: "", text: "Seleccione un pais" },
        { value: "Colombia", text: "Colombia" },
      ],

      Ciudad: "",
      optionsCiudad: [],

      Barrio: "",

      statusActivarDireccion: "Desactivada",
    };
  },

  computed: {
    HabilitarCiudad() {
      if (this.Pais == "") {
        return false;
      } else {
        return true;
      }
    },
    CambiarTamanoColumnaSinCiudad() {
      if (this.Pais == "") {
        return "col-md-6";
      } else {
        return "col-md-4";
      }
    },
    PaisCorrecto() {
      return this.verificarCampo(this.Pais);
    },
    CiudadCorracta() {
      return this.verificarCampo(this.Ciudad);
    },
    BarrioCorrecto() {
      return this.verificarCampo(this.Barrio);
    },
    DireccionCorrecta() {
      var camposCorrectos = [];
      camposCorrectos.push(this.verificarCampo(this.Calle));
      camposCorrectos.push(this.verificarCampo(this.Carrera));
      camposCorrectos.push(this.verificarCampo(this.Trasversal));
      camposCorrectos.push(this.verificarCampo(this.Localidad));
      camposCorrectos.push(this.verificarCampo(this.residencia));
      camposCorrectos.push(this.verificarCampo(this.adicionales));

      return camposCorrectos.includes(true);
    },

    activarCampo(){
        if(this.statusActivarDireccion == "Activada"){
            return false;
        }
        return true;
    }
  },

  methods: {
    imputBarrio({ newValue }) {
      this.Barrio = newValue;
    },

    verificarCampo(campo) {
      return campo == "" ? true : false;
    },

    verificarTodosLosCampos(){
        var camposCorrectos = [];
        camposCorrectos.push(this.verificarCampo(this.Calle))
        camposCorrectos.push(this.verificarCampo(this.Carrera))
        camposCorrectos.push(this.verificarCampo(this.Trasversal))
        camposCorrectos.push(this.verificarCampo(this.Localidad))
        camposCorrectos.push(this.verificarCampo(this.residencia))
        camposCorrectos.push(this.verificarCampo(this.adicionales))

        camposCorrectos.push(this.verificarCampo(this.Pais))
        camposCorrectos.push(this.verificarCampo(this.Ciudad))
        camposCorrectos.push(this.verificarCampo(this.Barrio))

        return camposCorrectos.includes(true);
    },

    concatenarDatosDireccion(){
        return this.Calle +" - "+ this.Carrera
            +" - "+ this.Trasversal +" - "+ this.Localidad +" - "+ this.residencia +" - "+this.adicionales
    },

    //retorna stado 0: si los campos estan desabilitados
    //retorna stado 1: si los campos estan incompletos
    //retorna stado 2: si los campos estan completos
    GetCamposDireccion(){
        var obj = {stado: null, objeto: null};
        var camposCorrectos = this.verificarTodosLosCampos();

        if(this.statusActivarDireccion == "Desactivada"){
            obj.stado = 0
            obj.objeto = null;
        }
        else if(camposCorrectos == true){
            obj.stado = 1
            obj.objeto = null;
        }
        else if(camposCorrectos == false){
            obj.stado = 2
            obj.objeto = {
                AddressText: this.concatenarDatosDireccion(),
                Neighborhood: this.Barrio,
                City : this.Ciudad,
                Country :this.Pais
            };
        }

        return obj;
    },
    ResetearCampos(){
      //reseteamos variables
      this.Calle = ""
      this.Carrera = ""
      this.Trasversal = ""
      this.Localidad = ""
      this.residencia = ""
      this.adicionales = ""

      this.Pais = ""

      this.Ciudad = ""
      this.optionsCiudad = []

      this.Barrio = ""

      this.statusActivarDireccion = "Desactivada"

      //reseteamos valores de componentes imput
      this.$refs.resetBarrio.ResetearCampo();
    }
  },
};
</script>

<style>
</style>