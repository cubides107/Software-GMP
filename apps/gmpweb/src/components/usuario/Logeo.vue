<template>
  <div class="mt-3">
    <Alert></Alert>

    <form @submit.prevent>
      <div class="mb-3">
        <label for="InputCorreo" class="form-label">Correo</label>
        <input
          type="email"
          class="form-control"
          id="InputCorreo"
          aria-describedby="emailHelp"
          v-model="user.correo"
        />
        <div id="CorreoHelp" class="form-text">
          {{ correoHelp }} <span class="badge badge-pill badge-warning" v-if="correoCorrecta">Campo invalido</span>
        </div>
      </div>

      <div class="mb-3">
        <label for="InputContrasena" class="form-label">Contraseña</label>
        <input
          type="password"
          class="form-control"
          id="InputContrasena"
          v-model="user.contrasena"
        />
        <div id="CorreoHelp" class="form-text">
          {{ contrasenaHelp }} <span class="badge badge-pill badge-warning" v-if="contrasenaCorrecta">Campo invalido</span>
        </div>
      </div>

      <div class="row">
        <div  class="col-6">
          <button type="submit" class="btn btn-primary" :disabled="botonLoqueadoActivado" @click="Loguear">Ingresar</button>
        </div>
        <div  class="col-6">

          <div class="float-right">
            <a type="button" @click="verificarAdministrativo()" class="badge badge-secondary">
              <b-icon icon="person-badge"></b-icon> verificar cuenta administrativa
            </a>
          </div>
          
        </div>
      </div>
    </form>
  </div>
</template>

<script>
import { logeoHelp } from "@/assets/enums/HelpText.js";
import { loqueo } from "@/assets/enums/excepcionMensajes.js";
import { mapActions, mapGetters } from "vuex";
import Alert from "@/components/basico/Alert.vue";

import { existeItemLocal, obtenerItemLocal, crearItemLocal } from '@/assets/localStorage/index.js'
import { localStorageKey } from '@/assets/enums/localStorageKey.js'
import { GuardarDatosTokenMixin, verificarSihayExcepcionesMixin } from '@/assets/mixins/AccesoUsuarioMixin.js'

export default {
  name: "Logeo",

  //compartidos
  mixins: [GuardarDatosTokenMixin, verificarSihayExcepcionesMixin],

  //componentes externos
  components: {
    Alert,
  },

  //modelo
  data() {
    return {
      user: {
        correo: "",
        contrasena: "",
      },
      bloqueoBotonLoqueo: true,
    };
  },

  //antes de renderizar
  mounted() {
    //guardamos datos del token en el estado de la aplicacion y nos vamos al home
    if (existeItemLocal(localStorageKey.token) == true) {
      const token = obtenerItemLocal(localStorageKey.token);
      
      if (this.guardarDatosToken(token) == true) {
        this.$emit("irAHome");
      }
    }
  },

  //propiedades computadas
  computed: {
    ...mapGetters(["GetToken"]),

    correoHelp() {
      return logeoHelp.ingresoCorreo;
    },
    contrasenaHelp() {
      return logeoHelp.ingresoContraseña;
    },
    sessionCaducada() {
      return loqueo.sessionCaducada;
    },
    contrasenaCorrecta(){
      return this.user.contrasena == "" ? true : false;
    },
    correoCorrecta(){
      return this.user.correo == "" ? true : false;
    },
    botonLoqueadoActivado(){
      var camposCorrectos = [];
      camposCorrectos.push(this.contrasenaCorrecta, this.correoCorrecta)

      return camposCorrectos.includes(true)
    }
  },

  //metodos
  methods: {
    ...mapActions(["LoginUserAction"]),

    async Loguear() {
      //pasamos los datos al action
      await this.LoginUserAction({
        correo: this.user.correo,
        contrasena: this.user.contrasena,
      });

      //reseteamos el formulario
      this.user.correo = "";
      this.user.contrasena = "";

      //verificar exepciones
      if (this.verificarSihayExcepciones() == false) {
        //guardamos en token en el storage
        crearItemLocal(localStorageKey.token, this.GetToken);

        //guardamos datos del token en el estado de la aplicacion
        if (this.guardarDatosToken(this.GetToken) == true) {
          //por ultimo, nos dirigimos al home
          this.$emit("irAHome");
        }
      }
    },

    verificarAdministrativo(){
      this.$emit("verificarAdministrativo")
    },
  },
};
</script>

<style>
</style>