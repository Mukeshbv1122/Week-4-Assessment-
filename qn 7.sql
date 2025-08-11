-- 1. Drop table if it exists
IF OBJECT_ID('dbo.Worker', 'U') IS NOT NULL
    DROP TABLE dbo.Worker;
GO

-- 2. Create table
CREATE TABLE Worker (
    WORKER_ID INT PRIMARY KEY,
    FIRST_NAME CHAR(25) NOT NULL,
    LAST_NAME CHAR(25),
    SALARY INT CHECK (SALARY BETWEEN 10000 AND 25000),
    JOINING_DATE DATETIME,
    DEPARTMENT CHAR(25) CHECK (DEPARTMENT IN ('HR', 'Sales', 'Accts', 'IT'))
);
GO

-- 3. Insert sample data
INSERT INTO Worker (WORKER_ID, FIRST_NAME, LAST_NAME, SALARY, JOINING_DATE, DEPARTMENT) VALUES
(1, 'Mukesh', 'Kumar', 15000, '2023-01-15', 'HR'),
(2, 'Virat', 'Kohli', 20000, '2022-05-10', 'Sales'),
(3, 'Rohit', 'Sharma', 25000, '2024-02-01', 'IT'),
(4, 'Hardik', 'Pandya', 18000, '2023-09-20', 'Accts');
GO

-- 4. Get output
SELECT * FROM Worker;
