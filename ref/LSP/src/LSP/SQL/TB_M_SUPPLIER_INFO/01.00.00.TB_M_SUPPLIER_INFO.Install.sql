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
 

if exists (select * from dbo.sysobjects where id = object_id(N'TB_M_SUPPLIER_INFO_Insert') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_M_SUPPLIER_INFO_Insert 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_M_SUPPLIER_INFOGet') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_M_SUPPLIER_INFOGet
GO

 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_M_SUPPLIER_INFO_Update') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_M_SUPPLIER_INFO_Update
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_M_SUPPLIER_INFO_Delete') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_M_SUPPLIER_INFO_Delete
GO
 

CREATE PROCEDURE TB_M_SUPPLIER_INFOGet
	@id varchar(50)
AS
 
select * from TB_M_SUPPLIER_INFO where id=@id
GO



CREATE PROCEDURE TB_M_SUPPLIER_INFO_Insert 
	@SUPPLIER_CODE nvarchar(max), 
@SUPPLIER_PLANT_CODE nvarchar(max), 
@SUPPLIER_NAME nvarchar(max), 
@ADDRESS nvarchar(max), 
@DOCK_X nvarchar(max), 
@DOCK_X_ADDRESS nvarchar(max), 
@DELIVERY_METHOD nvarchar(max), 
@DELIVERY_FREQUENCY nvarchar(max), 
@CD nvarchar(max), 
@ORDER_DATE_TYPE nvarchar(max), 
@KEIHEN_TYPE nvarchar(max), 
@STK_CONCEPT_TMV_MIN nvarchar(max), 
@STK_CONCEPT_TMV_MAX nvarchar(max), 
@STK_CONCEPT_SUP_M_MIN nvarchar(max), 
@STK_CONCEPT_SUP_M_MAX nvarchar(max), 
@STK_CONCEPT_SUP_P_MIN nvarchar(max), 
@STK_CONCEPT_SUP_P_MAX nvarchar(max), 
@TMV_PRODUCT_PERCENTAGE nvarchar(max), 
@PIC_MAIN_ID nvarchar(max), 
@DELIVERY_LT nvarchar(max), 
@PRODUCTION_SHIFT nvarchar(max), 
@TC_FROM DateTime, 
@TC_TO DateTime, 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime, 
@IS_ACTIVE nvarchar(max)
AS

INSERT INTO TB_M_SUPPLIER_INFO (
	[SUPPLIER_CODE],
	[SUPPLIER_PLANT_CODE],
	[SUPPLIER_NAME],
	[ADDRESS],
	[DOCK_X],
	[DOCK_X_ADDRESS],
	[DELIVERY_METHOD],
	[DELIVERY_FREQUENCY],
	[CD],
	[ORDER_DATE_TYPE],
	[KEIHEN_TYPE],
	[STK_CONCEPT_TMV_MIN],
	[STK_CONCEPT_TMV_MAX],
	[STK_CONCEPT_SUP_M_MIN],
	[STK_CONCEPT_SUP_M_MAX],
	[STK_CONCEPT_SUP_P_MIN],
	[STK_CONCEPT_SUP_P_MAX],
	[TMV_PRODUCT_PERCENTAGE],
	[PIC_MAIN_ID],
	[DELIVERY_LT],
	[PRODUCTION_SHIFT],
	[TC_FROM],
	[TC_TO],
	[CREATED_BY],
	[CREATED_DATE],
	[UPDATED_BY],
	[UPDATED_DATE],
	[IS_ACTIVE]
) VALUES (
	@SUPPLIER_CODE,
	@SUPPLIER_PLANT_CODE,
	@SUPPLIER_NAME,
	@ADDRESS,
	@DOCK_X,
	@DOCK_X_ADDRESS,
	@DELIVERY_METHOD,
	@DELIVERY_FREQUENCY,
	@CD,
	@ORDER_DATE_TYPE,
	@KEIHEN_TYPE,
	@STK_CONCEPT_TMV_MIN,
	@STK_CONCEPT_TMV_MAX,
	@STK_CONCEPT_SUP_M_MIN,
	@STK_CONCEPT_SUP_M_MAX,
	@STK_CONCEPT_SUP_P_MIN,
	@STK_CONCEPT_SUP_P_MAX,
	@TMV_PRODUCT_PERCENTAGE,
	@PIC_MAIN_ID,
	@DELIVERY_LT,
	@PRODUCTION_SHIFT,
	@TC_FROM,
	@TC_TO,
	@CREATED_BY,
	@CREATED_DATE,
	@UPDATED_BY,
	@UPDATED_DATE,
	@IS_ACTIVE
)

select SCOPE_IDENTITY()
GO

CREATE PROCEDURE TB_M_SUPPLIER_INFO_Update
	@id varchar(50),
@SUPPLIER_CODE nvarchar(max), 
@SUPPLIER_PLANT_CODE nvarchar(max), 
@SUPPLIER_NAME nvarchar(max), 
@ADDRESS nvarchar(max), 
@DOCK_X nvarchar(max), 
@DOCK_X_ADDRESS nvarchar(max), 
@DELIVERY_METHOD nvarchar(max), 
@DELIVERY_FREQUENCY nvarchar(max), 
@CD nvarchar(max), 
@ORDER_DATE_TYPE nvarchar(max), 
@KEIHEN_TYPE nvarchar(max), 
@STK_CONCEPT_TMV_MIN nvarchar(max), 
@STK_CONCEPT_TMV_MAX nvarchar(max), 
@STK_CONCEPT_SUP_M_MIN nvarchar(max), 
@STK_CONCEPT_SUP_M_MAX nvarchar(max), 
@STK_CONCEPT_SUP_P_MIN nvarchar(max), 
@STK_CONCEPT_SUP_P_MAX nvarchar(max), 
@TMV_PRODUCT_PERCENTAGE nvarchar(max), 
@PIC_MAIN_ID nvarchar(max), 
@DELIVERY_LT nvarchar(max), 
@PRODUCTION_SHIFT nvarchar(max), 
@TC_FROM DateTime, 
@TC_TO DateTime, 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime, 
@IS_ACTIVE nvarchar(max)
AS
	UPDATE TB_M_SUPPLIER_INFO 
	   SET 
		   [SUPPLIER_CODE] = @SUPPLIER_CODE,

		   [SUPPLIER_PLANT_CODE] = @SUPPLIER_PLANT_CODE,

		   [SUPPLIER_NAME] = @SUPPLIER_NAME,

		   [ADDRESS] = @ADDRESS,

		   [DOCK_X] = @DOCK_X,

		   [DOCK_X_ADDRESS] = @DOCK_X_ADDRESS,

		   [DELIVERY_METHOD] = @DELIVERY_METHOD,

		   [DELIVERY_FREQUENCY] = @DELIVERY_FREQUENCY,

		   [CD] = @CD,

		   [ORDER_DATE_TYPE] = @ORDER_DATE_TYPE,

		   [KEIHEN_TYPE] = @KEIHEN_TYPE,

		   [STK_CONCEPT_TMV_MIN] = @STK_CONCEPT_TMV_MIN,

		   [STK_CONCEPT_TMV_MAX] = @STK_CONCEPT_TMV_MAX,

		   [STK_CONCEPT_SUP_M_MIN] = @STK_CONCEPT_SUP_M_MIN,

		   [STK_CONCEPT_SUP_M_MAX] = @STK_CONCEPT_SUP_M_MAX,

		   [STK_CONCEPT_SUP_P_MIN] = @STK_CONCEPT_SUP_P_MIN,

		   [STK_CONCEPT_SUP_P_MAX] = @STK_CONCEPT_SUP_P_MAX,

		   [TMV_PRODUCT_PERCENTAGE] = @TMV_PRODUCT_PERCENTAGE,

		   [PIC_MAIN_ID] = @PIC_MAIN_ID,

		   [DELIVERY_LT] = @DELIVERY_LT,

		   [PRODUCTION_SHIFT] = @PRODUCTION_SHIFT,

		   [TC_FROM] = @TC_FROM,

		   [TC_TO] = @TC_TO,

		   [CREATED_BY] = @CREATED_BY,

		   [CREATED_DATE] = @CREATED_DATE,

		   [UPDATED_BY] = @UPDATED_BY,

		   [UPDATED_DATE] = @UPDATED_DATE,

		   [IS_ACTIVE] = @IS_ACTIVE
	 WHERE 
		   [ID] = @ID
GO
 
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_M_SUPPLIER_INFO_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_M_SUPPLIER_INFO_Delete]
GO
create procedure [dbo].TB_M_SUPPLIER_INFO_Delete
@id nvarchar(max)
as
begin try
	delete TB_M_SUPPLIER_INFO 
	 where Id in (select * from fnSplit(@id,';'))
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_M_SUPPLIER_INFO_Gets]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_M_SUPPLIER_INFO_Gets]
GO
create procedure [dbo].TB_M_SUPPLIER_INFO_Gets
@id varchar(4000)
as
begin try
	select * 
	  from TB_M_SUPPLIER_INFO 
	 where id in (select * from fnSplit(@id,';')) OR @id = ''
end try
begin catch
	
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_M_SUPPLIER_INFO_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_M_SUPPLIER_INFO_Get]
GO
create procedure [dbo].TB_M_SUPPLIER_INFO_Get 
@id varchar(50)
as
begin try
	select * 
	  from TB_M_SUPPLIER_INFO 
	 where id=@id
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_M_SUPPLIER_INFO_Search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_M_SUPPLIER_INFO_Search]
GO
CREATE procedure [dbo].TB_M_SUPPLIER_INFO_Search 
as
BEGIN
	SELECT *,ROW_NUMBER() OVER (ORDER BY Id desc) as ThuTuBanGhi
	  FROM dbo.TB_M_SUPPLIER_INFO
	 WHERE 1 = 1
END
GO



