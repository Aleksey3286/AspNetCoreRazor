
CREATE PROCEDURE [dbo].[TaskUpdate]
	@Id INT,
	@Name VARCHAR(100),
	@Priority SMALLINT,
	@StatusId INT
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @ResultValue INT = 0;
	BEGIN TRAN
		IF NOT EXISTS (SELECT 1 FROM dbo.Tasks WHERE Name = @Name)
		BEGIN
			UPDATE dbo.Tasks
			SET Name = @Name,
			    Priority = @Priority,
				StatusId = @StatusId
			WHERE Id = @Id
		END
		ELSE
		BEGIN
			RAISERROR('Task with this name is exist', -1, -1, 'dbo.TaskUpdate') 
		END
		
		SET @ResultValue = @@ERROR  
    
	IF @ResultValue <> 0
		BEGIN  
			ROLLBACK TRAN  
	    END  
	ELSE
		BEGIN  
			COMMIT TRAN  
	    END 
END