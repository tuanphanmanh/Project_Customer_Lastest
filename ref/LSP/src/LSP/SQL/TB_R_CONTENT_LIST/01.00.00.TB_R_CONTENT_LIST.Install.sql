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
 

if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_CONTENT_LIST_Insert') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_CONTENT_LIST_Insert 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_CONTENT_LISTGet') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_CONTENT_LISTGet
GO

 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_CONTENT_LIST_Update') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_CONTENT_LIST_Update
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_CONTENT_LIST_Delete') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_CONTENT_LIST_Delete
GO
 

CREATE PROCEDURE TB_R_CONTENT_LISTGet
	@id varchar(50)
AS
 
select * from TB_R_CONTENT_LIST where id=@id
GO



CREATE PROCEDURE TB_R_CONTENT_LIST_Insert 
	@SUPPLIER_NAME nvarchar(max), 
@SUPPLIER_CODE nvarchar(max), 
@RENBAN_NO nvarchar(max), 
@PC_ADDRESS nvarchar(max), 
@DOCK_NO nvarchar(max), 
@ORDER_NO nvarchar(max), 
@ORDER_DATETIME DateTime, 
@TRIP_NO nvarchar(max), 
@PALLET_BOX_QTY nvarchar(max), 
@EST_PACKING_DATETIME DateTime, 
@EST_ARRIVAL_DATETIME DateTime, 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime, 
@IS_ACTIVE nvarchar(max)
AS

INSERT INTO TB_R_CONTENT_LIST (
	[SUPPLIER_NAME],
	[SUPPLIER_CODE],
	[RENBAN_NO],
	[PC_ADDRESS],
	[DOCK_NO],
	[ORDER_NO],
	[ORDER_DATETIME],
	[TRIP_NO],
	[PALLET_BOX_QTY],
	[EST_PACKING_DATETIME],
	[EST_ARRIVAL_DATETIME],
	[CREATED_BY],
	[CREATED_DATE],
	[UPDATED_BY],
	[UPDATED_DATE],
	[IS_ACTIVE]
) VALUES (
	@SUPPLIER_NAME,
	@SUPPLIER_CODE,
	@RENBAN_NO,
	@PC_ADDRESS,
	@DOCK_NO,
	@ORDER_NO,
	@ORDER_DATETIME,
	@TRIP_NO,
	@PALLET_BOX_QTY,
	@EST_PACKING_DATETIME,
	@EST_ARRIVAL_DATETIME,
	@CREATED_BY,
	@CREATED_DATE,
	@UPDATED_BY,
	@UPDATED_DATE,
	@IS_ACTIVE
)

select SCOPE_IDENTITY()
GO

CREATE PROCEDURE TB_R_CONTENT_LIST_Update
	@id varchar(50),
@SUPPLIER_NAME nvarchar(max), 
@SUPPLIER_CODE nvarchar(max), 
@RENBAN_NO nvarchar(max), 
@PC_ADDRESS nvarchar(max), 
@DOCK_NO nvarchar(max), 
@ORDER_NO nvarchar(max), 
@ORDER_DATETIME DateTime, 
@TRIP_NO nvarchar(max), 
@PALLET_BOX_QTY nvarchar(max), 
@EST_PACKING_DATETIME DateTime, 
@EST_ARRIVAL_DATETIME DateTime, 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime, 
@IS_ACTIVE nvarchar(max)
AS
	UPDATE TB_R_CONTENT_LIST 
	   SET 
		   [SUPPLIER_NAME] = @SUPPLIER_NAME,

		   [SUPPLIER_CODE] = @SUPPLIER_CODE,

		   [RENBAN_NO] = @RENBAN_NO,

		   [PC_ADDRESS] = @PC_ADDRESS,

		   [DOCK_NO] = @DOCK_NO,

		   [ORDER_NO] = @ORDER_NO,

		   [ORDER_DATETIME] = @ORDER_DATETIME,

		   [TRIP_NO] = @TRIP_NO,

		   [PALLET_BOX_QTY] = @PALLET_BOX_QTY,

		   [EST_PACKING_DATETIME] = @EST_PACKING_DATETIME,

		   [EST_ARRIVAL_DATETIME] = @EST_ARRIVAL_DATETIME,

		   [CREATED_BY] = @CREATED_BY,

		   [CREATED_DATE] = @CREATED_DATE,

		   [UPDATED_BY] = @UPDATED_BY,

		   [UPDATED_DATE] = @UPDATED_DATE,

		   [IS_ACTIVE] = @IS_ACTIVE
	 WHERE 
		   [ID] = @ID
GO
 
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_CONTENT_LIST_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_CONTENT_LIST_Delete]
GO
create procedure [dbo].TB_R_CONTENT_LIST_Delete
@id nvarchar(max)
as
begin try
	delete TB_R_CONTENT_LIST 
	 where Id in (select * from fnSplit(@id,';'))
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_CONTENT_LIST_Gets]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_CONTENT_LIST_Gets]
GO
create procedure [dbo].TB_R_CONTENT_LIST_Gets
@id varchar(4000)
as
begin try
	select * 
	  from TB_R_CONTENT_LIST 
	 where id in (select * from fnSplit(@id,';')) OR @id = ''
end try
begin catch
	
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_CONTENT_LIST_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_CONTENT_LIST_Get]
GO
create procedure [dbo].TB_R_CONTENT_LIST_Get 
@id varchar(50)
as
begin try
	select * 
	  from TB_R_CONTENT_LIST 
	 where id=@id
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_CONTENT_LIST_Search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_CONTENT_LIST_Search]
GO
CREATE procedure [dbo].TB_R_CONTENT_LIST_Search 
as
BEGIN
	SELECT *,ROW_NUMBER() OVER (ORDER BY Id desc) as ThuTuBanGhi
	  FROM dbo.TB_R_CONTENT_LIST
	 WHERE 1 = 1
END
GO



