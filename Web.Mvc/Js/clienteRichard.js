

$(function () {
    JsGetCliente();

    $("#btnCliente").on("click", function () {
        JsGetCliente($(this).val());
    });

    $("#AddCliente").click(function () {
        $('.tit-clienteId').html('');
        $("#ClienteId").val(0);
        $("#salvarCliente")[0].reset();
        $("#addModal").modal("show");
        $("#addModalLabel").html('Adicionar Novo Cliente');
    });



    Submit();
});

//$('#example2').on('click', '.btnUsuarios', function () {
//    var data = table.row($(this).parents('tr')).data();
//    openUsuariosModal(data.ClienteId);
//    console.log('Gerenciar usuários para o ID: ' + data.ClienteId);
//});


var ClienteCriarUsuario;


//$('#btnSalvarCadastroUsuario').click(function () {
//    var nome = $('#usuarioNomeCadastro').val();
//    var email = $('#usuarioEmailCadastro').val();
//    var senha = $('#usuarioSenhaCadastro').val();


//    var clienteId = parseInt(ClienteCriarUsuario, 10);



//    SetUsuarioCliente(clienteId, nome, email, senha);
//});


function openUsuariosModal(clienteId) {
    $('#cadastroUsuarioClienteModal').modal('show');
    loadUsuarios(clienteId);
    $('#Usuario_ClienteId').val(clienteId);


   
}



function openUsuariosEditModal(usuarioclienteId) {

    $('#cadastroUsuarioClienteModal').modal('hide');
    $.ajax({
        type: "POST",
        url: "/Cliente/JsGetUsuarioCliente/",
        data: { UsuarioClienteId: usuarioclienteId },
        dataType: "json",
        success: function (result) {

            var item = result.data[0];
            console.log(item);


            $('#editUsuarioModal').modal('show');
            $('.editClienteId').val(item.ClienteId);
            $('#editUsuarioClienteId').val(item.UsuarioClienteId);



            $('#editUsuarioNome').val(item.Usuario);
            $('#editUsuarioEmail').val(item.Email);
            $('#editUsuarioSenha').val(item.Senha);

            //NESSA PARTE DO CODIGO EU GOSTARIA QUE ELE TAMBÉM DIRECIONASSE OS OUTROS DADOS DO CLIENTE SOLICITADO PRA DENTRO DO FORM PARA PREENCHER AUTOMATICAMENTE O FORM.

        },
        error: function (xhr, textStatus, errorThrown) {
            console.error("Erro na requisição Ajax:", errorThrown);
        }
    });


}




function SetUsuarioCliente(clienteId, nome, email, senha) {
    console.log("Valores a serem passados para SetUsuarioCliente:", clienteId, nome, email, senha);
    $.ajax({
        type: "POST",
        url: "/Cliente/JsSetUsuarioCliente",
        data: JSON.stringify({
            'ClienteId': clienteId,
            'Nome': nome,
            'Email': email,
            'Senha': senha
        }),
        contentType: 'application/json',
        dataType: "json",
        success: function (data) {
            alert(data.mensagem);
            loadUsuarios(clienteId);
            $('#UsuarioNomeCadastro').val('');
            $('#UsuarioEmailCadastro').val('');
            $('#UsuarioSenhaCadastro').val('');

        },
        error: function (xhr, textStatus, errorThrown) {
            console.error("Erro na requisição Ajax:", errorThrown);
        }
    });
}


var selectedUsuarioClienteId;
function loadUsuarios(clienteId) {
    var tableUsuarios = $('#tableUsuarios').DataTable({
        "processing": true,
        "destroy": true,
        "serverSide": true,
        "aaSorting": [[0, "desc"]],
        "ajax": {
            "url": "/Cliente/JsGetUsuarioCliente/",
            "type": 'POST',
            'dataType': 'json',
            "data": {
                ClienteId: clienteId
            }
        },
        "columnDefs": [
            {
                "render": function (data, type, row) {

                    return '<button class="btnEditarUsuario" data-clienteid="' + row.ClienteId + '" onclick="openUsuariosEditModal(' + row.UsuarioClienteId + ')">Editar</button>' +
                        '<button type="button" class="btnDeletarUsuario" data-clienteid="' + row.ClienteId + '" onclick="DeleteUsuarioCliente(' + row.ClienteId + ')">Deletar</button>';
                },
                "targets": 6,
            }
        ],
        "columns": [
           /*{0}*/ { "data": "ClienteId", "autoWidth": true },
           /*{1}*/ { "data": "Usuario", "autoWidth": true },
           /*{2}*/ { "data": "Email", "autoWidth": true },
           /*{3}*/ { "data": "Senha", "autoWidth": true },
           /*{4}*/ { "data": "Ativo", "autoWidth": true },
           /*{5}*/ { "data": "sDataCadastro", "autoWidth": true },
           /*{6}*/ { "data": null, "autoWidth": true },

        ],
        "language": {
            "url": "/Js/Portuguese.json",
            "initComplete": function (settings, json) {

                //$('#example2').off()
                //    .on('click', '.btnDeletar', function () {

                //        alert();
                //        var clienteid = $(this).data('clienteid');

                //        DeleteCliente(clienteid);

                //    });

                //$('#example2').off()
                //    .on('click', '.btnEditar', function () {

                //        var clienteid = $(this).data('clienteid');
                //        //updateClienteSelecionado(data);
                //        openEditModal(clienteid);
                //        console.log('Editar cliente com ID: ' + clienteid);
                //    });

            }
        }
    });

}



