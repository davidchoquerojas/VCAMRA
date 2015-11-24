$(document).ready(function (e) {
    //EVENTOS
    $("section").delegate("#ctl00_ContentPlaceHolder1_btn_borrar", "click", function () {
        var tablaidcont = $("#tblContratoView").length;
        var tablaidDet = $("#tblReasegurador").length;
        if (tablaidcont == 1 && tablaidDet == 0) {
            var idcontrat = $("#ctl00_ContentPlaceHolder1_txt_idContrato_c").val();
            if (idcontrat == 0) {
                MessageBox("Selecione un Registro"); return false;
            } else {
                return confirm("¿ Está Seguro de Eliminar el Registro ?");
            }
        } else if (tablaidcont == 0 && tablaidDet == 1) {
            var idcontdet = $("#ctl00_ContentPlaceHolder1_txt_idContratoDetalle_c").val();
            if (idcontdet == 0) {
                MessageBox("Selecione un Registro"); return false;
            } else {
                return confirm("¿ Está Seguro de Eliminar el Registro ?");
            }
        } else {
            return confirm("¿ Está Seguro de Eliminar el Registro ?");
        }
    });

    $('#ctl00_ContentPlaceHolder1_txt_vigente').numeric();
    $('#ctl00_ContentPlaceHolder1_txt_mes_vig').numeric();
    $('#ctl00_ContentPlaceHolder1_txt_tcamesCont').numeric();
    $('#ctl00_ContentPlaceHolder1_txt_tcaCierre').numeric();
    $('#ctl00_ContentPlaceHolder1_txt_cantidad_decimal').numeric();

    function MessageBox(texto) {
        $("<div style='font-size:14px;text-align:center;'>" + texto + "</div>").dialog({ title: 'Alerta', modal: true, width: 400, height: 160, buttons: [{ id: 'aceptar', text: 'Aceptar', icons: { primary: 'ui-icon-circle-check' }, click: function () { $(this).dialog('close'); } }] })
    }
});