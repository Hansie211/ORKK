CREATE TABLE [dbo].[CableChecklistTable] (
    [Cable_ID]                  INT NOT NULL,
    [Rupture_6D]                INT NOT NULL,
    [Rupture_30D]               INT NOT NULL,
    [Damage_Outsides]           INT NOT NULL,
    [Damage_Rust_Corrosion]     INT NOT NULL,
    [Reduced_Cable_Diameter]    INT NOT NULL,
    [Position_Measuring_Points] INT NOT NULL,
    [Total_Damages]             INT NOT NULL,
    [Type_Damage_Rust]          INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Cable_ID] ASC), 
);

