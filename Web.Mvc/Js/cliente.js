$(function () {
    JsGetCliente();

    $(".btnCliente").on("click", function () {
        JsGetCliente($(this).val());
    });

    Submit();
});


function JsGetCliente(clienteId = 0) {

    $('#example2').show().dataTable({
        "processing": true,
        "destroy": true,
        "serverSide": true,
        "aaSorting": [[0, "desc"]],
        "ajax": {
            "url": "/Cliente/JsGetCliente/",
            "type": 'POST',
            "dataType": "json",
            "data": {
                ClienteId: clienteId
            }
        },
        "columns": [
         /*0*/ { "data": "ClienteId", "autoWidth": true },
         /*1*/ { "data": "Nome", "autoWidth": true },
         /*2*/ { "data": "Email", "autoWidth": true },
         /*3*/ { "data": "Celular", "autoWidth": true },
         /*4*/ { "data": "Idade", "autoWidth": true }
        ],
        "language": {
            "url": "/Js/Portuguese.json"
        },
        "initComplete": function (settings, json) {


        }
    });
}

function Submit() {

    $("#salvarCliente").submit(function (event) {
        event.preventDefault(); // Impede o comportamento padr�o do formul�rio (recarregar a p�gina)

        var formData = $(this).serialize(); // Serializa o formul�rio

        $.ajax({
            type: "POST",
            url: "/Cliente/JsSetCliente", // Substitua "Controller" pelo nome real do seu controlador
            data: formData, // Use os dados serializados do formul�rio
            dataType: "json",
            success: function (data) {
                alert(data.mensagem);
                JsGetCliente();
            },
            error: function (xhr, textStatus, errorThrown) {
                console.error("Erro na requisi��o Ajax:", errorThrown);
            }
        });
    });
}
