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
 

if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_KANBAN_Insert') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_KANBAN_Insert 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_KANBANGet') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_KANBANGet
GO

 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_KANBAN_Update') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_KANBAN_Update
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_KANBAN_Delete') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_KANBAN_Delete
GO
 

CREATE PROCEDURE TB_R_KANBANGet
	@id varchar(50)
AS
 
select * from TB_R_KANBAN where id=@id
GO



CREATE PROCEDURE TB_R_KANBAN_Insert 
	@CONTENT_LIST_ID nvarchar(max), 
@BACK_NO nvarchar(max), 
@PART_NO nvarchar(max), 
@COLOR_SFX nvarchar(max), 
@PART_NAME nvarchar(max), 
@BOX_SIZE nvarchar(max), 
@BOX_QTY nvarchar(max), 
@PC_ADDRESS nvarchar(max), 
@WH_SPS_PICKING nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime, 
@IS_ACTIVE nvarchar(max)
AS

INSERT INTO TB_R_KANBAN (
	[CONTENT_LIST_ID],
	[BACK_NO],
	[PART_NO],
	[COLOR_SFX],
	[PART_NAME],
	[BOX_SIZE],
	[BOX_QTY],
	[PC_ADDRESS],
	[WH_SPS_PICKING],
	[CREATED_BY],
	[CREATED_DATE],
	[UPDATED_BY],
	[UPDATED_DATE],
	[IS_ACTIVE]
) VALUES (
	@CONTENT_LIST_ID,
	@BACK_NO,
	@PART_NO,
	@COLOR_SFX,
	@PART_NAME,
	@BOX_SIZE,
	@BOX_QTY,
	@PC_ADDRESS,
	@WH_SPS_PICKING,
	@CREATED_BY,
	@CREATED_DATE,
	@UPDATED_BY,
	@UPDATED_DATE,
	@IS_ACTIVE
)

select SCOPE_IDENTITY()
GO

CREATE PROCEDURE TB_R_KANBAN_Update
	@id varchar(50),
@CONTENT_LIST_ID nvarchar(max), 
@BACK_NO nvarchar(max), 
@PART_NO nvarchar(max), 
@COLOR_SFX nvarchar(max), 
@PART_NAME nvarchar(max), 
@BOX_SIZE nvarchar(max), 
@BOX_QTY nvarchar(max), 
@PC_ADDRESS nvarchar(max), 
@WH_SPS_PICKING nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime, 
@IS_ACTIVE nvarchar(max)
AS
	UPDATE TB_R_KANBAN 
	   SET 
		   [CONTENT_LIST_ID] = @CONTENT_LIST_ID,

		   [BACK_NO] = @BACK_NO,

		   [PART_NO] = @PART_NO,

		   [COLOR_SFX] = @COLOR_SFX,

		   [PART_NAME] = @PART_NAME,

		   [BOX_SIZE] = @BOX_SIZE,

		   [BOX_QTY] = @BOX_QTY,

		   [PC_ADDRESS] = @PC_ADDRESS,

		   [WH_SPS_PICKING] = @WH_SPS_PICKING,

		   [CREATED_BY] = @CREATED_BY,

		   [CREATED_DATE] = @CREATED_DATE,

		   [UPDATED_BY] = @UPDATED_BY,

		   [UPDATED_DATE] = @UPDATED_DATE,

		   [IS_ACTIVE] = @IS_ACTIVE
	 WHERE 
		   [ID] = @ID
GO
 
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_KANBAN_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_KANBAN_Delete]
GO
create procedure [dbo].TB_R_KANBAN_Delete
@id nvarchar(max)
as
begin try
	delete TB_R_KANBAN 
	 where Id in (select * from fnSplit(@id,';'))
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_KANBAN_Gets]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_KANBAN_Gets]
GO
create procedure [dbo].TB_R_KANBAN_Gets
@id varchar(4000)
as
begin try
	select * 
	  from TB_R_KANBAN 
	 where id in (select * from fnSplit(@id,';')) OR @id = ''
end try
begin catch
	
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_KANBAN_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_KANBAN_Get]
GO
create procedure [dbo].TB_R_KANBAN_Get 
@id varchar(50)
as
begin try
	select * 
	  from TB_R_KANBAN 
	 where id=@id
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_KANBAN_Search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_KANBAN_Search]
GO
CREATE procedure [dbo].TB_R_KANBAN_Search 
as
BEGIN
	SELECT *,ROW_NUMBER() OVER (ORDER BY Id desc) as ThuTuBanGhi
	  FROM dbo.TB_R_KANBAN
	 WHERE 1 = 1
END
GO



