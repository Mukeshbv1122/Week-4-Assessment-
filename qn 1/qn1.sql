-- Drop tables if they exist (to avoid errors on re-run)
DROP TABLE IF EXISTS dbo.ContractEmployees;
DROP TABLE IF EXISTS dbo.PayrollEmployees;
DROP TABLE IF EXISTS dbo.Employees;

-- Employees table (common details)
CREATE TABLE Employees (
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    ReportingManager NVARCHAR(100) NOT NULL,
    EmployeeType NVARCHAR(20) NOT NULL -- 'Payroll' or 'Contract'
);

-- PayrollEmployees table
CREATE TABLE PayrollEmployees (
    EmployeeID INT PRIMARY KEY,
    JoiningDate DATE NOT NULL,
    Experience INT NOT NULL,
    BasicSalary DECIMAL(18, 2) NOT NULL,
    DA DECIMAL(18, 2),
    HRA DECIMAL(18, 2),
    PF DECIMAL(18, 2),
    NetSalary DECIMAL(18, 2),
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);

-- ContractEmployees table
CREATE TABLE ContractEmployees (
    EmployeeID INT PRIMARY KEY,
    ContractDate DATE NOT NULL,
    Duration INT NOT NULL, -- Duration in months
    Charges DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);
