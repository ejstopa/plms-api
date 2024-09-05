-- --------------------------------------------------------
-- Servidor:                     localhost
-- Versão do servidor:           Microsoft SQL Server 2022 (RTM) - 16.0.1000.6
-- OS do Servidor:               Windows 10 Home Single Language 10.0 <X64> (Build 22631: ) (Hypervisor)
-- HeidiSQL Versão:              11.2.0.6213
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES  */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Copiando estrutura do banco de dados para plms_db
CREATE DATABASE IF NOT EXISTS "plms_db";
USE "plms_db";

-- Copiando estrutura para tabela plms_db.Items
CREATE TABLE IF NOT EXISTS "Items" (
	"Id" INT NOT NULL,
	"Name" VARCHAR(100) NOT NULL COLLATE 'Latin1_General_CI_AS',
	"LastRevision" VARCHAR(10) NOT NULL DEFAULT '''-''' COLLATE 'Latin1_General_CI_AS',
	"Description" VARCHAR(200) NOT NULL COLLATE 'Latin1_General_CI_AS',
	"Family" VARCHAR(20) NOT NULL COLLATE 'Latin1_General_CI_AS',
	"Status" VARCHAR(20) NULL DEFAULT NULL COLLATE 'Latin1_General_CI_AS',
	"CreatedBy" INT NOT NULL,
	"CreatedAt" DATE NOT NULL,
	"LastModifiedBy" INT NOT NULL,
	"LastModifiedAt" DATE NOT NULL,
	"CheckedOutBy" INT NULL DEFAULT '(0)',
	PRIMARY KEY ("Id")
);

-- Copiando dados para a tabela plms_db.Items: 2 rows
/*!40000 ALTER TABLE "Items" DISABLE KEYS */;
INSERT INTO "Items" ("Id", "Name", "LastRevision", "Description", "Family", "Status", "CreatedBy", "CreatedAt", "LastModifiedBy", "LastModifiedAt", "CheckedOutBy") VALUES
	(1, '00010001', '-', 'CHAPA PLANA', '0001', 'checkedOut', 1, '2024-08-30', 1, '2024-08-30', 1),
	(2, '00010002', '-', 'CHAPA PLANA', '0001', 'released', 1, '2024-08-30', 1, '2024-08-30', 0);
/*!40000 ALTER TABLE "Items" ENABLE KEYS */;

-- Copiando estrutura para tabela plms_db.Models
CREATE TABLE IF NOT EXISTS "Models" (
	"Id" INT NOT NULL,
	"Name" VARCHAR(100) NOT NULL DEFAULT '''''' COLLATE 'Latin1_General_CI_AS',
	"CommonName" VARCHAR(100) NOT NULL COLLATE 'Latin1_General_CI_AS',
	"Type" VARCHAR(20) NOT NULL COLLATE 'Latin1_General_CI_AS',
	"Version" INT NOT NULL DEFAULT '(1)',
	"Revision" VARCHAR(10) NOT NULL DEFAULT '''-''' COLLATE 'Latin1_General_CI_AS',
	"FilePath" VARCHAR(500) NOT NULL COLLATE 'Latin1_General_CI_AS',
	"ItemId" INT NOT NULL DEFAULT '(0)',
	"CreatedBy" INT NOT NULL,
	"CreatedAt" DATE NOT NULL,
	"CheckedOutBy" INT NULL DEFAULT NULL,
	"LastModifiedBy" INT NULL DEFAULT NULL,
	"LastModifiedAt" DATE NULL DEFAULT NULL,
	PRIMARY KEY ("Id")
);

-- Copiando dados para a tabela plms_db.Models: 3 rows
/*!40000 ALTER TABLE "Models" DISABLE KEYS */;
INSERT INTO "Models" ("Id", "Name", "CommonName", "Type", "Version", "Revision", "FilePath", "ItemId", "CreatedBy", "CreatedAt", "CheckedOutBy", "LastModifiedBy", "LastModifiedAt") VALUES
	(3, '00010001', '0001.0001', '.prt', 1, '-', 'C:\px-infrastructure\library\0001\00010001.prt', 1, 1, '2024-08-30', 1, NULL, NULL),
	(18, '00010002', '0001.0002', '.asm', 1, '-', 'C:\px-infrastructure\library\0001\00010002.asm', 2, 1, '2024-08-30', 0, NULL, NULL),
	(19, '00010001', '0001.0001', '.drw', 1, '-', 'C:\px-infrastructure\library\0001\00010001.drw', 1, 1, '2024-08-30', 1, NULL, NULL);
/*!40000 ALTER TABLE "Models" ENABLE KEYS */;

-- Copiando estrutura para tabela plms_db.Users
CREATE TABLE IF NOT EXISTS "Users" (
	"Id" INT NOT NULL,
	"Name" VARCHAR(50) NOT NULL COLLATE 'Latin1_General_CI_AS',
	"Password" VARCHAR(100) NOT NULL COLLATE 'Latin1_General_CI_AS',
	"WindowsUser" VARCHAR(30) NOT NULL COLLATE 'Latin1_General_CI_AS',
	"Role" VARCHAR(30) NOT NULL COLLATE 'Latin1_General_CI_AS',
	"IsActive" BIT NOT NULL,
	PRIMARY KEY ("Id")
);

-- Copiando dados para a tabela plms_db.Users: 1 rows
/*!40000 ALTER TABLE "Users" DISABLE KEYS */;
INSERT INTO "Users" ("Id", "Name", "Password", "WindowsUser", "Role", "IsActive") VALUES
	(1, 'user1', '$2a$11$GyTMUepWP64t7X9spW5MG.6U07CzzuLTq8IH7rb3llb2Pspz8wHji', 'ejsto', 'user', b'1');
/*!40000 ALTER TABLE "Users" ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
