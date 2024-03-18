USE DbArthur
GO

IF OBJECT_ID('dbo.GetUsuarioClienteByEmail') IS NOT NULL
    DROP PROCEDURE dbo.GetUsuarioClienteByEmail
GO

CREATE PROCEDURE dbo.GetUsuarioClienteByEmail
(
    @Email VARCHAR(256)
)
AS
BEGIN
    SELECT
        UC.UsuarioClienteId,
        UC.Nome AS UsuarioNome,
        UC.Email,
        UC.Senha,
        UC.Ativo,
        UC.DataCadastro,
        C.ClienteId,
        C.Nome AS ClienteNome,
        C.Celular,
        C.Idade
    FROM
        dbo.UsuarioCliente UC (NOLOCK)
    INNER JOIN
        dbo.Cliente C (NOLOCK) ON UC.ClienteId = C.ClienteId
    WHERE
        UC.Email = @Email;
END