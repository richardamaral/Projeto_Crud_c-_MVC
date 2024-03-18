USE DbArthur
GO
IF OBJECT_ID('dbo.GetUsuarioCliente') IS NOT NULL  DROP PROCEDURE dbo.GetUsuarioCliente
GO
Create Procedure [dbo].[GetUsuarioCliente]
(
	  @ClienteId			Int				= 0
	 ,@UsuarioClienteId		Int				= 0
	 ,@Email				VARCHAR(256)	= Null --'rickcreator155@gmail.com'
	 ,@Senha				VARCHAR(256)	= Null --'12345'
	 ,@AtivoTipo			Int				= 0 -- 0 = Todos, 1 = True(1) e 2 = False(0)
)
As
Begin	

    SELECT
         UsuarioClienteId
        ,Usuario				= U.Nome 
        ,U.Email 
        ,U.Senha
        ,U.Ativo
        ,U.DataCadastro
		,Cliente				= C.Nome
		,C.ClienteId
    FROM
				dbo.UsuarioCliente	U(Nolock)
	Inner Join	dbo.Cliente			C(Nolock) On C.ClienteId = U.ClienteId

    WHERE
			 (@ClienteId			= 0		Or U.ClienteId			= @ClienteId)
		And  (@UsuarioClienteId		= 0		Or U.UsuarioClienteId	= @UsuarioClienteId)
		And  (@Email			Is Null		Or U.Email				= @Email)
		And  (@Senha			Is Null		Or U.Senha				= @Senha)
		And  (@AtivoTipo			= 0		Or U.Ativo				= (Case When @AtivoTipo = 1 Then 1 Else 0 End))
		
END