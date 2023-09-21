-- Создание базы с проверкой
USE master
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SubsCity1')
BEGIN
    CREATE DATABASE SubsCity1
END

GO
ALTER DATABASE SubsCity1 SET RECOVERY SIMPLE
GO

USE [SubsCity1]

-- Создание таблиц с проверкой
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'PublicationType')
BEGIN
    CREATE TABLE [dbo].[PublicationType](
        [id] [int] IDENTITY(1,1) NOT NULL,
        [type] [nvarchar](20) NOT NULL,
        PRIMARY KEY CLUSTERED 
        (
            [id] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    )
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'RecipientAddress')
BEGIN
	CREATE TABLE [dbo].[RecipientAddress](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[street] [nvarchar](50) NULL,
		[house] [int] NULL,
		[apartment] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'EmployeePosition')
BEGIN
	CREATE TABLE [dbo].[EmployeePosition](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[position] [nvarchar](50) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
	UNIQUE NONCLUSTERED 
	(
		[position] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Publication')
BEGIN
	CREATE TABLE [dbo].[Publication](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[type_id] [int] NOT NULL,
		[name] [nvarchar](70) NOT NULL,
		[price] [decimal](10, 2) NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[Publication]  WITH CHECK ADD FOREIGN KEY([type_id])
	REFERENCES [dbo].[PublicationType] ([id])
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Recipient')
BEGIN
	CREATE TABLE [dbo].[Recipient](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[name] [nvarchar](20) NOT NULL,
		[middlename] [nvarchar](20) NOT NULL,
		[surname] [nvarchar](20) NOT NULL,
		[address_id] [int] NOT NULL,
		[mobile_phone] [nvarchar](20) NOT NULL,
		[email] [nvarchar](255) NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
	UNIQUE NONCLUSTERED 
	(
		[mobile_phone] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
	UNIQUE NONCLUSTERED 
	(
		[email] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[Recipient]  WITH CHECK ADD FOREIGN KEY([address_id])
	REFERENCES [dbo].[RecipientAddress] ([id])
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Office')
BEGIN
	CREATE TABLE [dbo].[Office](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[owner_name] [nvarchar](20) NOT NULL,
		[owner_middlename] [nvarchar](20) NOT NULL,
		[onwner_surname] [nvarchar](20) NOT NULL,
		[street_name] [nvarchar](50) NOT NULL,
		[mobile_phone] [nvarchar](20) NOT NULL,
		[email] [nvarchar](255) NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
	UNIQUE NONCLUSTERED 
	(
		[mobile_phone] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
	UNIQUE NONCLUSTERED 
	(
		[email] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Employee')
BEGIN
	CREATE TABLE [dbo].[Employee](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[name] [nvarchar](20) NOT NULL,
		[middlename] [nvarchar](20) NOT NULL,
		[surname] [nvarchar](20) NOT NULL,
		[position_id] [int] NOT NULL,
		[office_id] [int] NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[Employee]  WITH CHECK ADD FOREIGN KEY([office_id])
	REFERENCES [dbo].[Office] ([id])

	ALTER TABLE [dbo].[Employee]  WITH CHECK ADD FOREIGN KEY([position_id])
	REFERENCES [dbo].[EmployeePosition] ([id])
END



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Subscription')
BEGIN
	CREATE TABLE [dbo].[Subscription](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [recipient_id] [int] NOT NULL,
    [publication_id] [int] NOT NULL,
    [duration] [int] NOT NULL,
    [office_id] [int] NOT NULL,
    [subscription_start_date] [nvarchar](7) NOT NULL

	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[Subscription]  WITH CHECK ADD FOREIGN KEY([office_id])
	REFERENCES [dbo].[Office] ([id])


	ALTER TABLE [dbo].[Subscription]  WITH CHECK ADD FOREIGN KEY([publication_id])
	REFERENCES [dbo].[Publication] ([id])


	ALTER TABLE [dbo].[Subscription]  WITH CHECK ADD FOREIGN KEY([recipient_id])
	REFERENCES [dbo].[Recipient] ([id])
END

-- Заполнение таблиц (если они пусты)
IF NOT EXISTS (SELECT * FROM [dbo].[PublicationType])
BEGIN
    INSERT INTO [dbo].[PublicationType] ([type])
    VALUES ('газета'), ('журнал')
END

DECLARE @StreetNames TABLE (Name NVARCHAR(50))
    INSERT INTO @StreetNames (Name)
    VALUES
        ('Пролетарская улица'),
        ('Ленинская улица'),
        ('Гагарина улица'),
        ('Советская улица'),
        ('Пушкинская улица'),
        ('Московская улица'),
        ('Кировская улица'),
        ('Парковая улица'),
        ('Садовая улица'),
        ('Комсомольская улица'),
        ('Школьная улица'),
        ('Жукова улица'),
        ('Мичурина улица'),
        ('Свердлова улица'),
        ('Октябрьская улица'),
        ('Горького улица'),
        ('Красноармейская улица'),
        ('Рабочая улица'),
        ('Зеленая улица'),
        ('Трудовая улица'),
        ('Полярная улица'),
        ('Красная улица'),
        ('Строителей улица'),
        ('Молодежная улица'),
        ('Центральная улица'),
        ('Новая улица'),
        ('Солнечная улица'),
        ('Заречная улица'),
        ('Пионерская улица'),
        ('Речная улица'),
        ('Восточная улица'),
        ('Западная улица'),
        ('Южная улица'),
        ('Северная улица'),
        ('Цветочная улица'),
        ('Лесная улица'),
        ('Юбилейная улица'),
        ('Гранитная улица'),
        ('Маяковского улица'),
        ('Первомайская улица'),
        ('Коммунальная улица'),
        ('Чкалова улица'),
        ('Горная улица'),
        ('Сиреневая улица'),
        ('Сосновая улица'),
        ('Дружбы улица'),
        ('Озерная улица'),
        ('Заводская улица'),
        ('Вокзальная улица'),
        ('Партизанская улица'),
        ('Островская улица'),
        ('Городская улица'),
        ('Карла Маркса улица'),
        ('Железнодорожная улица'),
        ('Набережная улица'),
        ('Мирная улица'),
        ('Севастопольская улица'),
        ('Колхозная улица'),
        ('Совхозная улица'),
        ('Театральная улица'),
        ('Лермонтова улица'),
        ('Пушкинская улица'),
        ('Мичуринская улица'),
        ('Свердловская улица'),
        ('Щорса улица'),
        ('Смирнова улица'),
        ('Гусарская улица'),
        ('Петровская улица'),
        ('Космонавтов улица'),
        ('Суворова улица'),
        ('Фрунзе улица'),
        ('Толстого улица'),
        ('Горького улица'),
        ('Шевченко улица'),
        ('Греческая улица'),
        ('Воронцовская улица'),
        ('Скверная улица'),
        ('Сельская улица'),
        ('Холодильная улица'),
        ('Степная улица'),
        ('Заводская улица'),
        ('Космическая улица'),
        ('Речная улица'),
        ('Парковая улица'),
        ('Береговая улица'),
        ('Школьная улица'),
        ('Соседская улица'),
        ('Луговая улица'),
        ('Озерная улица'),
        ('Красивая улица'),
        ('Полевая улица'),
        ('Дачная улица'),
        ('Живописная улица'),
        ('Просторная улица'),
        ('Зеленая улица'),
        ('Чистая улица'),
        ('Тихая улица'),
        ('Песчаная улица')

IF NOT EXISTS (SELECT * FROM [dbo].[RecipientAddress])
BEGIN

    DECLARE @House INT
    DECLARE @Apartment INT
    DECLARE @RandomStreet NVARCHAR(20)

    WHILE (SELECT COUNT(*) FROM [dbo].[RecipientAddress]) < 100
    BEGIN
        SELECT TOP 1 @RandomStreet = Name FROM @StreetNames ORDER BY NEWID()
        SET @House = CAST(RAND() * 100 AS INT) + 1
        SET @Apartment = CAST(RAND() * 20 AS INT) + 1

        INSERT INTO [dbo].[RecipientAddress] ([street], [house], [apartment])
        VALUES (@RandomStreet, @House, @Apartment)
    END
END

IF NOT EXISTS (SELECT * FROM [dbo].[EmployeePosition])
BEGIN
	INSERT INTO [dbo].[EmployeePosition] ([position])
	VALUES
		('Почтальон'),
		('Кассир'),
		('Сортировщик'),
		('Менеджер по доставке')
END

-- Проверка, пуста ли таблица Publication
IF NOT EXISTS (SELECT * FROM [dbo].[Publication])
BEGIN
    -- Заполнение таблицы Publication случайными данными
    DECLARE @PublicationNames TABLE (Name NVARCHAR(70))
    INSERT INTO @PublicationNames (Name)
    VALUES
        ('Утренние новости'),
        ('Вечерний вестник'),
        ('Здоровье и красота'),
        ('Наука и техника'),
        ('Деловой мир'),
        ('Спортивные новости'),
        ('Искусство и культура'),
        ('Политика и общество'),
        ('Финансовые новости'),
        ('Путешествия и приключения'),
		('Асахи симбун'),
		('Спорт-Экспресс'),
		('Взгляд'),
		('Московский комсомолец'),
		('Комсомольская правда')


	DECLARE @PublicationType INT
    DECLARE @PublicationName NVARCHAR(50)
    DECLARE @PublicationPrice DECIMAL(10, 2)

    WHILE (SELECT COUNT(*) FROM [dbo].[Publication]) < 100 
    BEGIN
        SET @PublicationType = CASE WHEN RAND() > 0.5 THEN 1 ELSE 2 END
        SELECT TOP 1 @PublicationName = Name FROM @PublicationNames ORDER BY NEWID()
        SET @PublicationPrice = CAST(RAND() * 100 AS DECIMAL(10, 2)) 

        INSERT INTO [dbo].[Publication] ([type_id], [name], [price])
        VALUES (@PublicationType, @PublicationName, @PublicationPrice)
    END
END

DECLARE @Names TABLE (Name NVARCHAR(20))
DECLARE @MiddleNames TABLE (MiddleName NVARCHAR(20))
DECLARE @Surnames TABLE (Surname NVARCHAR(20))

INSERT INTO @Names (Name) VALUES ('Евгений'), ('Иван'), ('Петр'), ('Кирил'), ('Александр'), ('Арсений')
INSERT INTO @MiddleNames (MiddleName) VALUES ('Иванович'), ('Андреевич'), ('Петрович'), ('Сергеевна'), ('Александрович')
INSERT INTO @Surnames (Surname) VALUES ('Иванов'), ('Антонова'), ('Петров'), ('Смирнов'), ('Сидоров')

DECLARE @AddressID INT
DECLARE @MobilePhone NVARCHAR(20)
DECLARE @Email NVARCHAR(255)

IF NOT EXISTS (SELECT * FROM [dbo].[Recipient])
BEGIN
	-- Заполнение таблицы Recipient
	WHILE (SELECT COUNT(*) FROM [dbo].[Recipient]) < 100 
	BEGIN
		SET @AddressID = CAST(RAND() * 100 AS INT) 
		SET @MobilePhone = '+375' + CAST(100000000 + CAST(RAND() * 899999999 AS INT) AS NVARCHAR(20)) 
		SET @Email = LEFT(NEWID(), 10) + '@gmail.com'

		INSERT INTO [dbo].[Recipient] ([name], [middlename], [surname], [address_id], [mobile_phone], [email])
		SELECT TOP 1
			(SELECT TOP 1 Name FROM @Names ORDER BY NEWID()),
			(SELECT TOP 1 MiddleName FROM @MiddleNames ORDER BY NEWID()),
			(SELECT TOP 1 Surname FROM @Surnames ORDER BY NEWID()),
			@AddressID,
			@MobilePhone,
			@Email
	END
END


IF NOT EXISTS (SELECT * FROM [dbo].[Office])
BEGIN
	WHILE (SELECT COUNT(*) FROM [dbo].[Office]) < 100
	BEGIN
		SET @MobilePhone = '+375' + CAST(100000000 + CAST(RAND() * 899999999 AS INT) AS NVARCHAR(20))
		SET @Email = LEFT(NEWID(), 10) + '@gmail.com' 

		INSERT INTO [dbo].[Office] ([owner_name], [owner_middlename], [onwner_surname], [street_name], [mobile_phone], [email])
		SELECT TOP 1
			(SELECT TOP 1 Name FROM @Names ORDER BY NEWID()),
			(SELECT TOP 1 MiddleName FROM @MiddleNames ORDER BY NEWID()),
			(SELECT TOP 1 Surname FROM @Surnames ORDER BY NEWID()),
			(SELECT TOP 1 Name FROM @StreetNames ORDER BY NEWID()),
			@MobilePhone,
			@Email
	END
END

IF NOT EXISTS (SELECT * FROM [dbo].[Employee])
BEGIN
	DECLARE @PositionID INT
	DECLARE @OfficeID INT
	WHILE (SELECT COUNT(*) FROM [dbo].[Employee]) < 100 
	BEGIN
		SET @PositionID = CAST(RAND() * 100 + 1 AS INT) 
		SET @OfficeID = CAST(RAND() * 100 + 1 AS INT) 

		INSERT INTO [dbo].[Employee] ([name], [middlename], [surname], [position_id], [office_id])
		SELECT TOP 1
			(SELECT TOP 1 Name FROM @Names ORDER BY NEWID()),
			(SELECT TOP 1 MiddleName FROM @MiddleNames ORDER BY NEWID()),
			(SELECT TOP 1 Surname FROM @Surnames ORDER BY NEWID()),
			@PositionID,
			@OfficeID
	END
END	


IF NOT EXISTS (SELECT * FROM [dbo].[Subscription])
BEGIN
	DECLARE @RecipientID INT
	DECLARE @PublicationID INT
	DECLARE @Duration INT
	DECLARE @StartDate NVARCHAR(7)

	WHILE (SELECT COUNT(*) FROM [dbo].[Subscription]) < 100 
	BEGIN
		SET @RecipientID = CAST(RAND() * 100 + 1 AS INT) 
		SET @PublicationID = CAST(RAND() * 100 + 1 AS INT)
		SET @Duration = CAST(RAND() * 12 + 1 AS INT) 
		SET @OfficeID = CAST(RAND() * 100 + 1 AS INT)
    
		SET @StartDate = RIGHT('0' + CAST(ROUND(RAND() * 11 + 1, 0) AS NVARCHAR(2)), 2) + '.' + CAST(ROUND(RAND() * 9 + 2014, 0) AS NVARCHAR(4))
    
		INSERT INTO [dbo].[Subscription] ([recipient_id], [publication_id], [duration], [office_id], [subscription_start_date])
		VALUES (@RecipientID, @PublicationID, @Duration, @OfficeID, @StartDate)
	END
END


-- Создание хранимых процедур
IF OBJECT_ID ( 'dbo.InsertPublication', 'P' ) IS NOT NULL 
    DROP PROCEDURE dbo.InsertPublication;
GO
	CREATE PROCEDURE InsertPublication
		@type_id INT,
		@name NVARCHAR(70),
		@price DECIMAL(10, 2)
	AS
	BEGIN
		INSERT INTO [dbo].[Publication] ([type_id], [name], [price])
		VALUES (@type_id, @name, @price)
	END
GO

IF OBJECT_ID ( 'dbo.InsertRecipient', 'P' ) IS NOT NULL 
    DROP PROCEDURE dbo.InsertRecipient;
GO
	CREATE PROCEDURE InsertRecipient
		@name NVARCHAR(20),
		@middlename NVARCHAR(20),
		@surname NVARCHAR(20),
		@street NVARCHAR(50),
		@house INT,
		@apartment INT,
		@mobile_phone NVARCHAR(20),
		@email NVARCHAR(255)
	AS
	BEGIN
		DECLARE @AddressID INT

		INSERT INTO [dbo].[RecipientAddress] ([street], [house], [apartment])
		VALUES (@street, @house, @apartment)

		SET @AddressID = SCOPE_IDENTITY()

		INSERT INTO [dbo].[Recipient] ([name], [middlename], [surname], [address_id], [mobile_phone], [email])
		VALUES (@name, @middlename, @surname, @AddressID, @mobile_phone, @email)
	END
GO

IF OBJECT_ID ( 'dbo.InsertOffice', 'P' ) IS NOT NULL 
    DROP PROCEDURE dbo.InsertOffice;
GO
	CREATE PROCEDURE InsertOffice
		@owner_name NVARCHAR(20),
		@owner_middlename NVARCHAR(20),
		@owner_surname NVARCHAR(20),
		@street_name NVARCHAR(50),
		@mobile_phone NVARCHAR(20),
		@email NVARCHAR(255)
	AS
	BEGIN
		INSERT INTO [dbo].[Office] ([owner_name], [owner_middlename], [onwner_surname], [street_name], [mobile_phone], [email])
		VALUES (@owner_name, @owner_middlename, @owner_surname, @street_name, @mobile_phone, @email)
	END
GO

IF OBJECT_ID ( 'dbo.InsertSubscription', 'P' ) IS NOT NULL 
    DROP PROCEDURE dbo.InsertSubscription;
GO
	CREATE PROCEDURE InsertSubscription
		@recipient_id INT,
		@publication_id INT,
		@duration INT,
		@office_id INT,
		@subscription_start_date NVARCHAR(7)
	AS
	BEGIN
		INSERT INTO [dbo].[Subscription] ([recipient_id], [publication_id], [duration], [office_id], [subscription_start_date])
		VALUES (@recipient_id, @publication_id, @duration, @office_id, @subscription_start_date)
	END
GO

IF OBJECT_ID ( 'dbo.InsertEmployee', 'P' ) IS NOT NULL 
    DROP PROCEDURE dbo.InsertEmployee;
GO
	CREATE PROCEDURE InsertEmployee
		@name NVARCHAR(20),
		@middlename NVARCHAR(20),
		@surname NVARCHAR(20),
		@position_id INT,
		@office_id INT
	AS
	BEGIN
		INSERT INTO [dbo].[Employee] ([name], [middlename], [surname], [position_id], [office_id])
		VALUES (@name, @middlename, @surname, @position_id, @office_id)
	END
GO

-- Создание представлений
IF NOT EXISTS (SELECT * FROM sys.views WHERE [name] = 'PublicationView')
BEGIN
    EXEC('
        CREATE VIEW PublicationView AS
        SELECT
            p.id AS PublicationID,
            pt.type AS PublicationType,
            p.name AS PublicationName,
            p.price AS PublicationPrice
        FROM
            [dbo].[Publication] p
        JOIN
            [dbo].[PublicationType] pt ON p.type_id = pt.id;
    ');
END

IF NOT EXISTS (SELECT * FROM sys.views WHERE [name] = 'RecipientView')
BEGIN
    EXEC('
        CREATE VIEW RecipientView AS
    SELECT
        r.id AS RecipientID,
        r.name AS RecipientName,
        r.middlename AS RecipientMiddlename,
        r.surname AS RecipientSurname,
        ra.street AS RecipientStreet,
        ra.house AS RecipientHouse,
        ra.apartment AS RecipientApartment,
        r.mobile_phone AS RecipientMobilePhone,
        r.email AS RecipientEmail
    FROM
        [dbo].[Recipient] r
    JOIN
        [dbo].[RecipientAddress] ra ON r.address_id = ra.id;
    ');
END

IF NOT EXISTS (SELECT * FROM sys.views WHERE [name] = 'SubscriptionView')
BEGIN
    EXEC('
        CREATE VIEW SubscriptionView AS
	SELECT
		s.id AS SubscriptionID,
		r.name AS RecipientName,
		p.name AS PublicationName,
		s.duration AS SubscriptionDuration,
		o.owner_name AS OfficeOwnerName,
		s.subscription_start_date AS SubscriptionStartDate
	FROM
		[dbo].[Subscription] s
	JOIN
		[dbo].[Recipient] r ON s.recipient_id = r.id
	JOIN
		[dbo].[Publication] p ON s.publication_id = p.id
	JOIN
		[dbo].[Office] o ON s.office_id = o.id;
    ');
END

IF NOT EXISTS (SELECT * FROM sys.views WHERE [name] = 'OfficeView')
BEGIN
    EXEC('
       CREATE VIEW OfficeView AS
	SELECT
		id AS OfficeID,
		owner_name AS OwnerName,
		owner_middlename AS OwnerMiddlename,
		onwner_surname AS OwnerSurname,
		street_name AS StreetName,
		mobile_phone AS MobilePhone,
		email AS Email
	FROM
		[dbo].[Office];
    ');
END

