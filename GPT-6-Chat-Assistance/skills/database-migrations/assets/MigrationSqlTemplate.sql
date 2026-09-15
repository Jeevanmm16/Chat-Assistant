-- Use Idempotent SQL: Always check IF NOT EXISTS before inserting to prevent duplicate errors on retry.
IF NOT EXISTS (SELECT 1 FROM ExampleTable WHERE ExampleName = 'DefaultName')
BEGIN
    INSERT INTO ExampleTable (ExampleName, CreatedDate)
    VALUES ('DefaultName', GETUTCDATE());
END
