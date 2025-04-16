CREATE DATABASE  IF NOT EXISTS `tyemunuzhen_db` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `tyemunuzhen_db`;
-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: localhost    Database: tyemunuzhen_db
-- ------------------------------------------------------
-- Server version	8.0.40

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `act_of_completed_works`
--

DROP TABLE IF EXISTS `act_of_completed_works`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `act_of_completed_works` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `numOfAct` bigint DEFAULT NULL,
  `idNannyInHospitalization` int DEFAULT NULL,
  `countWorkDays` int DEFAULT NULL,
  `payment` decimal(10,2) DEFAULT NULL,
  `filePath` longtext,
  PRIMARY KEY (`ID`),
  KEY `fk_nannyInHospAct_idx` (`idNannyInHospitalization`),
  CONSTRAINT `fk_nannyInHospAct` FOREIGN KEY (`idNannyInHospitalization`) REFERENCES `nannies_in_hospitalization` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `act_of_completed_works`
--

LOCK TABLES `act_of_completed_works` WRITE;
/*!40000 ALTER TABLE `act_of_completed_works` DISABLE KEYS */;
/*!40000 ALTER TABLE `act_of_completed_works` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `actual_program`
--

DROP TABLE IF EXISTS `actual_program`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `actual_program` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `idChild` int DEFAULT NULL,
  `idProgramType` int DEFAULT NULL,
  `idCurator` int DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_childActualProgram_idx` (`idChild`),
  KEY `fk_program_idx` (`idProgramType`),
  KEY `fk_curatorActualProgram_idx` (`idCurator`),
  CONSTRAINT `fk_childActualProgram` FOREIGN KEY (`idChild`) REFERENCES `childrens` (`ID`),
  CONSTRAINT `fk_curatorActualProgram` FOREIGN KEY (`idCurator`) REFERENCES `curators` (`ID`),
  CONSTRAINT `fk_program` FOREIGN KEY (`idProgramType`) REFERENCES `program_type` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `actual_program`
--

LOCK TABLES `actual_program` WRITE;
/*!40000 ALTER TABLE `actual_program` DISABLE KEYS */;
/*!40000 ALTER TABLE `actual_program` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `administrators`
--

DROP TABLE IF EXISTS `administrators`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `administrators` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `surname` varchar(70) DEFAULT NULL,
  `name` varchar(70) DEFAULT NULL,
  `middleName` varchar(70) DEFAULT NULL,
  `phoneNumber` varchar(12) DEFAULT NULL,
  `email` varchar(150) DEFAULT NULL,
  `idUser` int NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_volunteers_users1_idx` (`idUser`),
  CONSTRAINT `fk_volunteers_users100` FOREIGN KEY (`idUser`) REFERENCES `users` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `administrators`
--

LOCK TABLES `administrators` WRITE;
/*!40000 ALTER TABLE `administrators` DISABLE KEYS */;
/*!40000 ALTER TABLE `administrators` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `agreement_doctors`
--

DROP TABLE IF EXISTS `agreement_doctors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `agreement_doctors` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `numOfAgreement` bigint DEFAULT NULL,
  `dateConclusion` date DEFAULT NULL,
  `idDoctor` int DEFAULT NULL,
  `filePath` longtext,
  PRIMARY KEY (`ID`),
  KEY `fk_doctorAgrement_idx` (`idDoctor`),
  CONSTRAINT `fk_doctorAgrement` FOREIGN KEY (`idDoctor`) REFERENCES `doctors_on_agreement` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `agreement_doctors`
--

LOCK TABLES `agreement_doctors` WRITE;
/*!40000 ALTER TABLE `agreement_doctors` DISABLE KEYS */;
/*!40000 ALTER TABLE `agreement_doctors` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `agreement_hospitalization`
--

DROP TABLE IF EXISTS `agreement_hospitalization`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `agreement_hospitalization` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `numOfAgreement` bigint DEFAULT NULL,
  `dateConclusion` date DEFAULT NULL,
  `idHospitalization` int DEFAULT NULL,
  `filePath` longtext,
  PRIMARY KEY (`ID`),
  KEY `fk_hospitalizationAgreement_idx` (`idHospitalization`),
  CONSTRAINT `fk_hospitalizationAgreement` FOREIGN KEY (`idHospitalization`) REFERENCES `hospitalization` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `agreement_hospitalization`
--

LOCK TABLES `agreement_hospitalization` WRITE;
/*!40000 ALTER TABLE `agreement_hospitalization` DISABLE KEYS */;
/*!40000 ALTER TABLE `agreement_hospitalization` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `agreement_nanny_at_hospitalization`
--

DROP TABLE IF EXISTS `agreement_nanny_at_hospitalization`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `agreement_nanny_at_hospitalization` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `numOfAgreement` bigint DEFAULT NULL,
  `dateConclusion` date DEFAULT NULL,
  `idNannyInHospitalization` int DEFAULT NULL,
  `costPerDay` decimal(10,2) DEFAULT NULL,
  `filePath` longtext,
  PRIMARY KEY (`ID`),
  KEY `fk_nannyInHospAgreement_idx` (`idNannyInHospitalization`),
  CONSTRAINT `fk_nannyInHospAgreement` FOREIGN KEY (`idNannyInHospitalization`) REFERENCES `nannies_in_hospitalization` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `agreement_nanny_at_hospitalization`
--

LOCK TABLES `agreement_nanny_at_hospitalization` WRITE;
/*!40000 ALTER TABLE `agreement_nanny_at_hospitalization` DISABLE KEYS */;
/*!40000 ALTER TABLE `agreement_nanny_at_hospitalization` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `agreement_orphanage`
--

DROP TABLE IF EXISTS `agreement_orphanage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `agreement_orphanage` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `numOfAgreement` bigint DEFAULT NULL,
  `dateConclusion` date DEFAULT NULL,
  `idOrphanage` int DEFAULT NULL,
  `filePath` longtext,
  PRIMARY KEY (`ID`),
  KEY `fk_orphanageAgreement_idx` (`idOrphanage`),
  CONSTRAINT `fk_orphanageAgreement` FOREIGN KEY (`idOrphanage`) REFERENCES `orphanages` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `agreement_orphanage`
--

LOCK TABLES `agreement_orphanage` WRITE;
/*!40000 ALTER TABLE `agreement_orphanage` DISABLE KEYS */;
INSERT INTO `agreement_orphanage` VALUES (1,123123,'2024-01-01',1,'фвфвыфыв'),(2,111111,'2024-01-01',3,'фывфывфы');
/*!40000 ALTER TABLE `agreement_orphanage` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `childphoto`
--

DROP TABLE IF EXISTS `childphoto`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `childphoto` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `idChild` int DEFAULT NULL,
  `filePath` longtext,
  `dateAdded` date DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_childphoto_idx` (`idChild`),
  CONSTRAINT `fk_childphoto` FOREIGN KEY (`idChild`) REFERENCES `childrens` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=56 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `childphoto`
--

LOCK TABLES `childphoto` WRITE;
/*!40000 ALTER TABLE `childphoto` DISABLE KEYS */;
INSERT INTO `childphoto` VALUES (9,1,'../../Images/Childrens/кошка.jpg','2025-04-02'),(10,2,'../../Images/Childrens/кошка2.jpg','2025-04-02'),(11,3,'../../Images/Childrens/птичка.jpg','2025-04-02'),(12,4,'../../Images/Childrens/собака.jpg','2025-04-02'),(13,9,'../../Images/Childrens/Пушок.jpg','2025-04-03'),(14,10,'../../Images/Childrens/гретта.jpg','2025-04-03'),(15,11,'../../Images/Childrens/четин.jpg','2025-04-03'),(44,11,'../../Images/Childrens/гретта.jpg','2025-04-05'),(45,14,'../../Images/Childrens/67e9867505590175a80f9412-2.jpg','2025-04-06'),(46,14,'../../Images/Childrens/67e98673348dbe36b20d4b7b-2.jpg','2025-04-06'),(47,15,'../../Images/Childrens/1391050430.jpg','2025-04-07'),(48,16,'../../Images/Childrens/maxresdefault.jpg','2025-04-07'),(49,17,'../../Images/Childrens/arrow-small-left.png','2025-04-07'),(50,18,'../../Images/Childrens/l1048752529.jpg','2025-04-07'),(51,19,'../../Images/Childrens/Пушок.jpg','2025-04-07'),(52,15,'../../Images/Childrens/6709.jpg','2025-04-07'),(53,14,'../../Images/Childrens/revva.jpeg','2025-04-12'),(54,14,'../../Images/Childrens/Children\\20250414202132_55571654-d64c-4d4f-aee7-a516a5d9949e.png','2025-04-14'),(55,9,'../../Images/Childrens/202504160002_excel.png','2025-04-16');
/*!40000 ALTER TABLE `childphoto` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `children_diagnosis`
--

DROP TABLE IF EXISTS `children_diagnosis`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `children_diagnosis` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `idConsultation` int DEFAULT NULL,
  `idDiagnosis` int DEFAULT NULL,
  `updateDate` date DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_diagnosis_idx` (`idDiagnosis`),
  KEY `fk_consultationChildrenDiagnosis_idx` (`idConsultation`),
  CONSTRAINT `fk_consultationChildrenDiagnosis` FOREIGN KEY (`idConsultation`) REFERENCES `consultation` (`ID`),
  CONSTRAINT `fk_diagnosis` FOREIGN KEY (`idDiagnosis`) REFERENCES `diagnoses` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `children_diagnosis`
--

LOCK TABLES `children_diagnosis` WRITE;
/*!40000 ALTER TABLE `children_diagnosis` DISABLE KEYS */;
INSERT INTO `children_diagnosis` VALUES (4,19,7,'2025-04-16'),(5,19,8,'2025-04-16'),(6,20,7,'2025-04-16'),(7,21,7,'2025-04-16'),(8,21,8,'2025-04-16'),(9,22,8,'2025-04-17'),(10,22,7,'2025-04-17');
/*!40000 ALTER TABLE `children_diagnosis` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `children_documents`
--

DROP TABLE IF EXISTS `children_documents`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `children_documents` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `idChild` int DEFAULT NULL,
  `idDocumentType` int DEFAULT NULL,
  `filePath` longtext,
  PRIMARY KEY (`ID`),
  KEY `fk_childDocuments_idx` (`idChild`),
  KEY `fk_documentsType_idx` (`idDocumentType`),
  CONSTRAINT `fk_childDocuments` FOREIGN KEY (`idChild`) REFERENCES `childrens` (`ID`),
  CONSTRAINT `fk_documentsType` FOREIGN KEY (`idDocumentType`) REFERENCES `documents_type` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `children_documents`
--

LOCK TABLES `children_documents` WRITE;
/*!40000 ALTER TABLE `children_documents` DISABLE KEYS */;
INSERT INTO `children_documents` VALUES (20,14,1,'../../Documents/Children/Обращение + согласие ДДИ - Иванов Алексей Петрович Морковка.pdf'),(21,14,4,'../../Documents/Children/202504160049_excel.png'),(22,10,1,'../../Documents/Children/AppealsConsentsОбращение + согласие ДДИ - Кошка Гретта  Теремок.pdf'),(23,10,4,'../../Documents/Children/Documents/Без имени3.png_10'),(24,10,2,'../../Documents/Children/Documents/word.png_10'),(25,10,3,'../../Documents/Children/Documents/bb3153.jpg_10'),(26,4,1,'../../Documents/Children/AppealsConsents/Обращение + согласие ДДИ - Авдеев Пёс  Теремок.pdf'),(27,4,4,'../../Documents/Children/Documents/excel.png_4'),(28,4,2,'../../Documents/Children/Documents/pdf.png_4'),(29,4,3,'../../Documents/Children/Documents/excel.png_4');
/*!40000 ALTER TABLE `children_documents` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `children_status`
--

DROP TABLE IF EXISTS `children_status`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `children_status` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `statusName` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `children_status`
--

LOCK TABLES `children_status` WRITE;
/*!40000 ALTER TABLE `children_status` DISABLE KEYS */;
INSERT INTO `children_status` VALUES (1,'Мониторинг'),(2,'Предварительно в работе'),(3,'В работе'),(4,'Работа завершена'),(5,'Требуется дополнительная медпомощь'),(6,'Проблем не выявлено');
/*!40000 ALTER TABLE `children_status` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `children_status_program`
--

DROP TABLE IF EXISTS `children_status_program`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `children_status_program` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `statusName` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `children_status_program`
--

LOCK TABLES `children_status_program` WRITE;
/*!40000 ALTER TABLE `children_status_program` DISABLE KEYS */;
INSERT INTO `children_status_program` VALUES (1,'Медицинское освидетельствование');
/*!40000 ALTER TABLE `children_status_program` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `childrens`
--

DROP TABLE IF EXISTS `childrens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `childrens` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `numOfQuestionnaire` varchar(12) DEFAULT NULL,
  `urlOfQuestionnaire` longtext,
  `surname` varchar(70) DEFAULT NULL,
  `name` varchar(70) DEFAULT NULL,
  `middleName` varchar(70) DEFAULT NULL,
  `birthday` date DEFAULT NULL,
  `idStatus` int DEFAULT NULL,
  `idStatusProgram` int DEFAULT NULL,
  `idRegion` int DEFAULT NULL,
  `idOrphanage` int DEFAULT NULL,
  `dateAdded` date DEFAULT NULL,
  `isAlert` tinyint DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_orphanageChild_idx` (`idOrphanage`),
  KEY `fk_regionChildren_idx` (`idRegion`),
  KEY `fk_statusChild_idx` (`idStatus`),
  KEY `fk_statusProgramChild_idx` (`idStatusProgram`),
  CONSTRAINT `fk_orphanageChild` FOREIGN KEY (`idOrphanage`) REFERENCES `orphanages` (`ID`),
  CONSTRAINT `fk_regionChildren` FOREIGN KEY (`idRegion`) REFERENCES `regions` (`ID`),
  CONSTRAINT `fk_statusChild` FOREIGN KEY (`idStatus`) REFERENCES `children_status` (`ID`),
  CONSTRAINT `fk_statusProgramChild` FOREIGN KEY (`idStatusProgram`) REFERENCES `children_status_program` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `childrens`
--

LOCK TABLES `childrens` WRITE;
/*!40000 ALTER TABLE `childrens` DISABLE KEYS */;
INSERT INTO `childrens` VALUES (1,'num1o-12r3','https://youla.ru/moskva/zhivotnye/koshki/koshka-67d0d89c031121b53c02bc24','Борисов','Кот','','2024-06-13',3,1,2,2,'2025-04-01',1),(2,'num2o-35е4','https://youla.ru/moskovskaya_oblast_krasnogorsk/zhivotnye/koshki/koshka-v-dobryie-ruki-67e6d2fd132c246d350db2a8','Иванова','Кошка','','2025-04-01',3,1,2,2,'2025-04-02',0),(3,'num3o-rt4b','https://youla.ru/enem/zhivotnye/pticy/sutochnyie-tsypliata-utiata-ghusiata-indiushata-5bb06b845eaa9e2ada7b6cc3','Жучкина','Птичка',NULL,'2025-04-01',1,NULL,1,NULL,'2025-04-03',0),(4,'num4o-h3d1','https://youla.ru/moskva/zhivotnye/sobaki/shikarnyie-shchienki-malchiki-i-dievochki-679fa25bbc76230244031874','Авдеев','Пёс','','2025-04-01',3,1,1,1,'2025-04-04',0),(9,'er1f-qw15','https://youla.ru/schelkovo/zhivotnye/koshki/pushok-62d2faafcbe7a46e8644335f','Кот','Пушок',NULL,'2025-04-01',1,NULL,1,NULL,'2025-04-05',0),(10,'asd1-qwe3','https://youla.ru/moskva/zhivotnye/koshki/koshiechka-67e7f38af9e967340602c434','Кошка','Гретта','','2024-06-13',3,NULL,1,1,'2025-04-05',1),(11,'h4v1-asf1','https://youla.ru/dmitrov/zhivotnye/sobaki/shchienki-vielsh-korghi-piembrok-6045dacd5fdbe12ca43f4c63','Корги','Четин','Иванович','2024-06-13',3,1,1,3,'2025-04-06',0),(14,'qwwe-123e','https://youla.ru/pushkino/zhivotnye/koshki/bielyi-kotik-67e986a29b244acd270ac4f1','Иванов','Алексей','Петрович','2025-04-07',3,1,1,3,'2025-04-07',0),(15,'eeee-wwww','https://learn.microsoft.com/ru-ru/visualstudio/ide/visual-studio-performance-tips-and-tricks?view=vs-2022','Белкин','Иван',NULL,'2025-04-07',2,NULL,6,NULL,'2025-04-07',1),(16,'qwe3-ec1s','qweqweqwe','Иванов','Андрей',NULL,'2025-04-07',6,NULL,3,NULL,'2025-04-07',0),(17,'фыв','фыв','Тест','Тест',NULL,'2025-04-07',6,NULL,4,NULL,'2025-04-07',0),(18,'123в-1у2в1','йывфйфыв','Тест1','Тест1',NULL,'2025-04-07',6,NULL,1,NULL,'2025-04-07',0),(19,'asdasd','dsasd','asd','asdsda',NULL,'2025-04-07',1,NULL,6,NULL,'2025-04-07',0);
/*!40000 ALTER TABLE `childrens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `childrens_description`
--

DROP TABLE IF EXISTS `childrens_description`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `childrens_description` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `idChild` int DEFAULT NULL,
  `description` longtext,
  `dateAdded` date DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_childDescription_idx` (`idChild`),
  CONSTRAINT `fk_childDescription` FOREIGN KEY (`idChild`) REFERENCES `childrens` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `childrens_description`
--

LOCK TABLES `childrens_description` WRITE;
/*!40000 ALTER TABLE `childrens_description` DISABLE KEYS */;
INSERT INTO `childrens_description` VALUES (1,1,'Кот по имени Борисов Кот','2025-04-02'),(2,2,'Кошка по имени Борисов Кот','2025-04-02'),(3,3,'Птичка по имени Птичка. Крилья целые, клюв тоже. Летает хорошо, поёт тоже','2025-04-02'),(4,4,'Просто Пёс, который Пёс. И кстати, у него бабочка','2025-04-02'),(5,1,'Новое примечание','2025-04-03'),(6,9,'Это кот пушок. С ним всё хорошо. Бегает, прыгает, с игрушками играет','2025-04-03'),(7,10,'Молодая, очень-очень ласковая кошка Грета. Стерилизована, здорова, лоток 100%. Возраст примерно 3 года. Спасена со стройки. Мечтает найти свой дом и любящих хозяев.','2025-04-03'),(8,11,'Щенки Вельш Корги Пемброк - большой выбор. Питомник предлагает к продаже большой выбор малышей., щенки с документами РКФ, привиты по возрасту, клеймо, вет.паспорт. У нас вы можите зарезервировать щенка и забрать в удобное для вас время .Родители щенков имеют все допуски в разведение, являются призерами и побителями всепородных и монопородных выставок. Дрессировка, передержка , корма , консультации. Фом Хаус Вольфсхкнд. Цены разные. Щенки Вельш Корги Пемброк - большой выбор. Питомник предлагает к продаже большой выбор малышей., щенки с документами РКФ, привиты по возрасту, клеймо, вет.паспорт. У нас вы можите зарезервировать щенка и забрать в удобное для вас время .Родители щенков имеют все допуски в разведение, являются призерами и побителями всепородных и монопородных выставок. Дрессировка, передержка , корма , консультации. Фом Хаус Вольфсхкнд. Цены разные.','2025-04-03'),(20,11,'Новое примечание','2025-04-05'),(21,14,'Белый котик','2025-04-06'),(22,14,'С ним всё хорошо','2025-04-06'),(23,15,'Пока наблюдаем','2025-04-07'),(24,16,'Купите пж 1050 ti. Она ряльно мощная','2025-04-07'),(25,17,'фывфывфывфыв','2025-04-07'),(26,18,'фывфвфывыфвффыв','2025-04-07'),(27,19,'asdasdasd','2025-04-07'),(28,15,'Появились первые признаки болячки','2025-04-07'),(29,15,'да','2025-04-07'),(31,1,'Новейшее примечание','2025-04-07'),(32,15,'Простите','2025-04-07'),(33,15,'Прощаю','2025-04-07'),(34,14,'Подготавливаемся к консультации','2025-04-12');
/*!40000 ALTER TABLE `childrens_description` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `consents`
--

DROP TABLE IF EXISTS `consents`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `consents` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `dateСonclusion` date DEFAULT NULL,
  `idDocument` int DEFAULT NULL,
  `idOrphanage` int DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_documentsConcent_idx` (`idDocument`),
  KEY `fk_orphanageConcent_idx` (`idOrphanage`),
  CONSTRAINT `fk_documentsConcent` FOREIGN KEY (`idDocument`) REFERENCES `children_documents` (`ID`),
  CONSTRAINT `fk_orphanageConcent` FOREIGN KEY (`idOrphanage`) REFERENCES `orphanages` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `consents`
--

LOCK TABLES `consents` WRITE;
/*!40000 ALTER TABLE `consents` DISABLE KEYS */;
INSERT INTO `consents` VALUES (9,'2025-04-15',20,3),(10,'2025-04-16',22,1),(11,'2025-04-17',26,1);
/*!40000 ALTER TABLE `consents` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `consultation`
--

DROP TABLE IF EXISTS `consultation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `consultation` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `idDoctor` int DEFAULT NULL,
  `idChild` int DEFAULT NULL,
  `filePath` longtext,
  PRIMARY KEY (`ID`),
  KEY `fk_doctorConsultation_idx` (`idDoctor`),
  KEY `fk_childrenConsultation_idx` (`idChild`),
  CONSTRAINT `fk_childrenConsultation` FOREIGN KEY (`idChild`) REFERENCES `childrens` (`ID`),
  CONSTRAINT `fk_doctorConsultation` FOREIGN KEY (`idDoctor`) REFERENCES `doctors_on_agreement` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `consultation`
--

LOCK TABLES `consultation` WRITE;
/*!40000 ALTER TABLE `consultation` DISABLE KEYS */;
INSERT INTO `consultation` VALUES (19,8,11,'../../Documents/Children/MedicalConclusion/tzvu0i1vxf83xm7k2ypwmcjvx39ck0gh.pdf_11'),(20,8,14,'../../Documents/Children/MedicalConclusion/yjfxred8m22nan6ocg44aauvmpu27s0f.pdf_14'),(21,8,10,'../../Documents/Children/MedicalConclusion/tzvu0i1vxf83xm7k2ypwmcjvx39ck0gh.pdf_10'),(22,8,4,'../../Documents/Children/MedicalConclusion/3gozhc8o1omejo2nhixi4l2x62tgqoq4.pdf_4');
/*!40000 ALTER TABLE `consultation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `curators`
--

DROP TABLE IF EXISTS `curators`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `curators` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `surname` varchar(70) DEFAULT NULL,
  `name` varchar(70) DEFAULT NULL,
  `middleName` varchar(70) DEFAULT NULL,
  `phoneNumber` varchar(12) DEFAULT NULL,
  `email` varchar(150) DEFAULT NULL,
  `idUser` int NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_volunteers_users1_idx` (`idUser`),
  CONSTRAINT `fk_volunteers_users10` FOREIGN KEY (`idUser`) REFERENCES `users` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `curators`
--

LOCK TABLES `curators` WRITE;
/*!40000 ALTER TABLE `curators` DISABLE KEYS */;
INSERT INTO `curators` VALUES (2,'Иванова','Мария','Ивановна',NULL,NULL,3);
/*!40000 ALTER TABLE `curators` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `diagnoses`
--

DROP TABLE IF EXISTS `diagnoses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `diagnoses` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `diagnosisName` varchar(150) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `diagnoses`
--

LOCK TABLES `diagnoses` WRITE;
/*!40000 ALTER TABLE `diagnoses` DISABLE KEYS */;
INSERT INTO `diagnoses` VALUES (7,'Гидроцефалия'),(8,'Cиндром Арнольда-Киари');
/*!40000 ALTER TABLE `diagnoses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `doctor_posts`
--

DROP TABLE IF EXISTS `doctor_posts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `doctor_posts` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `postName` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `doctor_posts`
--

LOCK TABLES `doctor_posts` WRITE;
/*!40000 ALTER TABLE `doctor_posts` DISABLE KEYS */;
INSERT INTO `doctor_posts` VALUES (1,'Нейрохирург'),(2,'Невролог'),(3,'Травматолог-ортопед');
/*!40000 ALTER TABLE `doctor_posts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `doctors_on_agreement`
--

DROP TABLE IF EXISTS `doctors_on_agreement`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `doctors_on_agreement` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `surname` varchar(70) DEFAULT NULL,
  `name` varchar(70) DEFAULT NULL,
  `middleName` varchar(70) DEFAULT NULL,
  `phoneNumber` varchar(12) DEFAULT NULL,
  `email` varchar(150) DEFAULT NULL,
  `idPost` int DEFAULT NULL,
  `idMedicalFacility` int DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_doctorPost_idx` (`idPost`),
  KEY `fk_medicalFacilityDoctor_idx` (`idMedicalFacility`),
  CONSTRAINT `fk_doctorPost` FOREIGN KEY (`idPost`) REFERENCES `doctor_posts` (`ID`),
  CONSTRAINT `fk_medicalFacilityDoctor` FOREIGN KEY (`idMedicalFacility`) REFERENCES `medical_facility` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `doctors_on_agreement`
--

LOCK TABLES `doctors_on_agreement` WRITE;
/*!40000 ALTER TABLE `doctors_on_agreement` DISABLE KEYS */;
INSERT INTO `doctors_on_agreement` VALUES (7,'Зенцов','Михаил','Юрьевич',NULL,NULL,1,NULL),(8,'Иванова','Анжела','Викторовна',NULL,NULL,1,NULL),(9,'Осипенко','Оксана','Викторовна',NULL,NULL,2,NULL);
/*!40000 ALTER TABLE `doctors_on_agreement` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `documents_type`
--

DROP TABLE IF EXISTS `documents_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `documents_type` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `documentType` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `documents_type`
--

LOCK TABLES `documents_type` WRITE;
/*!40000 ALTER TABLE `documents_type` DISABLE KEYS */;
INSERT INTO `documents_type` VALUES (1,'обращение на благовтворительную помощь и согласия'),(2,'свидетельство о рождении'),(3,'СНИЛС'),(4,'полис ОМС');
/*!40000 ALTER TABLE `documents_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hospitalization`
--

DROP TABLE IF EXISTS `hospitalization`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hospitalization` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `idMedicalFacility` int DEFAULT NULL,
  `idActualProgram` int DEFAULT NULL,
  `dateHospitalization` date DEFAULT NULL,
  `dateDischarge` date DEFAULT NULL,
  `totalCost` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_actualProgramHospitalization_idx` (`idActualProgram`),
  KEY `fk_medicalFacilityHosp_idx` (`idMedicalFacility`),
  CONSTRAINT `fk_actualProgramHospitalization` FOREIGN KEY (`idActualProgram`) REFERENCES `actual_program` (`ID`),
  CONSTRAINT `fk_medicalFacilityHosp` FOREIGN KEY (`idMedicalFacility`) REFERENCES `medical_facility` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hospitalization`
--

LOCK TABLES `hospitalization` WRITE;
/*!40000 ALTER TABLE `hospitalization` DISABLE KEYS */;
/*!40000 ALTER TABLE `hospitalization` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hospitalization_detail`
--

DROP TABLE IF EXISTS `hospitalization_detail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hospitalization_detail` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `idHospitalization` int DEFAULT NULL,
  `idTypeMedicalHelp` int DEFAULT NULL,
  `cost` decimal(10,2) DEFAULT NULL,
  `dateMedicalHelp` date DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_hospitalizationDetailH_idx` (`idHospitalization`),
  KEY `fk_typeMedicalCareDetailH_idx` (`idTypeMedicalHelp`),
  CONSTRAINT `fk_hospitalizationDetailH` FOREIGN KEY (`idHospitalization`) REFERENCES `hospitalization` (`ID`),
  CONSTRAINT `fk_typeMedicalCareDetailH` FOREIGN KEY (`idTypeMedicalHelp`) REFERENCES `medical_care_type` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hospitalization_detail`
--

LOCK TABLES `hospitalization_detail` WRITE;
/*!40000 ALTER TABLE `hospitalization_detail` DISABLE KEYS */;
/*!40000 ALTER TABLE `hospitalization_detail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `medical_care_type`
--

DROP TABLE IF EXISTS `medical_care_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `medical_care_type` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `medicalCareType` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `medical_care_type`
--

LOCK TABLES `medical_care_type` WRITE;
/*!40000 ALTER TABLE `medical_care_type` DISABLE KEYS */;
/*!40000 ALTER TABLE `medical_care_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `medical_facility`
--

DROP TABLE IF EXISTS `medical_facility`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `medical_facility` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `medicalFacilityName` varchar(200) DEFAULT NULL,
  `address` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `medical_facility`
--

LOCK TABLES `medical_facility` WRITE;
/*!40000 ALTER TABLE `medical_facility` DISABLE KEYS */;
/*!40000 ALTER TABLE `medical_facility` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `nannies`
--

DROP TABLE IF EXISTS `nannies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nannies` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `surname` varchar(70) DEFAULT NULL,
  `name` varchar(70) DEFAULT NULL,
  `middleName` varchar(70) DEFAULT NULL,
  `phoneNumber` varchar(12) DEFAULT NULL,
  `email` varchar(150) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nannies`
--

LOCK TABLES `nannies` WRITE;
/*!40000 ALTER TABLE `nannies` DISABLE KEYS */;
/*!40000 ALTER TABLE `nannies` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `nannies_in_hospitalization`
--

DROP TABLE IF EXISTS `nannies_in_hospitalization`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nannies_in_hospitalization` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `idNanny` int DEFAULT NULL,
  `idHospitalization` int DEFAULT NULL,
  `idStatus` int DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_hospitalizationNannyInHosp_idx` (`idHospitalization`),
  KEY `fk_nannyStatusN_idx` (`idStatus`),
  KEY `fk_nanniesH_idx` (`idNanny`),
  CONSTRAINT `fk_hospitalizationNannyInHosp` FOREIGN KEY (`idHospitalization`) REFERENCES `hospitalization` (`ID`),
  CONSTRAINT `fk_nanniesH` FOREIGN KEY (`idNanny`) REFERENCES `nannies` (`ID`),
  CONSTRAINT `fk_nannyStatusN` FOREIGN KEY (`idStatus`) REFERENCES `nanny_statuses` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nannies_in_hospitalization`
--

LOCK TABLES `nannies_in_hospitalization` WRITE;
/*!40000 ALTER TABLE `nannies_in_hospitalization` DISABLE KEYS */;
/*!40000 ALTER TABLE `nannies_in_hospitalization` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `nanny_statuses`
--

DROP TABLE IF EXISTS `nanny_statuses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nanny_statuses` (
  `ID` int NOT NULL,
  `statusName` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nanny_statuses`
--

LOCK TABLES `nanny_statuses` WRITE;
/*!40000 ALTER TABLE `nanny_statuses` DISABLE KEYS */;
/*!40000 ALTER TABLE `nanny_statuses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orphanages`
--

DROP TABLE IF EXISTS `orphanages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orphanages` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `nameOrphanage` varchar(200) DEFAULT NULL,
  `directorSurname` varchar(70) DEFAULT NULL,
  `directorName` varchar(70) DEFAULT NULL,
  `directorMiddleName` varchar(70) DEFAULT NULL,
  `idRegion` int DEFAULT NULL,
  `address` varchar(200) DEFAULT NULL,
  `email` varchar(150) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_regionOrphanage_idx` (`idRegion`),
  CONSTRAINT `fk_regionOrphanage` FOREIGN KEY (`idRegion`) REFERENCES `regions` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orphanages`
--

LOCK TABLES `orphanages` WRITE;
/*!40000 ALTER TABLE `orphanages` DISABLE KEYS */;
INSERT INTO `orphanages` VALUES (1,'Теремок','Петров','Алексей','Викторович',1,'г. Мурманск','0'),(2,'Избушка','Алексеев','Михаил','Юрьевич',2,'г. Москва','1'),(3,'Морковка','Анушкин','Фёдор','Николаевич',1,'г. Мурманск','1');
/*!40000 ALTER TABLE `orphanages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `program_type`
--

DROP TABLE IF EXISTS `program_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `program_type` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `programType` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `program_type`
--

LOCK TABLES `program_type` WRITE;
/*!40000 ALTER TABLE `program_type` DISABLE KEYS */;
/*!40000 ALTER TABLE `program_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `regions`
--

DROP TABLE IF EXISTS `regions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `regions` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `regionName` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `regions`
--

LOCK TABLES `regions` WRITE;
/*!40000 ALTER TABLE `regions` DISABLE KEYS */;
INSERT INTO `regions` VALUES (1,'Мурманская область'),(2,'Московская область'),(3,'Пермская область'),(4,'Новосибирская область'),(5,'Костромская область'),(6,'Вологодская область');
/*!40000 ALTER TABLE `regions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `results_consultation`
--

DROP TABLE IF EXISTS `results_consultation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `results_consultation` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `idConsiltation` int DEFAULT NULL,
  `filePath` longtext,
  PRIMARY KEY (`ID`),
  KEY `fk_consultationResults_idx` (`idConsiltation`),
  CONSTRAINT `fk_consultationResults` FOREIGN KEY (`idConsiltation`) REFERENCES `consultation` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `results_consultation`
--

LOCK TABLES `results_consultation` WRITE;
/*!40000 ALTER TABLE `results_consultation` DISABLE KEYS */;
INSERT INTO `results_consultation` VALUES (16,19,'../../Documents/Children/MedicalResults/word.png_11'),(17,20,'../../Documents/Children/MedicalResults/box.png_14'),(18,21,'../../Documents/Children/MedicalResults/word.png_10'),(19,22,'../../Documents/Children/MedicalResults/photo_2020-03-16_15-53-49.jpg_4'),(20,22,'../../Documents/Children/MedicalResults/1590242241185097951.jpg_4');
/*!40000 ALTER TABLE `results_consultation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `roles` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `roleName` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` VALUES (1,'Волонтёр'),(2,'Куратор'),(3,'Директор');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `transfer`
--

DROP TABLE IF EXISTS `transfer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transfer` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `idActualProgram` int DEFAULT NULL,
  `dateDeparture` datetime DEFAULT NULL,
  `dateArrival` datetime DEFAULT NULL,
  `totalCost` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_actualProgramTransfer_idx` (`idActualProgram`),
  CONSTRAINT `fk_actualProgramTransfer` FOREIGN KEY (`idActualProgram`) REFERENCES `actual_program` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `transfer`
--

LOCK TABLES `transfer` WRITE;
/*!40000 ALTER TABLE `transfer` DISABLE KEYS */;
/*!40000 ALTER TABLE `transfer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `transfer_detail`
--

DROP TABLE IF EXISTS `transfer_detail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transfer_detail` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `idTransfer` int DEFAULT NULL,
  `idTransportType` int DEFAULT NULL,
  `cost` decimal(10,2) DEFAULT NULL,
  `filePath` longtext,
  PRIMARY KEY (`ID`),
  KEY `fk_transferDetailT_idx` (`idTransfer`),
  KEY `fk_transportTypeTransfer_idx` (`idTransportType`),
  CONSTRAINT `fk_transferDetailT` FOREIGN KEY (`idTransfer`) REFERENCES `transfer` (`ID`),
  CONSTRAINT `fk_transportTypeTransfer` FOREIGN KEY (`idTransportType`) REFERENCES `transport_type` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `transfer_detail`
--

LOCK TABLES `transfer_detail` WRITE;
/*!40000 ALTER TABLE `transfer_detail` DISABLE KEYS */;
/*!40000 ALTER TABLE `transfer_detail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `transport_type`
--

DROP TABLE IF EXISTS `transport_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transport_type` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `transportType` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `transport_type`
--

LOCK TABLES `transport_type` WRITE;
/*!40000 ALTER TABLE `transport_type` DISABLE KEYS */;
/*!40000 ALTER TABLE `transport_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `login` varchar(10) DEFAULT NULL,
  `password` varchar(8) DEFAULT NULL,
  `idRole` int DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_roleUsers_idx` (`idRole`),
  CONSTRAINT `fk_roleUsers` FOREIGN KEY (`idRole`) REFERENCES `roles` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'0','0',1),(3,'cur','0',2),(4,'dir','0',3);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `volunteers`
--

DROP TABLE IF EXISTS `volunteers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `volunteers` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `surname` varchar(70) DEFAULT NULL,
  `name` varchar(70) DEFAULT NULL,
  `middleName` varchar(70) DEFAULT NULL,
  `phoneNumber` varchar(12) DEFAULT NULL,
  `email` varchar(150) DEFAULT NULL,
  `idRegion` int DEFAULT NULL,
  `idUser` int NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `idUser_UNIQUE` (`idUser`),
  KEY `fk_regionVolonteer_idx` (`idRegion`),
  KEY `fk_volunteers_users1_idx` (`idUser`),
  CONSTRAINT `fk_regionVolonteer` FOREIGN KEY (`idRegion`) REFERENCES `regions` (`ID`),
  CONSTRAINT `fk_volunteers_users1` FOREIGN KEY (`idUser`) REFERENCES `users` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `volunteers`
--

LOCK TABLES `volunteers` WRITE;
/*!40000 ALTER TABLE `volunteers` DISABLE KEYS */;
INSERT INTO `volunteers` VALUES (1,'Иванов','Иван','Иванович','+79009009090','ivanov@ya.ru',1,1);
/*!40000 ALTER TABLE `volunteers` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-17  2:13:02
