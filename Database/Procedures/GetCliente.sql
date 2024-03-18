use DbArthur
Go
IF OBJECT_ID('dbo.GetCliente') IS NOT NULL Drop Procedure dbo.GetCliente
GO
Create Procedure dbo.GetCliente
(
	 @ClienteId							Int				= 0
	,@UsuarioClienteId					Int				= 0
	,@DescricaoFiltro					Varchar(128)	= Null
	,@PageNumber						Int				= 0
    ,@PageSize							Int				= 0
	,@OrderColumn						Int				= 0
	,@OrderDir							Char(4)			= Null

)
As
Begin	

	Declare @RegCount Int	= 0;
	Set		@PageNumber			= (Case When @PageNumber	= 0 Then 1	Else @PageNumber	End);
	Set		@PageSize			= (Case When @PageSize		= 0 Then 10 Else @PageSize		End);


		Select 			
			 C.ClienteId	
			,C.Nome		
			,C.Email		
			,C.Celular	
			,C.Idade		
			,RegCount				= Count(*) Over()
		From 
					dbo.Cliente			C(Nolock)
		Left Join	dbo.UsuarioCliente	U(Nolock) On U.ClienteId = C.ClienteId
		Where
				(@ClienteId			= 0 Or C.ClienteId			= @ClienteId )
			And (@UsuarioClienteId	= 0 Or U.UsuarioClienteId	= @UsuarioClienteId )
			And (
 					 @DescricaoFiltro			Is Null
				OR	(
						C.ClienteId													Like '%'+ @DescricaoFiltro +'%'
					OR	C.Email				collate sql_latin1_general_cp1251_ci_as Like '%'+ @DescricaoFiltro +'%'
					OR	C.Nome				collate sql_latin1_general_cp1251_ci_as Like '%'+ @DescricaoFiltro +'%'
					OR	C.Celular				collate sql_latin1_general_cp1251_ci_as Like '%'+ @DescricaoFiltro +'%'
				)
)
		Order By
			(Case When @OrderColumn = 0 And @OrderDir = 'asc'	Then  C.ClienteId		else '' End) Asc,
			(Case When @OrderColumn = 0 And @OrderDir = 'desc'	Then  C.ClienteId		else '' End) Desc,
			(Case When @OrderColumn = 1 And @OrderDir = 'asc'	Then  C.Nome			else '' End) Asc,
			(Case When @OrderColumn = 1 And @OrderDir = 'desc'	Then  C.Nome			else '' End) Desc,
			(Case When @OrderColumn = 2 And @OrderDir = 'asc'	Then  C.Email			else '' End) Asc,
			(Case When @OrderColumn = 2 And @OrderDir = 'desc'	Then  C.Email			else '' End) Desc,
			(Case When @OrderColumn = 3 And @OrderDir = 'asc'	Then  C.Celular			else '' End) Asc,
			(Case When @OrderColumn = 3 And @OrderDir = 'desc'	Then  C.Celular			else '' End) Desc,
			(Case When @OrderColumn = 4 And @OrderDir = 'asc'	Then  C.Idade			else '' End) Asc,
			(Case When @OrderColumn = 4 And @OrderDir = 'desc'	Then  C.Idade			else '' End) Desc,
		ClienteId Desc
		OFFSET ((@PageNumber - 1) * @PageSize)  ROWS
					FETCH NEXT @PageSize ROWS ONLY 
End