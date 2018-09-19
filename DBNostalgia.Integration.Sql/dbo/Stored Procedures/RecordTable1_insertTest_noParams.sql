CREATE PROCEDURE RecordTable1_insertTest_noParams
AS
BEGIN

	INSERT RecordTable1 (
		Text1
	)
	VALUES ( 'TestData' )

END