function JsGetCliente(clienteId = 0) {
    var table = $('#example2').DataTable({
        "processing": true,
        "destroy": true,
        "serverSide": true,
        "aaSorting": [[0, "desc"]],
        "ajax": {
            "url": "/Cliente/JsGetCliente/",
            "type": 'POST',
            'dataType': 'json',
            "data": {
                ClienteId: clienteId
            }
        },
        "columnDefs": [
            {
                "render": function (data, type, row) {
                    return '<button class="btnEditar" data-clienteid="' + row.ClienteId + '" onclick="openEditModal(' + row.ClienteId + ')">Editar</button>' +
                        '<button type="button" class="btnDeletar" data-clienteid="' + row.ClienteId + '" onclick="DeleteCliente(' + row.ClienteId + ')">Deletar</button>' +
                        '<button type="button" class="btnUsuarios" data-clienteid="' + row.ClienteId + '" onclick="openUsuariosModal(' + row.ClienteId + ')" >Usuarios</button>';
                },
                "targets": 5,
            }
        ],
        "columns": [
           /*{0}*/ { "data": "ClienteId", "autoWidth": true },
           /*{1}*/ { "data": "Nome", "autoWidth": true },
           /*{2}*/ { "data": "Email", "autoWidth": true },
           /*{3}*/ { "data": "Celular", "autoWidth": true },
           /*{4}*/ { "data": "Idade", "autoWidth": true },
           /*{5}*/ { "data": "ClienteId", "autoWidth": true },
        ],
        "language": {
            "url": "/Js/Portuguese.json",
            "initComplete": function (settings, json) {

                //$('#example2').off()
                //    .on('click', '.btnDeletar', function () {

                //        alert();
                //        var clienteid = $(this).data('clienteid');

                //        DeleteCliente(clienteid);

                //    });

                //$('#example2').off()
                //    .on('click', '.btnEditar', function () {

                //        var clienteid = $(this).data('clienteid');
                //        //updateClienteSelecionado(data);
                //        openEditModal(clienteid);
                //        console.log('Editar cliente com ID: ' + clienteid);
                //    });

            }
        }
    });

}

//CRIAR FUNÇÃO DELETEUSUARIOCLIENTE ENVIANDO APENAS O USUARIOCLIENTEID, POIS DA FORMA APRESENTADA ABAIXO ELE TAMBÉM EXCLUE AS INFORMAÇÕES DO CLIENTEID DA TABLE CLIENTE

function DeleteUsuarioCliente(clienteid) {
    var confirmDelete = confirm('Tem certeza que deseja deletar o cliente com ID ' + clienteid + '?');

    if (confirmDelete) {
        $.ajax({
            type: "POST",
            url: "/Cliente/JsDeleteUsuarioCliente",
            data: { ClienteId: clienteid },
            dataType: "json",
            success: function (response) {
                loadUsuarios(clienteid);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.error("Erro na requisição Ajax:", errorThrown);
            }
        });
    }
}





function DeleteCliente(clienteid) {
    var confirmDelete = confirm('Tem certeza que deseja deletar o cliente com ID ' + clienteid + '?');

    if (confirmDelete) {
        $.ajax({
            type: "POST",
            url: "/Cliente/JsDeleteCliente",
            data: { ClienteId: clienteid },
            dataType: "json",
            success: function (response) {
                alert(response.mensagem);
                JsGetCliente();
            },
            error: function (xhr, textStatus, errorThrown) {
                console.error("Erro na requisição Ajax:", errorThrown);
            }
        });
    }
}






