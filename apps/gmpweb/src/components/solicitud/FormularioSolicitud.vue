<template>
  <div class="card m-3">
    <h6 class="card-subtitle mb-2 text-muted m-1">
      Formulario de autorizacion de solicitud
    </h6>

    <form class="m-3" enctype="multipart/form-data">
      <div class="my-2">
        <b-button
          :class="visibleSolicitud ? null : 'collapsed'"
          :aria-expanded="visibleSolicitud ? 'true' : 'false'"
          aria-controls="collapse-4"
          @click="visibleSolicitud = !visibleSolicitud"
          block
        >
          Complete la información de la solicitud
          <b-badge variant="warning">Todos los campos son obligatorios</b-badge>
        </b-button>
        <b-collapse id="collapse-4" v-model="visibleSolicitud" class="mt-2">
          <b-card>
            <ImputsCamposSolicitudes
              ref="ImputsCamposSolicitudes"
            ></ImputsCamposSolicitudes>
          </b-card>
        </b-collapse>
      </div>

      <div class="my-2">
        <b-button
          :class="visibleEmpleado ? null : 'collapsed'"
          :aria-expanded="visibleEmpleado ? 'true' : 'false'"
          aria-controls="collapse-4"
          @click="visibleEmpleado = !visibleEmpleado"
          block
        >
          Complete la información del empleado
          <b-badge variant="warning">Algunos campos obligatorios</b-badge>
        </b-button>
        <b-collapse id="collapse-4" v-model="visibleEmpleado" class="mt-2">
          <b-card>
            <ImputsCamposEmpleado
              ref="ImputsCamposEmpleado"
            ></ImputsCamposEmpleado>
          </b-card>
        </b-collapse>
      </div>

      <div class="my-2">
        <b-button
          :class="visibleDireccion ? null : 'collapsed'"
          :aria-expanded="visibleDireccion ? 'true' : 'false'"
          aria-controls="collapse-4"
          @click="visibleDireccion = !visibleDireccion"
          block
        >
          Complete la información de la direccion
          <b-badge variant="info"
            >No es obligatorio que complete esta parte, pero, en el caso de
            activarlo debe completar todos los campos.</b-badge
          >
        </b-button>

        <b-collapse id="collapse-4" v-model="visibleDireccion" class="mt-2">
          <b-card>
            <ImputsCamposDireccion
              ref="ImputsCamposDireccion"
            ></ImputsCamposDireccion>
          </b-card>
        </b-collapse>
      </div>

      <div class="my-2">
        <b-button
          :class="visibleMapa ? null : 'collapsed'"
          :aria-expanded="visibleMapa ? 'true' : 'false'"
          aria-controls="collapse-4"
          @click="visibleMapa = !visibleMapa"
          block
        >
          Complete la información de la ruta
          <b-badge variant="info"
            >No es obligatorio que complete esta parte, pero, en el caso de
            activarlo debe completar todos los campos.</b-badge
          >
        </b-button>

        <b-collapse id="collapse-4" v-model="visibleMapa" class="mt-2">
          <b-card>
            <ImputsCamposMapa ref="ImputsCamposMapa"></ImputsCamposMapa>
          </b-card>
        </b-collapse>
      </div>

      <div class="my-3">
        <AceptaPoliticaDatos
          @AceptaPoliticasDeTratamientoDeDatos="AceptarPolitica"
          ref="ImputsAceptaPoliticaDatos"
        >
        </AceptaPoliticaDatos>
      </div>

      <div class="m-2">
        <Alert></Alert>
      </div>

      <div class="row">
        <div class="col-12">
          <b-button
            class="mt-3"
            variant="outline-primary"
            block
            @click="CrearSolicitud()"
            :disabled="desactivarBotonCrearSolicitud"
            >Crear Solicitud
          </b-button>
        </div>
      </div>
    </form>
  </div>
</template>

