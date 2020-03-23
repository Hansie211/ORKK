CREATE TABLE [dbo].[CableChecklistTable]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OrderID] INT NOT NULL, 
	--[CableID] INT NOT NULL PRIMARY KEY,
    [rupture_6d] INT NOT NULL, 
    [rupture_30d] INT NOT NULL, 
    [damage_outside] INT NOT NULL, 
    [damage_rust_corrosion] INT NOT NULL, 
    [reduced_cable_diameter] INT NOT NULL, 
    [measuring_point_location] INT NOT NULL, 
    [damage_total] INT NOT NULL, 
    [type_damage_rust_formation] INT NOT NULL, 
    CONSTRAINT [FK_Table_ToTable] FOREIGN KEY ([OrderID]) REFERENCES [OrderTable]([ID]) 
)
