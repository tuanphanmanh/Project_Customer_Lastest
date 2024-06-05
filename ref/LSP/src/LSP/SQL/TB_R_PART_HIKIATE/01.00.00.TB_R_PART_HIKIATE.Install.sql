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
 

if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_PART_HIKIATE_Insert') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_PART_HIKIATE_Insert 
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_PART_HIKIATEGet') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_PART_HIKIATEGet
GO

 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_PART_HIKIATE_Update') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_PART_HIKIATE_Update
GO
 
if exists (select * from dbo.sysobjects where id = object_id(N'TB_R_PART_HIKIATE_Delete') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure TB_R_PART_HIKIATE_Delete
GO
 

CREATE PROCEDURE TB_R_PART_HIKIATEGet
	@id varchar(50)
AS
 
select * from TB_R_PART_HIKIATE where id=@id
GO



CREATE PROCEDURE TB_R_PART_HIKIATE_Insert 
	@FAMILY nvarchar(max), 
@PROD_SFX nvarchar(max), 
@PART_NO nvarchar(max), 
@COLOR_SFX nvarchar(max), 
@PART_NAME nvarchar(max), 
@QTY_PER_VEHICLE nvarchar(max), 
@BACK_NO nvarchar(max), 
@PARTS_MACHING_KEY nvarchar(max), 
@SUPPLIER_CODE nvarchar(max), 
@SHOP nvarchar(max), 
@DOCK nvarchar(max), 
@ORGANISATION nvarchar(max), 
@RECEIVING_TIME nvarchar(max), 
@PLANT_TC_FROM nvarchar(max), 
@PLANT_TC_TO nvarchar(max), 
@START_LOT nvarchar(max), 
@END_LOT nvarchar(max), 
@BOX_SIZE nvarchar(max), 
@PACKING_MIX nvarchar(max), 
@BOX_WEIGHT nvarchar(max), 
@BOX_W nvarchar(max), 
@BOX_H nvarchar(max), 
@BOX_L nvarchar(max), 
@PALLET_WEIGHT nvarchar(max), 
@QTY_BOX_PER_PALLET nvarchar(max), 
@PALLET_W nvarchar(max), 
@PALLET_H nvarchar(max), 
@PALLET_L nvarchar(max), 
@UNIT nvarchar(max), 
@COST nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime, 
@IS_ACTIVE nvarchar(max)
AS

INSERT INTO TB_R_PART_HIKIATE (
	[FAMILY],
	[PROD_SFX],
	[PART_NO],
	[COLOR_SFX],
	[PART_NAME],
	[QTY_PER_VEHICLE],
	[BACK_NO],
	[PARTS_MACHING_KEY],
	[SUPPLIER_CODE],
	[SHOP],
	[DOCK],
	[ORGANISATION],
	[RECEIVING_TIME],
	[PLANT_TC_FROM],
	[PLANT_TC_TO],
	[START_LOT],
	[END_LOT],
	[BOX_SIZE],
	[PACKING_MIX],
	[BOX_WEIGHT],
	[BOX_W],
	[BOX_H],
	[BOX_L],
	[PALLET_WEIGHT],
	[QTY_BOX_PER_PALLET],
	[PALLET_W],
	[PALLET_H],
	[PALLET_L],
	[UNIT],
	[COST],
	[CREATED_BY],
	[CREATED_DATE],
	[UPDATED_BY],
	[UPDATED_DATE],
	[IS_ACTIVE]
) VALUES (
	@FAMILY,
	@PROD_SFX,
	@PART_NO,
	@COLOR_SFX,
	@PART_NAME,
	@QTY_PER_VEHICLE,
	@BACK_NO,
	@PARTS_MACHING_KEY,
	@SUPPLIER_CODE,
	@SHOP,
	@DOCK,
	@ORGANISATION,
	@RECEIVING_TIME,
	@PLANT_TC_FROM,
	@PLANT_TC_TO,
	@START_LOT,
	@END_LOT,
	@BOX_SIZE,
	@PACKING_MIX,
	@BOX_WEIGHT,
	@BOX_W,
	@BOX_H,
	@BOX_L,
	@PALLET_WEIGHT,
	@QTY_BOX_PER_PALLET,
	@PALLET_W,
	@PALLET_H,
	@PALLET_L,
	@UNIT,
	@COST,
	@CREATED_BY,
	@CREATED_DATE,
	@UPDATED_BY,
	@UPDATED_DATE,
	@IS_ACTIVE
)

select SCOPE_IDENTITY()
GO

CREATE PROCEDURE TB_R_PART_HIKIATE_Update
	@id varchar(50),
@FAMILY nvarchar(max), 
@PROD_SFX nvarchar(max), 
@PART_NO nvarchar(max), 
@COLOR_SFX nvarchar(max), 
@PART_NAME nvarchar(max), 
@QTY_PER_VEHICLE nvarchar(max), 
@BACK_NO nvarchar(max), 
@PARTS_MACHING_KEY nvarchar(max), 
@SUPPLIER_CODE nvarchar(max), 
@SHOP nvarchar(max), 
@DOCK nvarchar(max), 
@ORGANISATION nvarchar(max), 
@RECEIVING_TIME nvarchar(max), 
@PLANT_TC_FROM nvarchar(max), 
@PLANT_TC_TO nvarchar(max), 
@START_LOT nvarchar(max), 
@END_LOT nvarchar(max), 
@BOX_SIZE nvarchar(max), 
@PACKING_MIX nvarchar(max), 
@BOX_WEIGHT nvarchar(max), 
@BOX_W nvarchar(max), 
@BOX_H nvarchar(max), 
@BOX_L nvarchar(max), 
@PALLET_WEIGHT nvarchar(max), 
@QTY_BOX_PER_PALLET nvarchar(max), 
@PALLET_W nvarchar(max), 
@PALLET_H nvarchar(max), 
@PALLET_L nvarchar(max), 
@UNIT nvarchar(max), 
@COST nvarchar(max), 
@CREATED_BY nvarchar(max), 
@CREATED_DATE DateTime, 
@UPDATED_BY nvarchar(max), 
@UPDATED_DATE DateTime, 
@IS_ACTIVE nvarchar(max)
AS
	UPDATE TB_R_PART_HIKIATE 
	   SET 
		   [FAMILY] = @FAMILY,

		   [PROD_SFX] = @PROD_SFX,

		   [PART_NO] = @PART_NO,

		   [COLOR_SFX] = @COLOR_SFX,

		   [PART_NAME] = @PART_NAME,

		   [QTY_PER_VEHICLE] = @QTY_PER_VEHICLE,

		   [BACK_NO] = @BACK_NO,

		   [PARTS_MACHING_KEY] = @PARTS_MACHING_KEY,

		   [SUPPLIER_CODE] = @SUPPLIER_CODE,

		   [SHOP] = @SHOP,

		   [DOCK] = @DOCK,

		   [ORGANISATION] = @ORGANISATION,

		   [RECEIVING_TIME] = @RECEIVING_TIME,

		   [PLANT_TC_FROM] = @PLANT_TC_FROM,

		   [PLANT_TC_TO] = @PLANT_TC_TO,

		   [START_LOT] = @START_LOT,

		   [END_LOT] = @END_LOT,

		   [BOX_SIZE] = @BOX_SIZE,

		   [PACKING_MIX] = @PACKING_MIX,

		   [BOX_WEIGHT] = @BOX_WEIGHT,

		   [BOX_W] = @BOX_W,

		   [BOX_H] = @BOX_H,

		   [BOX_L] = @BOX_L,

		   [PALLET_WEIGHT] = @PALLET_WEIGHT,

		   [QTY_BOX_PER_PALLET] = @QTY_BOX_PER_PALLET,

		   [PALLET_W] = @PALLET_W,

		   [PALLET_H] = @PALLET_H,

		   [PALLET_L] = @PALLET_L,

		   [UNIT] = @UNIT,

		   [COST] = @COST,

		   [CREATED_BY] = @CREATED_BY,

		   [CREATED_DATE] = @CREATED_DATE,

		   [UPDATED_BY] = @UPDATED_BY,

		   [UPDATED_DATE] = @UPDATED_DATE,

		   [IS_ACTIVE] = @IS_ACTIVE
	 WHERE 
		   [ID] = @ID
GO
 
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_PART_HIKIATE_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_PART_HIKIATE_Delete]
GO
create procedure [dbo].TB_R_PART_HIKIATE_Delete
@id nvarchar(max)
as
begin try
	delete TB_R_PART_HIKIATE 
	 where Id in (select * from fnSplit(@id,';'))
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_PART_HIKIATE_Gets]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_PART_HIKIATE_Gets]
GO
create procedure [dbo].TB_R_PART_HIKIATE_Gets
@id varchar(4000)
as
begin try
	select * 
	  from TB_R_PART_HIKIATE 
	 where id in (select * from fnSplit(@id,';')) OR @id = ''
end try
begin catch
	
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_PART_HIKIATE_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_PART_HIKIATE_Get]
GO
create procedure [dbo].TB_R_PART_HIKIATE_Get 
@id varchar(50)
as
begin try
	select * 
	  from TB_R_PART_HIKIATE 
	 where id=@id
end try
begin catch
	declare @loi nvarchar(max)
	select @loi = ERROR_MESSAGE()
	raiserror 50001 @loi
	return
end catch
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_PART_HIKIATE_Search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[TB_R_PART_HIKIATE_Search]
GO
CREATE procedure [dbo].TB_R_PART_HIKIATE_Search 
as
BEGIN
	SELECT *,ROW_NUMBER() OVER (ORDER BY Id desc) as ThuTuBanGhi
	  FROM dbo.TB_R_PART_HIKIATE
	 WHERE 1 = 1
END
GO



