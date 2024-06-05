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
 

if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_PART_SMQD_Insert') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_PART_SMQD_Insert 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_PART_SMQDGet') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_PART_SMQDGet
GO

 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_PART_SMQD_Update') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_PART_SMQD_Update
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_PART_SMQD_Delete') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_PART_SMQD_Delete
GO
 

CREATE PROCEDURE TB_R_PART_SMQDGet
	@id varchar(50)
AS
 
select * from TB_R_PART_SMQD where id=@id
GO



CREATE PROCEDURE TB_R_PART_SMQD_Insert 
	@PART_NO nvarchar(max), 
@COLOR_SFX nvarchar(max), 
@PART_NAME nvarchar(max), 
@BACK_NO nvarchar(max), 
@SUPPLIER_CODE nvarchar(max), 
@SMQD_DATETIME DateTime, 
@SMQD_QTY nvarchar(max), 
@SMQD_TYPE nvarchar(max), 
@PIC nvarchar(max), 
@RUN_NO nvarchar(max), 
@REASON nvarchar(max), 
@STATUS nvarchar(max), 
@IS_ACTIVE nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime
AS

INSERT INTO TB_R_PART_SMQD (
	[PART_NO],
	[COLOR_SFX],
	[PART_NAME],
	[BACK_NO],
	[SUPPLIER_CODE],
	[SMQD_DATETIME],
	[SMQD_QTY],
	[SMQD_TYPE],
	[PIC],
	[RUN_NO],
	[REASON],
	[STATUS],
	[IS_ACTIVE],
	[CREATED_BY],
	[CREATED_DATE],
	[UPDATED_BY],
	[UPDATED_DATE]
) VALUES (
	@PART_NO,
	@COLOR_SFX,
	@PART_NAME,
	@BACK_NO,
	@SUPPLIER_CODE,
	@SMQD_DATETIME,
	@SMQD_QTY,
	@SMQD_TYPE,
	@PIC,
	@RUN_NO,
	@REASON,
	@STATUS,
	@IS_ACTIVE,
	@CREATED_BY,
	@CREATED_DATE,
	@UPDATED_BY,
	@UPDATED_DATE
)

select SCOPE_IDENTITY()
GO

CREATE PROCEDURE TB_R_PART_SMQD_Update
	@id varchar(50),
@PART_NO nvarchar(max), 
@COLOR_SFX nvarchar(max), 
@PART_NAME nvarchar(max), 
@BACK_NO nvarchar(max), 
@SUPPLIER_CODE nvarchar(max), 
@SMQD_DATETIME DateTime, 
@SMQD_QTY nvarchar(max), 
@SMQD_TYPE nvarchar(max), 
@PIC nvarchar(max), 
@RUN_NO nvarchar(max), 
@REASON nvarchar(max), 
@STATUS nvarchar(max), 
@IS_ACTIVE nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime
AS
	UPDATE TB_R_PART_SMQD 
	   SET 
		   [PART_NO] = @PART_NO,

		   [COLOR_SFX] = @COLOR_SFX,

		   [PART_NAME] = @PART_NAME,

		   [BACK_NO] = @BACK_NO,

		   [SUPPLIER_CODE] = @SUPPLIER_CODE,

		   [SMQD_DATETIME] = @SMQD_DATETIME,

		   [SMQD_QTY] = @SMQD_QTY,

		   [SMQD_TYPE] = @SMQD_TYPE,

		   [PIC] = @PIC,

		   [RUN_NO] = @RUN_NO,

		   [REASON] = @REASON,

		   [STATUS] = @STATUS,

		   [IS_ACTIVE] = @IS_ACTIVE,

		   [CREATED_BY] = @CREATED_BY,

		   [CREATED_DATE] = @CREATED_DATE,

		   [UPDATED_BY] = @UPDATED_BY,

		   [UPDATED_DATE] = @UPDATED_DATE
	 WHERE 
		   [ID] = @ID
GO
 
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_PART_SMQD_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_PART_SMQD_Delete]
GO
create procedure [dbo].TB_R_PART_SMQD_Delete
@id nvarchar(max)
as
begin try
	delete TB_R_PART_SMQD 
	 where Id in (select * from fnSplit(@id,';'))
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_PART_SMQD_Gets]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_PART_SMQD_Gets]
GO
create procedure [dbo].TB_R_PART_SMQD_Gets
@id varchar(4000)
as
begin try
	select * 
	  from TB_R_PART_SMQD 
	 where id in (select * from fnSplit(@id,';')) OR @id = ''
end try
begin catch
	
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_PART_SMQD_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_PART_SMQD_Get]
GO
create procedure [dbo].TB_R_PART_SMQD_Get 
@id varchar(50)
as
begin try
	select * 
	  from TB_R_PART_SMQD 
	 where id=@id
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_PART_SMQD_Search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_PART_SMQD_Search]
GO
CREATE procedure [dbo].TB_R_PART_SMQD_Search 
as
BEGIN
	SELECT *,ROW_NUMBER() OVER (ORDER BY Id desc) as ThuTuBanGhi
	  FROM dbo.TB_R_PART_SMQD
	 WHERE 1 = 1
END
GO



