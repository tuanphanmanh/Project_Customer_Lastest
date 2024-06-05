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
 

if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_UP_PLAN_H_Insert') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_UP_PLAN_H_Insert 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_UP_PLAN_HGet') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_UP_PLAN_HGet
GO

 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_UP_PLAN_H_Update') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_UP_PLAN_H_Update
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_UP_PLAN_H_Delete') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_UP_PLAN_H_Delete
GO
 

CREATE PROCEDURE TB_R_UP_PLAN_HGet
	@id varchar(50)
AS
 
select * from TB_R_UP_PLAN_H where id=@id
GO



CREATE PROCEDURE TB_R_UP_PLAN_H_Insert 
	@ORDER_NO nvarchar(max), 
@LINE nvarchar(max), 
@CASE_NO nvarchar(max), 
@SUPPLIER_CODE nvarchar(max), 
@UNPACKING_TIME nvarchar(max), 
@UNPACKING_DATE nvarchar(max), 
@NO_IN_DATE nvarchar(max), 
@WORKING_DATE nvarchar(max), 
@SHIFT nvarchar(max), 
@INCOMP_REASON nvarchar(max), 
@UP_STATUS nvarchar(max), 
@IS_ACTIVE nvarchar(max), 
@IS_CURRENT nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime
AS

INSERT INTO TB_R_UP_PLAN_H (
	[ORDER_NO],
	[LINE],
	[CASE_NO],
	[SUPPLIER_CODE],
	[UNPACKING_TIME],
	[UNPACKING_DATE],
	[NO_IN_DATE],
	[WORKING_DATE],
	[SHIFT],
	[INCOMP_REASON],
	[UP_STATUS],
	[IS_ACTIVE],
	[IS_CURRENT],
	[CREATED_BY],
	[CREATED_DATE],
	[UPDATED_BY],
	[UPDATED_DATE]
) VALUES (
	@ORDER_NO,
	@LINE,
	@CASE_NO,
	@SUPPLIER_CODE,
	@UNPACKING_TIME,
	@UNPACKING_DATE,
	@NO_IN_DATE,
	@WORKING_DATE,
	@SHIFT,
	@INCOMP_REASON,
	@UP_STATUS,
	@IS_ACTIVE,
	@IS_CURRENT,
	@CREATED_BY,
	@CREATED_DATE,
	@UPDATED_BY,
	@UPDATED_DATE
)

select SCOPE_IDENTITY()
GO

CREATE PROCEDURE TB_R_UP_PLAN_H_Update
	@id varchar(50),
@ORDER_NO nvarchar(max), 
@LINE nvarchar(max), 
@CASE_NO nvarchar(max), 
@SUPPLIER_CODE nvarchar(max), 
@UNPACKING_TIME nvarchar(max), 
@UNPACKING_DATE nvarchar(max), 
@NO_IN_DATE nvarchar(max), 
@WORKING_DATE nvarchar(max), 
@SHIFT nvarchar(max), 
@INCOMP_REASON nvarchar(max), 
@UP_STATUS nvarchar(max), 
@IS_ACTIVE nvarchar(max), 
@IS_CURRENT nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime
AS
	UPDATE TB_R_UP_PLAN_H 
	   SET 
		   [ORDER_NO] = @ORDER_NO,

		   [LINE] = @LINE,

		   [CASE_NO] = @CASE_NO,

		   [SUPPLIER_CODE] = @SUPPLIER_CODE,

		   [UNPACKING_TIME] = @UNPACKING_TIME,

		   [UNPACKING_DATE] = @UNPACKING_DATE,

		   [NO_IN_DATE] = @NO_IN_DATE,

		   [WORKING_DATE] = @WORKING_DATE,

		   [SHIFT] = @SHIFT,

		   [INCOMP_REASON] = @INCOMP_REASON,

		   [UP_STATUS] = @UP_STATUS,

		   [IS_ACTIVE] = @IS_ACTIVE,

		   [IS_CURRENT] = @IS_CURRENT,

		   [CREATED_BY] = @CREATED_BY,

		   [CREATED_DATE] = @CREATED_DATE,

		   [UPDATED_BY] = @UPDATED_BY,

		   [UPDATED_DATE] = @UPDATED_DATE
	 WHERE 
		   [ID] = @ID
GO
 
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_UP_PLAN_H_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_UP_PLAN_H_Delete]
GO
create procedure [dbo].TB_R_UP_PLAN_H_Delete
@id nvarchar(max)
as
begin try
	delete TB_R_UP_PLAN_H 
	 where Id in (select * from fnSplit(@id,';'))
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_UP_PLAN_H_Gets]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_UP_PLAN_H_Gets]
GO
create procedure [dbo].TB_R_UP_PLAN_H_Gets
@id varchar(4000)
as
begin try
	select * 
	  from TB_R_UP_PLAN_H 
	 where id in (select * from fnSplit(@id,';')) OR @id = ''
end try
begin catch
	
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_UP_PLAN_H_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_UP_PLAN_H_Get]
GO
create procedure [dbo].TB_R_UP_PLAN_H_Get 
@id varchar(50)
as
begin try
	select * 
	  from TB_R_UP_PLAN_H 
	 where id=@id
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_UP_PLAN_H_Search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_UP_PLAN_H_Search]
GO
CREATE procedure [dbo].TB_R_UP_PLAN_H_Search 
as
BEGIN
	SELECT *,ROW_NUMBER() OVER (ORDER BY Id desc) as ThuTuBanGhi
	  FROM dbo.TB_R_UP_PLAN_H
	 WHERE 1 = 1
END
GO



