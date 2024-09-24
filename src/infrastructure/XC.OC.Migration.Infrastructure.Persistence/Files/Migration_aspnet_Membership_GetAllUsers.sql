/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetAllUsers]    Script Date: 05-07-2024 12:32:22 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE OR ALTER PROCEDURE [dbo].[Migration_aspnet_Membership_GetAllUsers]
    @ApplicationName       nvarchar(256),
	@UserNamePrefix		   nvarchar(50) = '',
	@StartDate	nvarchar(50),
	@EndDate	nvarchar(50),
    @PageIndex             int = 0,
    @PageSize              int = 50
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0


    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

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

	--Select * from #PageIndexForUsers

    SELECT @TotalRecords = @@ROWCOUNT

    SELECT p.IndexId, u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate,
			pr.PropertyNames,
			pr.PropertyValuesString,
			pr.PropertyValuesBinary
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p, dbo.aspnet_Profile pr
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND pr.UserId = u.UserId
           AND p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
		   --AND u.LoweredUserName LIKE @UserNamePrefix + '\%'
		   --AND p.LastLoginDate >= @StartDate AND p.LastLoginDate < @EndDate
    ORDER BY m.LastLoginDate DESC

	DROP TABLE #PageIndexForUsers

    RETURN @TotalRecords
END