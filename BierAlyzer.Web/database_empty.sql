-- phpMyAdmin SQL Dump
-- version 4.6.6
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1:3306
-- Erstellungszeit: 13. Mai 2018 um 09:18
-- Server-Version: 5.6.35
-- PHP-Version: 7.1.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";

--
-- Datenbank: `ef`
--
CREATE DATABASE IF NOT EXISTS `ef` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `ef`;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `drink`
--

CREATE TABLE `drink` (
  `DrinkId` char(36) NOT NULL,
  `Amount` double NOT NULL,
  `CreatedString` longtext,
  `ModifiedString` longtext,
  `Name` longtext,
  `Percentage` double NOT NULL,
  `Visible` bit(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `drinkentry`
--

CREATE TABLE `drinkentry` (
  `EntryId` char(36) NOT NULL,
  `DrinkId` char(36) NOT NULL,
  `EventId` char(36) NOT NULL,
  `UserId` char(36) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `event`
--

CREATE TABLE `event` (
  `EventId` char(36) NOT NULL,
  `Code` longtext,
  `CreatedString` longtext,
  `Description` longtext,
  `EndString` longtext,
  `ModifiedString` longtext,
  `Name` longtext,
  `OwnerId` char(36) NOT NULL,
  `StartString` longtext,
  `Type` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `user`
--

CREATE TABLE `user` (
  `UserId` char(36) NOT NULL,
  `CreatedString` longtext,
  `Enabled` bit(1) NOT NULL,
  `Hash` longtext,
  `LastLoginString` longtext,
  `Mail` longtext,
  `ModifiedString` longtext,
  `Origin` longtext,
  `Salt` longtext,
  `Type` int(11) NOT NULL,
  `Username` longtext
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `user`
--

INSERT INTO `user` (`UserId`, `CreatedString`, `Enabled`, `Hash`, `LastLoginString`, `Mail`, `ModifiedString`, `Origin`, `Salt`, `Type`, `Username`) VALUES
('08d5b861-bafd-7e56-b1a7-cdf17bd24680', '05/13/2018 01:40:23', b'1111111111111111111111111111111', 'B1B57C0699ED6120AA594127C84DB895', '05/13/2018 10:17:16', 'bier@troogs.de', '05/13/2018 01:40:23', 'Uni Siegen', '71BFDCDED04E94A8939E20A0DB8B174D', 100, 'Admin');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `userevent`
--

CREATE TABLE `userevent` (
  `UserId` char(36) NOT NULL,
  `EventId` char(36) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20180512225014_Initial', '2.0.2-rtm-10011');

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `drink`
--
ALTER TABLE `drink`
  ADD PRIMARY KEY (`DrinkId`);

--
-- Indizes für die Tabelle `drinkentry`
--
ALTER TABLE `drinkentry`
  ADD PRIMARY KEY (`EntryId`),
  ADD KEY `IX_DrinkEntry_DrinkId` (`DrinkId`),
  ADD KEY `IX_DrinkEntry_EventId` (`EventId`),
  ADD KEY `IX_DrinkEntry_UserId` (`UserId`);

--
-- Indizes für die Tabelle `event`
--
ALTER TABLE `event`
  ADD PRIMARY KEY (`EventId`),
  ADD KEY `IX_Event_OwnerId` (`OwnerId`);

--
-- Indizes für die Tabelle `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`UserId`);

--
-- Indizes für die Tabelle `userevent`
--
ALTER TABLE `userevent`
  ADD PRIMARY KEY (`UserId`,`EventId`),
  ADD UNIQUE KEY `AK_UserEvent_EventId_UserId` (`EventId`,`UserId`);

--
-- Indizes für die Tabelle `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `drinkentry`
--
ALTER TABLE `drinkentry`
  ADD CONSTRAINT `FK_DrinkEntry_Drink_DrinkId` FOREIGN KEY (`DrinkId`) REFERENCES `drink` (`DrinkId`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_DrinkEntry_Event_EventId` FOREIGN KEY (`EventId`) REFERENCES `event` (`EventId`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_DrinkEntry_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `user` (`UserId`) ON DELETE CASCADE;

--
-- Constraints der Tabelle `event`
--
ALTER TABLE `event`
  ADD CONSTRAINT `FK_Event_User_OwnerId` FOREIGN KEY (`OwnerId`) REFERENCES `user` (`UserId`) ON DELETE CASCADE;

--
-- Constraints der Tabelle `userevent`
--
ALTER TABLE `userevent`
  ADD CONSTRAINT `FK_UserEvent_Event_EventId` FOREIGN KEY (`EventId`) REFERENCES `event` (`EventId`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_UserEvent_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `user` (`UserId`) ON DELETE CASCADE;
