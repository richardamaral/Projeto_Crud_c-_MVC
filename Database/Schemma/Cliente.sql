Use DbArthur
Go
Drop Table dbo.Cliente
go
Create Table dbo.Cliente
(
	 ClienteId		Int				Primary Key Identity 
	,Nome			Varchar(256)	Not Null
	,Email			Varchar(32)		Not Null
	,Celular		Varchar(16)		Not Null
	,Idade			Int				Not Null
	,CaminhoImagem Varchar(MAX)     Not Null
)

Go
--Drop Table dbo.UsuarioCliente
Create Table dbo.UsuarioCliente
(
	 UsuarioClienteId	Int				Primary Key Identity
	,ClienteId			Int References Cliente(ClienteId)
	,Nome				Varchar(256)	Not Null
	,Email				Varchar(256)	Not Null
	,Senha				Varchar(256)	Not Null
	,Ativo				Bit				Default(0) Not Null
	,DataCadastro		DateTime		Default(GetDate()) Not Null
	,DataAtualizacao	DateTime		

)