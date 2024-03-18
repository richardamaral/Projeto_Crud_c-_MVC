



	Declare @x Int = 0, @Texto Varchar(256) = '';
	
	While (@x <= 5000)
	Begin
		Set @x = @x + 1;
		Set @Texto = Convert(Varchar(10),@x) + ' Cliente Cadastro teste';
		Exec dbo.SetCliente
			 @Nome		= @Texto
			,@Email		= 'Osmarola@toba.com'
			,@Celular	= '(11) 771771771'
			,@Idade		= @x
	End

	