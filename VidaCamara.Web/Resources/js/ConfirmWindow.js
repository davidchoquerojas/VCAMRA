function SomeMethod() {
    var message = "";
    var textbox = document.getElementById('ctl00_hdf_control');
    if (textbox.value == "210") {
        message = confirm("¿ Está Seguro de crear el Registro ?");
    } else if (textbox.value == "999") {
        message = confirm("¿ Está Seguro de Actualizar el Registro ?");
    }
    return message;
}
function DeleteMethod() {
    var message = ""
    var textbox = document.getElementById('ctl00_hdf_control');
    if (textbox.value == "210") {
        alert("Selecione un Registro");
        message = false;
    } else {
        message = confirm("¿ Está Seguro de Eliminar el Registro ?");
    }
    return message;

}
$(document).ready(function () {

    $('.tabBody').delegate('#ctl00_ContentPlaceHolder1_btn_nuevo', 'click', function () {
        $('#ctl00_hdf_control').val("210");
    });
    $.datepicker.regional['es'] = {
        closeText: 'Cerrar',
        prevText: '<Ant',
        nextText: 'Sig>',
        currentText: 'Hoy',
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
        dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
        weekHeader: 'Sm',
        dateFormat: 'dd/mm/yy',
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: '',
        showButtonPanel: true, changeMonth: true, changeYear: true
    };
    //funcion datepicker
    $.datepicker.setDefaults($.datepicker.regional['es']);
    $('#ctl00_ContentPlaceHolder1_txt_fecini_c').datepicker();
    $('#ctl00_ContentPlaceHolder1_txt_fecfin_c').datepicker();
    $('#ctl00_ContentPlaceHolder1_txt_fecha_creacion').datepicker();
    $('#ctl00_ContentPlaceHolder1_txt_hasta').datepicker();

    //validacion de numeros
    $('#ctl00_ContentPlaceHolder1_txt_impAbono_p').numeric();
    $('#ctl00_ContentPlaceHolder1_txt_primaEst_p').numeric();
    $('#ctl00_ContentPlaceHolder1_txt_impAbono_ib').numeric();
    //funcion para convertir numeros a fechas
    window.ConvertNumberToDate = ConvertNumberToDate;

    function ConvertNumberToDate(numberdate) {
        var milli = numberdate.replace(/\/Date\((-?\d+)\)\//, '$1');
        var d = new Date(parseInt(milli));
        mes = d.getMonth() + 1;
        dia = d.getDate()
        if (mes < 10) { mes = "0" + mes }
        if (dia < 10) { dia = "0" + dia }
        return f = dia + "/" + mes + "/" + d.getFullYear();
    }

    function dar_formato(num) {

        var cadena = ""; var aux;

        var cont = 1, m, k;

        if (num < 0) aux = 1; else aux = 0;

        num = num.toString();



        for (m = num.length - 1; m >= 0; m--) {

            cadena = num.charAt(m) + cadena;

            if (cont % 3 == 0 && m > aux) cadena = "," + cadena; else cadena = cadena;

            if (cont == 3) cont = 1; else cont++;

        }

        cadena = cadena.replace(/.,/, ",");

        return cadena;

    }

});