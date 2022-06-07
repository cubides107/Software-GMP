<template>
  <div>
    <CardAdmin
      tipo="warning"
      :titulo="creacionUsuarioInternoTitulo"
      :descripcion="creacionUsuarioInternoDescripcion"
      @ejecutarOpcionAdmin="abrirModal()"
    ></CardAdmin>

    <b-modal
      size="xl"
      ref="modal-crearInterno"
      hide-footer
      title="Cree un nuevo usuario root"
    >
      <form class="m-1">
        <div class="row">
          <div class="col-md-6">
            <ImputCampo
              labelName="Nombre"
              imputId="imputNombre"
              imputType="text"
              HelpId="helpNombre"
              HelpTxt="Ingrese el nombre"
              :campoCorrecto="nombreCorrecto"
              @GetValorCampoImput="imputNombre"
            >
            </ImputCampo>
          </div>

          <div class="col-md-6">
            <ImputCampo
              labelName="Apellido"
              imputId="imputApellido"
              imputType="text"
              HelpId="helpApellido"
              HelpTxt="Ingrese el Apellido"
              :campoCorrecto="apellidoCorrecto"
              @GetValorCampoImput="imputApellido"
            >
            </ImputCampo>
          </div>

          <div class="col-md-6">
            <ImputCampo
              labelName="Telefono"
              imputId="imputTelefono"
              imputType="number"
              HelpId="helpTelefono"
              HelpTxt="Ingrese un telefono"
              :campoCorrecto="telefonoCorrecto"
              @GetValorCampoImput="imputTelefono"
            >
            </ImputCampo>
          </div>

          <div class="col-md-6">
            <ImputCampo
              labelName="Correo"
              imputId="imputCorreo"
              imputType="email"
              HelpId="helpCorreo"
              HelpTxt="Ingrese un Correo"
              :campoCorrecto="correoCorrecto"
              @GetValorCampoImput="imputCorreo"
            >
            </ImputCampo>
          </div>

          <div class="col-md-6">
            <ImputCampo
              labelName="Contrasena"
              imputId="imputContrasena"
              imputType="password"
              HelpId="helpContrasena"
              HelpTxt="Ingrese un Contrasena"
              :campoCorrecto="contrasenaCorrecto"
              @GetValorCampoImput="imputContrasena"
            >
            </ImputCampo>
          </div>

          <div class="col-md-6">
            <ImputCampo
              labelName="Dirección"
              imputId="imputDireccion"
              imputType="email"
              HelpId="helpDireccion"
              HelpTxt="Ingrese un Dirección"
              :campoCorrecto="direccionCorrecto"
              @GetValorCampoImput="imputDireccion"
            >
            </ImputCampo>
          </div>
        </div>

        <div class="row">
          <div class="col-6">
            <b-button
              class="mt-3"
              variant="outline-primary"
              block
              @click="CrearInterno()"
              :disabled="botonCrearInternoBloqueado"
              >Crear usuario interno
            </b-button>
          </div>
          <div class="col-6">
            <b-button
              class="mt-3"
              variant="outline-warning"
              block
              @click="cerrarModal()"
              >Cancelar
            </b-button>
          </div>
        </div>
      </form>
    </b-modal>

    <div class="my-2">
      <Alert></Alert>
    </div>
  </div>
</template>

<script>
import CardAdmin from "../administrativo/CardAdmin.vue";
import ImputCampo from "../basico/ImputCampo.vue";
import { DescripcionesOpcionesAdminHelp, TituloOpcionesAdminHelp } from "@/assets/enums/HelpText.js";
import { mapActions, mapGetters } from "vuex";
import { verificarSihayExcepcionesMixin } from '@/assets/mixins/AccesoUsuarioMixin.js'
import Alert from "../basico/Alert.vue"
//import { descifrarToken } from "@/assets/JWT/index.js"

export default {
  name: "CreacionUsuarioInterno",

  data() {
    return {
        userInterno:{
          nombre: "",
          apellido: "",
          telefono: "",
          correo: "",
          contrasena: "",
          direccion: "",
      }
    };
  },

  //compartidos
  mixins: [verificarSihayExcepcionesMixin],

  components: {
    CardAdmin,
    ImputCampo,
    Alert
  },

  computed: {
    ...mapGetters(["GetExcepcion", "GetSession"]),

    creacionUsuarioInternoDescripcion() {
      return DescripcionesOpcionesAdminHelp.creacionUsuarioInternoDescripcion;
    },
    creacionUsuarioInternoTitulo() {
      return TituloOpcionesAdminHelp.creacionUsuarioInternoTitulo;
    },
    nombreCorrecto(){
      return (this.userInterno.nombre == "") ? true : false;
    },
    apellidoCorrecto(){
      return (this.userInterno.apellido == "") ? true : false;
    },
    telefonoCorrecto(){
      return (this.userInterno.telefono == "") ? true : false;
    },
    correoCorrecto(){
      return (this.userInterno.correo == "") ? true : false;
    },
    contrasenaCorrecto(){
      return (this.userInterno.contrasena == "") ? true : false;
    },
    direccionCorrecto(){
      return (this.userInterno.direccion == "") ? true : false;
    },
    botonCrearInternoBloqueado(){
      var camposCorrectos = [];

      camposCorrectos.push(
        this.nombreCorrecto, 
        this.apellidoCorrecto, 
        this.telefonoCorrecto,
        this.correoCorrecto,
        this.contrasenaCorrecto,
        this.direccionCorrecto)

        return camposCorrectos.includes(true)
    }
  },

  methods: {
    ...mapActions(["RegisterInternalAction"]),

    abrirModal() {
      this.userInterno = {
          nombre: "",
          apellido: "",
          telefono: "",
          correo: "",
          contrasena: "",
          direccion: "",
      }
      if(this.GetSession.role === "root"){
        this.$refs["modal-crearInterno"].show();
      }else{
        this.$emit("showAlert",  "No tiene permisos", "danger");
      }
      
    },
    cerrarModal() {
      this.$refs["modal-crearInterno"].hide();
    },
    imputNombre({ newValue }) {
      this.userInterno.nombre = newValue;
    },
    imputApellido({ newValue }) {
      this.userInterno.apellido = newValue;
    },
    imputTelefono({ newValue }) {
      this.userInterno.telefono = newValue;
    },
    imputCorreo({ newValue }) {
      this.userInterno.correo = newValue;
    },
    imputContrasena({ newValue }) {
      this.userInterno.contrasena = newValue;
    },
    imputDireccion({ newValue }) {
      this.userInterno.direccion = newValue;
    },
    async CrearInterno() {
      var token = await this.RegisterInternalAction(this.userInterno);

      if(this.verificarSihayExcepciones() == false){
        //emitimos la alerta correctamente
        this.$emit("showAlert",  "El usuario " + token.name + " fue creado correctamente con el correo " + token.mail, "success");
      }

      //cerramos el modal
      this.cerrarModal();
    },
  },
};
</script>

<style>
</style>