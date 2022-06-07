import { descifrarToken } from "@/assets/JWT/index.js";
import { eliminarItemLocal } from '@/assets/localStorage/index.js'
import { localStorageKey } from '@/assets/enums/localStorageKey.js'
import { mapMutations, mapGetters } from "vuex";

export var GuardarDatosTokenMixin = {
  methods: {
    ...mapMutations(["ModificarSession"]),

    guardarDatosToken(token) {
      var datosToken = descifrarToken(token);

      if (typeof datosToken != 'string') {
        this.ModificarSession(datosToken);

        return true;
      } else {
        eliminarItemLocal(localStorageKey.token);
        this.$emit("showAlert", this.sessionCaducada, "danger");

        return false;
      }
    },
  }
}

export var verificarSihayExcepcionesMixin = {
  computed:{
    ...mapGetters(["GetExcepcion"]),
  },
  methods: {
    ...mapMutations(["ModificarExcepcion"]),

    verificarSihayExcepciones() {
      var hayExcepcion = false;
      var excepcionVacia = {
        mensaje: "",
        tipo: "",
        aplicacion: ""
      }
      
      if (this.GetExcepcion.mensaje != "" && this.GetExcepcion.mensaje != null) {
        //emitimos la alerta
        this.$emit("showAlert", this.GetExcepcion.mensaje, "danger");

        //Y, borramos la exepcion para volver a repetir el proceso
        this.ModificarExcepcion(excepcionVacia);
        hayExcepcion = true;
      }

      return hayExcepcion;
    },
  }
}