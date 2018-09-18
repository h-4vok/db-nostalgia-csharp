CREATE PROCEDURE NoTable_getOne_noParams
AS
BEGIN

	SELECT 
		Column1 = 'This is data from Column1'
	FROM		(
		SELECT 'Nothing' = 1 
		UNION ALL SELECT 'Nothing' = 1 
		UNION ALL SELECT 'Nothing' = 1) DATA -- We use this FROM to generate 3 rows, but we only fetch one on this test

END