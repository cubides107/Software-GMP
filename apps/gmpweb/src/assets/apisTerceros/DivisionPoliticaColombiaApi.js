import Axios from 'axios'
import {divicionPoliticaColombiaEmp} from "../url/TercerosEndpoint"


export async function GetCiudades(){

    //configurar el objeto de peticion
    const config = {
        url: divicionPoliticaColombiaEmp,
        method: 'get',
        headers: {
          'Content-Type': 'application/json'
        },
        data: {
        }
      };

      //haciendo la peticion
      return Axios.request(config)
        .then((rs) => {
            var datos = rs.data;
            var municipios = [{ value: "", text: "Seleccione una ciudad" }]

            datos.forEach(d => {
                municipios.push({ value: d.municipio, text: d.municipio });
            });
            
            return municipios;
        })
        .catch(function(error) {
            // handle error
            console.log(error);
        })
        .finally(function() {
            // always executed
        });
}

