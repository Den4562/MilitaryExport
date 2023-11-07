Create database MilitaryExport
go use MilitaryExport

---Ген-штаб

--- ВВС
create table [Airplane]
(
[Id] int identity primary key,
[Name] nvarchar(50),
[Count] int,
[Unit_Cost] decimal,
[Total_Cost] decimal 
)

create table [Ammo]
(
[Id] int identity primary key,
[Name] nvarchar(50),
[Count] int,
[Unit_Cost] decimal,
[Total_Cost] decimal 
)

create table [Details]
(
[Id] int identity primary key,
[Name] nvarchar(50),
[Count] int,
[Unit_Cost] decimal,
[Total_Cost] decimal 
)


create table [Air_forces_request]
(
[Id]int identity primary key,
[AirplaneId] int not null foreign key references [Airplane](Id),
[AmmoId] int not null foreign key references [Ammo](Id),
[DetailsId] int not null foreign key references [Details](Id),
[Cost] decimal
)

go

create trigger UpdateAirplaneTotalCost
on Airplane
after insert, update 

as begin
  update a
  set Total_Cost = i.Count * i.Unit_Cost
  from Airplane a
  inner join inserted i on a.Id = i.Id;
end;

go


create trigger UpdateAmmoTotalCost
on Ammo
after insert, update 

as begin
  update a
  set Total_Cost = i.Count * i.Unit_Cost
  from Ammo a
  inner join inserted i on a.Id = i.Id;
end;

go

create trigger UpdateAirDetailsTotalCost
on Details
after insert, update 

as begin
  update d
  set Total_Cost = i.Count * i.Unit_Cost
  from Details d
  inner join inserted i on d.Id = i.Id;
end;

go

create trigger UpdateAirCost
on Air_forces_request
after insert, update 
as begin
  -- Обновляем Cost для каждой комбинации Navy_WeaponId и Navy_DetailsId
  UPDATE aif
  SET Cost = subquery.totalCost
  FROM Air_forces_request aif
  JOIN (
    SELECT i.AirplaneId, i.AmmoId,i.DetailsId, SUM(Air.Total_Cost + Amm.Total_Cost+ Det.Total_Cost) AS totalCost
    FROM inserted i
    JOIN Airplane Air ON i.AirplaneId = Air.Id
    JOIN Ammo Amm ON i.AmmoId = Amm.Id
	JOIN Details Det ON i.DetailsId = Det.Id
    GROUP BY i.AirplaneId, i.AmmoId,i.DetailsId
  ) AS subquery ON aif.AirplaneId = subquery.AirplaneId AND aif.AmmoId = subquery.AmmoId AND aif.DetailsId = subquery.DetailsId;
end;




---Сухопутные силы

create table [Infarny_Weapon]
(
[Id] int identity primary key,
[Name] nvarchar(50),
[Count] int,
[Unit_Cost] decimal,
[Total_Cost] decimal
)

create table [Infantry_equipment]
(
[Id] int identity primary key,
[Name] nvarchar(50),
[Count] int,
[Unit_Cost] decimal,
[Total_Cost] decimal
)

create table [Ground_forces_request]
(
[Id] int identity primary key,
[Infarny_WeaponId] int not null foreign key references [Infarny_Weapon](Id),
[Infantry_equipmentId] int not null foreign key references [Infantry_equipment](Id),
[Cost] decimal
)

go

create trigger Infarny_WeaponTotalCost
on Infarny_Weapon
after insert, update 

as begin
  update inf
  set Total_Cost = i.Count * i.Unit_Cost
  from Infarny_Weapon inf
  inner join inserted i on inf.Id = i.Id;
end;

go

create trigger Infantry_equipmentTotalCost
on Infantry_equipment
after insert, update 

as begin
  update eq
  set Total_Cost = i.Count * i.Unit_Cost
  from Infantry_equipment eq
  inner join inserted i on eq.Id = i.Id;
end;

go

create trigger UpdateGroundCost
on Ground_forces_request
after insert, update 
as begin
  -- Обновляем Cost для каждой комбинации Navy_WeaponId и Navy_DetailsId
  UPDATE gfr
  SET Cost = subquery.totalCost
  FROM Ground_forces_request gfr
  JOIN (
    SELECT i.Infarny_WeaponId, i.Infantry_equipmentId, SUM(we.Total_Cost + eq.Total_Cost) AS totalCost
    FROM inserted i
    JOIN Infarny_Weapon we ON i.Infarny_WeaponId = we.Id
    JOIN Infantry_equipment eq ON i.Infantry_equipmentId = eq.Id
    GROUP BY i.Infarny_WeaponId, i.Infantry_equipmentId
  ) AS subquery ON gfr.Infarny_WeaponId = subquery.Infarny_WeaponId AND gfr.Infantry_equipmentId = subquery.Infantry_equipmentId;
end;


--- ВМС


create table [Navy_Weapon]
(
 [Id] int identity primary key,
 [Name] nvarchar(50),
 [Count] int,
 [Unit_Cost] decimal,
 [Total_Cost] decimal 
)



create table [Navy_Details]
(
[Id] int identity primary key,
[Name] nvarchar(50),
[Count] int,
[Unit_Cost] decimal,
[Total_Cost] decimal 
)

create table [Navy_forces_request]
(
[Id]int identity primary key,
[Navy_WeaponId] int not null foreign key references [Navy_Weapon](Id),
[Navy_DetailsId] int not null foreign key references [Navy_Details](Id),
[Cost] decimal
)

