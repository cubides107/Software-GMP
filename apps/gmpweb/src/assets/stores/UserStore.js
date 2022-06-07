import Vue from 'vue'
import Vuex from 'vuex'
import Axios from 'axios'
import { Server } from '../url/Server'
import { UserEmp } from '../url/Endpoint'
import { TypeMessagesHttp } from '../enums/TypeMessagesHttp'

Vue.use(Vuex)

//estado
const state = {
    token: "",
    session: {
        UserId: "",
        Username: "",
        mail: "",
        exp: null,
        role: ""
    },
    excepcion: {
        mensaje: "",
        tipo: "",
        aplicacion: ""
    }
}

//mutaciones
const mutations = {
    ModificarSession(state, data) {
        state.session.UserId = data.UserId;
        state.session.Username = data.Username;
        state.session.mail = data.mail;
        state.session.exp = data.exp;
        state.session.role = data.role;
    },
    ModificarExcepcion(state, data) {
        state.excepcion = data;
    },
    ModificarToken(state, data) {
        state.token = data;
    }
}

//gets
const getters = {
    GetSession: state => {
        return state.session;
    },
    GetExcepcion: state => {
        return state.excepcion;
    },
    GetToken: state => {
        return state.token;
    }
}

//acciones
const actions = {
    LoginUserAction(context, { correo, contrasena }) {

        //configurar el objeto de peticion
        const config = {
            url: Server + UserEmp.ClientRegisteEmp,
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            data: {
                'mail': correo,
                'password': contrasena,
            }
        };

        //haciendo la peticion
        return Axios.request(config)
            .then((rs) => {
                if (rs.data.messageType === TypeMessagesHttp.exception) {
                    context.commit('ModificarExcepcion', rs.data);
                }
                else {
                    context.commit('ModificarToken', rs.data.token);
                }

            })
            .catch(function (error) {
                // handle error
                console.log(error);
            })
            .finally(function () {
                // always executed
            });
    },

    LogoutAction(context, { token }) {
        //configurar el objeto de peticion
        const config = {
            url: Server + UserEmp.LogOutEmp,
            method: 'post',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token
            },
            data: {}
        };

        console.log(config);
        //haciendo la peticion
        return Axios.request(config)
            .then((rs) => {
                if (rs.data.messageType === TypeMessagesHttp.exception) {
                    context.commit('ModificarExcepcion', rs.data);
                }
                else {
                    context.commit('ModificarToken', rs.data.token);
                    context.commit('ModificarSession', {
                        UserId: "",
                        Username: "",
                        mail: "",
                        exp: null,
                        role: ""
                    });
                }

            })
            .catch(function (error) {
                // handle error
                console.log(error);
            })
            .finally(function () {
                // always executed
            });
    },

    AccessRoot(context, { correo, contrasena }) {
        //configurar el objeto de peticion
        const config = {
            url: Server + UserEmp.AccessRoot,
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            data: {
                'mail': correo,
                'password': contrasena,
            }
        };

        //haciendo la peticion
        return Axios.request(config)
            .then((rs) => {
                if (rs.data.messageType === TypeMessagesHttp.exception) {
                    context.commit('ModificarExcepcion', rs.data);
                }
                else {
                    context.commit('ModificarToken', rs.data.token);
                }

            })
            .catch(function (error) {
                // handle error
                console.log(error);
            })
            .finally(function () {
                // always executed
            });
    },

    RegisterInternalAction(context, usuarioInterno) {
        //obtenemos la session
        var token = context.getters.GetToken

        //configurar el objeto de peticion
        const config = {
            url: Server + UserEmp.RootRegisterUserInternal,
            method: 'put',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token
            },
            data: {
                "newUserInternal": {
                    'name': usuarioInterno.nombre,
                    'lastname': usuarioInterno.apellido,
                    'phone': usuarioInterno.telefono,
                    'mail': usuarioInterno.correo,
                    'password': usuarioInterno.contrasena,
                    'address': usuarioInterno.direccion,
                }
            }
        };

        //haciendo la peticion
        return Axios.request(config)
            .then((rs) => {
                if (rs.data.messageType === TypeMessagesHttp.exception) {
                    context.commit('ModificarExcepcion', rs.data);
                    //return null;
                }
                else {
                    return rs.data;
                }

            })
            .catch(function (error) {
                // handle error
                console.log(error);
            })
            .finally(function () {
                // always executed
            });
    },

    RootRegisterExternalAction(context, usuarioExterno) {
        //obtenemos la session
        var token = context.getters.GetToken

        //configurar el objeto de peticion
        const config = {
            url: Server + UserEmp.RootRegisterUserExternal,
            method: 'put',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token
            },
            data: {
                "newUserExternal": {
                    'name': usuarioExterno.nombre,
                    'lastname': usuarioExterno.apellido,
                    'phone': usuarioExterno.telefono,
                    'mail': usuarioExterno.correo,
                    'password': usuarioExterno.contrasena,
                    'address': usuarioExterno.direccion,
                }
            }
        };

        //haciendo la peticion
        return Axios.request(config)
            .then((rs) => {
                if (rs.data.messageType === TypeMessagesHttp.exception) {
                    context.commit('ModificarExcepcion', rs.data);
                    //return null;
                }
                else {
                    return rs.data;
                }

            })
            .catch(function (error) {
                // handle error
                console.log(error);
            })
            .finally(function () {
                // always executed
            });
    },

    InternalRegisterExternalAction(context, usuarioExterno) {
        //obtenemos la session
        var token = context.getters.GetToken

        //configurar el objeto de peticion
        const config = {
            url: Server + UserEmp.UsuarioInternoRegistraExterno,
            method: 'put',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token
            },
            data: {
                "newUserExternal": {
                    'name': usuarioExterno.nombre,
                    'lastname': usuarioExterno.apellido,
                    'phone': usuarioExterno.telefono,
                    'mail': usuarioExterno.correo,
                    'password': usuarioExterno.contrasena,
                    'address': usuarioExterno.direccion,
                }
            }
        };

        //haciendo la peticion
        return Axios.request(config)
            .then((rs) => {
                if (rs.data.messageType === TypeMessagesHttp.exception) {
                    context.commit('ModificarExcepcion', rs.data);
                    //return null;
                }
                else {
                    return rs.data;
                }

            })
            .catch(function (error) {
                // handle error
                console.log(error);
            })
            .finally(function () {
                // always executed
            });
    },

    QueryUsersAction(context, consultaUsuarios) {
        //obtenemos la session
        var token = context.getters.GetToken

        //configurar el objeto de peticion
        const config = {
            url: Server + UserEmp.QueryUsers,
            method: 'get',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token
            },
            params: {
                'filterName': consultaUsuarios.filterName,
                'page': consultaUsuarios.page,
                'pageSize': consultaUsuarios.pageSize,
            }
        };

        //haciendo la peticion
        return Axios.request(config)
            .then((rs) => {
                if (rs.data.messageType === TypeMessagesHttp.exception) {
                    context.commit('ModificarExcepcion', rs.data);
                }
                else {
                    return rs.data;
                }

            })
            .catch(function (error) {
                // handle error
                console.log(error);
            })
            .finally(function () {
                // always executed
            });
    },

    RegisterStudyResearch(context, solicitud){
        //obtenemos la session
        var token = context.getters.GetToken

        //configurar el objeto de peticion
        const config = {
            url: Server + StudyResearch.RegisterStudyResearch,
            method: 'put',
            headers: {
              'Content-Type': 'multipart/form-data',
              'Authorization': 'Bearer '+ token
            },
            data: solicitud
          };

          //haciendo la peticion
          return Axios.request(config)
            .then((rs) => {
                if(rs.data.messageType === TypeMessagesHttp.exception){
                    context.commit('ModificarExcepcion', rs.data);
                    //return null;
                }
                else{
                    return rs.data;
                }
                
            })
            .catch(function(error) {
                // handle error
                console.log(error);
            })
            .finally(function() {
                // always executed
            });
    }
}

export const UserStore = new Vuex.Store({
    state: state,
    mutations: mutations,
    actions: actions,
    getters: getters
});
