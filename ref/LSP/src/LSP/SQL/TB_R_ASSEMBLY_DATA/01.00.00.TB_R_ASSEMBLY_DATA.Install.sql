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
 

if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_ASSEMBLY_DATA_Insert') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_ASSEMBLY_DATA_Insert 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_ASSEMBLY_DATAGet') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_ASSEMBLY_DATAGet
GO

 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_ASSEMBLY_DATA_Update') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_ASSEMBLY_DATA_Update
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_ASSEMBLY_DATA_Delete') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_ASSEMBLY_DATA_Delete
GO
 

CREATE PROCEDURE TB_R_ASSEMBLY_DATAGet
	@id varchar(50)
AS
 
select * from TB_R_ASSEMBLY_DATA where id=@id
GO



CREATE PROCEDURE TB_R_ASSEMBLY_DATA_Insert 
	@LINE nvarchar(max), 
@PROCESS nvarchar(max), 
@MODEL nvarchar(max), 
@BODY_NO nvarchar(max), 
@SEQ_NO nvarchar(max), 
@GRADE nvarchar(max), 
@LOT_NO nvarchar(max), 
@NO_IN_LOT nvarchar(max), 
@COLOR nvarchar(max), 
@WORKING_DATE nvarchar(max), 
@NO_IN_DATE nvarchar(max), 
@A_IN_DATE_PLAN DateTime, 
@A_IN_TIME_PLAN nvarchar(max), 
@A_IN_DATE_ACTUAL DateTime, 
@A_IN_TIME_ACTUAL nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime
AS

INSERT INTO TB_R_ASSEMBLY_DATA (
	[LINE],
	[PROCESS],
	[MODEL],
	[BODY_NO],
	[SEQ_NO],
	[GRADE],
	[LOT_NO],
	[NO_IN_LOT],
	[COLOR],
	[WORKING_DATE],
	[NO_IN_DATE],
	[A_IN_DATE_PLAN],
	[A_IN_TIME_PLAN],
	[A_IN_DATE_ACTUAL],
	[A_IN_TIME_ACTUAL],
	[CREATED_BY],
	[CREATED_DATE],
	[UPDATED_BY],
	[UPDATED_DATE]
) VALUES (
	@LINE,
	@PROCESS,
	@MODEL,
	@BODY_NO,
	@SEQ_NO,
	@GRADE,
	@LOT_NO,
	@NO_IN_LOT,
	@COLOR,
	@WORKING_DATE,
	@NO_IN_DATE,
	@A_IN_DATE_PLAN,
	@A_IN_TIME_PLAN,
	@A_IN_DATE_ACTUAL,
	@A_IN_TIME_ACTUAL,
	@CREATED_BY,
	@CREATED_DATE,
	@UPDATED_BY,
	@UPDATED_DATE
)

select SCOPE_IDENTITY()
GO

CREATE PROCEDURE TB_R_ASSEMBLY_DATA_Update
	@id varchar(50),
@LINE nvarchar(max), 
@PROCESS nvarchar(max), 
@MODEL nvarchar(max), 
@BODY_NO nvarchar(max), 
@SEQ_NO nvarchar(max), 
@GRADE nvarchar(max), 
@LOT_NO nvarchar(max), 
@NO_IN_LOT nvarchar(max), 
@COLOR nvarchar(max), 
@WORKING_DATE nvarchar(max), 
@NO_IN_DATE nvarchar(max), 
@A_IN_DATE_PLAN DateTime, 
@A_IN_TIME_PLAN nvarchar(max), 
@A_IN_DATE_ACTUAL DateTime, 
@A_IN_TIME_ACTUAL nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime
AS
	UPDATE TB_R_ASSEMBLY_DATA 
	   SET 
		   [LINE] = @LINE,

		   [PROCESS] = @PROCESS,

		   [MODEL] = @MODEL,

		   [BODY_NO] = @BODY_NO,

		   [SEQ_NO] = @SEQ_NO,

		   [GRADE] = @GRADE,

		   [LOT_NO] = @LOT_NO,

		   [NO_IN_LOT] = @NO_IN_LOT,

		   [COLOR] = @COLOR,

		   [WORKING_DATE] = @WORKING_DATE,

		   [NO_IN_DATE] = @NO_IN_DATE,

		   [A_IN_DATE_PLAN] = @A_IN_DATE_PLAN,

		   [A_IN_TIME_PLAN] = @A_IN_TIME_PLAN,

		   [A_IN_DATE_ACTUAL] = @A_IN_DATE_ACTUAL,

		   [A_IN_TIME_ACTUAL] = @A_IN_TIME_ACTUAL,

		   [CREATED_BY] = @CREATED_BY,

		   [CREATED_DATE] = @CREATED_DATE,

		   [UPDATED_BY] = @UPDATED_BY,

		   [UPDATED_DATE] = @UPDATED_DATE
	 WHERE 
		   [ID] = @ID
GO
 
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_ASSEMBLY_DATA_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_ASSEMBLY_DATA_Delete]
GO
create procedure [dbo].TB_R_ASSEMBLY_DATA_Delete
@id nvarchar(max)
as
begin try
	delete TB_R_ASSEMBLY_DATA 
	 where Id in (select * from fnSplit(@id,';'))
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_ASSEMBLY_DATA_Gets]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_ASSEMBLY_DATA_Gets]
GO
create procedure [dbo].TB_R_ASSEMBLY_DATA_Gets
@id varchar(4000)
as
begin try
	select * 
	  from TB_R_ASSEMBLY_DATA 
	 where id in (select * from fnSplit(@id,';')) OR @id = ''
end try
begin catch
	
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_ASSEMBLY_DATA_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_ASSEMBLY_DATA_Get]
GO
create procedure [dbo].TB_R_ASSEMBLY_DATA_Get 
@id varchar(50)
as
begin try
	select * 
	  from TB_R_ASSEMBLY_DATA 
	 where id=@id
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_ASSEMBLY_DATA_Search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_ASSEMBLY_DATA_Search]
GO
CREATE procedure [dbo].TB_R_ASSEMBLY_DATA_Search 
as
BEGIN
	SELECT *,ROW_NUMBER() OVER (ORDER BY Id desc) as ThuTuBanGhi
	  FROM dbo.TB_R_ASSEMBLY_DATA
	 WHERE 1 = 1
END
GO



