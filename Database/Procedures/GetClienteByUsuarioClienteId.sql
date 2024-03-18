USE DbArthur
GO

IF OBJECT_ID('dbo.GetClienteByUsuarioClienteId') IS NOT NULL DROP PROCEDURE dbo.GetClienteByUsuarioClienteId
GO

CREATE PROCEDURE dbo.GetClienteByUsuarioClienteId
(
    @UsuarioClienteId INT
)
AS
BEGIN
    SELECT
         C.ClienteId
        ,C.Nome
        ,C.Email
        ,C.Celular
        ,C.Idade
        ,Nome_Usuario        = U.Nome
        ,Caminho_Imagem      = C.CaminhoImagem
    FROM
                 dbo.Cliente        C(Nolock)
    INNER JOIN   dbo.UsuarioCliente U(Nolock) ON C.ClienteId = U.ClienteId
    WHERE
        U.UsuarioClienteId = @UsuarioClienteId;
END