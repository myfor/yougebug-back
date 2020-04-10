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
-- Table structure for table `Admins`
--

DROP TABLE IF EXISTS `Admins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Admins` (
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

LOCK TABLES `Admins` WRITE;
/*!40000 ALTER TABLE `admins` DISABLE KEYS */;
INSERT INTO `Admins` VALUES (1,1,0,'2020-01-01 00:00:00.000000',0,'2020-01-01 00:00:00.000000','admin','96cae35ce8a9b0244178bf28e4966c2ce1b8385723a96a6b838858cdd6ca0a1e','9c32988e-d786-4ace-ad77-a215c1304dd6','admin@yougebug.com');
/*!40000 ALTER TABLE `admins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `answerbackrecords`
--

DROP TABLE IF EXISTS `AnswerBackRecords`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `AnswerBackRecords` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `State` int(11) NOT NULL,
  `CreatorId` int(11) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `ModifyId` int(11) NOT NULL,
  `ModifyDate` datetime(6) NOT NULL,
  `AnswerId` int(11) NOT NULL,
  `Description` varchar(1024) CHARACTER SET utf8mb4 NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AnswerBackRecords_AnswerId` (`AnswerId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `answerbackrecords`
--

LOCK TABLES `AnswerBackRecords` WRITE;
/*!40000 ALTER TABLE `answerbackrecords` DISABLE KEYS */;
/*!40000 ALTER TABLE `answerbackrecords` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AnswerReportRecords`
--

DROP TABLE IF EXISTS `AnswerReportRecords`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `AnswerReportRecords` (
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
  KEY `IX_AnswerReportRecords_AnswerId` (`AnswerId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AnswerReportRecords`
--

LOCK TABLES `AnswerReportRecords` WRITE;
/*!40000 ALTER TABLE `AnswerReportRecords` DISABLE KEYS */;
/*!40000 ALTER TABLE `AnswerReportRecords` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Answers`
--

DROP TABLE IF EXISTS `Answers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Answers` (
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
  KEY `IX_Answers_AnswererId` (`AnswererId`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `answercomments`
--

DROP TABLE IF EXISTS `AnswerComments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `AnswerComments` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `State` int(11) NOT NULL,
  `CreatorId` int(11) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `ModifyId` int(11) NOT NULL,
  `ModifyDate` datetime(6) NOT NULL,
  `AnswerId` int(11) NOT NULL,
  `Content` varchar(1024) CHARACTER SET utf8mb4 NOT NULL,
  `CommenterId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AnswerComments_AnswerId` (`AnswerId`),
  KEY `IX_AnswerComments_CommenterId` (`CommenterId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `Files`
--

DROP TABLE IF EXISTS `Files`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Files` (
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
-- Dumping data for table `Files`
--

LOCK TABLES `Files` WRITE;
/*!40000 ALTER TABLE `Files` DISABLE KEYS */;
INSERT INTO `Files` VALUES (1,1,0,'2020-01-01 00:00:00.000000',0,'2020-01-01 00:00:00.000000','default.png','.png',40,'/Files/default.png','/Files/default.png');
/*!40000 ALTER TABLE `Files` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `questionbackrecords`
--

DROP TABLE IF EXISTS `QuestionBackRecords`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `QuestionBackRecords` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `State` int(11) NOT NULL,
  `CreatorId` int(11) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `ModifyId` int(11) NOT NULL,
  `ModifyDate` datetime(6) NOT NULL,
  `QuestionId` int(11) NOT NULL,
  `Description` varchar(1024) CHARACTER SET utf8mb4 NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_QuestionBackRecords_QuestionId` (`QuestionId`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `QuestionReportRecords`
--

DROP TABLE IF EXISTS `QuestionReportRecords`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `QuestionReportRecords` (
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
  KEY `IX_QuestionReportRecords_QuestionId` (`QuestionId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `QuestionReportRecords`
--

LOCK TABLES `QuestionReportRecords` WRITE;
/*!40000 ALTER TABLE `QuestionReportRecords` DISABLE KEYS */;
/*!40000 ALTER TABLE `QuestionReportRecords` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Questions`
--

DROP TABLE IF EXISTS `Questions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Questions` (
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
  KEY `IX_Questions_AskerId` (`AskerId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `QuestionComments`
--

DROP TABLE IF EXISTS `QuestionComments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `QuestionComments`(
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `State` int(11) NOT NULL,
  `CreatorId` int(11) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `ModifyId` int(11) NOT NULL,
  `ModifyDate` datetime(6) NOT NULL,
  `QuestionId` int(11) NOT NULL,
  `Content` varchar(1024) CHARACTER SET utf8mb4 NOT NULL,
  `CommenterId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_QuestionComments_CommenterId` (`CommenterId`),
  KEY `IX_QuestionComments_QuestionId` (`QuestionId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--

--
-- Table structure for table `Tags`
--

DROP TABLE IF EXISTS `Tags`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Tags` (
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

DROP TABLE IF EXISTS `Users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Users` (
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
  KEY `IX_Users_AvatarId` (`AvatarId`)
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
