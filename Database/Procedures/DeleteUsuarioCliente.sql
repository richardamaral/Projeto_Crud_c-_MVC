use DbArthur
Go
IF OBJECT_ID('dbo.DeleteUsuarioCliente') IS NOT NULL Drop Procedure dbo.DeleteUsuarioCliente
GO
Create Procedure dbo.DeleteUsuarioCliente
(
	@ClienteId		      Int				=       0 
)
As
Begin	

      DELETE FROM UsuarioCliente WHERE ClienteId = @ClienteId
		

End