<template>
  <div>
    <div class="row">
      <div class="col-md-12 d-flex justify-content-center">
        <b-form-checkbox
          id="checkboxCriticidad"
          v-model="statusTrabajoCritico"
          name="checkboxCriticidad"
          :value="true"
          :unchecked-value="false"
        >
          El trabajo que realiza es critico
        </b-form-checkbox>
      </div>

      <div class="col-lg-4 col-md-4">
        <ImputCampo
          labelName="Nombre"
          imputId="imputNombre"
          imputType="text"
          HelpId="helpNombre"
          HelpTxt="Ingrese el nombre del empleado"
          :campoCorrecto="NombreCorrecto"
          @GetValorCampoImput="imputNombre"
          ref="resetNombre"
        >
        </ImputCampo>
      </div>

      <div class="col-lg-4 col-md-4">
        <ImputCampo
          labelName="Apellido"
          imputId="imputApellido"
          imputType="text"
          HelpId="helpApellido"
          HelpTxt="Ingrese el apellido del empleado"
          :campoCorrecto="ApellidoCorrecto"
          @GetValorCampoImput="imputApellido"
          ref="resetApellido"
        >
        </ImputCampo>
      </div>

      <div class="col-lg-4 col-md-4">
        <label for="imputTipoDocumento" class="form-label"
          >tipo de documento</label
        >

        <b-form-select
          id="imputTipoDocumento"
          v-model="tipoDocumento"
          :options="optionsTipoDocumento"
        ></b-form-select>

        <div class="form-text" id="HelpId">
          <span
            class="badge badge-pill badge-warning"
            v-if="TipoDocumentoCorrecta"
            >Campo invalido</span
          >
        </div>
      </div>

      <div class="col-lg-4 col-md-4">
        <ImputCampo
          labelName="NumeroDocumento"
          imputId="imputNumeroDocumento"
          imputType="text"
          HelpId="helpNumeroDocumento"
          HelpTxt="Ingrese el Numero Documento del empleado"
          :campoCorrecto="NumeroDocumentoCorrecto"
          @GetValorCampoImput="imputNumeroDocumento"
          ref="resetNumeroDocumento"
        >
        </ImputCampo>
      </div>

      <div class="col-lg-4 col-md-4">
        <ImputCampo
          labelName="Telefono"
          imputId="imputTelefono"
          imputType="text"
          HelpId="helpTelefono"
          HelpTxt="Ingrese el telefono empleado"
          :campoCorrecto="TelefonoCorrecto"
          @GetValorCampoImput="imputTelefono"
          ref="resetTelefono"
        >
        </ImputCampo>
      </div>

      <div class="col-lg-4 col-md-4">
        <ImputCampo
          labelName="Correo"
          imputId="imputCorreo"
          imputType="mail"
          HelpId="helpCorreo"
          HelpTxt="Ingrese la Correo empleado"
          :campoCorrecto="false"
          :campoOpcional="true"
          @GetValorCampoImput="imputCorreo"
          ref="resetCorreo"
        >
        </ImputCampo>
      </div>

      <div class="col-lg-3 col-md-4">
        <ImputCampo
          labelName="LandLine"
          imputId="imputLandLine"
          imputType="mail"
          HelpId="helpLandLine"
          HelpTxt="Ingrese la Land Line empleado"
          :campoCorrecto="false"
          :campoOpcional="true"
          @GetValorCampoImput="imputLandLine"
          ref="resetLandLine"
        >
        </ImputCampo>
      </div>

       <div class="col-lg-3 col-md-4">
        <ImputCampo
          labelName="Cargo"
          imputId="imputEspecialidad"
          imputType="text"
          HelpId="helpEspecialidad"
          HelpTxt="Ingrese el cargo del empleado"
          :campoCorrecto="false"
          :campoOpcional="true"
          @GetValorCampoImput="imputEspecialidad"
          ref="resetEspecialidad"
        >
        </ImputCampo>
      </div>

      <div class="col-lg-3 col-md-4">
        <ImputCampo
          labelName="Telefono Familiar"
          imputId="imputTelefonoFamiliar"
          imputType="text"
          HelpId="helpTelefonoFamiliar"
          HelpTxt="Ingrese el telefono familiar"
          :campoCorrecto="false"
          :campoOpcional="true"
          @GetValorCampoImput="imputTelefonoFamiliar"
          ref="resetTelefonoFamiliar"
        >
        </ImputCampo>
      </div>

      <div class="col-lg-3 col-md-4">
        <ImputCampo
          labelName="Compañía"
          imputId="imputCompania"
          imputType="text"
          HelpId="helpCompania"
          HelpTxt="Ingrese la area de la compañía"
          :campoCorrecto="false"
          :campoOpcional="true"
          @GetValorCampoImput="imputCompania"
          ref="resetCompania"
        >
        </ImputCampo>
      </div>
    </div>
  </div>
