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
 

if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_UNLOADING_PLAN_Insert') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_UNLOADING_PLAN_Insert 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_UNLOADING_PLANGet') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_UNLOADING_PLANGet
GO

 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_UNLOADING_PLAN_Update') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_UNLOADING_PLAN_Update
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_UNLOADING_PLAN_Delete') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_UNLOADING_PLAN_Delete
GO
 

CREATE PROCEDURE TB_R_UNLOADING_PLANGet
	@id varchar(50)
AS
 
select * from TB_R_UNLOADING_PLAN where id=@id
GO



CREATE PROCEDURE TB_R_UNLOADING_PLAN_Insert 
	@DOCK nvarchar(max), 
@TRUCK nvarchar(max), 
@SUPPLIERS nvarchar(max), 
@WORKING_DATE nvarchar(max), 
@SHIFT nvarchar(max), 
@SEQUENCE_NO nvarchar(max), 
@PLAN_START_UP_DATETIME DateTime, 
@PLAN_FINISH_UP_DATETIME DateTime, 
@ACTUAL_START_UP_DATETIME DateTime, 
@ACTUAL_FINISH_UP_DATETIME DateTime, 
@REVISED_PLAN_START_UP_DATETIME DateTime, 
@REVISED_PLAN_FINISH_UP_DATETIME DateTime, 
@ACTUAL_START_UP_DELAY nvarchar(max), 
@ACTUAL_FINISH_UP_DELAY nvarchar(max), 
@STATUS nvarchar(max), 
@ISSUES nvarchar(max), 
@CAUSE nvarchar(max), 
@COUTERMEASURE nvarchar(max), 
@PIC_RECORDER nvarchar(max), 
@PIC_ACTION nvarchar(max), 
@ACTION_DUEDATE DateTime, 
@RESULT nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime, 
@IS_ACTIVE nvarchar(max)
AS

INSERT INTO TB_R_UNLOADING_PLAN (
	[DOCK],
	[TRUCK],
	[SUPPLIERS],
	[WORKING_DATE],
	[SHIFT],
	[SEQUENCE_NO],
	[PLAN_START_UP_DATETIME],
	[PLAN_FINISH_UP_DATETIME],
	[ACTUAL_START_UP_DATETIME],
	[ACTUAL_FINISH_UP_DATETIME],
	[REVISED_PLAN_START_UP_DATETIME],
	[REVISED_PLAN_FINISH_UP_DATETIME],
	[ACTUAL_START_UP_DELAY],
	[ACTUAL_FINISH_UP_DELAY],
	[STATUS],
	[ISSUES],
	[CAUSE],
	[COUTERMEASURE],
	[PIC_RECORDER],
	[PIC_ACTION],
	[ACTION_DUEDATE],
	[RESULT],
	[CREATED_BY],
	[CREATED_DATE],
	[UPDATED_BY],
	[UPDATED_DATE],
	[IS_ACTIVE]
) VALUES (
	@DOCK,
	@TRUCK,
	@SUPPLIERS,
	@WORKING_DATE,
	@SHIFT,
	@SEQUENCE_NO,
	@PLAN_START_UP_DATETIME,
	@PLAN_FINISH_UP_DATETIME,
	@ACTUAL_START_UP_DATETIME,
	@ACTUAL_FINISH_UP_DATETIME,
	@REVISED_PLAN_START_UP_DATETIME,
	@REVISED_PLAN_FINISH_UP_DATETIME,
	@ACTUAL_START_UP_DELAY,
	@ACTUAL_FINISH_UP_DELAY,
	@STATUS,
	@ISSUES,
	@CAUSE,
	@COUTERMEASURE,
	@PIC_RECORDER,
	@PIC_ACTION,
	@ACTION_DUEDATE,
	@RESULT,
	@CREATED_BY,
	@CREATED_DATE,
	@UPDATED_BY,
	@UPDATED_DATE,
	@IS_ACTIVE
)

select SCOPE_IDENTITY()
GO

CREATE PROCEDURE TB_R_UNLOADING_PLAN_Update
	@id varchar(50),
