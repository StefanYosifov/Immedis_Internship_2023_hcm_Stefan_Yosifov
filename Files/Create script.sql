use [master]
go

Create database [_HCM]
GO

USE [_HCM]
GO

create table Genders
(
	[Id] tinyint primary key identity,
	[Name] varchar(10),
)

GO

create table Countries
(
	[Id] int primary key identity,
	[Name] varchar(100) unique not null,
	[ISO] varchar(5) unique not null,
	[TaxRate] decimal(5,2) null,
)
GO
create table Roles
(
	[Id] int primary key identity,
	[Name] varchar(100) unique not null
)
GO


create table Departments
(
	[Id] int primary key identity,
	[Name] varchar(100) unique not null,
	[MaxPeopleCount] INT,
	[ImageURL] varchar(max) null,
	[CountryId] int FOREIGN KEY REFERENCES Countries(Id),
)

GO

create table Positions
(
	[Id] int primary key identity,
	[Name] varchar(100) unique not null,
	DepartmentId int FOREIGN KEY REFERENCES Departments(Id)
)
GO

create table Seniority
(
	[Id] int primary key identity,
	[Name] varchar(100) unique not null,
)

GO

CREATE TABLE PositionSeniority
(
    PositionId int,
    SeniorityId int,
    FOREIGN KEY (PositionId) REFERENCES Positions(Id),
    FOREIGN KEY (SeniorityId) REFERENCES Seniority(Id)
);
GO

create table Employees
(
	[Id] varchar(450) Primary key,
	[Username] varchar(100) unique not null,
	[Email] varchar(200) unique not null,
	[PasswordHash] varchar(512) not null,
	[FirstName] varchar(100) null,
	[LastName] varchar(100) null,
	[BirthDate] DateTime2 null,
	[PhoneNumber] varchar(10) null,
	[NationalityId] int Foreign Key References Countries(Id),
	[GenderId] tinyint Foreign Key References Genders(Id),
	[DepartmentId] int Foreign Key References Departments(Id),
	[PositionId] int Foreign Key References Positions(Id),
	[SeniorityId] int Foreign Key References Seniority(Id),
	[CreatedOn] DateTime2 null,
	[CreatedBy] varchar(100) null,
	[ModifiedOn] DateTime2 null,
	[ModifiedBy] varchar(100) null,
)
GO

create table EmployeeRoles (
    EmployeeId VARCHAR(450),
    RoleId int,
    PRIMARY KEY (EmployeeId, RoleId),
    FOREIGN KEY (EmployeeId) REFERENCES Employees(Id),
    FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);
GO

create table Salaries (
    EmployeeID varchar(450) PRIMARY KEY,
    SalaryAmount DECIMAL(10, 2),
    EffectiveDate DATE,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(Id)
);
GO

create table Payroll (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    EmployeeID varchar(450) FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    Salary DECIMAL(10, 2) NOT NULL,
	Bonuses Decimal(10,2) NOT NULL,
    Deductions DECIMAL(10, 2) NOT NULL,
	GrossPay Decimal(10,2) NOT NULL,
    NetPay DECIMAL(10, 2) NOT NULL,
	CreatedOn DateTime2 NOT NULL,
	PaidOn DateTime2,
	DeletedOn DateTime2,
	DeletedBy VARCHAR(1000),
	IsDeleted bit NOT NULL,
);


create table BonusesReasons(
	[Id] INT primary key identity,
	[Name] varchar(500) not null,
)

create table Bonuses(
	[Id] BIGINT primary key identity,
	[Amount] decimal(10,2) not null,
	[CreatedOn] DateTime2 not null,
	[CreatedBy] varchar(100) not null,
	[EmployeeId] varchar(450) FOREIGN KEY REFERENCES Employees(Id),
	[ReasonId] INT FOREIGN KEY REFERENCES BonusesReasons(Id),
	[PayrollId] INT FOREIGN KEY REFERENCES Payroll(Id),
)


create table DeductionReasons(
	[Id] INT primary key identity,
	[Name] varchar(500) not null,
)

create table Deductions(
	[Id] BIGINT PRIMARY KEY IDENTITY,
	[Amount] decimal(10,2),
	[CreatedOn] DateTime2 not null,
	[CreatedBy] varchar(100) not null,
	[EmployeeId] varchar(450) FOREIGN KEY REFERENCES Employees(Id),
	[ReasonId] INT FOREIGN KEY REFERENCES DeductionReasons(Id),
	[PayrollId] INT FOREIGN KEY REFERENCES Payroll(Id)
)


CREATE TABLE Priority (
    Id INT PRIMARY KEY Identity,
    PriorityName VARCHAR(50)
);

CREATE TABLE Status (
    [Id] INT PRIMARY KEY Identity,
    [StatusName] VARCHAR(50)
);

CREATE TABLE Tasks (
    [Id] INT PRIMARY KEY Identity,
    TaskName VARCHAR(255),
    Description TEXT,
    DueDate DATE,
    PriorityID INT FOREIGN KEY REFERENCES Priority(Id),
    StatusID INT FOREIGN KEY REFERENCES Status(Id),
	EmployeeID VARCHAR(450) FOREIGN KEY REFERENCES Employees(Id),
	IssuerId VARCHAR(450) ForeIGN KEY REFERENCES Employees(Id),
);

CREATE TABLE AuditLog
(
    Id BIGINT PRIMARY KEY IDENTITY,
    EmployeeId VARCHAR(450) FOREIGN KEY REFERENCES Employees(Id),
    EntityName VARCHAR(255) NOT NULL,
    Action VARCHAR(255) NOT NULL,
    Timestamp DATETIME NOT NULL,
    Changes VARCHAR(MAX) NOT NULL
);
