CREATE DATABASE Test_db;

CREATE TABLE dbo.Test_Accounts (
    AccountId int,
    FirstName varchar(255),
    LastName varchar(255)
);

Insert Into Test_Accounts values(2344,'Tommy','Test')
,(2233,'Barry'	,'Test')
,(8766,'Sally'	,'Test')
,(2345,'Jerry'	,'Test')
,(2346,'Ollie'	,'Test')
,(2347,'Tara'	,'Test')
,(2348,'Tammy'	,'Test')
,(2349,'Simon'	,'Test')
,(2350,'Colin'	,'Test')
,(2351,'Gladys'	,'Test')
,(2352,'Greg'	,'Test')
,(2353,'Tony'	,'Test')
,(2355,'Arthur'	,'Test')
,(2356,'Craig'	,'Test')
,(6776,'Laura'	,'Test')
,(4534,'JOSH'	,'Test')
,(1234,'Freya'	,'Test')
,(1239,'Noddy'	,'Test')
,(1240,'Archie'	,'Test')
,(1241,'Lara'	,'Test')
,(1242,'Tim'	,'Test')
,(1243,'Graham'	,'Test')
,(1244,'Tony'	,'Test')
,(1245,'Neville','Test')
,(1246,'Jo'		,'Test')
,(1247,'Jim'	,'Test')
,(1248,'Pam'	,'Test');





Select * from Test_Accounts



Create Procedure dbo.CheckTest_AccountNoExist    
@AccountId INT,        
@RefID INT OUTPUT          
AS      
BEGIN     
IF EXISTS(SELECT AccountId FROM Test_Accounts WHERE AccountId=@AccountId)  
BEGIN  
SET @RefID = 1    
END  
ELSE
BEGIN
SET @RefID = 0    
END
END


Create Table dbo.MeterReadings(
AccountId Int,
MeterReadingDateTime varchar(500),
MeterReadValue varchar(20)
)


select * from MeterReadings

 CREATE Procedure dbo.Sp_AddMeterReadings    
@AccountId INT, 
@MeterReadingDateTime varchar(500),
@MeterReadValue varchar(20),
@RefID INT OUTPUT      
      
AS      
BEGIN     
IF EXISTS(SELECT AccountId FROM MeterReadings WHERE AccountId=@AccountId)  
BEGIN  
UPDATE MeterReadings SET MeterReadingDateTime=@MeterReadingDateTime,MeterReadValue=@MeterReadValue WHERE AccountId=@AccountId  
SET @RefID = 1    
END 
ELSE
BEGIN
INSERT INTO MeterReadings VALUES(@AccountId,@MeterReadingDateTime,@MeterReadValue)
SET @RefID = 1
END
END





















