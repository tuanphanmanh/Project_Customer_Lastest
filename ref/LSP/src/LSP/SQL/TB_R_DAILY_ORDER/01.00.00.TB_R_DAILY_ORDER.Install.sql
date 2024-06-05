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
 

if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_DAILY_ORDER_Insert') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_DAILY_ORDER_Insert 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_DAILY_ORDERGet') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_DAILY_ORDERGet
GO

 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_DAILY_ORDER_Update') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_DAILY_ORDER_Update
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_DAILY_ORDER_Delete') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_DAILY_ORDER_Delete
GO
 

CREATE PROCEDURE TB_R_DAILY_ORDERGet
	@id varchar(50)
AS
 
select * from TB_R_DAILY_ORDER where id=@id
GO



CREATE PROCEDURE TB_R_DAILY_ORDER_Insert 
	@WORKING_DATE nvarchar(max), 
@SHIFT nvarchar(max), 
@SUPPLIER_NAME nvarchar(max), 
@SUPPLIER_CODE nvarchar(max), 
@ORDER_NO nvarchar(max), 
@ORDER_DATETIME DateTime, 
@TRIP_NO nvarchar(max), 
@TRUCK_NO nvarchar(max), 
@EST_ARRIVAL_DATETIME DateTime, 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime, 
@IS_ACTIVE nvarchar(max), 
@STATUS nvarchar(max)
AS

INSERT INTO TB_R_DAILY_ORDER (
	[WORKING_DATE],
	[SHIFT],
	[SUPPLIER_NAME],
	[SUPPLIER_CODE],
	[ORDER_NO],
	[ORDER_DATETIME],
	[TRIP_NO],
	[TRUCK_NO],
	[EST_ARRIVAL_DATETIME],
	[CREATED_BY],
	[CREATED_DATE],
	[UPDATED_BY],
	[UPDATED_DATE],
	[IS_ACTIVE],
	[STATUS]
) VALUES (
	@WORKING_DATE,
	@SHIFT,
	@SUPPLIER_NAME,
	@SUPPLIER_CODE,
	@ORDER_NO,
	@ORDER_DATETIME,
	@TRIP_NO,
	@TRUCK_NO,
	@EST_ARRIVAL_DATETIME,
	@CREATED_BY,
	@CREATED_DATE,
	@UPDATED_BY,
	@UPDATED_DATE,
	@IS_ACTIVE,
	@STATUS
)

select SCOPE_IDENTITY()
GO

CREATE PROCEDURE TB_R_DAILY_ORDER_Update
	@id varchar(50),
@WORKING_DATE nvarchar(max), 
@SHIFT nvarchar(max), 
@SUPPLIER_NAME nvarchar(max), 
@SUPPLIER_CODE nvarchar(max), 
@ORDER_NO nvarchar(max), 
@ORDER_DATETIME DateTime, 
@TRIP_NO nvarchar(max), 
@TRUCK_NO nvarchar(max), 
@EST_ARRIVAL_DATETIME DateTime, 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime, 
@IS_ACTIVE nvarchar(max), 
@STATUS nvarchar(max)
AS
	UPDATE TB_R_DAILY_ORDER 
	   SET 
		   [WORKING_DATE] = @WORKING_DATE,

		   [SHIFT] = @SHIFT,

		   [SUPPLIER_NAME] = @SUPPLIER_NAME,

		   [SUPPLIER_CODE] = @SUPPLIER_CODE,

		   [ORDER_NO] = @ORDER_NO,

		   [ORDER_DATETIME] = @ORDER_DATETIME,

		   [TRIP_NO] = @TRIP_NO,

		   [TRUCK_NO] = @TRUCK_NO,

		   [EST_ARRIVAL_DATETIME] = @EST_ARRIVAL_DATETIME,

		   [CREATED_BY] = @CREATED_BY,

		   [CREATED_DATE] = @CREATED_DATE,

		   [UPDATED_BY] = @UPDATED_BY,

		   [UPDATED_DATE] = @UPDATED_DATE,

		   [IS_ACTIVE] = @IS_ACTIVE,

		   [STATUS] = @STATUS
	 WHERE 
		   [ID] = @ID
GO
 
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_DAILY_ORDER_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_DAILY_ORDER_Delete]
GO
create procedure [dbo].TB_R_DAILY_ORDER_Delete
@id nvarchar(max)
as
begin try
	delete TB_R_DAILY_ORDER 
	 where Id in (select * from fnSplit(@id,';'))
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_DAILY_ORDER_Gets]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_DAILY_ORDER_Gets]
GO
create procedure [dbo].TB_R_DAILY_ORDER_Gets
@id varchar(4000)
as
begin try
	select * 
	  from TB_R_DAILY_ORDER 
	 where id in (select * from fnSplit(@id,';')) OR @id = ''
end try
begin catch
	
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_DAILY_ORDER_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_DAILY_ORDER_Get]
GO
create procedure [dbo].TB_R_DAILY_ORDER_Get 
@id varchar(50)
as
begin try
	select * 
	  from TB_R_DAILY_ORDER 
	 where id=@id
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_DAILY_ORDER_Search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_DAILY_ORDER_Search]
GO
CREATE procedure [dbo].TB_R_DAILY_ORDER_Search 
as
BEGIN
	SELECT *,ROW_NUMBER() OVER (ORDER BY Id desc) as ThuTuBanGhi
	  FROM dbo.TB_R_DAILY_ORDER
	 WHERE 1 = 1
END
GO



