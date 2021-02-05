CREATE DATABASE `testdb`;

USE testdb;

CREATE TABLE `departments` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Title` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ManagerId` int DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `employees` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `LastName` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `FirstName` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `MiddleName` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `BirthDate` datetime(6) NOT NULL,
  `DepartmentId` int DEFAULT NULL,
  `Gender` tinyint unsigned NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `orders` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `CustomerName` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreationDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CreatorId` int DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `products` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `OrderId` int DEFAULT NULL,
  `Quantity` int NOT NULL,
  `Price` decimal(18,2) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;



ALTER TABLE departments ADD KEY `IX_Departments_ManagerId` (`ManagerId`);
ALTER TABLE departments ADD CONSTRAINT `FK_Departments_Employees_ManagerId` FOREIGN KEY (`ManagerId`) REFERENCES `employees` (`Id`) ON DELETE SET null;
  

ALTER TABLE employees ADD KEY `IX_Employees_DepartmentId` (`DepartmentId`);
ALTER TABLE employees ADD CONSTRAINT `FK_Employees_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `departments` (`Id`) ON DELETE SET null;
  
ALTER TABLE orders ADD KEY `IX_Orders_CreatorId` (`CreatorId`);
ALTER TABLE orders ADD CONSTRAINT `FK_Orders_Employees_CreatorId` FOREIGN KEY (`CreatorId`) REFERENCES `employees` (`Id`) ON DELETE SET null;
  
ALTER TABLE products ADD KEY `IX_Products_OrderId` (`OrderId`);
ALTER TABLE products ADD CONSTRAINT `FK_Products_Orders_OrderId` FOREIGN KEY (`OrderId`) REFERENCES `orders` (`Id`) ON DELETE CASCADE;