</template>

<script>
import ImputCampo from "../basico/ImputCampo.vue";

export default {
  name: "ImputsCamposEmpleado",

  components: {
    ImputCampo,
  },

  data() {
    return {
      nombre: "",
      apellido: "",
      tipoDocumento: "",
      numeroDocumento: "",
      especialidad: "",
      correo: "",
      landLine: "",
      telefonoFamiliar: "",
      telefono: "",
      optionsTipoDocumento: [
        { value: "", text: "Seleccione un tipo de documento" },
        { value: "0", text: "CC" },
        { value: "1", text: "TT" },
        { value: "2", text: "Pasaporte" },
      ],
      statusTrabajoCritico: false,
      compania: ""
    };
  },

  computed: {
    NombreCorrecto() {
      return this.nombre == "" ? true : false;
    },
    ApellidoCorrecto() {
      return this.apellido == "" ? true : false;
    },
    TipoDocumentoCorrecta() {
      return this.tipoDocumento == "" ? true : false;
    },
    NumeroDocumentoCorrecto() {
      return this.numeroDocumento == "" ? true : false;
    },
    TelefonoCorrecto() {
      return this.telefono == "" ? true : false;
    },
  },

  methods: {
    imputNombre({ newValue }) {
      this.nombre = newValue;
    },
    imputApellido({ newValue }) {
      this.apellido = newValue;
    },
    imputTipoDocumento({ newValue }) {
      this.tipoDocumento = newValue;
    },
    imputNumeroDocumento({ newValue }) {
      this.numeroDocumento = newValue;
    },
    imputEspecialidad({ newValue }) {
      this.Especialidad = newValue;
    },
    imputCorreo({ newValue }) {
      this.correo = newValue;
    },
    imputTelefonoFamiliar({ newValue }) {
      this.telefonoFamiliar = newValue;
    },
    imputLandLine({ newValue }) {
      this.LandLine = newValue;
    },
    imputTelefono({ newValue }) {
      this.telefono = newValue;
    },
    imputCompania({ newValue }){
      this.compania = newValue;
    },
    GetCamposEmpleado() {
      if (this.nombre == "") {
        return null;
      } else if (this.apellido == "") {
        return null;
      } else if (this.tipoDocumento == "") {
        return null;
      } else if (this.numeroDocumento == "") {
        return null;
      } else if (this.telefono == "") {
        return null;
      } else {
        return {
          Name: this.nombre,
          LastName: this.apellido,
          DocumentType: parseInt(this.tipoDocumento),
          DocumentNumber: this.numeroDocumento,
          Specialty: this.especialidad,
          Post: this.correo,
          FamilyCellPhone: this.telefonoFamiliar,
          LandLine: this.landLine,
          Phone: this.telefono,
          Company: this.compania,
          IsCritical: this.statusTrabajoCritico
        };
      }
    },
    ResetearCampos() {
      //reseteamos variables
      this.nombre = "";
      this.apellido = "";
      this.tipoDocumento = "";
      this.numeroDocumento = "";
      this.especialidad = "";
      this.correo = "";
      this.landLine = "";
      this.telefonoFamiliar = "";
      this.telefono = "";
      this.compania = "";
      this.statusTrabajoCritico = false;

      //reseteamos valores de componentes imput
      this.$refs.resetNombre.ResetearCampo();
      this.$refs.resetApellido.ResetearCampo();
      this.$refs.resetNumeroDocumento.ResetearCampo();
      this.$refs.resetEspecialidad.ResetearCampo();
      this.$refs.resetCorreo.ResetearCampo();
      this.$refs.resetLandLine.ResetearCampo();
      this.$refs.resetTelefono.ResetearCampo();
      this.$refs.resetTelefonoFamiliar.ResetearCampo();
      this.$refs.resetCompania.ResetearCampo();
    },
  },
};
</script>

<style>
</style>