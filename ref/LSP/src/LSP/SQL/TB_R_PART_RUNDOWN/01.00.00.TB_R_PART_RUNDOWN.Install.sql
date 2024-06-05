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
 

if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_PART_RUNDOWN_Insert') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_PART_RUNDOWN_Insert 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_PART_RUNDOWNGet') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_PART_RUNDOWNGet
GO

 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_PART_RUNDOWN_Update') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_PART_RUNDOWN_Update
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_PART_RUNDOWN_Delete') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_PART_RUNDOWN_Delete
GO
 

CREATE PROCEDURE TB_R_PART_RUNDOWNGet
	@id varchar(50)
AS
 
select * from TB_R_PART_RUNDOWN where id=@id
GO



CREATE PROCEDURE TB_R_PART_RUNDOWN_Insert 
	@PART_ID nvarchar(max), 
@STOCK_QTY nvarchar(max), 
@STOCK_DATE nvarchar(max), 
@IS_ACTIVE nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime
AS

INSERT INTO TB_R_PART_RUNDOWN (
	[PART_ID],
	[STOCK_QTY],
	[STOCK_DATE],
	[IS_ACTIVE],
	[CREATED_BY],
	[CREATED_DATE],
	[UPDATED_BY],
	[UPDATED_DATE]
) VALUES (
	@PART_ID,
	@STOCK_QTY,
	@STOCK_DATE,
	@IS_ACTIVE,
	@CREATED_BY,
	@CREATED_DATE,
	@UPDATED_BY,
	@UPDATED_DATE
)

select SCOPE_IDENTITY()
GO

CREATE PROCEDURE TB_R_PART_RUNDOWN_Update
	@id varchar(50),
@PART_ID nvarchar(max), 
@STOCK_QTY nvarchar(max), 
@STOCK_DATE nvarchar(max), 
@IS_ACTIVE nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime
AS
	UPDATE TB_R_PART_RUNDOWN 
	   SET 
		   [PART_ID] = @PART_ID,

		   [STOCK_QTY] = @STOCK_QTY,

		   [STOCK_DATE] = @STOCK_DATE,

		   [IS_ACTIVE] = @IS_ACTIVE,

		   [CREATED_BY] = @CREATED_BY,

		   [CREATED_DATE] = @CREATED_DATE,

		   [UPDATED_BY] = @UPDATED_BY,

		   [UPDATED_DATE] = @UPDATED_DATE
	 WHERE 
		   [ID] = @ID
GO
 
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_PART_RUNDOWN_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_PART_RUNDOWN_Delete]
GO
create procedure [dbo].TB_R_PART_RUNDOWN_Delete
@id nvarchar(max)
as
begin try
	delete TB_R_PART_RUNDOWN 
	 where Id in (select * from fnSplit(@id,';'))
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_PART_RUNDOWN_Gets]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_PART_RUNDOWN_Gets]
GO
create procedure [dbo].TB_R_PART_RUNDOWN_Gets
@id varchar(4000)
as
begin try
	select * 
	  from TB_R_PART_RUNDOWN 
	 where id in (select * from fnSplit(@id,';')) OR @id = ''
end try
begin catch
	
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_PART_RUNDOWN_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_PART_RUNDOWN_Get]
GO
create procedure [dbo].TB_R_PART_RUNDOWN_Get 
@id varchar(50)
as
begin try
	select * 
	  from TB_R_PART_RUNDOWN 
	 where id=@id
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_PART_RUNDOWN_Search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_PART_RUNDOWN_Search]
GO
CREATE procedure [dbo].TB_R_PART_RUNDOWN_Search 
as
BEGIN
	SELECT *,ROW_NUMBER() OVER (ORDER BY Id desc) as ThuTuBanGhi
	  FROM dbo.TB_R_PART_RUNDOWN
	 WHERE 1 = 1
END
GO



