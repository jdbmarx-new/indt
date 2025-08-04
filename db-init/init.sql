IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ContratacaoDB')
BEGIN
	CREATE DATABASE ContratacaoDB;
END
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'PropostaDB')
BEGIN
	CREATE DATABASE PropostaDB;
END
GO