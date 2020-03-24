-- MariaDB dump 10.17  Distrib 10.4.7-MariaDB, for Win64 (AMD64)
--
-- Host: localhost    Database: yougebug
-- ------------------------------------------------------
-- Server version	10.4.7-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20200119094650_init','3.1.1'),('20200225065851_20190225','3.1.1'),('20200309010758_20190309','3.1.1'),('20200309011010_20190309_1','3.1.1');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `admins`
--

DROP TABLE IF EXISTS `admins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `admins` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `State` int(11) NOT NULL,
  `CreatorId` int(11) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `ModifyId` int(11) NOT NULL,
  `ModifyDate` datetime(6) NOT NULL,
  `Account` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
  `Password` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
  `Token` char(36) NOT NULL,
  `Email` longtext CHARACTER SET utf8mb4 DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `admins`
--

LOCK TABLES `admins` WRITE;
/*!40000 ALTER TABLE `admins` DISABLE KEYS */;
INSERT INTO `admins` VALUES (1,1,0,'2020-01-01 00:00:00.000000',0,'2020-01-01 00:00:00.000000','admin','96cae35ce8a9b0244178bf28e4966c2ce1b8385723a96a6b838858cdd6ca0a1e','9c32988e-d786-4ace-ad77-a215c1304dd6','admin@yougebug.com');
/*!40000 ALTER TABLE `admins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `answerbackrecords`
--

DROP TABLE IF EXISTS `answerbackrecords`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `answerbackrecords` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `State` int(11) NOT NULL,
  `CreatorId` int(11) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `ModifyId` int(11) NOT NULL,
  `ModifyDate` datetime(6) NOT NULL,
  `AnswerId` int(11) NOT NULL,
  `Description` varchar(1024) CHARACTER SET utf8mb4 NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AnswerBackRecords_AnswerId` (`AnswerId`),
  CONSTRAINT `FK_AnswerBackRecords_Answers_AnswerId` FOREIGN KEY (`AnswerId`) REFERENCES `answers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `answerbackrecords`
--

LOCK TABLES `answerbackrecords` WRITE;
/*!40000 ALTER TABLE `answerbackrecords` DISABLE KEYS */;
/*!40000 ALTER TABLE `answerbackrecords` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `answerreportrecords`
--

DROP TABLE IF EXISTS `answerreportrecords`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `answerreportrecords` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `State` int(11) NOT NULL,
  `CreatorId` int(11) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `ModifyId` int(11) NOT NULL,
  `ModifyDate` datetime(6) NOT NULL,
  `AnswerId` int(11) NOT NULL,
  `Reason` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
  `Description` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AnswerReportRecords_AnswerId` (`AnswerId`),
  CONSTRAINT `FK_AnswerReportRecords_Answers_AnswerId` FOREIGN KEY (`AnswerId`) REFERENCES `answers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `answerreportrecords`
--

LOCK TABLES `answerreportrecords` WRITE;
/*!40000 ALTER TABLE `answerreportrecords` DISABLE KEYS */;
/*!40000 ALTER TABLE `answerreportrecords` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `answers`
--

DROP TABLE IF EXISTS `answers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `answers` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `State` int(11) NOT NULL,
  `CreatorId` int(11) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `ModifyId` int(11) NOT NULL,
  `ModifyDate` datetime(6) NOT NULL,
  `Content` longtext CHARACTER SET utf8mb4 NOT NULL,
  `Votes` int(11) NOT NULL,
  `QuestionId` int(11) NOT NULL,
  `AnswererId` int(11) DEFAULT NULL,
  `NickName` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Answers_QuestionId` (`QuestionId`),
  KEY `IX_Answers_AnswererId` (`AnswererId`),
  CONSTRAINT `FK_Answers_Questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `questions` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Answers_Users_AnswererId` FOREIGN KEY (`AnswererId`) REFERENCES `users` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `files`
--

DROP TABLE IF EXISTS `files`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `files` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `State` int(11) NOT NULL,
  `CreatorId` int(11) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `ModifyId` int(11) NOT NULL,
  `ModifyDate` datetime(6) NOT NULL,
  `Name` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
  `ExtensionName` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
  `Size` bigint(20) NOT NULL,
  `Path` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
  `Thumbnail` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `files`
--

LOCK TABLES `files` WRITE;
/*!40000 ALTER TABLE `files` DISABLE KEYS */;
INSERT INTO `files` VALUES (1,1,0,'2020-01-01 00:00:00.000000',0,'2020-01-01 00:00:00.000000','default.png','.png',40,'/files/default.png','/files/default.png');
/*!40000 ALTER TABLE `files` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `questionbackrecords`
--

DROP TABLE IF EXISTS `questionbackrecords`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `questionbackrecords` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `State` int(11) NOT NULL,
  `CreatorId` int(11) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `ModifyId` int(11) NOT NULL,
  `ModifyDate` datetime(6) NOT NULL,
  `QuestionId` int(11) NOT NULL,
  `Description` varchar(1024) CHARACTER SET utf8mb4 NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_QuestionBackRecords_QuestionId` (`QuestionId`),
  CONSTRAINT `FK_QuestionBackRecords_Questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `questions` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `questionreportrecords`
--

DROP TABLE IF EXISTS `questionreportrecords`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `questionreportrecords` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `State` int(11) NOT NULL,
  `CreatorId` int(11) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `ModifyId` int(11) NOT NULL,
  `ModifyDate` datetime(6) NOT NULL,
  `QuestionId` int(11) NOT NULL,
  `Reason` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
  `Description` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_QuestionReportRecords_QuestionId` (`QuestionId`),
  CONSTRAINT `FK_QuestionReportRecords_Questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `questions` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `questionreportrecords`
--

LOCK TABLES `questionreportrecords` WRITE;
/*!40000 ALTER TABLE `questionreportrecords` DISABLE KEYS */;
/*!40000 ALTER TABLE `questionreportrecords` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `questions`
--

DROP TABLE IF EXISTS `questions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `questions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `State` int(11) NOT NULL,
  `CreatorId` int(11) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `ModifyId` int(11) NOT NULL,
  `ModifyDate` datetime(6) NOT NULL,
  `Title` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
  `Tags` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
  `Votes` int(11) NOT NULL,
  `Views` int(11) NOT NULL,
  `Actived` datetime(6) NOT NULL,
  `AskerId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Questions_AskerId` (`AskerId`),
  CONSTRAINT `FK_Questions_Users_AskerId` FOREIGN KEY (`AskerId`) REFERENCES `users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `tags`
--

DROP TABLE IF EXISTS `tags`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tags` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `State` int(11) NOT NULL,
  `CreatorId` int(11) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `ModifyId` int(11) NOT NULL,
  `ModifyDate` datetime(6) NOT NULL,
  `Name` varchar(16) CHARACTER SET utf8mb4 NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `State` int(11) NOT NULL,
  `CreatorId` int(11) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `ModifyId` int(11) NOT NULL,
  `ModifyDate` datetime(6) NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 DEFAULT NULL,
  `Email` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
  `Password` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
  `AvatarId` int(11) NOT NULL,
  `Token` char(36) NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  PRIMARY KEY (`Id`),
  KEY `IX_Users_AvatarId` (`AvatarId`),
  CONSTRAINT `FK_Users_Files_AvatarId` FOREIGN KEY (`AvatarId`) REFERENCES `files` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-03-24 16:25:22
