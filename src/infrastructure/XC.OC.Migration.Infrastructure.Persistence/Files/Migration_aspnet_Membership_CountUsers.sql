/****** Object:  StoredProcedure [dbo].[aspnet_Membership_CountUsers_For_Migration]    Script Date: 05-07-2024 20:21:57 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE OR ALTER PROCEDURE [dbo].[Migration_aspnet_Membership_CountUsers]
    @ApplicationName       nvarchar(256),
	@UserNamePrefix		   nvarchar(50) = '',
	@StartDate	nvarchar(50),
	@EndDate	nvarchar(50)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0	

    DECLARE @TotalRecords   int    

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
	SELECT m.UserId
	FROM   dbo.aspnet_Membership m
	WHERE m.UserId in (Select u.UserId from dbo.aspnet_Users u where u.ApplicationId = @ApplicationId AND u.LoweredUserName LIKE LOWER(@UserNamePrefix) + '\%')
	AND m.LastLoginDate >= @StartDate AND m.LastLoginDate < @EndDate
	ORDER BY m.LastLoginDate DESC

    SELECT @TotalRecords = @@ROWCOUNT
    
    RETURN @TotalRecords
END