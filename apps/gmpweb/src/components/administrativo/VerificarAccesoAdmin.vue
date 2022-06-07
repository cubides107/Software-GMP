<template>
  <div>
    <b-modal
      ref="modal-ingresarConOtroUsuario"
      hide-footer
      title="Ingrese con otro usuario"
    >
      <p class="text-center">
        Verifica por primera vez el usuario administrador del sistema o restaure
        el usuario administrador si coincide con la configuración administrada
        por el sistema.
      </p>

      <form @submit.prevent>
        <div class="mb-3">
          <label for="InputCorreoAdmin" class="form-label">Correo</label>
          <input
            type="email"
            class="form-control"
            id="InputCorreoAdmin"
            aria-describedby="emailHelp"
            v-model="userAdmin.correo"
          />
          <div id="CorreoHelp" class="form-text">
            {{ correoHelp }} <span class="badge badge-pill badge-warning" v-if="correoAdminCorrecta">Campo invalido</span>
          </div>
        </div>

        <div class="mb-3">
          <label for="InputContrasenaAdmin" class="form-label">Contraseña</label>
          <input
            type="password"
            class="form-control"
            id="InputContrasenaAdmin"
            v-model="userAdmin.contrasena"
          />
          <div id="CorreoHelp" class="form-text">
            {{ contrasenaHelp }} <span class="badge badge-pill badge-warning" v-if="contrasenaAdminCorrecta">Campo invalido</span>
          </div>
        </div>

        <b-button class="mt-3" variant="outline-primary" block :disabled="botonCrearOVerificarActivado" @click="CrearOVerificar()">Crear o Restaurar</b-button>
        <b-button class="mt-2" variant="outline-warning" block @click="cerrarModal()">Cancelar</b-button>
      </form>
    </b-modal>

    <div class="m-3">
      <Alert></Alert>
    </div>
  </div>
</template>

<script>
import { verificarAdminHelp } from "@/assets/enums/HelpText.js";
import { mapActions, mapGetters } from "vuex";
import { crearItemLocal } from '@/assets/localStorage/index.js'
import { localStorageKey } from '@/assets/enums/localStorageKey.js'
import { loqueo } from "@/assets/enums/excepcionMensajes.js";
import { GuardarDatosTokenMixin, verificarSihayExcepcionesMixin } from '@/assets/mixins/AccesoUsuarioMixin.js'
import Alert from "@/components/basico/Alert.vue";

export default {
  name: "VerificarAccesoAdmin",

  components:{
    Alert
  },

  //compartidos
  mixins: [GuardarDatosTokenMixin, verificarSihayExcepcionesMixin],

  //modelo
  data() {
    return {
      userAdmin: {
        correo: "",
        contrasena: "",
      },
      bloqueoBotonLoqueo: true,
    };
  },

  computed: {
    ...mapGetters(["GetToken"]),

    correoHelp() {
      return verificarAdminHelp.ingresoCorreo;
    },

    contrasenaHelp() {
      return verificarAdminHelp.ingresoContraseña;
    },

    sessionCaducada() {
      return loqueo.sessionCaducada;
    },
    contrasenaAdminCorrecta(){
      return this.userAdmin.contrasena == "" ? true : false;
    },
    correoAdminCorrecta(){
      return this.userAdmin.correo == "" ? true : false;
    },
    botonCrearOVerificarActivado(){
      if(this.contrasenaAdminCorrecta != false){
        return true;
      }
      if(this.correoAdminCorrecta != false){
        return true;
      }

      return false;
    }
  },

  methods: {
    ...mapActions(["AccessRoot"]),
    
    abrirModal() {
      this.$refs["modal-ingresarConOtroUsuario"].show();
    },

    cerrarModal() {
      this.$refs["modal-ingresarConOtroUsuario"].hide();
    },

    async CrearOVerificar(){
        await this.AccessRoot({correo: this.userAdmin.correo, contrasena: this.userAdmin.contrasena});

        //reseteamos el formulario
        this.userAdmin.correo = "";
        this.userAdmin.contrasena = "";

        //verificamos exepciones si no, no hace lo que sique
        if(this.verificarSihayExcepciones() == false){
          //guardamos en token en el storage
          crearItemLocal(localStorageKey.token, this.GetToken);

          //guardamos datos token
          if(this.guardarDatosToken(this.GetToken) == true){
              this.$emit("irAdministrativa");
          }
        }

        //cerrando el modal
        this.cerrarModal();
    },
    
  },
};
</script>

<style>
</style>