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
 

if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_PART_HIKIATE_STOCK_STD_Insert') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_PART_HIKIATE_STOCK_STD_Insert 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_PART_HIKIATE_STOCK_STDGet') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_PART_HIKIATE_STOCK_STDGet
GO

 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_PART_HIKIATE_STOCK_STD_Update') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_PART_HIKIATE_STOCK_STD_Update
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_PART_HIKIATE_STOCK_STD_Delete') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_PART_HIKIATE_STOCK_STD_Delete
GO
 

CREATE PROCEDURE TB_R_PART_HIKIATE_STOCK_STDGet
	@id varchar(50)
AS
 
select * from TB_R_PART_HIKIATE_STOCK_STD where id=@id
GO



CREATE PROCEDURE TB_R_PART_HIKIATE_STOCK_STD_Insert 
	@PART_ID nvarchar(max), 
@MIN_STOCK nvarchar(max), 
@MAX_STOCK nvarchar(max), 
@TC_FROM DateTime, 
@TC_TO DateTime, 
@IS_ACTIVE nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime
AS

INSERT INTO TB_R_PART_HIKIATE_STOCK_STD (
	[PART_ID],
	[MIN_STOCK],
	[MAX_STOCK],
	[TC_FROM],
	[TC_TO],
	[IS_ACTIVE],
	[CREATED_BY],
	[CREATED_DATE],
	[UPDATED_BY],
	[UPDATED_DATE]
) VALUES (
	@PART_ID,
	@MIN_STOCK,
	@MAX_STOCK,
	@TC_FROM,
	@TC_TO,
	@IS_ACTIVE,
	@CREATED_BY,
	@CREATED_DATE,
	@UPDATED_BY,
	@UPDATED_DATE
)

select SCOPE_IDENTITY()
GO

CREATE PROCEDURE TB_R_PART_HIKIATE_STOCK_STD_Update
	@id varchar(50),
@PART_ID nvarchar(max), 
@MIN_STOCK nvarchar(max), 
@MAX_STOCK nvarchar(max), 
@TC_FROM DateTime, 
@TC_TO DateTime, 
@IS_ACTIVE nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime
AS
	UPDATE TB_R_PART_HIKIATE_STOCK_STD 
	   SET 
		   [PART_ID] = @PART_ID,

		   [MIN_STOCK] = @MIN_STOCK,

		   [MAX_STOCK] = @MAX_STOCK,

		   [TC_FROM] = @TC_FROM,

		   [TC_TO] = @TC_TO,

		   [IS_ACTIVE] = @IS_ACTIVE,

		   [CREATED_BY] = @CREATED_BY,

		   [CREATED_DATE] = @CREATED_DATE,

		   [UPDATED_BY] = @UPDATED_BY,

		   [UPDATED_DATE] = @UPDATED_DATE
	 WHERE 
		   [ID] = @ID
GO
 
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_PART_HIKIATE_STOCK_STD_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_PART_HIKIATE_STOCK_STD_Delete]
GO
create procedure [dbo].TB_R_PART_HIKIATE_STOCK_STD_Delete
@id nvarchar(max)
as
begin try
	delete TB_R_PART_HIKIATE_STOCK_STD 
	 where Id in (select * from fnSplit(@id,';'))
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_PART_HIKIATE_STOCK_STD_Gets]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_PART_HIKIATE_STOCK_STD_Gets]
GO
create procedure [dbo].TB_R_PART_HIKIATE_STOCK_STD_Gets
@id varchar(4000)
as
begin try
	select * 
	  from TB_R_PART_HIKIATE_STOCK_STD 
	 where id in (select * from fnSplit(@id,';')) OR @id = ''
end try
begin catch
	
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_PART_HIKIATE_STOCK_STD_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_PART_HIKIATE_STOCK_STD_Get]
GO
create procedure [dbo].TB_R_PART_HIKIATE_STOCK_STD_Get 
@id varchar(50)
as
begin try
	select * 
	  from TB_R_PART_HIKIATE_STOCK_STD 
	 where id=@id
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_PART_HIKIATE_STOCK_STD_Search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_PART_HIKIATE_STOCK_STD_Search]
GO
CREATE procedure [dbo].TB_R_PART_HIKIATE_STOCK_STD_Search 
as
BEGIN
	SELECT *,ROW_NUMBER() OVER (ORDER BY Id desc) as ThuTuBanGhi
	  FROM dbo.TB_R_PART_HIKIATE_STOCK_STD
	 WHERE 1 = 1
END
GO



