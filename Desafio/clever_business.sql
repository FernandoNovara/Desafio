-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 10-05-2023 a las 15:48:46
-- Versión del servidor: 10.4.28-MariaDB
-- Versión de PHP: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `clever_business`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `business`
--

CREATE TABLE `business` (
  `IdBusiness` int(11) NOT NULL,
  `location` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `business`
--

INSERT INTO `business` (`IdBusiness`, `location`) VALUES
(1, 'Brasil'),
(2, 'España'),
(3, 'Argentina');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `employee`
--

CREATE TABLE `employee` (
  `idEmployee` int(11) NOT NULL,
  `Nombre` varchar(20) NOT NULL,
  `Apellido` varchar(20) NOT NULL,
  `Genero` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `employee`
--

INSERT INTO `employee` (`idEmployee`, `Nombre`, `Apellido`, `Genero`) VALUES
(1, 'Fernando', 'Novara', 'Masculino'),
(2, 'Armando', 'Paredes', 'Masculino'),
(3, 'Julieta', 'Benitez', 'Femenino'),
(4, 'Jimena', 'Gimenez', 'Femenino'),
(5, 'Stefani', 'Rodrigues', 'Femenino'),
(6, 'Estaban', 'Cerca', 'Masculino'),
(7, 'Iker', 'Casillas', 'Masculino'),
(8, 'Armando', 'Ventas', 'Masculino');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `register`
--

CREATE TABLE `register` (
  `IdRegister` int(11) NOT NULL,
  `idEmployee` int(11) NOT NULL,
  `IdBusiness` int(11) NOT NULL,
  `Date` datetime NOT NULL,
  `RegisterType` varchar(15) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `register`
--

INSERT INTO `register` (`IdRegister`, `idEmployee`, `IdBusiness`, `Date`, `RegisterType`) VALUES
(1, 2, 3, '2023-05-10 11:30:11', 'Ingreso'),
(2, 1, 3, '2023-05-10 06:35:34', 'Ingreso'),
(3, 3, 3, '2023-05-10 06:37:58', 'Ingreso'),
(4, 4, 1, '2023-05-10 06:38:54', 'Ingreso'),
(5, 5, 1, '2023-05-10 06:38:59', 'Ingreso'),
(6, 6, 1, '2023-05-10 06:39:02', 'Ingreso'),
(7, 7, 2, '2023-05-10 06:39:08', 'Ingreso'),
(8, 7, 2, '2023-05-10 06:39:21', 'Egreso'),
(9, 8, 2, '2023-05-10 06:39:30', 'Ingreso'),
(10, 8, 2, '2023-05-10 06:39:36', 'Egreso');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `business`
--
ALTER TABLE `business`
  ADD PRIMARY KEY (`IdBusiness`);

--
-- Indices de la tabla `employee`
--
ALTER TABLE `employee`
  ADD PRIMARY KEY (`idEmployee`);

--
-- Indices de la tabla `register`
--
ALTER TABLE `register`
  ADD PRIMARY KEY (`IdRegister`),
  ADD KEY `idEmployee` (`idEmployee`,`IdBusiness`),
  ADD KEY `IdBusiness` (`IdBusiness`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `business`
--
ALTER TABLE `business`
  MODIFY `IdBusiness` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `employee`
--
ALTER TABLE `employee`
  MODIFY `idEmployee` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `register`
--
ALTER TABLE `register`
  MODIFY `IdRegister` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `register`
--
ALTER TABLE `register`
  ADD CONSTRAINT `register_ibfk_1` FOREIGN KEY (`IdBusiness`) REFERENCES `business` (`IdBusiness`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `register_ibfk_2` FOREIGN KEY (`idEmployee`) REFERENCES `employee` (`idEmployee`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
