<template>
  <div>
    <div class="row">
      <div class="col-md-12 d-flex justify-content-center">
        <b-form-checkbox-group
          v-model="tipoSolicitud"
          :options="optionsSolicitud"
          class="mb-3"
          value-field="item"
          text-field="name"
          disabled-field="notEnabled"
        ></b-form-checkbox-group>
      </div>

      <div class="col-md-4">
        <ImputCampo
          labelName="Fecha de solicitud"
          imputId="imputFechaSolicitud"
          imputType="date"
          HelpId="helpFechaSolicitud"
          HelpTxt="Ingrese la Fecha de solicitud"
          :campoCorrecto="FechaSolicitudCorrecto"
          @GetValorCampoImput="imputFechaSolicitud"
          ref="resetFechaSolicitud"
        >
        </ImputCampo>
      </div>

      <div class="col-md-4">
        <ImputCampo
          labelName="Fecha inicial solicitud"
          imputId="imputFechaInicialSolicitud"
          imputType="date"
          HelpId="helpFechaInicialSolicitud"
          HelpTxt="Ingrese el rango de la fecha inicial de la solicitud"
          :campoCorrecto="FechaInicialSolicitudCorrecto"
          @GetValorCampoImput="imputFechaInicialSolicitud"
          ref="resetFechaInicialSolicitud"
        >
        </ImputCampo>
      </div>

      <div class="col-md-4">
        <ImputCampo
          labelName="Fecha final solicitud"
          imputId="imputFechaFinalSolicitud"
          imputType="date"
          HelpId="helpimputFechaFinalSolicitud"
          HelpTxt="Ingrese el rango de la fecha inicial de la solicitud"
          :campoCorrecto="FechaFinalSolicitudCorrecto"
          @GetValorCampoImput="imputFechaFinalSolicitud"
          ref="resetFechaFinalSolicitud"
        >
        </ImputCampo>
      </div>

      <div class="col-md-6">
        <label for="imputJornada" class="form-label">Jornada</label>

        <b-form-select
          id="imputJornada"
          v-model="jornada"
          :options="optionsJornada"
        ></b-form-select>

        <div class="form-text" id="HelpId">
          <span class="badge badge-pill badge-warning" v-if="JornadaCorrecta"
            >Campo invalido</span
          >
        </div>
      </div>

      <div class="col-md-6">
        <label for="imputTipoVisita" class="form-label">Tipo visita</label>

        <b-form-select
          id="imputTipoVisita"
          v-model="tipoVisita"
          :options="optionsTipoVisita"
        ></b-form-select>

        <div class="form-text" id="HelpId">
          <span class="badge badge-pill badge-warning" v-if="TipoVisitaCorrecta"
            >Campo invalido</span
          >
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ImputCampo from "../basico/ImputCampo.vue";
export default {
  name: "ImputsCamposSolicitudes",

  components: {
    ImputCampo,
  },

  data() {
    return {
      fecha: "",
      fechaInicial: "",
      fechaFinal: "",

      jornada: "",
      optionsJornada: [
        { value: "", text: "Seleccione una jornada" },
        { value: "0", text: "Mañana" },
        { value: "1", text: "Tarde" },
      ],

      tipoVisita: "",
      optionsTipoVisita: [
        { value: "", text: "Seleccione una tipo de visita" },
        { value: "0", text: "Presencial" },
        { value: "1", text: "virtual" },
      ],

      tipoSolicitud: [],
      optionsSolicitud: [
        { item: 0, name: "estudio de seguridad" },
        { item: 1, name: "visita domiciliaria" },
        { item: 2, name: "verificación académica" },
        { item: 3, name: "verificación laboral" },
        { item: 4, name: "análisis financiero" },
        { item: 5, name: "listas restrictivas" },
        { item: 6, name: "referencia personal" },
        { item: 7, name: "antecedentes" },
      ],
    };
  },

  computed: {
    FechaSolicitudCorrecto() {
      return this.fecha == "" ? true : false;
    },
    FechaInicialSolicitudCorrecto() {
      return this.fechaInicial == "" ? true : false;
    },
    FechaFinalSolicitudCorrecto() {
      return this.fechaFinal == "" ? true : false;
    },
    JornadaCorrecta() {
      return this.jornada == "" ? true : false;
    },
    fechaActual() {
      var f = new Date();
      var fActual = f.toISOString().substring(0, 10); //quitamos la hora
      return fActual;
    },
    TipoVisitaCorrecta() {
      return this.jornada == "" ? true : false;
    },
  },

  methods: {
    imputFechaSolicitud({ newValue }) {
      this.fecha = newValue;
    },
    imputFechaInicialSolicitud({ newValue }) {
      this.fechaInicial = newValue;
    },
    imputFechaFinalSolicitud({ newValue }) {
      this.fechaFinal = newValue;
    },
    GetCamposSolicitud() {
      if (this.fecha == "") {
        return null;
      } else if (this.fechaInicial == "") {
        return null;
      } else if (this.fechaFinal == "") {
        return null;
      } else if (this.jornada == "") {
        return null;
      } else if (this.tipoVisita == "") {
        return null;
      } else if (this.tipoSolicitud == []) {
        return null;
      } else {
        return {
          SolicitationDate: this.fecha,
          StartDate: this.fechaInicial,
          EndDate: this.fechaFinal,
          Journey: parseInt(this.jornada),
          TipoVisitaEnum: parseInt(this.tipoVisita),
          TipoSolicitudEnums:this.tipoSolicitud
        };
      }
    },
    ResetearCampos() {
      //reseteamos los valores de las variables
      this.fecha = "";
      this.fechaInicial = "";
      this.fechaFinal = "";
      this.jornada = "";
      this.tipoVisita = "",
      this.tipoSolicitud = [];

      //ahora reseteamos los valores de los compenentes imput
      this.$refs.resetFechaSolicitud.ResetearCampo();
      this.$refs.resetFechaInicialSolicitud.ResetearCampo();
      this.$refs.resetFechaFinalSolicitud.ResetearCampo();
    },
  },
};
</script>

<style>
</style>