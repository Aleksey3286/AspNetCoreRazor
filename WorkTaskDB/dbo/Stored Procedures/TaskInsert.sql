-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
CREATE PROCEDURE [dbo].[TaskInsert]
	@Name VARCHAR(100),
	@Priority SMALLINT,
	@StatusId INT,
	@Id INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @ResultValue INT = 0;
	BEGIN TRAN
		IF NOT EXISTS (SELECT 1 FROM dbo.Tasks WHERE Name = @Name)
		BEGIN
			INSERT INTO dbo.Tasks(Name, Priority, StatusId)
			VALUES(@Name, @Priority, @StatusId)
		END
		ELSE
		BEGIN
			RAISERROR('Task with this name is exist', -1, -1, 'dbo.TaskInsert') 
		END
		
		SET @ResultValue = @@ERROR  
    
	IF @ResultValue <> 0
		BEGIN  
			ROLLBACK TRAN  
	    END  
	ELSE
		BEGIN  
			SET @Id = SCOPE_IDENTITY()
			COMMIT TRAN  
	    END  

	RETURN @Id 
END