$(document).ready(function () {
    $('#ctl00_ContentPlaceHolder1_txt_primaced_m,#ctl00_ContentPlaceHolder1_txt_impuesto_m,#ctl00_ContentPlaceHolder1_txt_prima_x_pag_m,#ctl00_ContentPlaceHolder1_txt_prima_x_cob_m,#ctl00_ContentPlaceHolder1_txt_sin_directo_m,#ctl00_ContentPlaceHolder1_txt_sin_x_cob_m,#ctl00_ContentPlaceHolder1_txt_sin_x_pag_m,#ctl00_ContentPlaceHolder1_txt_otr_x_cob_m,#ctl00_ContentPlaceHolder1_txt_otr_x_pag_m,#ctl00_ContentPlaceHolder1_txt_dscto_comis_m').numeric(".");
    $("#tblopemanual input[type='text']").val("0.00");
    $("section").delegate("#ctl00_ContentPlaceHolder1_btnGuardar_m", "click", function (ev) {
        var calcula = 0;
        $("#tblopemanual input[type='text'],#tblopemanual select").each(function (index, element) {
            valor = $(this).val();
            if (valor == 0) {
                calcula++;
            }
        });
        if ($("#ctl00_ContentPlaceHolder1_ddl_contrato_m").val() == 0) {
            MessageBox("Seleccione el Contrato"); return false;
        } else if (calcula > 0) {
            return confirm("Hay Algunos Campos con Valor (0) ¿Esta Seguro de Grabar?");
        } else {
            return confirm("¿Esta Seguro de Grabar " + $("#ctl00_ContentPlaceHolder1_ddl_tipope_m option:selected").text() + "?");
        }
    });
    $("section").delegate("#ctl00_ContentPlaceHolder1_btnNuevo_m", "click", function (ev) {
        ev.preventDefault();
        $("#tblopemanual input[type='text']").val("0.00");
        $("#tblopemanual select").val("0");
    });
    //funcion mensaje
    function MessageBox(texto) {
        $("<div style='font-size:14px;text-align:center;'>" + texto + "</div>").dialog({ title: 'Alerta', modal: true, width: 400, height: 160, buttons: [{ id: 'aceptar', text: 'Aceptar', icons: { primary: 'ui-icon-circle-check' }, click: function () { $(this).dialog('close'); } }] })
    }
});
