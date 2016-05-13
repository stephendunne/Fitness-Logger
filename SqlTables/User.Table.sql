CREATE TABLE [dbo].[Table]
(
	[UserId] INT NOT NULL PRIMARY KEY, 
    [Username] CHAR(10) NOT NULL, 
    [Password] NCHAR(10) NOT NULL, 
    [IsMetric] TEXT NOT NULL,
    [IsMale] TEXT NOT NULL,
    [RealName] TEXT NOT NULL,
    [Height] FLOAT NOT NULL,
    [StartWeight] FLOAT NOT NULL,
    [TargetWeight] FLOAT NOT NULL,
    [CalorieAllowance] int NOT NULL,
    [WaterTarget] int NOT NULL,
)