function openEditModal(clienteId) {

    console.log('Chamando openEditModal com:', clienteId);
    $.ajax({
        type: "POST",
        url: "/Cliente/JsGetCliente/",
        data: { ClienteId: clienteId },
        dataType: "json",
        success: function (result) {

            var item = result.data[0];
            console.log(item);
            $('#editModal').modal('show');
            $('#editClienteId').val(clienteId).prop('readonly', true);


            $('#editNome').val(item.Nome);
            $('#editEmail').val(item.Email);
            $('#editCelular').val(item.Celular);
            $('#editIdade').val(item.Idade);

            //NESSA PARTE DO CODIGO EU GOSTARIA QUE ELE TAMBÉM DIRECIONASSE OS OUTROS DADOS DO CLIENTE SOLICITADO PRA DENTRO DO FORM PARA PREENCHER AUTOMATICAMENTE O FORM.

        },
        error: function (xhr, textStatus, errorThrown) {
            console.error("Erro na requisição Ajax:", errorThrown);
        }
    });
}


$(document).ready(function () {

    $("#AddCliente").click(function () {

        $("#addModal").modal("show");
    });
});






function Submit() {

    $("#salvarCliente").submit(function (event) {
        event.preventDefault(); 

        var formData = new FormData(this); 

        $.ajax({
            type: "POST",
            url: "/Cliente/JsSetCliente", 
            data: formData,
            contentType: false, 
            processData: false, 
            dataType: "json",
            success: function (data) {
                console.log(data)
                alert(data.mensagem);
                $('.blocker').hide();
                $('#addModal').hide();
                JsGetCliente(0);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.error("Erro na requisição Ajax:", errorThrown);
            }
        });
    });

    $("#editUsuarioForm").submit(function (event) {
        event.preventDefault(); // Impede o comportamento padrão do formulário (recarregar a página)

        var formData = $(this).serialize(); // Serializa o formulário
        console.log(formData)
        var clienteId = $(this).find('.editClienteId').val();

        $.ajax({
            type: "POST",
            url: "/Cliente/JsSetUsuarioCliente", // Substitua "Controller" pelo nome real do seu controlador
            data: formData, // Use os dados serializados do formulário
            dataType: "json",
            success: function (data) {


                $("#editUsuarioForm")[0].reset();
                $('.blocker').hide();
                $("#editUsuarioModal").hide()
                $('#cadastroUsuarioClienteModal').modal('show');
                loadUsuarios(clienteId)

                


            },
            error: function (xhr, textStatus, errorThrown) {
                console.error("Erro na requisição Ajax:", errorThrown);
            }
        });
    });




    $("#cadastroUsuarioClienteForm").submit(function (event) {
        event.preventDefault(); // Impede o comportamento padrão do formulário (recarregar a página)

        var formData = $(this).serialize(); // Serializa o formulário
        var clienteId = $("#Usuario_ClienteId").val();

        $.ajax({
            type: "POST",
            url: "/Cliente/JsSetUsuarioCliente", // Substitua "Controller" pelo nome real do seu controlador
            data: formData, // Use os dados serializados do formulário
            dataType: "json",
            success: function (data) {
                alert(data.mensagem);

                $("#cadastroUsuarioClienteForm")[0].reset();



                loadUsuarios(clienteId);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.error("Erro na requisição Ajax:", errorThrown);
            }
        });
    });

    $("#editForm").submit(function (event) {
        event.preventDefault(); // Impede o comportamento padrão do formulário (recarregar a página)

        var formData = $(this).serialize(); // Serializa o formulário

        $.ajax({
            type: "POST",
            url: "/Cliente/JsSetCliente", // Substitua "Controller" pelo nome real do seu controlador
            data: formData, // Use os dados serializados do formulário
            dataType: "json",
            success: function (data) {
                alert(data.mensagem);
                $('.blocker').hide();
                $('#editModal').hide();
                JsGetCliente(0);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.error("Erro na requisição Ajax:", errorThrown);
            }

        });
    });






}


function efetuarLogin() {
    var email = $("#UsuarioLoginEmail").val();
    var senha = $("#UsuarioLoginSenha").val();

    $.ajax({
        type: "POST",
        url: "/Cliente/JsLogin/",
        data: { email: email, senha: senha },
        dataType: "json",
        success: function (data) {
            if (data.success) {
                alert(data.mensagem);
                // Redirecionar para a página relacionada ao cliente após o login
                location.href = "/Cliente/Index/";
            } else {
                alert(data.mensagem);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error("Erro na requisição Ajax:", errorThrown);
        }
    });
}


