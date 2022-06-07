<template>
  <div>
    <CardAdmin
      tipo="secondary"
      :titulo="consultarUsuarioTitulo"
      :descripcion="consultarUsuariosDescripcion"
      @ejecutarOpcionAdmin="abrirModal()"
    ></CardAdmin>

    <b-modal
      size="xl"
      ref="modal-consultarUsuarios"
      hide-footer
      title="Consultar usuarios"
    >
      <!-- formulario de imput de busqueda -->
      <form @submit.prevent>
        <div class="d-flex bd-highlight mb-3">
          <!-- imput buscar por nombre -->
          <div class="p-2 bd-highlight">
            <label for="InputBuscar" class="form-label my-2"
              ><b-icon icon="search" animation="fade"></b-icon
            ></label>
          </div>
          <div class="p-2 flex-grow-1 bd-highlight">
            <input
              type="text"
              class="form-control"
              id="InputBuscar"
              placeholder="Ingrese un nombre de usuario existente"
              v-model="nombreUsuario"
            />
          </div>

          <!-- imput cantidad de registros -->
          <div class="p-2 bd-highlight">
            <label for="InputTamanoPagina" class="form-label my-2"
              ><b-icon
                icon="receipt-cutoff"
                v-b-popover.hover="
                  'Cantidad de registros en pagina, donde, maximos registros es 30 y minimo 0'
                "
              ></b-icon
            ></label>
          </div>
          <div class="p-2 bd-highlight">
            <input
              type="number"
              min="1"
              max="15"
              class="form-control"
              id="InputTamanoPagina"
              v-model="tamanoPagina"
            />
          </div>
        </div>
      </form>

      <!-- tabla de lista de usuarios -->
      <b-table
        responsive
        striped
        hover
        :items="usuarios"
        :fields="camposTablaUsuarios"
      ></b-table>

      <!-- paginacion -->
      <b-pagination
        v-model="paginaActual"
        :total-rows="totalRegistros"
        :per-page="tamanoPagina"
        first-text="Primero"
        prev-text="Anterior"
        next-text="Siquinte"
        last-text="Ultimo"
        align="center"
        size="sm"
      ></b-pagination>

      <!-- boton cerrar modal -->
      <b-button
        class="mt-3"
        variant="outline-warning"
        block
        @click="cerrarModal()"
        >cerrar</b-button
      >
    </b-modal>

    <!-- alerta -->
    <div class="my-2">
      <Alert></Alert>
    </div>
  </div>
</template>

<script>
import CardAdmin from "./CardAdmin.vue";
import {
  DescripcionesOpcionesAdminHelp,
  TituloOpcionesAdminHelp,
} from "@/assets/enums/HelpText.js";
import { mapActions } from "vuex";
import { verificarSihayExcepcionesMixin } from '@/assets/mixins/AccesoUsuarioMixin.js'
import Alert from "../basico/Alert.vue";

export default {
  name: "ConsultarUsuarios",

  //compartidos
  mixins: [verificarSihayExcepcionesMixin],

  data() {
    return {
      nombreUsuario: "", //nombre del usuario por el cual se va a buscar
      totalRegistros: 5, //numero total de registros que existen en la db
      tamanoPagina: 5, //numero de registros que tiene la pagina seleccionada
      paginaActual: 1, //numemo que indica que pagina se selecciono
      usuarios: [], //listado de usuarios
      camposTablaUsuarios: [
        { key: "name", label: "Nombre" },
        { key: "lastname", label: "Apellido" },
        { key: "mail", label: "Correo" },
        { key: "phone", label: "Teefono" },
        { key: "address", label: "Direccion" },
      ],
    };
  },

  components: {
    CardAdmin,
    Alert
  },

  computed: {
    consultarUsuariosDescripcion() {
      return DescripcionesOpcionesAdminHelp.consultarUsuariosDescripcion;
    },
    consultarUsuarioTitulo() {
      return TituloOpcionesAdminHelp.consultarUsuariosTitulo;
    },
  },

  watch: {
    async nombreUsuario(newnombreUsuario) {
      await this.consultarUsuarios(newnombreUsuario, this.paginaActual, this.tamanoPagina
      );
    },
    async paginaActual(newPaginaActual) {
      await this.consultarUsuarios(this.nombreUsuario, newPaginaActual, this.tamanoPagina);
    },
    async tamanoPagina(newtamanoPagina) {
      if (newtamanoPagina != "" && newtamanoPagina <= 15 && newtamanoPagina > 1) {
        await this.consultarUsuarios(this.nombreUsuario, this.paginaActual, newtamanoPagina
        );
      } 
      else {
        this.tamanoPagina = 1;
        await this.consultarUsuarios(this.nombreUsuario, this.paginaActual, this.tamanoPagina);
      }
    },
  },

  methods: {
    ...mapActions(["QueryUsersAction"]),

    abrirModal() {
      this.nombreUsuario = "", //nombre del usuario por el cual se va a buscar
      this.totalRegistros = 5, //numero total de registros que existen en la db
      this.tamanoPagina = 5, //numero de registros que tiene la pagina seleccionada
      this.paginaActual = 1, //numemo que indica que pagina se selecciono
      this.usuarios = [], //listado de usuarios
      this.$refs["modal-consultarUsuarios"].show();
    },
    cerrarModal() {
      this.$refs["modal-consultarUsuarios"].hide();
    },
    async consultarUsuarios(nombreUsuario, paginaActual, tamanoPagina) {
      //verificar datos imputs
      if (nombreUsuario != "" && nombreUsuario.length >= 4) {
        //obtener datos de busqueda
        var consultaUsuarios = {
          filterName: nombreUsuario,
          page: paginaActual - 1,
          pageSize: tamanoPagina,
        };

        var query = await this.QueryUsersAction(consultaUsuarios);

        //lista de usuarios
        this.usuarios = query.listUsers;

        //total registros segun la coincidencia de nombres
        this.totalRegistros = query.countUsers;

        //si existen excepciones cerrar modal
        if(this.verificarSihayExcepciones() == true){
          this.cerrarModal();
        }
      }
    },
  },
};
</script>

<style>
</style>