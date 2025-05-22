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
  `idNannyOnProgram` int DEFAULT NULL,
  `countWorkDays` int DEFAULT NULL,
  `payment` decimal(10,2) DEFAULT NULL,
  `filePath` longtext,
  PRIMARY KEY (`ID`),
  KEY `fk_nannyInHospAct_idx` (`idNannyOnProgram`),
  CONSTRAINT `fk_nannyOnProgrAct` FOREIGN KEY (`idNannyOnProgram`) REFERENCES `nannies_on_program` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `actual_children_diagnosis`
--

DROP TABLE IF EXISTS `actual_children_diagnosis`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `actual_children_diagnosis` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `idDiagnosis` int DEFAULT NULL,
  `idChild` int DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_diagnosis_idx` (`idDiagnosis`),
  KEY `fk_childrenActualDiagnosis_idx` (`idChild`),
  CONSTRAINT `fk_childrenActualDiagnosis` FOREIGN KEY (`idChild`) REFERENCES `childrens` (`ID`),
  CONSTRAINT `fk_diagnosis0` FOREIGN KEY (`idDiagnosis`) REFERENCES `diagnoses` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `actual_program`
--

DROP TABLE IF EXISTS `actual_program`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `actual_program` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `idChild` int DEFAULT NULL,
  `idCurator` int DEFAULT NULL,
  `idProgramType` int DEFAULT NULL,
  `dateBegin` date DEFAULT NULL,
  `dateEnd` date DEFAULT NULL,
  `filePath` longtext,
  `status` tinyint DEFAULT NULL,
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
-- Table structure for table `agreement_nanny_on_program`
--

DROP TABLE IF EXISTS `agreement_nanny_on_program`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `agreement_nanny_on_program` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `numOfAgreement` bigint DEFAULT NULL,
  `dateConclusion` date DEFAULT NULL,
  `idNannyOnProgram` int DEFAULT NULL,
  `costPerDay` decimal(10,2) DEFAULT NULL,
  `filePath` longtext,
  PRIMARY KEY (`ID`),
  KEY `fk_nannyInHospAgreement_idx` (`idNannyOnProgram`),
  CONSTRAINT `fk_nannyOnProgramAgreement` FOREIGN KEY (`idNannyOnProgram`) REFERENCES `nannies_on_program` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `agreement_orphanage`
--

DROP TABLE IF EXISTS `agreement_orphanage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `agreement_orphanage` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `numAgreement` int(6) unsigned zerofill DEFAULT NULL,
  `dateConclusion` date DEFAULT NULL,
  `idOrphanage` int DEFAULT NULL,
  `filePath` longtext,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `numAgreement_UNIQUE` (`numAgreement`),
  KEY `fk_orphanageAgreement_idx` (`idOrphanage`),
  CONSTRAINT `fk_orphanageAgreement` FOREIGN KEY (`idOrphanage`) REFERENCES `orphanages` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
  PRIMARY KEY (`ID`),
  KEY `fk_diagnosis_idx` (`idDiagnosis`),
  KEY `fk_consultationChildrenDiagnosis_idx` (`idConsultation`),
  CONSTRAINT `fk_consultationChildrenDiagnosis` FOREIGN KEY (`idConsultation`) REFERENCES `consultation` (`ID`),
  CONSTRAINT `fk_diagnosis` FOREIGN KEY (`idDiagnosis`) REFERENCES `diagnoses` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `children_status`
--

LOCK TABLES `children_status` WRITE;
/*!40000 ALTER TABLE `children_status` DISABLE KEYS */;
INSERT INTO `children_status` VALUES (1,'Мониторинг'),(2,'Предварительно в работе'),(3,'Медосвидетельствование'),(4,'Программа \"Чтобы успеть вовремя\"'),(5,'Программа \"Маршрут здоровья сироты\"'),(6,'Требуется дополнительная медпомощь'),(11,'Работа завершена');
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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `children_status_program`
--

LOCK TABLES `children_status_program` WRITE;
/*!40000 ALTER TABLE `children_status_program` DISABLE KEYS */;
INSERT INTO `children_status_program` VALUES (1,'Требуются документы'),(2,'Требуется няня'),(3,'Ожидает госпитализации'),(4,'Госпитализация'),(5,'Программа пройдена');
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
  `idOrphanage` int DEFAULT NULL,
  `idRegion` int DEFAULT NULL,
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `consents`
--

DROP TABLE IF EXISTS `consents`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `consents` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `numAppeal` int(6) unsigned zerofill DEFAULT NULL,
  `dateСonclusion` date DEFAULT NULL,
  `idOrphanage` int DEFAULT NULL,
  `idDocument` int DEFAULT NULL,
  `idActualProgram` int DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `numApeal_UNIQUE` (`numAppeal`),
  KEY `fk_documentsConcent_idx` (`idDocument`),
  KEY `fk_orphanageConcent_idx` (`idOrphanage`),
  KEY `fk_actrualProgramConsent_idx` (`idActualProgram`),
  CONSTRAINT `fk_actrualProgramConsent` FOREIGN KEY (`idActualProgram`) REFERENCES `actual_program` (`ID`),
  CONSTRAINT `fk_documentsConcent` FOREIGN KEY (`idDocument`) REFERENCES `children_documents` (`ID`),
  CONSTRAINT `fk_orphanageConcent` FOREIGN KEY (`idOrphanage`) REFERENCES `orphanages` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
  `dateConsultation` date DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_doctorConsultation_idx` (`idDoctor`),
  KEY `fk_childrenConsultation_idx` (`idChild`),
  CONSTRAINT `fk_childrenConsultation` FOREIGN KEY (`idChild`) REFERENCES `childrens` (`ID`),
  CONSTRAINT `fk_doctorConsultation` FOREIGN KEY (`idDoctor`) REFERENCES `doctors_on_agreement` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `curators`
--

LOCK TABLES `curators` WRITE;
/*!40000 ALTER TABLE `curators` DISABLE KEYS */;
INSERT INTO `curators` VALUES (2,'Иванова','Мария','Ивановна','71232313211','w@w.w',3),(9,'2','2','2','2','2',4),(10,'adasd','asddsadas','asddasadsads','71231231231','qwe@da.d',17);
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `directors`
--

DROP TABLE IF EXISTS `directors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `directors` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `surname` varchar(70) DEFAULT NULL,
  `name` varchar(70) DEFAULT NULL,
  `middleName` varchar(70) DEFAULT NULL,
  `phoneNumber` varchar(12) DEFAULT NULL,
  `email` varchar(150) DEFAULT NULL,
  `idUser` int NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `phoneNumber_UNIQUE` (`phoneNumber`),
  UNIQUE KEY `email_UNIQUE` (`email`),
  KEY `fk_volunteers_users1_idx` (`idUser`),
  CONSTRAINT `fk_volunteers_users100` FOREIGN KEY (`idUser`) REFERENCES `users` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `directors`
