<template>
  <div>
    <CardAdmin
      tipo="secondary"
      :titulo="creacionUsuarioExternoTitulo"
      :descripcion="creacionUsuarioExternoDescripcion"
      @ejecutarOpcionAdmin="abrirModal()"
    ></CardAdmin>

    <b-modal
      size="xl"
      ref="modal-crearExterno"
      hide-footer
      title="Crear usuario externo"
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
              @click="CrearExterno()"
              :disabled="botonCrearInternoBloqueado"
              >Crear usuario externo
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
import {
  DescripcionesOpcionesAdminHelp,
  TituloOpcionesAdminHelp,
} from "@/assets/enums/HelpText.js";
import CardAdmin from "./CardAdmin.vue";
import ImputCampo from "../basico/ImputCampo.vue";
import { mapActions, mapGetters } from "vuex";
import { verificarSihayExcepcionesMixin } from "@/assets/mixins/AccesoUsuarioMixin.js";
import Alert from "../basico/Alert.vue";

export default {
  name: "CreacionUsuarioExterno",

  data() {
    return {
      userExterno: {
        nombre: "",
        apellido: "",
        telefono: "",
        correo: "",
        contrasena: "",
        direccion: "",
      },
    };
  },

  //compartidos
  mixins: [verificarSihayExcepcionesMixin],

  components: {
    CardAdmin,
    ImputCampo,
    Alert,
  },

  computed: {
    ...mapGetters(["GetExcepcion", "GetSession"]),

    creacionUsuarioExternoDescripcion() {
      return DescripcionesOpcionesAdminHelp.creacionUsuarioExternoDescripcion;
    },
    creacionUsuarioExternoTitulo() {
      return TituloOpcionesAdminHelp.creacionUsuarioExternoTitulo;
    },
    nombreCorrecto() {
      return this.userExterno.nombre == "" ? true : false;
    },
    apellidoCorrecto() {
      return this.userExterno.apellido == "" ? true : false;
    },
    telefonoCorrecto() {
      return this.userExterno.telefono == "" ? true : false;
    },
    correoCorrecto() {
      return this.userExterno.correo == "" ? true : false;
    },
    contrasenaCorrecto() {
      return this.userExterno.contrasena == "" ? true : false;
    },
    direccionCorrecto() {
      return this.userExterno.direccion == "" ? true : false;
    },
    botonCrearInternoBloqueado() {
      var camposCorrectos = [];

      camposCorrectos.push(
        this.nombreCorrecto,
        this.apellidoCorrecto,
        this.telefonoCorrecto,
        this.correoCorrecto,
        this.contrasenaCorrecto,
        this.direccionCorrecto
      );

      return camposCorrectos.includes(true);
    },
  },

  methods: {
    ...mapActions([
      "RootRegisterExternalAction",
      "InternalRegisterExternalAction",
    ]),

    abrirModal() {
      //cada vez que abrimos el modal limpiamos campos  
      this.userInterno = {
        nombre: "",
        apellido: "",
        telefono: "",
        correo: "",
        contrasena: "",
        direccion: "",
      };

      //abrir
      this.$refs["modal-crearExterno"].show();
    },
    cerrarModal() {
      this.$refs["modal-crearExterno"].hide();
    },
    imputNombre({ newValue }) {
      this.userExterno.nombre = newValue;
    },
    imputApellido({ newValue }) {
      this.userExterno.apellido = newValue;
    },
    imputTelefono({ newValue }) {
      this.userExterno.telefono = newValue;
    },
    imputCorreo({ newValue }) {
      this.userExterno.correo = newValue;
    },
    imputContrasena({ newValue }) {
      this.userExterno.contrasena = newValue;
    },
    imputDireccion({ newValue }) {
      this.userExterno.direccion = newValue;
    },
    async CrearExterno() {
      var token = null;

      //registramos el usuario segun el rol quien va a ejecutar la accion
      if (this.GetSession.role == "root") {
        token = await this.RootRegisterExternalAction(this.userExterno);

      } else if (this.GetSession.role == "interno") {
        token = await this.InternalRegisterExternalAction(this.userExterno);
      }

      //verificamos exepciones
      if (this.verificarSihayExcepciones() == false) {
        //emitimos la alerta correctamente
        this.$emit("showAlert", "El usuario " + token.name + " fue creado correctamente con el correo " + token.mail,"success");
      }

      //cerramos el modal
      this.cerrarModal();
    },
  },
};
</script>

<style>
</style>