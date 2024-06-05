-- select top 1 username  from TB_M_USER where USERNAME = @userName; 

UPDATE TB_M_USER 
	SET [PASSWORD] = @password		
		, CHANGED_DATE= SYSDATETIME() 
	WHERE USERNAME = @userName
	
