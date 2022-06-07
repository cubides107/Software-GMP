import { mapActions } from "vuex";
import { ImagenEnlace } from "@/assets/enums/Enlaces.js";
import { OpcionInformacionUsuarioHelp } from "@/assets/enums/HelpText.js";
import { verificarSihayExcepcionesMixin } from '@/assets/mixins/AccesoUsuarioMixin.js'


export var MostrarInformacionUsuarionMixin = {

    mixins: [verificarSihayExcepcionesMixin],

    data() {
        return {
            usuario: {
                name: "",
                lastName: "",
                mail: "",
                phone: "",
                address: ""
            },
        };
    },

    methods: {
        ...mapActions(["QueryUserAction"]),

        abrirModalMostrarInformacionUsuario() {
            this.consultarUsuario();
            this.$refs["modal-informacionUsuario"].show();
        },

        async consultarUsuario() {
            var informacionUsuario = await this.QueryUserAction();

            if (this.verificarSihayExcepciones() == false) {
                if (informacionUsuario != null) {
                    this.usuario.name = informacionUsuario.name;
                    this.usuario.lastName = informacionUsuario.lastName;
                    this.usuario.mail = informacionUsuario.mail;
                    this.usuario.phone = informacionUsuario.phone;
                    this.usuario.address = informacionUsuario.address;
                }
            }
        },
    },

    computed: {
        imagenInformacionUsuario() {
            return ImagenEnlace.informacionUsuario;
        },

        imagenInformacionUsuarionCardOpcion() {
            return ImagenEnlace.imagenInformacionUsuarionCardOpcion;
        },

        tituloCardOpcionInformacionUser() {
            return OpcionInformacionUsuarioHelp.tituloCardOpcion;
        },

        descripcionCardOpcionInformacionUser() {
            return OpcionInformacionUsuarioHelp.descripcionCardOpcion;
        },

        tituloModalInformacionUser() {
            return OpcionInformacionUsuarioHelp.tituloModal;
        },

        nombreUsuario() {
            return OpcionInformacionUsuarioHelp.nombreTexto + this.usuario.name;
        },

        apellidoUsuario() {
            return OpcionInformacionUsuarioHelp.apellidoTexto + this.usuario.lastName;
        },

        correoUsuario() {
            return OpcionInformacionUsuarioHelp.mailTexto + this.usuario.mail;
        },

        telefonoUsuario() {
            return OpcionInformacionUsuarioHelp.phoneTexto + this.usuario.phone;
        },

        direccionUsuario() {
            return OpcionInformacionUsuarioHelp.addresTexto + this.usuario.address;
        }
    },
};