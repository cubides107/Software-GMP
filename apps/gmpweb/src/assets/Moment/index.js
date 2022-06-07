const moment = require('moment');

export function convertirFecha(fechaDateTime){
    let d4 = moment(fechaDateTime);
    return d4.format('ll');
}