@DOCK nvarchar(max), 
@TRUCK nvarchar(max), 
@SUPPLIERS nvarchar(max), 
@WORKING_DATE nvarchar(max), 
@SHIFT nvarchar(max), 
@SEQUENCE_NO nvarchar(max), 
@PLAN_START_UP_DATETIME DateTime, 
@PLAN_FINISH_UP_DATETIME DateTime, 
@ACTUAL_START_UP_DATETIME DateTime, 
@ACTUAL_FINISH_UP_DATETIME DateTime, 
@REVISED_PLAN_START_UP_DATETIME DateTime, 
@REVISED_PLAN_FINISH_UP_DATETIME DateTime, 
@ACTUAL_START_UP_DELAY nvarchar(max), 
@ACTUAL_FINISH_UP_DELAY nvarchar(max), 
@STATUS nvarchar(max), 
@ISSUES nvarchar(max), 
@CAUSE nvarchar(max), 
@COUTERMEASURE nvarchar(max), 
@PIC_RECORDER nvarchar(max), 
@PIC_ACTION nvarchar(max), 
@ACTION_DUEDATE DateTime, 
@RESULT nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime, 
@IS_ACTIVE nvarchar(max)
AS
	UPDATE TB_R_UNLOADING_PLAN 
	   SET 
		   [DOCK] = @DOCK,

		   [TRUCK] = @TRUCK,

		   [SUPPLIERS] = @SUPPLIERS,

		   [WORKING_DATE] = @WORKING_DATE,

		   [SHIFT] = @SHIFT,

		   [SEQUENCE_NO] = @SEQUENCE_NO,

		   [PLAN_START_UP_DATETIME] = @PLAN_START_UP_DATETIME,

		   [PLAN_FINISH_UP_DATETIME] = @PLAN_FINISH_UP_DATETIME,

		   [ACTUAL_START_UP_DATETIME] = @ACTUAL_START_UP_DATETIME,

		   [ACTUAL_FINISH_UP_DATETIME] = @ACTUAL_FINISH_UP_DATETIME,

		   [REVISED_PLAN_START_UP_DATETIME] = @REVISED_PLAN_START_UP_DATETIME,

		   [REVISED_PLAN_FINISH_UP_DATETIME] = @REVISED_PLAN_FINISH_UP_DATETIME,

		   [ACTUAL_START_UP_DELAY] = @ACTUAL_START_UP_DELAY,

		   [ACTUAL_FINISH_UP_DELAY] = @ACTUAL_FINISH_UP_DELAY,

		   [STATUS] = @STATUS,

		   [ISSUES] = @ISSUES,

		   [CAUSE] = @CAUSE,

		   [COUTERMEASURE] = @COUTERMEASURE,

		   [PIC_RECORDER] = @PIC_RECORDER,

		   [PIC_ACTION] = @PIC_ACTION,

		   [ACTION_DUEDATE] = @ACTION_DUEDATE,

		   [RESULT] = @RESULT,

		   [CREATED_BY] = @CREATED_BY,

		   [CREATED_DATE] = @CREATED_DATE,

		   [UPDATED_BY] = @UPDATED_BY,

		   [UPDATED_DATE] = @UPDATED_DATE,

		   [IS_ACTIVE] = @IS_ACTIVE
	 WHERE 
		   [ID] = @ID
GO
 
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_UNLOADING_PLAN_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_UNLOADING_PLAN_Delete]
GO
create procedure [dbo].TB_R_UNLOADING_PLAN_Delete
@id nvarchar(max)
as
begin try
	delete TB_R_UNLOADING_PLAN 
	 where Id in (select * from fnSplit(@id,';'))
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_UNLOADING_PLAN_Gets]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_UNLOADING_PLAN_Gets]
GO
create procedure [dbo].TB_R_UNLOADING_PLAN_Gets
@id varchar(4000)
as
begin try
	select * 
	  from TB_R_UNLOADING_PLAN 
	 where id in (select * from fnSplit(@id,';')) OR @id = ''
end try
begin catch
	
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_UNLOADING_PLAN_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_UNLOADING_PLAN_Get]
GO
create procedure [dbo].TB_R_UNLOADING_PLAN_Get 
@id varchar(50)
as
begin try
	select * 
	  from TB_R_UNLOADING_PLAN 
	 where id=@id
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_UNLOADING_PLAN_Search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_UNLOADING_PLAN_Search]
GO
CREATE procedure [dbo].TB_R_UNLOADING_PLAN_Search 
as
BEGIN
	SELECT *,ROW_NUMBER() OVER (ORDER BY Id desc) as ThuTuBanGhi
	  FROM dbo.TB_R_UNLOADING_PLAN
	 WHERE 1 = 1
END
GO



