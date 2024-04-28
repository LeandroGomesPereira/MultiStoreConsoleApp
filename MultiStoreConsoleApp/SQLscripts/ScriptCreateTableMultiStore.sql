CREATE SCHEMA stage
	CREATE TABLE MultiStore
	(
		RowID int PRIMARY KEY,
		OrderID varchar(15),
		OrderDate int,
		ShipDate int,
		ShipMode varchar(50),
		CustomerID varchar(10),
		CustomerName varchar(50),
		CustomerAge int,
		CustomerBirthday varchar(5),
		CustomerState varchar(20),
		Segment varchar(20),
		Country varchar(50),
		City varchar(50),
		State varchar(50),
		RegionalManagerID varchar(15),
		RegionalManager varchar(50),
		PostalCode int,
		Region varchar(50),
		ProductID varchar(15),
		Category varchar(50),
		SubCategory varchar(50),
		ProductName varchar(50),
		Sales decimal(18, 2),
		Quantity int,
		Discount int,
		Profit decimal(18, 4)
	);
GO