<script>
import ImputsCamposSolicitudes from "../solicitud/ImputsCamposSolicitudes.vue";
import Alert from "../basico/Alert.vue";
import ImputsCamposEmpleado from "../solicitud/ImputsCamposEmpleado.vue";
import ImputsCamposDireccion from "../solicitud/ImputsCamposDireccion.vue";
import ImputsCamposMapa from "../solicitud/ImputsCamposMapa.vue";
import { mapActions, mapGetters } from "vuex";
import { verificarSihayExcepcionesMixin } from "@/assets/mixins/AccesoUsuarioMixin.js";
import AceptaPoliticaDatos from "./AceptaPoliticaDatos.vue"

export default {
  name: "FormularioSolicitud",

  //compartidos
  mixins: [verificarSihayExcepcionesMixin],

  components: {
    ImputsCamposSolicitudes,
    Alert,
    ImputsCamposEmpleado,
    ImputsCamposDireccion,
    ImputsCamposMapa,
    AceptaPoliticaDatos
  },

  data() {
    return {
      visibleSolicitud: true,
      visibleEmpleado: false,
      visibleDireccion: false,
      visibleMapa: false,
      desactivarBotonCrearSolicitud: true,
    };
  },

  computed: {
    ...mapGetters(["GetExcepcion"]),
  },

  methods: {
    ...mapActions(["RegisterStudyResearch"]),

    async CrearSolicitud() {
      //obtener valores
      var camposSolicitud = this.$refs.ImputsCamposSolicitudes.GetCamposSolicitud();
      var camposEmpleado = this.$refs.ImputsCamposEmpleado.GetCamposEmpleado();
      var camposDireccion = this.$refs.ImputsCamposDireccion.GetCamposDireccion();
      var camposMapa = this.$refs.ImputsCamposMapa.GetCamposMapa();
      var formData = new FormData();

      //verificar
      if (camposSolicitud == null) {
        this.$emit(
          "showAlert",
          "Recuerde que los campos de solicitud básicos son obligatorios ",
          "warning"
        );
      } else if (camposEmpleado == null) {
        this.$emit(
          "showAlert",
          "Recuerde que algunos campos del empleado son obligatorios ",
          "warning"
        );
      } else if (camposDireccion.stado == 1) {
        this.$emit(
          "showAlert",
          "Recuerde que si activa la parte de direccion del formulario debe completarlo en su totalidad. ",
          "warning"
        );
      } else if (camposMapa.stado == 1) {
        this.$emit(
          "showAlert",
          "Recuerde que si activa la parte de mapa del formulario debe completarlo en su totalidad. ",
          "warning"
        );
      }

      //trasformar imagen y los demas campos a form data (multipart/form-data)
      var solicitudJSON = JSON.stringify(camposSolicitud);
      formData.append("solicitation", solicitudJSON);

      var empleadoJSON = JSON.stringify(camposEmpleado);
      formData.append("employee", empleadoJSON);

      if (camposMapa.stado == 2) {
        formData.append("file", camposMapa.File);
      }
      if(camposDireccion.stado == 2){
        var direccionJSON = JSON.stringify(camposDireccion.objeto);
        formData.append("address", direccionJSON);
      }

      //envir datos
      var data = await this.RegisterStudyResearch(formData);

      //verificamos exepciones
      if (this.verificarSihayExcepciones() == false) {
        //emitimos la alerta correctamente
        this.$emit(
          "showAlert",
          "La solicitud se a creado correctamente en los siquentes rangos de fechas. " +
            "Inicio: " +
            data.solicitationStartDate +
            " Fin: " +
            data.solicitationEndDate,
          "success"
        );
      }

      //resetar campos formulario
      this.$refs.ImputsCamposSolicitudes.ResetearCampos();
      this.$refs.ImputsCamposEmpleado.ResetearCampos();
      this.$refs.ImputsCamposDireccion.ResetearCampos();
      this.$refs.ImputsCamposMapa.ResetearCampos();
      this.$refs.ImputsAceptaPoliticaDatos.ResetearCampos();

      //por ultimo, cerramos las secciones del formulario
      this.visibleSolicitud= false;
      this.visibleEmpleado= false;
      this.visibleDireccion= false;
      this.visibleMapa= false;

    },

    AceptarPolitica(data){
      this.desactivarBotonCrearSolicitud = !data
    }
  },
};
</script>

<style>
</style>