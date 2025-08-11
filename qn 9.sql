--------------------------------------------------------------------------------
-- Q9: Create DB/table + DeleteEmployee procedure + test run (error-free)
--------------------------------------------------------------------------------

-- 1) Create database if missing (won't drop existing DB)
IF DB_ID('HR_EmployeeDB_Q9') IS NULL
BEGIN
    CREATE DATABASE HR_EmployeeDB_Q9;
END
GO

-- 2) Use the database
USE HR_EmployeeDB_Q9;
GO

-- 3) Ensure a clean Employee table (drop if exists)
IF OBJECT_ID('dbo.Employee', 'U') IS NOT NULL
    DROP TABLE dbo.Employee;
GO

-- 4) Create Employee table
CREATE TABLE dbo.Employee (
    employeeId INT PRIMARY KEY,
    name NVARCHAR(50) NOT NULL,
    exp INT NULL,
    salary DECIMAL(10,2) NULL,
    departmentName NVARCHAR(50) NULL,
    managerId INT NULL
);
GO

-- 5) Insert sample data (employee 1 is manager for 2 and 3)
INSERT INTO dbo.Employee (employeeId, name, exp, salary, departmentName, managerId) VALUES
(1, 'John', 5, 50000.00, 'IT',    NULL),
(2, 'Alice',3, 40000.00, 'HR',    1),
(3, 'Bob',  4, 45000.00, 'Finance',1);
GO

-- 6) Create or alter the DeleteEmployee procedure (safe, prints result)
CREATE OR ALTER PROCEDURE dbo.DeleteEmployee
    @employeeId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- 6.a: Remove this employee as manager for others
    UPDATE dbo.Employee
    SET managerId = NULL
    WHERE managerId = @employeeId;

    -- 6.b: Delete the employee and capture number of deleted rows
    DELETE FROM dbo.Employee
    WHERE employeeId = @employeeId;

    DECLARE @deletedCount INT = @@ROWCOUNT;

    IF @deletedCount > 0
        PRINT 'Record Deleted Successfully';
    ELSE
        PRINT 'No matching record found to delete';
END;
GO

-- 7) Execute the procedure to delete employee with ID = 1
EXEC dbo.DeleteEmployee @employeeId = 1;
GO

-- 8) Show remaining records to verify results
SELECT * FROM dbo.Employee;
GO

--------------------------------------------------------------------------------
-- End of script
--------------------------------------------------------------------------------
