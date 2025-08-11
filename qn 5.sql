-- 1. Drop existing function if it exists
IF OBJECT_ID('dbo.CountWords', 'FN') IS NOT NULL
    DROP FUNCTION dbo.CountWords;
GO

-- 2. Create the CountWords function
CREATE FUNCTION dbo.CountWords(@sentence NVARCHAR(MAX))
RETURNS INT
AS
BEGIN
    DECLARE @count INT;

    -- Remove extra spaces at start and end
    SET @sentence = LTRIM(RTRIM(@sentence));

    -- If sentence is empty, return 0
    IF LEN(@sentence) = 0
        RETURN 0;

    -- Count spaces + 1
    SET @count = LEN(@sentence) - LEN(REPLACE(@sentence, ' ', '')) + 1;

    RETURN @count;
END;
GO

-- 3. Run sample queries to get output
SELECT 'Hello Champ, how are you?' AS Sentence, dbo.CountWords('Hello Champ, how are you?') AS WordCount
UNION ALL
SELECT 'SQL Server is powerful', dbo.CountWords('SQL Server is powerful')
UNION ALL
SELECT 'I love learning SQL functions', dbo.CountWords('I love learning SQL functions');


