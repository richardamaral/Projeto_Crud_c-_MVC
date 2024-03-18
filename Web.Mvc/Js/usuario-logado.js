$(function () {

    GetCliente();

});

function GetCliente() {
    var usuarioClienteId = $('#UsuarioClienteId').val();

    var tableUsuarios = $('#tbCliente').DataTable({
        "processing": true,
        "destroy": true,
        "serverSide": true,
        "aaSorting": [[0, "desc"]],
        "ajax": {
            "url": "/Cliente/JsGetCliente/",
            "type": 'POST',
            'dataType': 'json',
            "data": {
                UsuarioClienteId: usuarioClienteId
            }
        },
        "columnDefs": [
            {
                "render": function (data, type, row) {
                    return null;
                },
                "targets": null,
            }
        ],
        "columns": [
         /*{0}*/ { "data": "ClienteId", "autoWidth": true },
         /*{1}*/ { "data": "Nome", "autoWidth": true },
         /*{2}*/ { "data": "Email", "autoWidth": true },
         /*{3}*/ { "data": "Celular", "autoWidth": true },
         /*{4}*/ { "data": "Idade", "autoWidth": true },
        ],
        "language": {
            "url": "/Js/Portuguese.json",
        }
    });
}