SELECT 
    NFR.Id AS RequestId,
    NW.Name AS WeaponName,
    NW.Count AS WeaponCount,
    NW.Unit_Cost AS WeaponUnitCost,
    NW.Total_Cost AS WeaponTotalCost,
    ND.Name AS DetailsName,
    ND.Count AS DetailsCount,
    ND.Unit_Cost AS DetailsUnitCost,
    ND.Total_Cost AS DetailsTotalCost,
    NFR.Cost AS RequestCost
FROM Navy_forces_request AS NFR
JOIN Navy_Weapon AS NW ON NFR.Navy_WeaponId = NW.Id
JOIN Navy_Details AS ND ON NFR.Navy_DetailsId = ND.Id;

go

--- Триггер для нахождению общей суммы в Таблице Navy_Weapon

create trigger UpdateWeaponTotalCost
on Navy_Weapon
after insert, update 

as begin
  update nw
  set Total_Cost = i.Count * i.Unit_Cost
  from Navy_Weapon nw
  inner join inserted i on nw.Id = i.Id;
end;

go



create trigger UpdateDetailTotalCost
on Navy_Details
after insert, update 

as begin
  update nw
  set Total_Cost = i.Count * i.Unit_Cost
  from Navy_Details nw
  inner join inserted i on nw.Id = i.Id;
end;


go

create trigger UpdateNavyCost
on Navy_forces_request
after insert, update 
as begin
  
  UPDATE nfr
  SET Cost = subquery.totalCost
  FROM Navy_forces_request nfr
  JOIN (
    SELECT i.Navy_WeaponId, i.Navy_DetailsId, SUM(NW.Total_Cost + ND.Total_Cost) AS totalCost
    FROM inserted i
    JOIN Navy_Weapon NW ON i.Navy_WeaponId = NW.Id
    JOIN Navy_Details ND ON i.Navy_DetailsId = ND.Id
    GROUP BY i.Navy_WeaponId, i.Navy_DetailsId
  ) AS subquery ON nfr.Navy_WeaponId = subquery.Navy_WeaponId AND nfr.Navy_DetailsId = subquery.Navy_DetailsId;
end;




create table [Army_Order]
(
[Id] int  identity primary key,
[Ground_forces_requestId] int foreign key references [Ground_forces_request](Id),
[Air_forces_requestId] int foreign key references [Air_forces_request](Id),
[Navy_forces_requestId] int foreign key references [Navy_forces_request](Id),
[Cost] decimal
)
go

create trigger UpdateArmyCost
on Army_Order
after insert, update 
as begin
  
  UPDATE ao
  SET Cost = subquery.Cost
  FROM Army_Order ao
  JOIN (
    SELECT i.Ground_forces_requestId, i.Air_forces_requestId, i.Navy_forces_requestId, SUM(GFR.Cost + AFR.Cost + NFR.Cost) AS Cost
    FROM inserted i
    JOIN Ground_forces_request GFR ON i.Ground_forces_requestId = GFR.Id
    JOIN Air_forces_request AFR ON i.Air_forces_requestId = AFR.Id
    JOIN Navy_forces_request NFR ON i.Navy_forces_requestId = NFR.Id
    GROUP BY i.Ground_forces_requestId, i.Air_forces_requestId, i.Navy_forces_requestId
  ) AS subquery ON ao.Ground_forces_requestId = subquery.Ground_forces_requestId AND ao.Air_forces_requestId = subquery.Air_forces_requestId AND ao.Navy_forces_requestId = subquery.Navy_forces_requestId;
end;



---!!! Министерство Обороны

create table [Order_Ministry_of_Defence]
(
[Id] int identity primary key,
[Army_OrderID]  int not null foreign key references [Army_Order](Id),
[StartDate] datetime not null, 
)

go

CREATE TRIGGER trg_SetCurrentTime
ON [Order_Ministry_of_Defence]
AFTER INSERT
AS
BEGIN
    UPDATE [Order_Ministry_of_Defence]
    SET StartDate = GETDATE()
    WHERE Id IN (SELECT Id FROM INSERTED)
END;



Create table [Account_Ministry]
(
[Id] int identity primary key,
[Login] nvarchar(100),
[Password] nvarchar(100)
)

INSERT INTO [Account_Ministry] ([Login], [Password])
VALUES ('Oleg', '123');

Create table [Account_Command]
(
[Id] int identity primary key,
[Login] nvarchar(100),
[Password] nvarchar(100)
)

INSERT INTO [Account_Command] ([Login], [Password])
VALUES ('Markus', '123');

SELECT OM.[Id] AS MinistryOfDefenceOrderId, OM.[StartDate], OM.[EndDate],
       AO.[Id] AS ArmyOrderId,
       AF.[Id] AS Air_forces_requestId, 
       A.[Name] AS AirplaneName, A.[Count] AS AirplaneCount,
       AM.[Name] AS AmmoName, AM.[Count] AS AmmoCount,
       D.[Name] AS DetailsName, D.[Count] AS DetailsCount
FROM [Order_Ministry_of_Defence] OM
JOIN [Army_Order] AO ON OM.[Id] = AO.[ID]
LEFT JOIN [Air_forces_request] AF ON AO.[Air_forces_requestId] = AF.[Id]
LEFT JOIN [Airplane] A ON AF.[AirplaneId] = A.[Id]
LEFT JOIN [Ammo] AM ON AF.[AmmoId] = AM.[Id]
LEFT JOIN [Details] D ON AF.[DetailsId] = D.[Id]
WHERE OM.[EndDate] >= GETDATE();

---!!!

