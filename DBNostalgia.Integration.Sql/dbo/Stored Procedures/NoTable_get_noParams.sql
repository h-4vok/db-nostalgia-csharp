CREATE PROCEDURE NoTable_get_noParams
AS
BEGIN

	SELECT
		Column1 = 'Column1 value is ' + data.Val

	FROM		(
		SELECT Val = '1'
		UNION ALL SELECT Val = '2'
		UNION ALL SELECT Val = '3'

	) DATA

END