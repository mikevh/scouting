
CREATE TABLE [Player](
	[PlayerId] [int] IDENTITY(1,1) NOT NULL,
	[PlayerNumber] [varchar](10) NOT NULL,
	[PlayerName] [varchar](100) NOT NULL,
	[LeagueAge] [int] NOT NULL,
	[LeaguePlayed] [varchar](1) NULL,
	[ASMI] [varchar](2) NULL,
	[HBB] [int] NULL,
	[HSO] [int] NULL,
	[HAVG] [decimal](18, 3) NULL,
	[HOPS] [decimal](18, 3) NULL,
	[PIP] [decimal](18, 3) NULL,
	[PBB] [int] NULL,
	[PSO] [int] NULL,
	[PWHIP] [decimal](18, 3) NULL,
	[Size] [varchar](1) NULL,
	[Throws] [varchar](1) NULL,
	[Bats] [varchar](1) NULL,
	[PlayerNote] [varchar](255) NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL DEFAULT (getdate()),
 CONSTRAINT [pkPlayerId] PRIMARY KEY CLUSTERED 
(
	[PlayerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [Fielding](
	[FieldingId] [int] IDENTITY(1,1) NOT NULL,
	[PlayerId] [int] NOT NULL,
	[Mechanics] [decimal](18, 0) NULL,
	[Range] [decimal](18, 0) NULL,
	[Hands] [decimal](18, 0) NULL,
	[ArmStrength] [decimal](18, 0) NULL,
	[FieldingNote] [varchar](255) NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [pkFieldingId] PRIMARY KEY CLUSTERED 
(
	[FieldingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [Hitting](
	[HittingId] [int] IDENTITY(1,1) NOT NULL,
	[PlayerId] [int] NOT NULL,
	[Mechanics] [decimal](18, 0) NULL,
	[Power] [decimal](18, 0) NULL,
	[Contact] [decimal](18, 0) NULL,
	[HittingNote] [varchar](255) NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [pkHittingId] PRIMARY KEY CLUSTERED 
(
	[HittingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [Pitching](
	[PitchingId] [int] IDENTITY(1,1) NOT NULL,
	[PlayerId] [int] NOT NULL,
	[Mechanics] [decimal](18, 0) NULL,
	[Velocity] [decimal](18, 0) NULL,
	[Command] [decimal](18, 0) NULL,
	[PitchingNote] [varchar](255) NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [pkPitchingId] PRIMARY KEY CLUSTERED 
(
	[PitchingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

