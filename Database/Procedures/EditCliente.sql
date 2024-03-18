USE DbArthur
GO
IF OBJECT_ID('dbo.EditCliente') IS NOT NULL DROP PROCEDURE dbo.EditCliente
GO

CREATE PROCEDURE dbo.EditCliente
(
	 @ClienteId		INT				= 0,
	 @Nome			VARCHAR(256)	= NULL,
	 @Email			VARCHAR(32)		= NULL,
	 @Celular		VARCHAR(16)		= NULL,
	 @Idade			INT				= 0,
	 @DescricaoFiltro	VARCHAR(128)	= NULL,
	 @PageNumber		INT				= 0,
	 @PageSize			INT				= 0,
	 @OrderColumn		INT				= 0,
	 @OrderDir			CHAR(4)			= NULL
)
AS
BEGIN	
	-- Declare and set variables
	DECLARE @RegCount INT = 0;
	SET @PageNumber = (CASE WHEN @PageNumber = 0 THEN 1 ELSE @PageNumber END);
	SET @PageSize = (CASE WHEN @PageSize = 0 THEN 10 ELSE @PageSize END);

	-- Check if @ClienteId is provided for specific client retrieval
	IF @ClienteId > 0
	BEGIN
		-- Retrieve specific client information
		-- You can modify this part based on your requirements
		SELECT
			ClienteId,
			Nome,
			Email,
			Celular,
			Idade
		FROM dbo.Cliente
		WHERE ClienteId = @ClienteId;
	END
	ELSE
	BEGIN
		-- Retrieve clients based on filter criteria
		SELECT
			ClienteId,
			Nome,
			Email,
			Celular,
			Idade,
			RegCount = COUNT(*) OVER ()
		FROM dbo.Cliente
		WHERE
			(@ClienteId = 0 OR ClienteId = @ClienteId)
			AND (
				@DescricaoFiltro IS NULL
				OR (
					ClienteId LIKE '%' + @DescricaoFiltro + '%'
					OR Email COLLATE SQL_Latin1_General_CP1251_CI_AS LIKE '%' + @DescricaoFiltro + '%'
					OR Nome COLLATE SQL_Latin1_General_CP1251_CI_AS LIKE '%' + @DescricaoFiltro + '%'
					OR Celular COLLATE SQL_Latin1_General_CP1251_CI_AS LIKE '%' + @DescricaoFiltro + '%'
				)
			)
		ORDER BY
			CASE WHEN @OrderColumn = 0 AND @OrderDir = 'asc' THEN ClienteId ELSE '' END ASC,
			CASE WHEN @OrderColumn = 0 AND @OrderDir = 'desc' THEN ClienteId ELSE '' END DESC,
			CASE WHEN @OrderColumn = 1 AND @OrderDir = 'asc' THEN Nome ELSE '' END ASC,
			CASE WHEN @OrderColumn = 1 AND @OrderDir = 'desc' THEN Nome ELSE '' END DESC,
			CASE WHEN @OrderColumn = 2 AND @OrderDir = 'asc' THEN Email ELSE '' END ASC,
			CASE WHEN @OrderColumn = 2 AND @OrderDir = 'desc' THEN Email ELSE '' END DESC,
			CASE WHEN @OrderColumn = 3 AND @OrderDir = 'asc' THEN Celular ELSE '' END ASC,
			CASE WHEN @OrderColumn = 3 AND @OrderDir = 'desc' THEN Celular ELSE '' END DESC,
			CASE WHEN @OrderColumn = 4 AND @OrderDir = 'asc' THEN Idade ELSE '' END ASC,
			CASE WHEN @OrderColumn = 4 AND @OrderDir = 'desc' THEN Idade ELSE '' END DESC,
			ClienteId DESC
		OFFSET ((@PageNumber - 1) * @PageSize) ROWS
		FETCH NEXT @PageSize ROWS ONLY;
	END
END