--

LOCK TABLES `directors` WRITE;
/*!40000 ALTER TABLE `directors` DISABLE KEYS */;
INSERT INTO `directors` VALUES (1,'Петрова','Татьяна','Васильевна','71231212311','email@ya.com',4);
/*!40000 ALTER TABLE `directors` ENABLE KEYS */;
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
  UNIQUE KEY `phoneNumber_UNIQUE` (`phoneNumber`),
  UNIQUE KEY `email_UNIQUE` (`email`),
  KEY `fk_doctorPost_idx` (`idPost`),
  KEY `fk_medicalFacilityDoctor_idx` (`idMedicalFacility`),
  CONSTRAINT `fk_doctorPost` FOREIGN KEY (`idPost`) REFERENCES `doctor_posts` (`ID`),
  CONSTRAINT `fk_medicalFacilityDoctor` FOREIGN KEY (`idMedicalFacility`) REFERENCES `medical_facility` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
  `filePath` longtext,
  PRIMARY KEY (`ID`),
  KEY `fk_actualProgramHospitalization_idx` (`idActualProgram`),
  KEY `fk_medicalFacilityHosp_idx` (`idMedicalFacility`),
  CONSTRAINT `fk_actualProgramHospitalization` FOREIGN KEY (`idActualProgram`) REFERENCES `actual_program` (`ID`),
  CONSTRAINT `fk_medicalFacilityHosp` FOREIGN KEY (`idMedicalFacility`) REFERENCES `medical_facility` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
INSERT INTO `medical_facility` VALUES (1,'Тест1','ул Пушкина, дом Колотушкина'),(2,'Тест2','ул Набережная, дом Подводный');
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
  `passSeries` int DEFAULT NULL,
  `passNum` int DEFAULT NULL,
  `passDateOfIssue` date DEFAULT NULL,
  `passOrganizationOfIssue` varchar(200) DEFAULT NULL,
  `passCode` varchar(7) DEFAULT NULL,
  `addressRegister` varchar(200) DEFAULT NULL,
  `phoneNumber` varchar(12) DEFAULT NULL,
  `email` varchar(150) DEFAULT NULL,
  `status` tinyint DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `nannies_on_program`
--

DROP TABLE IF EXISTS `nannies_on_program`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nannies_on_program` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `idNanny` int DEFAULT NULL,
  `idActualProgram` int DEFAULT NULL,
  `status` tinyint DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_nanniesH_idx` (`idNanny`),
  KEY `fk_ActualProgramNannyInHosp_idx` (`idActualProgram`),
  CONSTRAINT `fk_actualProgramNannyOnProgram` FOREIGN KEY (`idActualProgram`) REFERENCES `actual_program` (`ID`),
  CONSTRAINT `fk_nanniesH` FOREIGN KEY (`idNanny`) REFERENCES `nannies` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `program_type`
--

LOCK TABLES `program_type` WRITE;
/*!40000 ALTER TABLE `program_type` DISABLE KEYS */;
INSERT INTO `program_type` VALUES (1,'Чтобы успеть вовремя'),(2,'Маршрут здоровья сироты');
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


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
  `idHospitalization` int DEFAULT NULL,
  `dateDeparture` datetime DEFAULT NULL,
  `dateArrival` datetime DEFAULT NULL,
  `totalCost` decimal(10,2) DEFAULT NULL,
  `transferSide` tinyint DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_hospitalizationTransfer_idx` (`idHospitalization`),
  CONSTRAINT `fk_hospitalizationTransfer` FOREIGN KEY (`idHospitalization`) REFERENCES `hospitalization` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `login` varchar(10) DEFAULT NULL,
  `password` varchar(32) DEFAULT NULL,
  `idRole` int DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `login_UNIQUE` (`login`),
  KEY `fk_roleUsers_idx` (`idRole`),
  CONSTRAINT `fk_roleUsers` FOREIGN KEY (`idRole`) REFERENCES `roles` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (3,'cur','cfcd208495d565ef66e7dff9f98764da',2),(4,'dir','cfcd208495d565ef66e7dff9f98764da',3),(13,'vol1','7a371de86fe43ecbb1b8c27b0bbd59f7',1),(14,'vol2','7a371de86fe43ecbb1b8c27b0bbd59f7',1),(17,'qwe','7a371de86fe43ecbb1b8c27b0bbd59f7',2);
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `volunteers`
--

LOCK TABLES `volunteers` WRITE;
/*!40000 ALTER TABLE `volunteers` DISABLE KEYS */;
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

-- Dump completed on 2025-05-22  5:21:31
