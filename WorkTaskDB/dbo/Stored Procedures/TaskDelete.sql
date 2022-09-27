
CREATE PROCEDURE [dbo].[TaskDelete]
	@Id VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM dbo.Tasks
	WHERE Id = @Id and StatusId not in (SELECT 1 FROM Statuses WHERE Name like '%completed%' )
	
	RETURN @@ROWCOUNT
END