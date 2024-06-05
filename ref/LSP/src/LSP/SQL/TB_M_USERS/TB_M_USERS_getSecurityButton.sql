SELECT DISTINCT
       [APPLICATION]
      ,[FUNCTION] 
      ,[BTN_ID] 
      ,[STATUS]
  FROM  [TB_M_AUTHORIZATION_BTN]
  WHERE [APPLICATION] = @App
	  AND [ROLE] in (select * from fnSplit(@Roles,','))
	  AND [FUNCTION] = @Function
 