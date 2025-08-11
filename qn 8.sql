-- Step 1: Create a unique database
CREATE DATABASE EmployeeDB_Nova;
GO

-- Step 2: Use the newly created database
USE EmployeeDB_Nova;
GO

-- Step 3: Create the Employee table
CREATE TABLE Employee1 (
    EmployeeID INT PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Exp INT,
    Salary DECIMAL(10,2),
    DepartmentName NVARCHAR(50)
);
GO

-- Step 4: Create procedure to Insert or Update
CREATE PROCEDURE InsertOrUpdateEmployee1
    @EmployeeID INT,
    @Name NVARCHAR(50),
    @Exp INT,
    @Salary DECIMAL(10,2),
    @DepartmentName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    MERGE Employee AS target
    USING (SELECT @EmployeeID AS EmployeeID, 
                  @Name AS Name, 
                  @Exp AS Exp, 
                  @Salary AS Salary, 
                  @DepartmentName AS DepartmentName) AS source
    ON (target.EmployeeID = source.EmployeeID)
    WHEN MATCHED THEN
        UPDATE SET Name = source.Name,
                   Exp = source.Exp,
                   Salary = source.Salary,
                   DepartmentName = source.DepartmentName
    WHEN NOT MATCHED THEN
        INSERT (EmployeeID, Name, Exp, Salary, DepartmentName)
        VALUES (source.EmployeeID, source.Name, source.Exp, source.Salary, source.DepartmentName);

    -- Check whether it was insert or update
    IF EXISTS (SELECT 1 FROM Employee WHERE EmployeeID = @EmployeeID AND Name = @Name)
    BEGIN
        PRINT 'Record Inserted or Updated Successfully';
    END
END;
GO

-- Step 5: Test - Insert New Record
EXEC InsertOrUpdateEmployee 1, 'John Doe', 5, 50000, 'IT';

-- Step 6: Test - Update Existing Record
EXEC InsertOrUpdateEmployee 1, 'John Doe', 6, 55000, 'HR';

-- Step 7: View the table to see results
SELECT * FROM Employee;
GO
