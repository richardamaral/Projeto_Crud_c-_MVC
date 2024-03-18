use DbArthur
Go
IF OBJECT_ID('dbo.SetUsuarioCliente') IS NOT NULL Drop Procedure dbo.SetUsuarioCliente
GO
CREATE PROCEDURE dbo.SetUsuarioCliente
(
	 @ClienteId				INT				= 0 
	,@UsuarioClienteId		INT				= 0 
	,@Nome					VARCHAR(256)	= NULL
	,@Email					VARCHAR(32)		= NULL
	,@Senha					VARCHAR(64)		= NULL
	,@Ativo					Bit				= 0
)
AS
BEGIN	 
	IF NOT EXISTS (SELECT * FROM UsuarioCliente WHERE UsuarioClienteId = @UsuarioClienteId)
	BEGIN
		INSERT INTO dbo.UsuarioCliente
		(
			 ClienteId
			,Nome 
			,Email
			,Senha
			,Ativo
		)
		VALUES
		(
			 @ClienteId
			,@Nome
			,@Email
			,@Senha
			,@Ativo
		)

		Set @UsuarioClienteId = @@IDENTITY
	END
	ELSE 
	BEGIN
		UPDATE dbo.UsuarioCliente SET
			 Nome				= ISNULL(@Nome, Nome)
			,Email				= ISNULL(@Email, Email)
			,Senha				= ISNULL(@Senha, Senha)
			,Ativo				= @Ativo 
			,DataAtualizacao	= GetDate()
		WHERE
			UsuarioClienteId = @UsuarioClienteId
	END

	Select @UsuarioClienteId
END