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
 

if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_UP_PLAN_D_Insert') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_UP_PLAN_D_Insert 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_UP_PLAN_DGet') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_UP_PLAN_DGet
GO

 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_UP_PLAN_D_Update') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_UP_PLAN_D_Update
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_UP_PLAN_D_Delete') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_UP_PLAN_D_Delete
GO
 

CREATE PROCEDURE TB_R_UP_PLAN_DGet
	@id varchar(50)
AS
 
select * from TB_R_UP_PLAN_D where id=@id
GO



CREATE PROCEDURE TB_R_UP_PLAN_D_Insert 
	@UP_PLAN_H_ID nvarchar(max), 
@LINE nvarchar(max), 
@NO nvarchar(max), 
@BACK_NO nvarchar(max), 
@CASE_NO nvarchar(max), 
@SUPPLIER_NO nvarchar(max), 
@MODEL nvarchar(max), 
@PART_NO nvarchar(max), 
@PART_NAME nvarchar(max), 
@PC_ADDRESS nvarchar(max), 
@QTY nvarchar(max), 
@BOX_SIZE nvarchar(max), 
@QTY_BOX nvarchar(max), 
@QTY_ACT nvarchar(max), 
@PXP_LOCATION nvarchar(max), 
@WORKING_DATE nvarchar(max), 
@SHIFT nvarchar(max), 
@UP_STATUS nvarchar(max), 
@INCOMP_REASON nvarchar(max), 
@IS_ACTIVE nvarchar(max), 
@IS_OVER nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime
AS

INSERT INTO TB_R_UP_PLAN_D (
	[UP_PLAN_H_ID],
	[LINE],
	[NO],
	[BACK_NO],
	[CASE_NO],
	[SUPPLIER_NO],
	[MODEL],
	[PART_NO],
	[PART_NAME],
	[PC_ADDRESS],
	[QTY],
	[BOX_SIZE],
	[QTY_BOX],
	[QTY_ACT],
	[PXP_LOCATION],
	[WORKING_DATE],
	[SHIFT],
	[UP_STATUS],
	[INCOMP_REASON],
	[IS_ACTIVE],
	[IS_OVER],
	[CREATED_BY],
	[CREATED_DATE],
	[UPDATED_BY],
	[UPDATED_DATE]
) VALUES (
	@UP_PLAN_H_ID,
	@LINE,
	@NO,
	@BACK_NO,
	@CASE_NO,
	@SUPPLIER_NO,
	@MODEL,
	@PART_NO,
	@PART_NAME,
	@PC_ADDRESS,
	@QTY,
	@BOX_SIZE,
	@QTY_BOX,
	@QTY_ACT,
	@PXP_LOCATION,
	@WORKING_DATE,
	@SHIFT,
	@UP_STATUS,
	@INCOMP_REASON,
	@IS_ACTIVE,
	@IS_OVER,
	@CREATED_BY,
	@CREATED_DATE,
	@UPDATED_BY,
	@UPDATED_DATE
)

select SCOPE_IDENTITY()
GO

CREATE PROCEDURE TB_R_UP_PLAN_D_Update
	@id varchar(50),
@UP_PLAN_H_ID nvarchar(max), 
@LINE nvarchar(max), 
@NO nvarchar(max), 
@BACK_NO nvarchar(max), 
@CASE_NO nvarchar(max), 
@SUPPLIER_NO nvarchar(max), 
@MODEL nvarchar(max), 
@PART_NO nvarchar(max), 
@PART_NAME nvarchar(max), 
@PC_ADDRESS nvarchar(max), 
@QTY nvarchar(max), 
@BOX_SIZE nvarchar(max), 
@QTY_BOX nvarchar(max), 
@QTY_ACT nvarchar(max), 
@PXP_LOCATION nvarchar(max), 
@WORKING_DATE nvarchar(max), 
@SHIFT nvarchar(max), 
@UP_STATUS nvarchar(max), 
@INCOMP_REASON nvarchar(max), 
@IS_ACTIVE nvarchar(max), 
@IS_OVER nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime
AS
	UPDATE TB_R_UP_PLAN_D 
	   SET 
		   [UP_PLAN_H_ID] = @UP_PLAN_H_ID,

		   [LINE] = @LINE,

		   [NO] = @NO,

		   [BACK_NO] = @BACK_NO,

		   [CASE_NO] = @CASE_NO,

		   [SUPPLIER_NO] = @SUPPLIER_NO,

		   [MODEL] = @MODEL,

		   [PART_NO] = @PART_NO,

		   [PART_NAME] = @PART_NAME,

		   [PC_ADDRESS] = @PC_ADDRESS,

		   [QTY] = @QTY,

		   [BOX_SIZE] = @BOX_SIZE,

		   [QTY_BOX] = @QTY_BOX,

		   [QTY_ACT] = @QTY_ACT,

		   [PXP_LOCATION] = @PXP_LOCATION,

		   [WORKING_DATE] = @WORKING_DATE,

		   [SHIFT] = @SHIFT,

		   [UP_STATUS] = @UP_STATUS,

		   [INCOMP_REASON] = @INCOMP_REASON,

		   [IS_ACTIVE] = @IS_ACTIVE,

		   [IS_OVER] = @IS_OVER,

		   [CREATED_BY] = @CREATED_BY,

		   [CREATED_DATE] = @CREATED_DATE,

		   [UPDATED_BY] = @UPDATED_BY,

		   [UPDATED_DATE] = @UPDATED_DATE
	 WHERE 
		   [ID] = @ID
GO
 
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_UP_PLAN_D_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_UP_PLAN_D_Delete]
GO
create procedure [dbo].TB_R_UP_PLAN_D_Delete
@id nvarchar(max)
as
begin try
	delete TB_R_UP_PLAN_D 
	 where Id in (select * from fnSplit(@id,';'))
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_UP_PLAN_D_Gets]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_UP_PLAN_D_Gets]
GO
create procedure [dbo].TB_R_UP_PLAN_D_Gets
@id varchar(4000)
as
begin try
	select * 
	  from TB_R_UP_PLAN_D 
	 where id in (select * from fnSplit(@id,';')) OR @id = ''
end try
begin catch
	
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_UP_PLAN_D_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_UP_PLAN_D_Get]
GO
create procedure [dbo].TB_R_UP_PLAN_D_Get 
@id varchar(50)
as
begin try
	select * 
	  from TB_R_UP_PLAN_D 
	 where id=@id
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_UP_PLAN_D_Search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_UP_PLAN_D_Search]
GO
CREATE procedure [dbo].TB_R_UP_PLAN_D_Search 
as
BEGIN
	SELECT *,ROW_NUMBER() OVER (ORDER BY Id desc) as ThuTuBanGhi
	  FROM dbo.TB_R_UP_PLAN_D
	 WHERE 1 = 1
END
GO



