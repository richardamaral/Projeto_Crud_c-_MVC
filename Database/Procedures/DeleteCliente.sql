use DbArthur
Go
IF OBJECT_ID('dbo.DeleteCliente') IS NOT NULL Drop Procedure dbo.DeleteCliente
GO
Create Procedure dbo.DeleteCliente
(
	 @ClienteId		Int				= 0 
)
As
Begin	
        Delete From UsuarioCliente Where ClienteId = @ClienteId;
		Delete From Cliente Where ClienteId = @ClienteId
		

		 Select @ClienteId
End