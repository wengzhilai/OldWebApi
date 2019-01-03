CREATE DATABASE  IF NOT EXISTS `fa` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `fa`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: fa
-- ------------------------------------------------------
-- Server version	5.7.18-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `fa_app_version`
--

DROP TABLE IF EXISTS `fa_app_version`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_app_version` (
  `ID` int(11) NOT NULL,
  `IS_NEW` decimal(1,0) NOT NULL,
  `TYPE` varchar(20) NOT NULL COMMENT 'ipk\n            apk',
  `REMARK` varchar(1000) DEFAULT NULL,
  `UPDATE_TIME` datetime DEFAULT NULL,
  `UPDATE_URL` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_app_version`
--

LOCK TABLES `fa_app_version` WRITE;
/*!40000 ALTER TABLE `fa_app_version` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_app_version` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_bulletin`
--

DROP TABLE IF EXISTS `fa_bulletin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_bulletin` (
  `ID` int(11) NOT NULL,
  `TITLE` varchar(255) NOT NULL,
  `PIC` varchar(255) DEFAULT NULL,
  `TYPE_CODE` varchar(50) DEFAULT NULL,
  `CONTENT` text,
  `USER_ID` int(11) DEFAULT NULL,
  `PUBLISHER` varchar(255) NOT NULL,
  `ISSUE_DATE` datetime NOT NULL,
  `IS_SHOW` decimal(1,0) NOT NULL,
  `IS_IMPORT` decimal(1,0) NOT NULL,
  `IS_URGENT` decimal(1,0) NOT NULL,
  `AUTO_PEN` decimal(1,0) NOT NULL,
  `CREATE_TIME` datetime NOT NULL,
  `UPDATE_TIME` datetime NOT NULL,
  `REGION` varchar(10) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_bulletin`
--

LOCK TABLES `fa_bulletin` WRITE;
/*!40000 ALTER TABLE `fa_bulletin` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_bulletin` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_bulletin_file`
--

DROP TABLE IF EXISTS `fa_bulletin_file`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_bulletin_file` (
  `BULLETIN_ID` int(11) NOT NULL,
  `FILE_ID` int(11) NOT NULL,
  PRIMARY KEY (`BULLETIN_ID`,`FILE_ID`),
  KEY `FK_FA_BULLETIN_FILE_REF_FILE` (`FILE_ID`),
  CONSTRAINT `FK_FA_BULLETIN_FILE_REF_BULL` FOREIGN KEY (`BULLETIN_ID`) REFERENCES `fa_bulletin` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_FA_BULLETIN_FILE_REF_FILE` FOREIGN KEY (`FILE_ID`) REFERENCES `fa_files` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_bulletin_file`
--

LOCK TABLES `fa_bulletin_file` WRITE;
/*!40000 ALTER TABLE `fa_bulletin_file` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_bulletin_file` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_bulletin_log`
--

DROP TABLE IF EXISTS `fa_bulletin_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_bulletin_log` (
  `ID` int(11) NOT NULL,
  `BULLETIN_ID` int(11) NOT NULL,
  `USER_ID` int(11) NOT NULL,
  `LOOK_TIME` datetime NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_BULLETIN_LOG_REF_BULLETIN` (`BULLETIN_ID`),
  CONSTRAINT `FK_BULLETIN_LOG_REF_BULLETIN` FOREIGN KEY (`BULLETIN_ID`) REFERENCES `fa_bulletin` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_bulletin_log`
--

LOCK TABLES `fa_bulletin_log` WRITE;
/*!40000 ALTER TABLE `fa_bulletin_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_bulletin_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_bulletin_review`
--

DROP TABLE IF EXISTS `fa_bulletin_review`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_bulletin_review` (
  `ID` int(11) NOT NULL,
  `PARENT_ID` int(11) DEFAULT NULL,
  `BULLETIN_ID` int(11) NOT NULL,
  `NAME` varchar(50) DEFAULT NULL,
  `CONTENT` text,
  `USER_ID` int(11) NOT NULL,
  `ADD_TIME` datetime NOT NULL,
  `STATUS` varchar(10) NOT NULL COMMENT '正常\n            删除',
  `STATUS_TIME` datetime NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_FA_BULLETIN_REVIEW_REF_BULL` (`BULLETIN_ID`),
  KEY `FK_FA_BULLETIN_RE_REF_REVIEW` (`PARENT_ID`),
  CONSTRAINT `FK_FA_BULLETIN_REVIEW_REF_BULL` FOREIGN KEY (`BULLETIN_ID`) REFERENCES `fa_bulletin` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_FA_BULLETIN_RE_REF_REVIEW` FOREIGN KEY (`PARENT_ID`) REFERENCES `fa_bulletin_review` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_bulletin_review`
--

LOCK TABLES `fa_bulletin_review` WRITE;
/*!40000 ALTER TABLE `fa_bulletin_review` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_bulletin_review` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_bulletin_role`
--

DROP TABLE IF EXISTS `fa_bulletin_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_bulletin_role` (
  `BULLETIN_ID` int(11) NOT NULL,
  `ROLE_ID` int(11) NOT NULL,
  PRIMARY KEY (`BULLETIN_ID`,`ROLE_ID`),
  KEY `FK_FA_BULLETIN_ROLE_REF_ROLE` (`ROLE_ID`),
  CONSTRAINT `FK_FA_BULLETIN_ROLE_REF_BULL` FOREIGN KEY (`BULLETIN_ID`) REFERENCES `fa_bulletin` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_FA_BULLETIN_ROLE_REF_ROLE` FOREIGN KEY (`ROLE_ID`) REFERENCES `fa_role` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_bulletin_role`
--

LOCK TABLES `fa_bulletin_role` WRITE;
/*!40000 ALTER TABLE `fa_bulletin_role` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_bulletin_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_bulletin_type`
--

DROP TABLE IF EXISTS `fa_bulletin_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_bulletin_type` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `NAME` varchar(80) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_bulletin_type`
--

LOCK TABLES `fa_bulletin_type` WRITE;
/*!40000 ALTER TABLE `fa_bulletin_type` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_bulletin_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_config`
--

DROP TABLE IF EXISTS `fa_config`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_config` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `TYPE` varchar(10) DEFAULT NULL,
  `CODE` varchar(32) NOT NULL,
  `NAME` varchar(50) DEFAULT NULL,
  `VALUE` varchar(300) DEFAULT NULL,
  `REMARK` varchar(500) DEFAULT NULL,
  `REGION` varchar(10) NOT NULL,
  `ADD_USER_ID` int(11) DEFAULT NULL,
  `ADD_TIEM` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_config`
--

LOCK TABLES `fa_config` WRITE;
/*!40000 ALTER TABLE `fa_config` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_config` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_db_server`
--

DROP TABLE IF EXISTS `fa_db_server`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_db_server` (
  `ID` int(11) NOT NULL,
  `DB_TYPE_ID` int(11) NOT NULL,
  `TYPE` varchar(10) NOT NULL COMMENT 'DB2\n            ORACLE',
  `IP` varchar(20) NOT NULL,
  `PORT` int(11) NOT NULL,
  `DBNAME` varchar(20) DEFAULT NULL,
  `DBUID` varchar(20) NOT NULL,
  `PASSWORD` varchar(32) NOT NULL,
  `REMARK` varchar(500) DEFAULT NULL,
  `DB_LINK` varchar(200) DEFAULT NULL,
  `NICKNAME` varchar(32) DEFAULT NULL,
  `TO_PATH_M` varchar(300) DEFAULT NULL,
  `TO_PATH_D` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_FA_DB_SERVER_REF_TYPE` (`DB_TYPE_ID`),
  CONSTRAINT `FK_FA_DB_SERVER_REF_TYPE` FOREIGN KEY (`DB_TYPE_ID`) REFERENCES `fa_db_server_type` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_db_server`
--

LOCK TABLES `fa_db_server` WRITE;
/*!40000 ALTER TABLE `fa_db_server` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_db_server` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_db_server_type`
--

DROP TABLE IF EXISTS `fa_db_server_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_db_server_type` (
  `ID` int(11) NOT NULL,
  `NAME` varchar(20) DEFAULT NULL,
  `REMARK` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_db_server_type`
--

LOCK TABLES `fa_db_server_type` WRITE;
/*!40000 ALTER TABLE `fa_db_server_type` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_db_server_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_district`
--

DROP TABLE IF EXISTS `fa_district`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_district` (
  `ID` int(11) NOT NULL,
  `PARENT_ID` int(11) DEFAULT NULL,
  `NAME` varchar(255) NOT NULL,
  `CODE` varchar(50) DEFAULT NULL,
  `IN_USE` decimal(1,0) NOT NULL,
  `LEVEL_ID` int(11) NOT NULL COMMENT '1市\n            2区县\n            3片区\n            4网格',
  `ID_PATH` varchar(200) DEFAULT NULL,
  `REGION` varchar(10) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_FA_DISTRICT_REF_DISTRICT` (`PARENT_ID`),
  CONSTRAINT `FK_FA_DISTRICT_REF_DISTRICT` FOREIGN KEY (`PARENT_ID`) REFERENCES `fa_district` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_district`
--

LOCK TABLES `fa_district` WRITE;
/*!40000 ALTER TABLE `fa_district` DISABLE KEYS */;
INSERT INTO `fa_district` VALUES (1,NULL,'百家姓','1',1,0,'.','1');
/*!40000 ALTER TABLE `fa_district` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_dynasty`
--

DROP TABLE IF EXISTS `fa_dynasty`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_dynasty` (
  `ID` int(11) NOT NULL,
  `NAME` varchar(20) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_dynasty`
--

LOCK TABLES `fa_dynasty` WRITE;
/*!40000 ALTER TABLE `fa_dynasty` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_dynasty` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_elder`
--

DROP TABLE IF EXISTS `fa_elder`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_elder` (
  `ID` int(11) NOT NULL,
  `FAMILY_ID` int(11) DEFAULT NULL,
  `NAME` varchar(2) NOT NULL,
  `SORT` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_FA_ELDER_REF_FAMILY` (`FAMILY_ID`),
  CONSTRAINT `FK_FA_ELDER_REF_FAMILY` FOREIGN KEY (`FAMILY_ID`) REFERENCES `fa_family` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_elder`
--

LOCK TABLES `fa_elder` WRITE;
/*!40000 ALTER TABLE `fa_elder` DISABLE KEYS */;
INSERT INTO `fa_elder` VALUES (19,1,'允',1),(20,1,'大',2),(24,1,'炳',3),(25,1,'世',4),(26,1,'泽',5);
/*!40000 ALTER TABLE `fa_elder` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_event_files`
--

DROP TABLE IF EXISTS `fa_event_files`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_event_files` (
  `EVENT_ID` int(11) NOT NULL,
  `FILES_ID` int(11) NOT NULL,
  PRIMARY KEY (`EVENT_ID`,`FILES_ID`),
  KEY `FK_FA_EVENT_FILES_REF_FILES` (`FILES_ID`),
  CONSTRAINT `FK_FA_EVENT_FILES_REF_EVENT` FOREIGN KEY (`EVENT_ID`) REFERENCES `fa_user_event` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_FA_EVENT_FILES_REF_FILES` FOREIGN KEY (`FILES_ID`) REFERENCES `fa_files` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_event_files`
--

LOCK TABLES `fa_event_files` WRITE;
/*!40000 ALTER TABLE `fa_event_files` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_event_files` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_export_log`
--

DROP TABLE IF EXISTS `fa_export_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_export_log` (
  `ID` int(11) NOT NULL,
  `USER_ID` int(11) DEFAULT NULL,
  `LOGIN_NAME` varchar(50) DEFAULT NULL,
  `NAME` varchar(50) DEFAULT NULL,
  `SQL_CONTENT` text,
  `EXPORT_TIME` datetime DEFAULT NULL,
  `REMARK` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_export_log`
--

LOCK TABLES `fa_export_log` WRITE;
/*!40000 ALTER TABLE `fa_export_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_export_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_family`
--

DROP TABLE IF EXISTS `fa_family`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_family` (
  `ID` int(11) NOT NULL,
  `NAME` varchar(20) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_family`
--

LOCK TABLES `fa_family` WRITE;
/*!40000 ALTER TABLE `fa_family` DISABLE KEYS */;
INSERT INTO `fa_family` VALUES (1,'翁');
/*!40000 ALTER TABLE `fa_family` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_files`
--

DROP TABLE IF EXISTS `fa_files`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_files` (
  `ID` int(11) NOT NULL,
  `NAME` varchar(50) NOT NULL,
  `PATH` varchar(200) NOT NULL,
  `USER_ID` int(11) DEFAULT NULL,
  `LENGTH` int(11) NOT NULL,
  `UPLOAD_TIME` datetime DEFAULT NULL,
  `REMARK` varchar(2000) DEFAULT NULL,
  `URL` varchar(254) DEFAULT NULL,
  `FILE_TYPE` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_files`
--

LOCK TABLES `fa_files` WRITE;
/*!40000 ALTER TABLE `fa_files` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_files` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_flow`
--

DROP TABLE IF EXISTS `fa_flow`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_flow` (
  `ID` int(11) NOT NULL,
  `NAME` varchar(100) NOT NULL,
  `FLOW_TYPE` varchar(20) NOT NULL COMMENT '数据外导\n            薪酬结果\n            政策修改',
  `REMARK` varchar(100) DEFAULT NULL,
  `X_Y` varchar(500) DEFAULT NULL COMMENT 'ID,\n            X,\n            Y',
  `REGION` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_flow`
--

LOCK TABLES `fa_flow` WRITE;
/*!40000 ALTER TABLE `fa_flow` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_flow` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_flow_flownode`
--

DROP TABLE IF EXISTS `fa_flow_flownode`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_flow_flownode` (
  `ID` int(11) NOT NULL,
  `NAME` varchar(100) NOT NULL,
  `HANDLE_URL` varchar(200) DEFAULT NULL,
  `SHOW_URL` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_flow_flownode`
--

LOCK TABLES `fa_flow_flownode` WRITE;
/*!40000 ALTER TABLE `fa_flow_flownode` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_flow_flownode` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_flow_flownode_flow`
--

DROP TABLE IF EXISTS `fa_flow_flownode_flow`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_flow_flownode_flow` (
  `ID` int(11) NOT NULL,
  `FLOW_ID` int(11) NOT NULL,
  `FROM_FLOWNODE_ID` int(11) NOT NULL,
  `TO_FLOWNODE_ID` int(11) NOT NULL,
  `HANDLE` decimal(1,0) NOT NULL COMMENT '0:一人处理即可\n            1:所有人必须处理',
  `ASSIGNER` decimal(1,0) NOT NULL COMMENT '0表示指定角色\n            1表示指定人\n            2返回上级\n            3发起人\n            4已处理人',
  `STATUS` varchar(20) DEFAULT NULL,
  `REMARK` varchar(20) DEFAULT NULL,
  `EXPIRE_HOUR` int(11) NOT NULL COMMENT '下一步处理时长',
  PRIMARY KEY (`ID`),
  KEY `FK_FA_FLOWNODE_FLOW_REF_FLOW` (`FLOW_ID`),
  KEY `FK_FA_FLOWNODE_FLOW_REF_NODE` (`FROM_FLOWNODE_ID`),
  CONSTRAINT `FK_FA_FLOWNODE_FLOW_REF_FLOW` FOREIGN KEY (`FLOW_ID`) REFERENCES `fa_flow` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_FA_FLOWNODE_FLOW_REF_NODE` FOREIGN KEY (`FROM_FLOWNODE_ID`) REFERENCES `fa_flow_flownode` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_flow_flownode_flow`
--

LOCK TABLES `fa_flow_flownode_flow` WRITE;
/*!40000 ALTER TABLE `fa_flow_flownode_flow` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_flow_flownode_flow` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_flow_flownode_role`
--

DROP TABLE IF EXISTS `fa_flow_flownode_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_flow_flownode_role` (
  `FLOW_ID` int(11) NOT NULL,
  `ROLE_ID` int(11) NOT NULL,
  PRIMARY KEY (`FLOW_ID`,`ROLE_ID`),
  KEY `FK_FA_FLOW_REF_ROLE` (`ROLE_ID`),
  CONSTRAINT `FK_FA_FLOW_REF_ROLE` FOREIGN KEY (`ROLE_ID`) REFERENCES `fa_role` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_FA_FLOW_ROLE_REF_FLOWN_FLOW` FOREIGN KEY (`FLOW_ID`) REFERENCES `fa_flow_flownode_flow` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_flow_flownode_role`
--

LOCK TABLES `fa_flow_flownode_role` WRITE;
/*!40000 ALTER TABLE `fa_flow_flownode_role` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_flow_flownode_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_function`
--

DROP TABLE IF EXISTS `fa_function`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_function` (
  `ID` int(11) NOT NULL,
  `REMARK` varchar(100) DEFAULT NULL,
  `FULL_NAME` varchar(100) DEFAULT NULL,
  `NAMESPACE` varchar(100) DEFAULT NULL,
  `CLASS_NAME` varchar(100) DEFAULT NULL,
  `METHOD_NAME` varchar(100) DEFAULT NULL,
  `DLL_NAME` varchar(100) DEFAULT NULL,
  `XML_NOTE` varchar(254) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_function`
--

LOCK TABLES `fa_function` WRITE;
/*!40000 ALTER TABLE `fa_function` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_function` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_log`
--

DROP TABLE IF EXISTS `fa_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_log` (
  `ID` int(11) NOT NULL,
  `ADD_TIME` datetime NOT NULL,
  `MODULE_NAME` varchar(100) NOT NULL,
  `USER_ID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_log`
--

LOCK TABLES `fa_log` WRITE;
/*!40000 ALTER TABLE `fa_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_login`
--

DROP TABLE IF EXISTS `fa_login`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_login` (
  `ID` int(11) NOT NULL,
  `LOGIN_NAME` varchar(20) DEFAULT NULL,
  `PASSWORD` varchar(255) DEFAULT NULL,
  `PHONE_NO` varchar(20) DEFAULT NULL,
  `EMAIL_ADDR` varchar(255) DEFAULT NULL,
  `VERIFY_CODE` varchar(10) DEFAULT NULL,
  `VERIFY_TIME` datetime DEFAULT NULL,
  `IS_LOCKED` decimal(1,0) DEFAULT NULL,
  `PASS_UPDATE_DATE` datetime DEFAULT NULL,
  `LOCKED_REASON` varchar(255) DEFAULT NULL,
  `REGION` varchar(10) NOT NULL,
  `FAIL_COUNT` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_login`
--

LOCK TABLES `fa_login` WRITE;
/*!40000 ALTER TABLE `fa_login` DISABLE KEYS */;
INSERT INTO `fa_login` VALUES (1,'12345678901','aasf','12345678901',NULL,NULL,NULL,NULL,NULL,NULL,'1',NULL),(16,'18180770313','e10adc3949ba59abbe56e057f20f883e','18180770313',NULL,NULL,NULL,0,NULL,NULL,'1',0);
/*!40000 ALTER TABLE `fa_login` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_login_history`
--

DROP TABLE IF EXISTS `fa_login_history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_login_history` (
  `ID` int(11) NOT NULL,
  `USER_ID` int(11) DEFAULT NULL,
  `LOGIN_TIME` datetime DEFAULT NULL,
  `LOGIN_HOST` varchar(255) DEFAULT NULL,
  `LOGOUT_TIME` datetime DEFAULT NULL,
  `LOGIN_HISTORY_TYPE` int(11) DEFAULT NULL COMMENT '1:正常登录\n            2:密码错误\n            3:验证码错误\n            4:工号锁定',
  `MESSAGE` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_login_history`
--

LOCK TABLES `fa_login_history` WRITE;
/*!40000 ALTER TABLE `fa_login_history` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_login_history` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_message`
--

DROP TABLE IF EXISTS `fa_message`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_message` (
  `ID` int(11) NOT NULL,
  `MESSAGE_TYPE_ID` int(11) DEFAULT NULL,
  `KEY_ID` int(11) DEFAULT NULL,
  `TITLE` varchar(100) DEFAULT NULL,
  `CONTENT` varchar(500) DEFAULT NULL,
  `CREATE_TIME` datetime DEFAULT NULL,
  `CREATE_USERNAME` varchar(50) DEFAULT NULL,
  `CREATE_USERID` int(11) DEFAULT NULL,
  `STATUS` varchar(10) DEFAULT NULL COMMENT '正常\n            禁用',
  `PUSH_TYPE` varchar(10) DEFAULT NULL COMMENT '短信\n            手机推送\n            WEB获取\n            智能推送',
  `DISTRICT_ID` int(11) DEFAULT NULL,
  `ALL_ROLE_ID` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_FA_MESSAGE_REF_MESSAGE_TYPE` (`MESSAGE_TYPE_ID`),
  CONSTRAINT `FK_FA_MESSAGE_REF_MESSAGE_TYPE` FOREIGN KEY (`MESSAGE_TYPE_ID`) REFERENCES `fa_message_type` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_message`
--

LOCK TABLES `fa_message` WRITE;
/*!40000 ALTER TABLE `fa_message` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_message` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_message_type`
--

DROP TABLE IF EXISTS `fa_message_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_message_type` (
  `ID` int(11) NOT NULL,
  `NAME` varchar(50) DEFAULT NULL,
  `TABLE_NAME` varchar(50) DEFAULT NULL,
  `IS_USE` int(11) DEFAULT NULL,
  `REMARK` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_message_type`
--

LOCK TABLES `fa_message_type` WRITE;
/*!40000 ALTER TABLE `fa_message_type` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_message_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_module`
--

DROP TABLE IF EXISTS `fa_module`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_module` (
  `ID` int(11) NOT NULL,
  `PARENT_ID` int(11) DEFAULT NULL,
  `NAME` varchar(60) DEFAULT NULL,
  `LOCATION` varchar(2000) DEFAULT NULL,
  `CODE` varchar(20) DEFAULT NULL,
  `IS_DEBUG` decimal(1,0) NOT NULL,
  `IS_HIDE` decimal(1,0) NOT NULL,
  `SHOW_ORDER` decimal(2,0) NOT NULL,
  `DESCRIPTION` varchar(2000) DEFAULT NULL,
  `IMAGE_URL` varchar(2000) DEFAULT NULL,
  `DESKTOP_ROLE` varchar(200) DEFAULT NULL,
  `W` int(11) DEFAULT NULL,
  `H` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_FA_MODULE_REF_MODULE` (`PARENT_ID`),
  CONSTRAINT `FK_FA_MODULE_REF_MODULE` FOREIGN KEY (`PARENT_ID`) REFERENCES `fa_module` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_module`
--

LOCK TABLES `fa_module` WRITE;
/*!40000 ALTER TABLE `fa_module` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_module` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_oauth`
--

DROP TABLE IF EXISTS `fa_oauth`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_oauth` (
  `ID` int(11) NOT NULL,
  `NAME` varchar(50) DEFAULT NULL,
  `REG_URL` varchar(500) DEFAULT NULL,
  `LOGIN_URL` varchar(500) DEFAULT NULL,
  `REMARK` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_oauth`
--

LOCK TABLES `fa_oauth` WRITE;
/*!40000 ALTER TABLE `fa_oauth` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_oauth` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_oauth_login`
--

DROP TABLE IF EXISTS `fa_oauth_login`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_oauth_login` (
  `OAUTH_ID` int(11) NOT NULL,
  `LOGIN_ID` int(11) NOT NULL,
  PRIMARY KEY (`OAUTH_ID`,`LOGIN_ID`),
  KEY `FK_FA_OAUTH_REF_LOGIN` (`LOGIN_ID`),
  CONSTRAINT `FK_FA_OAUTH_REF_LOGIN` FOREIGN KEY (`LOGIN_ID`) REFERENCES `fa_login` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_FA_OAUTH_REF_OAUTH` FOREIGN KEY (`OAUTH_ID`) REFERENCES `fa_oauth` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_oauth_login`
--

LOCK TABLES `fa_oauth_login` WRITE;
/*!40000 ALTER TABLE `fa_oauth_login` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_oauth_login` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_query`
--

DROP TABLE IF EXISTS `fa_query`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_query` (
  `ID` int(11) NOT NULL,
  `NAME` varchar(50) NOT NULL,
  `CODE` varchar(20) NOT NULL,
  `AUTO_LOAD` decimal(1,0) NOT NULL,
  `PAGE_SIZE` int(11) NOT NULL,
  `SHOW_CHECKBOX` decimal(1,0) NOT NULL,
  `IS_DEBUG` decimal(1,0) NOT NULL,
  `FILTR_LEVEL` decimal(1,0) DEFAULT NULL,
  `DB_SERVER_ID` int(11) DEFAULT NULL,
  `QUERY_CONF` text,
  `QUERY_CFG_JSON` text,
  `IN_PARA_JSON` text,
  `JS_STR` text,
  `ROWS_BTN` text,
  `HEARD_BTN` text,
  `REPORT_SCRIPT` text,
  `CHARTS_CFG` text,
  `CHARTS_TYPE` varchar(50) DEFAULT NULL,
  `FILTR_STR` text,
  `REMARK` text,
  `NEW_DATA` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_query`
--

LOCK TABLES `fa_query` WRITE;
/*!40000 ALTER TABLE `fa_query` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_query` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_role`
--

DROP TABLE IF EXISTS `fa_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_role` (
  `ID` int(11) NOT NULL,
  `NAME` varchar(80) DEFAULT NULL,
  `REMARK` varchar(255) DEFAULT NULL,
  `TYPE` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_role`
--

LOCK TABLES `fa_role` WRITE;
/*!40000 ALTER TABLE `fa_role` DISABLE KEYS */;
INSERT INTO `fa_role` VALUES (4,'系统管理员5','超级管理员使用111',NULL);
/*!40000 ALTER TABLE `fa_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_role_config`
--

DROP TABLE IF EXISTS `fa_role_config`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_role_config` (
  `ID` int(11) NOT NULL,
  `ROLE_ID` int(11) NOT NULL,
  `TYPE` varchar(10) DEFAULT NULL,
  `NAME` varchar(50) NOT NULL,
  `VALUE` varchar(300) DEFAULT NULL,
  `REMARK` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_FA_ROLE_CONFIG_REF_ROLE` (`ROLE_ID`),
  CONSTRAINT `FK_FA_ROLE_CONFIG_REF_ROLE` FOREIGN KEY (`ROLE_ID`) REFERENCES `fa_role` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_role_config`
--

LOCK TABLES `fa_role_config` WRITE;
/*!40000 ALTER TABLE `fa_role_config` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_role_config` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_role_function`
--

DROP TABLE IF EXISTS `fa_role_function`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_role_function` (
  `FUNCTION_ID` int(11) NOT NULL,
  `ROLE_ID` int(11) NOT NULL,
  PRIMARY KEY (`FUNCTION_ID`,`ROLE_ID`),
  KEY `FK_FA_ROLE_FUNCTION_REF_ROLE` (`ROLE_ID`),
  CONSTRAINT `FK_FA_ROLE_FUNCTION_REF_FUNCT` FOREIGN KEY (`FUNCTION_ID`) REFERENCES `fa_function` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_FA_ROLE_FUNCTION_REF_ROLE` FOREIGN KEY (`ROLE_ID`) REFERENCES `fa_role` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_role_function`
--

LOCK TABLES `fa_role_function` WRITE;
/*!40000 ALTER TABLE `fa_role_function` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_role_function` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_role_module`
--

DROP TABLE IF EXISTS `fa_role_module`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_role_module` (
  `ROLE_ID` int(11) NOT NULL,
  `MODULE_ID` int(11) NOT NULL,
  PRIMARY KEY (`ROLE_ID`,`MODULE_ID`),
  KEY `FK_FA_ROLE_MODULE_REF_MODULE` (`MODULE_ID`),
  CONSTRAINT `FK_FA_ROLE_MODULE_REF_MODULE` FOREIGN KEY (`MODULE_ID`) REFERENCES `fa_module` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_FA_ROLE_MODULE_REF_ROLE` FOREIGN KEY (`ROLE_ID`) REFERENCES `fa_role` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_role_module`
--

LOCK TABLES `fa_role_module` WRITE;
/*!40000 ALTER TABLE `fa_role_module` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_role_module` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_role_query_authority`
--

DROP TABLE IF EXISTS `fa_role_query_authority`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_role_query_authority` (
  `ROLE_ID` int(11) NOT NULL,
  `QUERY_ID` int(11) NOT NULL,
  `NO_AUTHORITY` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`ROLE_ID`,`QUERY_ID`),
  KEY `FK_FA_ROLE_QUERY_REF_QUERY` (`QUERY_ID`),
  CONSTRAINT `FK_FA_ROLE_QUERY_REF_QUERY` FOREIGN KEY (`QUERY_ID`) REFERENCES `fa_query` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_FA_ROLE_QUERY_REF_ROLE` FOREIGN KEY (`ROLE_ID`) REFERENCES `fa_role` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_role_query_authority`
--

LOCK TABLES `fa_role_query_authority` WRITE;
/*!40000 ALTER TABLE `fa_role_query_authority` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_role_query_authority` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_script`
--

DROP TABLE IF EXISTS `fa_script`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_script` (
  `ID` int(11) NOT NULL,
  `CODE` varchar(20) NOT NULL,
  `NAME` varchar(255) NOT NULL,
  `BODY_TEXT` text NOT NULL,
  `BODY_HASH` varchar(255) NOT NULL,
  `RUN_WHEN` varchar(30) DEFAULT NULL,
  `RUN_ARGS` varchar(255) DEFAULT NULL,
  `RUN_DATA` varchar(20) NOT NULL DEFAULT '0',
  `STATUS` varchar(10) DEFAULT NULL COMMENT '正常\n            禁用',
  `DISABLE_REASON` varchar(50) DEFAULT NULL,
  `SERVICE_FLAG` varchar(50) DEFAULT NULL,
  `REGION` varchar(10) DEFAULT NULL,
  `IS_GROUP` decimal(1,0) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_script`
--

LOCK TABLES `fa_script` WRITE;
/*!40000 ALTER TABLE `fa_script` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_script` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_script_group_list`
--

DROP TABLE IF EXISTS `fa_script_group_list`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_script_group_list` (
  `SCRIPT_ID` int(11) NOT NULL,
  `GROUP_ID` int(11) NOT NULL,
  `ORDER_INDEX` int(11) NOT NULL,
  PRIMARY KEY (`SCRIPT_ID`,`GROUP_ID`),
  KEY `FK_FA_GROUP_LIST_REF_SCRIPT` (`GROUP_ID`),
  CONSTRAINT `FK_FA_GROUP_LIST_REF_SCRIPT` FOREIGN KEY (`GROUP_ID`) REFERENCES `fa_script` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_script_group_list`
--

LOCK TABLES `fa_script_group_list` WRITE;
/*!40000 ALTER TABLE `fa_script_group_list` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_script_group_list` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_script_task`
--

DROP TABLE IF EXISTS `fa_script_task`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_script_task` (
  `ID` int(11) NOT NULL,
  `SCRIPT_ID` int(11) NOT NULL,
  `BODY_TEXT` text NOT NULL,
  `BODY_HASH` varchar(255) NOT NULL,
  `RUN_STATE` varchar(10) NOT NULL DEFAULT '0' COMMENT '0等待\n            1运行\n            2停止',
  `RUN_WHEN` varchar(30) DEFAULT NULL,
  `RUN_ARGS` varchar(255) DEFAULT NULL,
  `RUN_DATA` varchar(20) NOT NULL DEFAULT '0',
  `LOG_TYPE` decimal(1,0) DEFAULT '0',
  `DSL_TYPE` varchar(255) DEFAULT NULL,
  `RETURN_CODE` varchar(10) DEFAULT '0' COMMENT '成功\n            失败',
  `START_TIME` datetime DEFAULT NULL,
  `END_TIME` datetime DEFAULT NULL,
  `DISABLE_DATE` datetime DEFAULT NULL,
  `DISABLE_REASON` varchar(50) DEFAULT NULL,
  `SERVICE_FLAG` varchar(50) DEFAULT NULL,
  `REGION` varchar(10) DEFAULT NULL,
  `GROUP_ID` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_FA_SCRIPT_TASK_REF_SCRIPT` (`SCRIPT_ID`),
  CONSTRAINT `FK_FA_SCRIPT_TASK_REF_SCRIPT` FOREIGN KEY (`SCRIPT_ID`) REFERENCES `fa_script` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_script_task`
--

LOCK TABLES `fa_script_task` WRITE;
/*!40000 ALTER TABLE `fa_script_task` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_script_task` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_script_task_log`
--

DROP TABLE IF EXISTS `fa_script_task_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_script_task_log` (
  `ID` int(11) NOT NULL,
  `SCRIPT_TASK_ID` int(11) NOT NULL,
  `LOG_TIME` datetime NOT NULL,
  `LOG_TYPE` decimal(1,0) NOT NULL DEFAULT '1' COMMENT '0正常日志\n            1错误日志',
  `MESSAGE` text,
  `SQL_TEXT` text,
  PRIMARY KEY (`ID`),
  KEY `FK_FA_SCRIPT_TASK_LOG_REF_TASK` (`SCRIPT_TASK_ID`),
  CONSTRAINT `FK_FA_SCRIPT_TASK_LOG_REF_TASK` FOREIGN KEY (`SCRIPT_TASK_ID`) REFERENCES `fa_script_task` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_script_task_log`
--

LOCK TABLES `fa_script_task_log` WRITE;
/*!40000 ALTER TABLE `fa_script_task_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_script_task_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_sms_send`
--

DROP TABLE IF EXISTS `fa_sms_send`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_sms_send` (
  `GUID` char(32) NOT NULL,
  `MESSAGE_ID` int(11) DEFAULT NULL,
  `PHONE_NO` varchar(50) NOT NULL,
  `ADD_TIME` datetime DEFAULT NULL,
  `SEND_TIME` datetime DEFAULT NULL,
  `CONTENT` varchar(500) NOT NULL,
  `STAUTS` varchar(15) DEFAULT NULL,
  `TRY_NUM` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`GUID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_sms_send`
--

LOCK TABLES `fa_sms_send` WRITE;
/*!40000 ALTER TABLE `fa_sms_send` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_sms_send` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_task`
--

DROP TABLE IF EXISTS `fa_task`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_task` (
  `ID` int(11) NOT NULL,
  `FLOW_ID` int(11) DEFAULT NULL,
  `TASK_NAME` varchar(50) DEFAULT NULL,
  `CREATE_TIME` datetime DEFAULT NULL,
  `CREATE_USER` int(11) DEFAULT NULL,
  `CREATE_USER_NAME` varchar(50) DEFAULT NULL,
  `STATUS` varchar(50) DEFAULT NULL,
  `STATUS_TIME` datetime DEFAULT NULL,
  `REMARK` text,
  `REGION` varchar(10) DEFAULT NULL,
  `KEY_ID` varchar(32) DEFAULT NULL,
  `START_TIME` datetime DEFAULT NULL,
  `END_TIME` datetime DEFAULT NULL,
  `DEAL_TIME` datetime DEFAULT NULL,
  `ROLE_ID_STR` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_FA_FLOW_TASK_REF_FLOW` (`FLOW_ID`),
  CONSTRAINT `FK_FA_FLOW_TASK_REF_FLOW` FOREIGN KEY (`FLOW_ID`) REFERENCES `fa_flow` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_task`
--

LOCK TABLES `fa_task` WRITE;
/*!40000 ALTER TABLE `fa_task` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_task` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_task_flow`
--

DROP TABLE IF EXISTS `fa_task_flow`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_task_flow` (
  `ID` int(11) NOT NULL,
  `PARENT_ID` int(11) DEFAULT NULL,
  `TASK_ID` int(11) NOT NULL,
  `LEVEL_ID` int(11) DEFAULT NULL,
  `FLOWNODE_ID` int(11) DEFAULT NULL,
  `EQUAL_ID` int(11) DEFAULT NULL,
  `IS_HANDLE` int(11) NOT NULL,
  `NAME` varchar(100) DEFAULT NULL,
  `HANDLE_URL` varchar(200) DEFAULT NULL,
  `SHOW_URL` varchar(200) DEFAULT NULL,
  `EXPIRE_TIME` datetime DEFAULT NULL,
  `START_TIME` datetime NOT NULL,
  `DEAL_STATUS` varchar(50) DEFAULT NULL COMMENT '阶段回复\n            回复',
  `ROLE_ID_STR` varchar(200) DEFAULT NULL,
  `HANDLE_USER_ID` int(11) DEFAULT NULL,
  `DEAL_TIME` datetime DEFAULT NULL,
  `ACCEPT_TIME` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_FA_TASK_FLOW_REF_TASK` (`TASK_ID`),
  KEY `FK_FA_TASK_FLOW_REF_TASK_FLOW` (`PARENT_ID`),
  CONSTRAINT `FK_FA_TASK_FLOW_REF_TASK` FOREIGN KEY (`TASK_ID`) REFERENCES `fa_task` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_FA_TASK_FLOW_REF_TASK_FLOW` FOREIGN KEY (`PARENT_ID`) REFERENCES `fa_task_flow` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_task_flow`
--

LOCK TABLES `fa_task_flow` WRITE;
/*!40000 ALTER TABLE `fa_task_flow` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_task_flow` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_task_flow_handle`
--

DROP TABLE IF EXISTS `fa_task_flow_handle`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_task_flow_handle` (
  `ID` int(11) NOT NULL,
  `TASK_FLOW_ID` int(11) NOT NULL,
  `DEAL_USER_ID` int(11) NOT NULL,
  `DEAL_USER_NAME` varchar(50) NOT NULL,
  `DEAL_TIME` datetime NOT NULL,
  `CONTENT` varchar(2000) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_TASK_FLOW_HANDLE_REF_FLOW` (`TASK_FLOW_ID`),
  CONSTRAINT `FK_TASK_FLOW_HANDLE_REF_FLOW` FOREIGN KEY (`TASK_FLOW_ID`) REFERENCES `fa_task_flow` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_task_flow_handle`
--

LOCK TABLES `fa_task_flow_handle` WRITE;
/*!40000 ALTER TABLE `fa_task_flow_handle` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_task_flow_handle` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_task_flow_handle_files`
--

DROP TABLE IF EXISTS `fa_task_flow_handle_files`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_task_flow_handle_files` (
  `FLOW_HANDLE_ID` int(11) NOT NULL,
  `FILES_ID` int(11) NOT NULL,
  PRIMARY KEY (`FLOW_HANDLE_ID`,`FILES_ID`),
  KEY `FK_FLOW_HANDLE_REF_FILES` (`FILES_ID`),
  CONSTRAINT `FK_FLOW_HANDLE_REF_FILES` FOREIGN KEY (`FILES_ID`) REFERENCES `fa_files` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_FLOW_HANDLE_REF_HANDLE` FOREIGN KEY (`FLOW_HANDLE_ID`) REFERENCES `fa_task_flow_handle` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_task_flow_handle_files`
--

LOCK TABLES `fa_task_flow_handle_files` WRITE;
/*!40000 ALTER TABLE `fa_task_flow_handle_files` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_task_flow_handle_files` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_task_flow_handle_user`
--

DROP TABLE IF EXISTS `fa_task_flow_handle_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_task_flow_handle_user` (
  `TASK_FLOW_ID` int(11) NOT NULL,
  `HANDLE_USER_ID` int(11) NOT NULL,
  PRIMARY KEY (`TASK_FLOW_ID`,`HANDLE_USER_ID`),
  CONSTRAINT `FK_TASK_HANDEL_USER_REF_FLOW` FOREIGN KEY (`TASK_FLOW_ID`) REFERENCES `fa_task_flow` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_task_flow_handle_user`
--

LOCK TABLES `fa_task_flow_handle_user` WRITE;
/*!40000 ALTER TABLE `fa_task_flow_handle_user` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_task_flow_handle_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_user`
--

DROP TABLE IF EXISTS `fa_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_user` (
  `ID` int(11) NOT NULL,
  `NAME` varchar(80) DEFAULT NULL,
  `LOGIN_NAME` varchar(20) DEFAULT NULL,
  `ICON_FILES_ID` int(11) DEFAULT NULL,
  `DISTRICT_ID` int(11) NOT NULL,
  `IS_LOCKED` decimal(1,0) DEFAULT NULL,
  `CREATE_TIME` datetime DEFAULT NULL,
  `LOGIN_COUNT` int(11) DEFAULT NULL,
  `LAST_LOGIN_TIME` datetime DEFAULT NULL,
  `LAST_LOGOUT_TIME` datetime DEFAULT NULL,
  `LAST_ACTIVE_TIME` datetime DEFAULT NULL,
  `REMARK` varchar(2000) DEFAULT NULL,
  `REGION` varchar(10) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_FA_USER_REF_DISTRICT` (`DISTRICT_ID`),
  CONSTRAINT `FK_FA_USER_REF_DISTRICT` FOREIGN KEY (`DISTRICT_ID`) REFERENCES `fa_district` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_user`
--

LOCK TABLES `fa_user` WRITE;
/*!40000 ALTER TABLE `fa_user` DISABLE KEYS */;
INSERT INTO `fa_user` VALUES (1,NULL,'12345678901',NULL,1,0,'2017-05-02 23:21:44',0,'2017-05-02 23:21:44',NULL,'2017-05-02 23:21:44',NULL,'1'),(2,'admin','18180770313',NULL,1,0,'2017-05-06 23:34:20',17,'2017-05-07 15:52:20',NULL,'2017-05-06 23:34:20','翁志来测试','1');
/*!40000 ALTER TABLE `fa_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_user_district`
--

DROP TABLE IF EXISTS `fa_user_district`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_user_district` (
  `USER_ID` int(11) NOT NULL,
  `DISTRICT_ID` int(11) NOT NULL,
  PRIMARY KEY (`USER_ID`,`DISTRICT_ID`),
  KEY `FK_FA_USER_DISTRICT_REF_DIST` (`DISTRICT_ID`),
  CONSTRAINT `FK_FA_USER_DISTRICT_REF_DIST` FOREIGN KEY (`DISTRICT_ID`) REFERENCES `fa_district` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_FA_USER_DISTRICT_REF_USER` FOREIGN KEY (`USER_ID`) REFERENCES `fa_user` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_user_district`
--

LOCK TABLES `fa_user_district` WRITE;
/*!40000 ALTER TABLE `fa_user_district` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_user_district` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_user_event`
--

DROP TABLE IF EXISTS `fa_user_event`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_user_event` (
  `ID` int(11) NOT NULL,
  `USER_ID` int(11) DEFAULT NULL,
  `NAME` varchar(50) DEFAULT NULL,
  `HAPPEN_TIME` datetime DEFAULT NULL,
  `CONTENT` varchar(500) DEFAULT NULL,
  `ADDRESS` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_FA_USER_EVENT_REF_USER` (`USER_ID`),
  CONSTRAINT `FK_FA_USER_EVENT_REF_USER` FOREIGN KEY (`USER_ID`) REFERENCES `fa_user_info` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_user_event`
--

LOCK TABLES `fa_user_event` WRITE;
/*!40000 ALTER TABLE `fa_user_event` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_user_event` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_user_friend`
--

DROP TABLE IF EXISTS `fa_user_friend`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_user_friend` (
  `USER_ID` int(11) NOT NULL,
  `FRIEND_ID` int(11) NOT NULL,
  PRIMARY KEY (`USER_ID`,`FRIEND_ID`),
  KEY `FK_FA_FRIEND_REF_USER` (`FRIEND_ID`),
  CONSTRAINT `FK_FA_FRIEND_REF_USER` FOREIGN KEY (`FRIEND_ID`) REFERENCES `fa_user` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_FA_FRIEND_REF_USERINFO` FOREIGN KEY (`USER_ID`) REFERENCES `fa_user_info` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_user_friend`
--

LOCK TABLES `fa_user_friend` WRITE;
/*!40000 ALTER TABLE `fa_user_friend` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_user_friend` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_user_info`
--

DROP TABLE IF EXISTS `fa_user_info`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_user_info` (
  `ID` int(11) NOT NULL,
  `LEVEL_ID` int(11) DEFAULT NULL,
  `FAMILY_ID` int(11) DEFAULT NULL,
  `ELDER_ID` int(11) DEFAULT NULL,
  `LEVEL_NAME` varchar(2) DEFAULT NULL,
  `FATHER_ID` int(11) DEFAULT NULL,
  `MOTHER_ID` int(11) DEFAULT NULL,
  `BIRTHDAY_TIME` datetime DEFAULT NULL,
  `BIRTHDAY_PLACE` varchar(500) DEFAULT NULL,
  `IS_LIVE` decimal(1,0) DEFAULT NULL,
  `DIED_TIME` datetime DEFAULT NULL,
  `BIRTHDAY_PLACE2` varchar(500) DEFAULT NULL,
  `REMARK` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_FA_USER_REF_ELDER` (`LEVEL_ID`),
  KEY `FK_FA_USER_REF_FAMILY` (`FAMILY_ID`),
  KEY `FK_USER_INFO_REF_USER_INFO` (`FATHER_ID`),
  CONSTRAINT `FK_FA_USER_INFO_REF_USER` FOREIGN KEY (`ID`) REFERENCES `fa_user` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_FA_USER_REF_ELDER` FOREIGN KEY (`LEVEL_ID`) REFERENCES `fa_elder` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_FA_USER_REF_FAMILY` FOREIGN KEY (`FAMILY_ID`) REFERENCES `fa_family` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_USER_INFO_REF_USER_INFO` FOREIGN KEY (`FATHER_ID`) REFERENCES `fa_user_info` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_user_info`
--

LOCK TABLES `fa_user_info` WRITE;
/*!40000 ALTER TABLE `fa_user_info` DISABLE KEYS */;
INSERT INTO `fa_user_info` VALUES (2,NULL,NULL,NULL,'定',NULL,NULL,'1981-03-13 00:00:00',NULL,NULL,NULL,NULL,'翁志来测试');
/*!40000 ALTER TABLE `fa_user_info` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_user_message`
--

DROP TABLE IF EXISTS `fa_user_message`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_user_message` (
  `MESSAGE_ID` int(11) NOT NULL,
  `USER_ID` int(11) NOT NULL,
  `PHONE_NO` varchar(20) DEFAULT NULL,
  `STATUS` varchar(10) DEFAULT NULL COMMENT '等待\n            已推送\n            成功\n            已阅',
  `STATUS_TIME` datetime NOT NULL,
  `REPLY` varchar(500) DEFAULT NULL,
  `PUSH_TYPE` varchar(10) DEFAULT NULL COMMENT '短信\n            手机推送\n            WEB获取\n            智能推送',
  PRIMARY KEY (`MESSAGE_ID`,`USER_ID`),
  CONSTRAINT `FK_FA_USER_MESSAGE_REF_MESSAGE` FOREIGN KEY (`MESSAGE_ID`) REFERENCES `fa_message` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_user_message`
--

LOCK TABLES `fa_user_message` WRITE;
/*!40000 ALTER TABLE `fa_user_message` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_user_message` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_user_module`
--

DROP TABLE IF EXISTS `fa_user_module`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_user_module` (
  `USER_ID` int(11) NOT NULL,
  `MODULE_ID` int(11) NOT NULL,
  PRIMARY KEY (`USER_ID`,`MODULE_ID`),
  KEY `FK_FA_USER_MODULE_REF_MODULE` (`MODULE_ID`),
  CONSTRAINT `FK_FA_USER_MODULE_REF_MODULE` FOREIGN KEY (`MODULE_ID`) REFERENCES `fa_module` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_FA_USER_MODULE_REF_USER` FOREIGN KEY (`USER_ID`) REFERENCES `fa_user` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_user_module`
--

LOCK TABLES `fa_user_module` WRITE;
/*!40000 ALTER TABLE `fa_user_module` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_user_module` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fa_user_role`
--

DROP TABLE IF EXISTS `fa_user_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fa_user_role` (
  `ROLE_ID` int(11) NOT NULL,
  `USER_ID` int(11) NOT NULL,
  PRIMARY KEY (`ROLE_ID`,`USER_ID`),
  KEY `FK_FA_USER_ROLE_REF_USER` (`USER_ID`),
  CONSTRAINT `FK_FA_USER_ROLE_REF_ROLE` FOREIGN KEY (`ROLE_ID`) REFERENCES `fa_role` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_FA_USER_ROLE_REF_USER` FOREIGN KEY (`USER_ID`) REFERENCES `fa_user` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fa_user_role`
--

LOCK TABLES `fa_user_role` WRITE;
/*!40000 ALTER TABLE `fa_user_role` DISABLE KEYS */;
/*!40000 ALTER TABLE `fa_user_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sequence`
--

DROP TABLE IF EXISTS `sequence`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sequence` (
  `seq_name` varchar(50) NOT NULL,
  `current_val` int(11) NOT NULL,
  `increment_val` int(11) NOT NULL DEFAULT '1',
  PRIMARY KEY (`seq_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sequence`
--

LOCK TABLES `sequence` WRITE;
/*!40000 ALTER TABLE `sequence` DISABLE KEYS */;
INSERT INTO `sequence` VALUES ('fa_district_SEQ',1,1),('FA_ELDER_SEQ',26,1),('FA_ELDER_SEQ1',5,1),('FA_ELDER_SEQ2',2,1),('FA_LOGIN_SEQ',16,1),('fa_role_SEQ',4,1),('FA_USER_SEQ',9,1),('seq_test1_num2',6,2);
/*!40000 ALTER TABLE `sequence` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'fa'
--
/*!50003 DROP FUNCTION IF EXISTS `currval` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`FA`@`%` FUNCTION `currval`(v_seq_name VARCHAR(50)) RETURNS int(11)
begin     
    declare value integer;       
    set value = 0;       
    select current_val into value  from sequence where seq_name = v_seq_name; 
    if(value=0)
    then
    insert into sequence(seq_name,current_val,increment_val)values(v_seq_name,1,1);
    select current_val into value  from sequence where seq_name = v_seq_name; 
    end if;
   return value; 
end ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `nextval` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`FA`@`%` FUNCTION `nextval`(v_seq_name VARCHAR(50)) RETURNS int(11)
begin
    update sequence set current_val = current_val + increment_val  where seq_name = v_seq_name;
    return currval(v_seq_name);
end ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-05-10 21:54:29
