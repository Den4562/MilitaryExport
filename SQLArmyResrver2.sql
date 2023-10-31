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

-- Добавление записей в таблицу Airplane
INSERT INTO [Airplane] ([Name], [Count], [Unit_Cost], [Total_Cost])
VALUES ('Airplane 1', 10, 50000.00, 0.00),
       ('Airplane 2', 5, 75000.00, 0.00),
       ('Airplane 3', 8, 60000.00, 0.00),
       ('Airplane 4', 12, 55000.00, 0.00),
       ('Airplane 5', 6, 70000.00, 0.00);

-- Добавление записей в таблицу Ammo
INSERT INTO [Ammo] ([Name], [Count], [Unit_Cost], [Total_Cost])
VALUES ('Ammo 1', 1000, 10.00, 0.00),
       ('Ammo 2', 1500, 8.00, 0.00),
       ('Ammo 3', 800, 12.00, 0.00),
       ('Ammo 4', 1200, 9.00, 0.00),
       ('Ammo 5', 1600, 7.00, 0.00);

-- Добавление записей в таблицу Details
INSERT INTO [Details] ([Name], [Count], [Unit_Cost], [Total_Cost])
VALUES ('Details 1', 200, 100.00, 0.00),
       ('Details 2', 150, 120.00, 0.00),
       ('Details 3', 100, 80.00, 0.00),
       ('Details 4', 180, 110.00, 0.00),
       ('Details 5', 220, 95.00, 0.00);

-- Добавление записей в таблицу Air_forces_request
INSERT INTO [Air_forces_request] ([AirplaneId], [AmmoId], [DetailsId], [Cost])
VALUES (1, 1, 1, 0.00),
       (2, 2, 2, 0.00),
       (3, 3, 3, 0.00),
       (4, 4, 4, 0.00),
       (5, 5, 5, 0.00);

---


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


INSERT INTO [Infarny_Weapon] ([Name], [Count], [Unit_Cost])
VALUES ('Weapon 1', 100, 500.00),
       ('Weapon 2', 50, 750.00),
       ('Weapon 3', 75, 600.00),
       ('Weapon 4', 60, 550.00),
       ('Weapon 5', 40, 720.00);

	   INSERT INTO [Infantry_equipment] ([Name], [Count], [Unit_Cost])
VALUES ('Equipment 1', 200, 100.00),
       ('Equipment 2', 150, 120.00),
       ('Equipment 3', 100, 80.00),
       ('Equipment 4', 180, 90.00),
       ('Equipment 5', 220, 110.00);

	   INSERT INTO [Ground_forces_request] ([Infarny_WeaponId], [Infantry_equipmentId], [Cost])
VALUES (1, 1, 0.00),
       (2, 2, 0.00),
       (3, 3, 0.00),
       (4, 4, 0.00),
       (5, 5, 0.00);

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

INSERT INTO [Navy_Weapon] ([Name], [Count], [Unit_Cost])
VALUES ('Weapon 1', 100, 500.00),
       ('Weapon 2', 50, 750.00),
       ('Weapon 3', 75, 600.00),
	   ('Weapon 4', 50, 750.00),
       ('Weapon 5', 75, 600.00);

INSERT INTO [Navy_Details] ([Name], [Count], [Unit_Cost])
VALUES ('Detail 1', 200, 100.00),
       ('Detail 2', 150, 120.00),
       ('Detail 3', 100, 80.00),
	   ('Detail 4', 150, 120.00),
       ('Detail 5', 100, 80.00);

INSERT INTO [Navy_forces_request] ([Navy_WeaponId], [Navy_DetailsId], [Cost])
VALUES (1, 1, 0.00), 
       (2, 2, 0.00),
       (3, 3, 0.00),
	   (4, 4, 0.00),
       (5, 5, 0.00);


create table [Army_Order]
(
[Id] int  identity primary key,
[Ground_forces_requestId] int foreign key references [Ground_forces_request](Id),
[Air_forces_requestId] int foreign key references [Air_forces_request](Id),
[Navy_forces_requestId] int foreign key references [Navy_forces_request](Id),
)


INSERT INTO [Army_Order] ([Ground_forces_requestId], [Air_forces_requestId], [Navy_forces_requestId])
VALUES (1, 1, 1),
       (2, 2, 2),
       (3, 3, 3),
       (4, 4, 4),
       (5, 5, 5);


---!!! Министерство Обороны

create table [Order_Ministry_of_Defence]
(
[Id] int identity primary key,
[Army_OrderID]  int not null foreign key references [Army_Order](Id),
[StartDate] datetime not null, 
[EndDate] datetime not null,
)

Create table [Production]
(
[Id] int identity primary key,
[Name] nvarchar (100),
[State] bit, -- предприятия государственное или нет 
[Region] nvarchar(50),
[OrderId] int foreign key references [Order_Ministry_of_Defence](Id)
)

INSERT INTO [Order_Ministry_of_Defence] ([Army_OrderID], [StartDate], [EndDate])
VALUES (1, '2023-10-25', '2023-11-30'),
       (2, '2023-10-26', '2023-12-01'),
       (3, '2023-10-27', '2023-12-02'),
       (4, '2023-10-28', '2023-12-03'),
       (5, '2023-10-29', '2023-12-04');

INSERT INTO [Production] ([Name], [State], [Region], [OrderId])
VALUES ('Production 1', 1, 'Region 1', 1),
       ('Production 2', 0, 'Region 2', 2),
       ('Production 3', 1, 'Region 3', 3),
       ('Production 4', 0, 'Region 4', 4),
       ('Production 5', 1, 'Region 5', 5);

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

