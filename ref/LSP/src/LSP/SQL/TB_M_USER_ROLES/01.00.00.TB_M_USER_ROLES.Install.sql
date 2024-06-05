IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnSplit]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fnSplit]
GO
 
 
create FUNCTION [dbo].[fnSplit](
    @sInputList VARCHAR(max) -- List of delimited items
  , @sDelimiter VARCHAR(max) = ',' -- delimiter that separates items
) RETURNS @List TABLE (item VARCHAR(max))

BEGIN
DECLARE @sItem VARCHAR(max)
WHILE CHARINDEX(@sDelimiter,@sInputList,0) <> 0
 BEGIN
 SELECT
  @sItem=RTRIM(LTRIM(SUBSTRING(@sInputList,1,CHARINDEX(@sDelimiter,@sInputList,0)-1))),
  @sInputList=RTRIM(LTRIM(SUBSTRING(@sInputList,CHARINDEX(@sDelimiter,@sInputList,0)+LEN(@sDelimiter),LEN(@sInputList))))
 
 IF LEN(@sItem) > 0
  INSERT INTO @List SELECT @sItem
 END

IF LEN(@sInputList) > 0
 INSERT INTO @List SELECT @sInputList -- Put the last item in
RETURN
END
GO
 

if exists (select * from dbo.sysobjects where id = object_id(N'TB_M_USER_ROLES_Insert') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_M_USER_ROLES_Insert 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_M_USER_ROLESGet') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_M_USER_ROLESGet
GO

 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_M_USER_ROLES_Update') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_M_USER_ROLES_Update
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_M_USER_ROLES_Delete') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_M_USER_ROLES_Delete
GO
 

CREATE PROCEDURE TB_M_USER_ROLESGet
	@id varchar(50)
AS
 
select * from TB_M_USER_ROLES where id=@id
GO



CREATE PROCEDURE TB_M_USER_ROLES_Insert 
	@USER_CC nvarchar(max), 
@TEAM_ID nvarchar(max), 
@SHIFT nvarchar(max), 
@CREATE_DATE DateTime, 
@UPDATE_DATE DateTime
AS

INSERT INTO TB_M_USER_ROLES (
	[USER_CC],
	[TEAM_ID],
	[SHIFT],
	[CREATE_DATE],
	[UPDATE_DATE]
) VALUES (
	@USER_CC,
	@TEAM_ID,
	@SHIFT,
	@CREATE_DATE,
	@UPDATE_DATE
)

select SCOPE_IDENTITY()
GO

CREATE PROCEDURE TB_M_USER_ROLES_Update
	@id varchar(50),
@USER_CC nvarchar(max), 
@TEAM_ID nvarchar(max), 
@SHIFT nvarchar(max), 
@CREATE_DATE DateTime, 
@UPDATE_DATE DateTime
AS
	UPDATE TB_M_USER_ROLES 
	   SET 
		   [USER_CC] = @USER_CC,

		   [TEAM_ID] = @TEAM_ID,

		   [SHIFT] = @SHIFT,

		   [CREATE_DATE] = @CREATE_DATE,

		   [UPDATE_DATE] = @UPDATE_DATE
	 WHERE 
		   [ID] = @ID
GO
 
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_M_USER_ROLES_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_M_USER_ROLES_Delete]
GO
create procedure [dbo].TB_M_USER_ROLES_Delete
@id nvarchar(max)
as
begin try
	delete TB_M_USER_ROLES 
	 where Id in (select * from fnSplit(@id,';'))
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_M_USER_ROLES_Gets]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_M_USER_ROLES_Gets]
GO
create procedure [dbo].TB_M_USER_ROLES_Gets
@id varchar(4000)
as
begin try
	select * 
	  from TB_M_USER_ROLES 
	 where id in (select * from fnSplit(@id,';')) OR @id = ''
end try
begin catch
	
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_M_USER_ROLES_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_M_USER_ROLES_Get]
GO
create procedure [dbo].TB_M_USER_ROLES_Get 
@id varchar(50)
as
begin try
	select * 
	  from TB_M_USER_ROLES 
	 where id=@id
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_M_USER_ROLES_Search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_M_USER_ROLES_Search]
GO
CREATE procedure [dbo].TB_M_USER_ROLES_Search 
as
BEGIN
	SELECT *,ROW_NUMBER() OVER (ORDER BY Id desc) as ThuTuBanGhi
	  FROM dbo.TB_M_USER_ROLES
	 WHERE 1 = 1
END
GO



