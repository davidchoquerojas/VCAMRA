$(document).ready(function () {
    $("#ctl00_ContentPlaceHolder1_ddl_estado_t").val("A");
    //validacion de informacion antes de enviar
    $("section").delegate("#ctl00_ContentPlaceHolder1_btn_enviar_t", "click", function (ev) {
        var tipo_tabla = $("#ctl00_ContentPlaceHolder1_ddl_tabla_t").val();
        var codigo = $("#ctl00_ContentPlaceHolder1_txt_codigo_t").val();
        var descripcion = $("#ctl00_ContentPlaceHolder1_txt_descripcion_t").val();
        var id_tabla = $("#ctl00_ContentPlaceHolder1_txt_idtabla").val();
        if (id_tabla == 0) {
            if (tipo_tabla == 0) {
                MessageBox("Selecione Tipo Tabla"); return false;
            } else if (codigo == "") {
                MessageBox("Ingrese Codigo"); return false;
            } else if (descripcion == "") {
                MessageBox("Escriba una Descripción"); return false;
            } else {
                return confirm("¿ Está Seguro de crear el Registro ?");
            }
        } else {
            return confirm("¿ Está Seguro de Actualizar el Registro ?");
        }
    });
    $('#ctl00_ContentPlaceHolder1_btn_nuevo_t').on('click', function () {
        $('#ctl00_hdf_control').val("210");
    });
    $('#ctl00_hdf_control').val("210");
    var clase = 0;
    $('#frmTabla').delegate('#ctl00_ContentPlaceHolder1_ddl_tabla_t', 'change', function (e) {
        clase = 1;
        jTableRequest($(this).val(),"NULL");
    });
    $("#frmTabla").delegate("#ctl00_ContentPlaceHolder1_txt_descripcion_t", "keyup", function (ev) {
        if(ev.keyCode != 13){
            if ($("#ctl00_ContentPlaceHolder1_ddl_tabla_t").val() == 0) {
                clase = 0;
                jTableRequest("9999", $(this).val());
            } else {
                clase = 1;
                jTableRequest($("#ctl00_ContentPlaceHolder1_ddl_tabla_t").val(), $(this).val());
            }
        }else{
            return false;
        }
    });
    var idtable = $("#ctl00_ContentPlaceHolder1_ddl_tabla_t").val();
    if (idtable == 0) {
        jTableRequest("9999","NULL");
    } else {
        clase = 1;
        jTableRequest(idtable,"NULL");
    }

    function jTableRequest(tipo_tabla,descripcion) {
        $('#tblTablaView').jtable({
            tableId: 'tableViewID',
            paging: true,
            sorting: true,
            pageSize: 12,
            defaultSorting: '_codigo ASC',
            selecting: true,
            actions: {
                listAction: '/WebPage/Mantenimiento/frmTabla.aspx/ListConcepto',
            },
            recordsLoaded: function (event, data) {
                GetTipoTabla($("#ctl00_ContentPlaceHolder1_ddl_tabla_t").val());
            },
            fields: {
                _id_Concepto: { key: true, list: false },
                _tipo_Tabla: { title: 'Tipo de Tabla ', width: '10%' },
                _codigo: { title: 'Código', width: '8%' },
                _descripcion: { title: 'Descripción', width: '30%' },
                _valor: { title: 'Valor', width: '10%' },
                _clase: { title: 'Clase', width: '15%' },
                _tipo: { title: 'Tipo', width: '10%' },
                _estado: { title: 'Estado', width: '10%', }
            },
            selectionChanged: function () {
                var $selectedRows = $('#tblTablaView').jtable('selectedRows');

                $('#SelectedRowList').empty();
                if ($selectedRows.length > 0) {
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        $('#ctl00_ContentPlaceHolder1_txt_descripcion_t').val(record._descripcion);
                        $('#ctl00_ContentPlaceHolder1_txt_codigo_t').val(record._codigo);
                        $('#ctl00_ContentPlaceHolder1_txt_clase_t').val(record._clase);
                        $('#ctl00_ContentPlaceHolder1_ddl_estado_t').val(record._estado);
                        if (clase == 0) {
                            $('#ctl00_ContentPlaceHolder1_ddl_tabla_t').val(record._codigo);
                        } else {
                            $('#ctl00_ContentPlaceHolder1_ddl_tabla_t').val(record._tipo_Tabla);
                        }
                        $('#ctl00_ContentPlaceHolder1_txt_valor_t').val(record._valor);
                        $('#ctl00_ContentPlaceHolder1_txt_tipo_t').val(record._tipo);
                        $('#ctl00_ContentPlaceHolder1_txt_idtabla').val(record._id_Concepto);
                        $('#ctl00_ContentPlaceHolder1_txt_9999').val(record._tipo_Tabla);
                        $('#ctl00_hdf_control').val("999");
                    });
                } else {
                    $('#SelectedRowList').append('No row selected! Select rows to see here...');
                }
            }
        });

        $('.jtable-main-container').css({ "width": "1170px" });
        $('#tblTablaView').jtable('load', { TipoTabla: tipo_tabla, descripcion: descripcion });
    }


    $('.tabBody').delegate('#ctl00_ContentPlaceHolder1_ddl_tabla_t', 'change', function (ev) {
        if ($(this).val() == "01") {
            $("label[for='txt_clase_t']").text('Pais :');
        } else {
            $("label[for='txt_clase_t']").text('Clase :');
        }
    });
    function MessageBox(texto) {
        $("<div style='font-size:14px;text-align:center;'>" + texto + "</div>").dialog({ title: 'Alerta', modal: true, width: 400, height: 160, buttons: [{ id: 'aceptar', text: 'Aceptar', icons: { primary: 'ui-icon-circle-check' }, click: function () { $(this).dialog('close'); } }] })
    }
    var GetTipoTabla = function (codigo) {
        $.ajax({
            url: "/WebPage/Mantenimiento/frmTabla.aspx/GetConceptoByCodigo",
            type: 'POST',
            contentType: "application/json;",
            dataType: 'json',
            data: JSON.stringify({ codigo: codigo }),
            success: function (data) {
                ControlarObjetos(parseInt(data.d.Records));
            }, error: function (e) {
                console.log(e);
            }

        });
    }
    var ControlarObjetos = function (tipo) {
        if (tipo == 2) {
            $("#ctl00_ContentPlaceHolder1_txt_codigo_t").attr("readonly", true).css({ "background": "rgba(0,10,10,0.1)" });
        } else {
            $("#ctl00_ContentPlaceHolder1_txt_codigo_t").attr("readonly", false).css({ "background": "white" });
        }
    }
});