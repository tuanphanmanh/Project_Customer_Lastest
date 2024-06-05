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
 

if exists (select * from dbo.sysobjects where id = object_id(N'TB_M_SUPPLIER_STK_CONCEPT_Insert') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_M_SUPPLIER_STK_CONCEPT_Insert 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_M_SUPPLIER_STK_CONCEPTGet') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_M_SUPPLIER_STK_CONCEPTGet
GO

 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_M_SUPPLIER_STK_CONCEPT_Update') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_M_SUPPLIER_STK_CONCEPT_Update
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_M_SUPPLIER_STK_CONCEPT_Delete') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_M_SUPPLIER_STK_CONCEPT_Delete
GO
 

CREATE PROCEDURE TB_M_SUPPLIER_STK_CONCEPTGet
	@id varchar(50)
AS
 
select * from TB_M_SUPPLIER_STK_CONCEPT where id=@id
GO



CREATE PROCEDURE TB_M_SUPPLIER_STK_CONCEPT_Insert 
	@SUPPLIER_CODE nvarchar(max), 
@MONTH_STK nvarchar(max), 
@MIN_STK_1 nvarchar(max), 
@MIN_STK_2 nvarchar(max), 
@MIN_STK_3 nvarchar(max), 
@MIN_STK_4 nvarchar(max), 
@MIN_STK_5 nvarchar(max), 
@MIN_STK_6 nvarchar(max), 
@MIN_STK_7 nvarchar(max), 
@MIN_STK_8 nvarchar(max), 
@MIN_STK_9 nvarchar(max), 
@MIN_STK_10 nvarchar(max), 
@MIN_STK_11 nvarchar(max), 
@MIN_STK_12 nvarchar(max), 
@MIN_STK_13 nvarchar(max), 
@MIN_STK_14 nvarchar(max), 
@MIN_STK_15 nvarchar(max), 
@MAX_STK_1 nvarchar(max), 
@MAX_STK_2 nvarchar(max), 
@MAX_STK_3 nvarchar(max), 
@MAX_STK_4 nvarchar(max), 
@MAX_STK_5 nvarchar(max), 
@MIN_STK_CONCEPT nvarchar(max), 
@MAX_STK_CONCEPT nvarchar(max), 
@IS_ACTIVE nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime
AS

INSERT INTO TB_M_SUPPLIER_STK_CONCEPT (
	[SUPPLIER_CODE],
	[MONTH_STK],
	[MIN_STK_1],
	[MIN_STK_2],
	[MIN_STK_3],
	[MIN_STK_4],
	[MIN_STK_5],
	[MIN_STK_6],
	[MIN_STK_7],
	[MIN_STK_8],
	[MIN_STK_9],
	[MIN_STK_10],
	[MIN_STK_11],
	[MIN_STK_12],
	[MIN_STK_13],
	[MIN_STK_14],
	[MIN_STK_15],
	[MAX_STK_1],
	[MAX_STK_2],
	[MAX_STK_3],
	[MAX_STK_4],
	[MAX_STK_5],
	[MIN_STK_CONCEPT],
	[MAX_STK_CONCEPT],
	[IS_ACTIVE],
	[CREATED_BY],
	[CREATED_DATE],
	[UPDATED_BY],
	[UPDATED_DATE]
) VALUES (
	@SUPPLIER_CODE,
	@MONTH_STK,
	@MIN_STK_1,
	@MIN_STK_2,
	@MIN_STK_3,
	@MIN_STK_4,
	@MIN_STK_5,
	@MIN_STK_6,
	@MIN_STK_7,
	@MIN_STK_8,
	@MIN_STK_9,
	@MIN_STK_10,
	@MIN_STK_11,
	@MIN_STK_12,
	@MIN_STK_13,
	@MIN_STK_14,
	@MIN_STK_15,
	@MAX_STK_1,
	@MAX_STK_2,
	@MAX_STK_3,
	@MAX_STK_4,
	@MAX_STK_5,
	@MIN_STK_CONCEPT,
	@MAX_STK_CONCEPT,
	@IS_ACTIVE,
	@CREATED_BY,
	@CREATED_DATE,
	@UPDATED_BY,
	@UPDATED_DATE
)

select SCOPE_IDENTITY()
GO

CREATE PROCEDURE TB_M_SUPPLIER_STK_CONCEPT_Update
	@id varchar(50),
@SUPPLIER_CODE nvarchar(max), 
@MONTH_STK nvarchar(max), 
@MIN_STK_1 nvarchar(max), 
@MIN_STK_2 nvarchar(max), 
@MIN_STK_3 nvarchar(max), 
@MIN_STK_4 nvarchar(max), 
@MIN_STK_5 nvarchar(max), 
@MIN_STK_6 nvarchar(max), 
@MIN_STK_7 nvarchar(max), 
@MIN_STK_8 nvarchar(max), 
@MIN_STK_9 nvarchar(max), 
@MIN_STK_10 nvarchar(max), 
@MIN_STK_11 nvarchar(max), 
@MIN_STK_12 nvarchar(max), 
@MIN_STK_13 nvarchar(max), 
@MIN_STK_14 nvarchar(max), 
@MIN_STK_15 nvarchar(max), 
@MAX_STK_1 nvarchar(max), 
@MAX_STK_2 nvarchar(max), 
@MAX_STK_3 nvarchar(max), 
@MAX_STK_4 nvarchar(max), 
@MAX_STK_5 nvarchar(max), 
@MIN_STK_CONCEPT nvarchar(max), 
@MAX_STK_CONCEPT nvarchar(max), 
@IS_ACTIVE nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime
AS
	UPDATE TB_M_SUPPLIER_STK_CONCEPT 
	   SET 
		   [SUPPLIER_CODE] = @SUPPLIER_CODE,

		   [MONTH_STK] = @MONTH_STK,

		   [MIN_STK_1] = @MIN_STK_1,

		   [MIN_STK_2] = @MIN_STK_2,

		   [MIN_STK_3] = @MIN_STK_3,

		   [MIN_STK_4] = @MIN_STK_4,

		   [MIN_STK_5] = @MIN_STK_5,

		   [MIN_STK_6] = @MIN_STK_6,

		   [MIN_STK_7] = @MIN_STK_7,

		   [MIN_STK_8] = @MIN_STK_8,

		   [MIN_STK_9] = @MIN_STK_9,

		   [MIN_STK_10] = @MIN_STK_10,

		   [MIN_STK_11] = @MIN_STK_11,

		   [MIN_STK_12] = @MIN_STK_12,

		   [MIN_STK_13] = @MIN_STK_13,

		   [MIN_STK_14] = @MIN_STK_14,

		   [MIN_STK_15] = @MIN_STK_15,

		   [MAX_STK_1] = @MAX_STK_1,

		   [MAX_STK_2] = @MAX_STK_2,

		   [MAX_STK_3] = @MAX_STK_3,

		   [MAX_STK_4] = @MAX_STK_4,

		   [MAX_STK_5] = @MAX_STK_5,

		   [MIN_STK_CONCEPT] = @MIN_STK_CONCEPT,

		   [MAX_STK_CONCEPT] = @MAX_STK_CONCEPT,

		   [IS_ACTIVE] = @IS_ACTIVE,

		   [CREATED_BY] = @CREATED_BY,

		   [CREATED_DATE] = @CREATED_DATE,

		   [UPDATED_BY] = @UPDATED_BY,

		   [UPDATED_DATE] = @UPDATED_DATE
	 WHERE 
		   [ID] = @ID
GO
 
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_M_SUPPLIER_STK_CONCEPT_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_M_SUPPLIER_STK_CONCEPT_Delete]
GO
create procedure [dbo].TB_M_SUPPLIER_STK_CONCEPT_Delete
@id nvarchar(max)
as
begin try
	delete TB_M_SUPPLIER_STK_CONCEPT 
	 where Id in (select * from fnSplit(@id,';'))
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_M_SUPPLIER_STK_CONCEPT_Gets]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_M_SUPPLIER_STK_CONCEPT_Gets]
GO
create procedure [dbo].TB_M_SUPPLIER_STK_CONCEPT_Gets
@id varchar(4000)
as
begin try
	select * 
	  from TB_M_SUPPLIER_STK_CONCEPT 
	 where id in (select * from fnSplit(@id,';')) OR @id = ''
end try
begin catch
	
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_M_SUPPLIER_STK_CONCEPT_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_M_SUPPLIER_STK_CONCEPT_Get]
GO
create procedure [dbo].TB_M_SUPPLIER_STK_CONCEPT_Get 
@id varchar(50)
as
begin try
	select * 
	  from TB_M_SUPPLIER_STK_CONCEPT 
	 where id=@id
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_M_SUPPLIER_STK_CONCEPT_Search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_M_SUPPLIER_STK_CONCEPT_Search]
GO
CREATE procedure [dbo].TB_M_SUPPLIER_STK_CONCEPT_Search 
as
BEGIN
	SELECT *,ROW_NUMBER() OVER (ORDER BY Id desc) as ThuTuBanGhi
	  FROM dbo.TB_M_SUPPLIER_STK_CONCEPT
	 WHERE 1 = 1
END
GO



