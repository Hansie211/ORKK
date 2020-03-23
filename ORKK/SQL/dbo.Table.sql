CREATE TABLE [dbo].[OrderTable]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [instruction] INT NOT NULL, 
    [date_execution] DATETIME NOT NULL, 
    [cablesupplier] NVARCHAR(250) NOT NULL, 
    [comment] NVARCHAR(500) NOT NULL, 
    [signature] IMAGE NOT NULL, 
    [hour_count] INT NOT NULL, 
    [reason] NVARCHAR(500) NOT NULL, 
    CONSTRAINT [AK_OrderTable_Column] UNIQUE ([ID])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Het nummer van de opdracht',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderTable',
    @level2type = N'COLUMN',
    @level2name = 'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'De werkinstructie',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderTable',
    @level2type = N'COLUMN',
    @level2name = N'instruction'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'De datum van uitvoering van de opdracht',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderTable',
    @level2type = N'COLUMN',
    @level2name = N'date_execution'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'De leverancier van de kabel',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderTable',
    @level2type = N'COLUMN',
    @level2name = N'cablesupplier'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Overige waarnemingen',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderTable',
    @level2type = N'COLUMN',
    @level2name = N'comment'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Handtekening controleur',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderTable',
    @level2type = N'COLUMN',
    @level2name = N'signature'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Aantal uren dat de kabel in bedrijf is.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderTable',
    @level2type = N'COLUMN',
    @level2name = N'hour_count'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Redenen voor het afleggen',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderTable',
    @level2type = N'COLUMN',
    @level2name = N'reason'
