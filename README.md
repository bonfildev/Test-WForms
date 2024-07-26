This is a version of a free Surveilance system developed on windows forms,
this version only works on x64, this work well on the integrated camera,
to work with external cameras it needs more configuration
We use EMGUCV for this eample.
Sql server connection, needed tables:

CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[email] [varchar](200) NULL,
	[Inactive] [bit] NULL,
	[Profile] [smallint] NULL,
	[Password] [varchar](20) NULL,
	[CreateUser] [int] NULL,
	[CreateDate] [datetime] NULL,
	[UpdateUser] [int] NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT Users(FirstName, LastName, email, Inactive, Password, CreateUser, CreateDate)
Values
(1,'Diego','Bonfil','diego.bonfil.paz@gmail.com',0,NULL,123,1,'2022-03-16 19:51:17.947',1,'2022-03-25 13:26:26.860')

CREATE TABLE [dbo].[Parameters](
	[ParameterID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Value] [sql_variant] NULL,
	[Description] [varchar](200) NULL,
 CONSTRAINT [PK_Parameters] PRIMARY KEY CLUSTERED 
(
	[ParameterID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

Insert Parameters(ParameterID,UserID,[Value],[Description])
Values(1,1,128,'BackGroundColorRed (entre 0 y 255)');
Insert Parameters(ParameterID,UserID,[Value],[Description])
Values(2,1,128,'BackGroundColorGreen (entre 0 y 255)');
Insert Parameters(ParameterID,UserID,[Value],[Description])
Values(1,1,255,'BackGroundColorBlue (entre 0 y 255)');
Insert Parameters(ParameterID,UserID,[Value],[Description])
Values(1,1,0.00000000000000000001,'Sensibilidad Deteccion de movimiento (menor a 0.001)');


For additional information can message me diego.bonfil.paz@gmail.com

