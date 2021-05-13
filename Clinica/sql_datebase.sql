/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
/**
 * Author:  eriss
 * Created: 17 abr. 2021
 */

Create table Medicamentos(
    id_medicamento INT IDENTITY(1,1) primary key not null,
    nombre varchar(50) not null,
    dosis varchar(10) not null,
    precio float not null,
    indicaciones varchar(100) not null
)

/******/

Create Table Usuarios(
    id_usuario INT IDENTITY(1,1) primary key not null,
    nombre varchar(30) not null,
    ape_pat varchar(30) not null,
    ape_mat varchar(30) not null,
    usuario varchar(80) not null,
    password varchar(64) not null
)

Create Table Pacientes(
    id_paciente INT IDENTITY(1,1) primary key not null,
    nombre varchar(30) not null,
    ape_pat varchar(30) not null,
    ape_mat varchar(30) not null,
    usuario varchar(80) ,
    password varchar(64)
)

Create Table Citas(
    id_cita  INT IDENTITY(1,1) primary key not null,
    id_paciente integer not null,
    id_doctor integer not null,
    fecha date not null,
    hora time not null,
    status int not null,
    observacion varchar(150) not null
)

Create Table Tickets(
    id_ticket INT IDENTITY(1,1) primary key not null,
    id_cita integer not null,
    documento TEXT not null,
    ruta varchar(100) not null,
    fecha date not null,
    total float not null
)

Create Table Recetas(
    id_receta INT IDENTITY(1,1) primary key not null,
    id_cita integer not null,
    documento TEXT not null,
    ruta varchar(100) not null,
    ids_medicamentos varchar(50) not null,
    fecha date not null,
    observacion varchar(100) not null,
    instruccion varchar(100) not null
)

/***********************************************************************/
INSERT INTO Usuarios(nombre,ape_pat,ape_mat,usuario,password) VALUES('Johnny', 'Sins', 'Echeverría','erisso@gmail.com','DVOZUIQnznlVbNpxkYAgwejRW1M=');


INSERT INTO Pacientes(nombre,ape_pat,ape_mat,usuario,password) VALUES('Victor', 'Vasquez', 'Poblete','victoe680@gmail.com','DVOZUIQnznlVbNpxkYAgwejRW1M=');



/* Install-Package Microsoft.AspNet.WebApi.Cors */

EXEC sp_rename 'Doctores','Usuarios'

ALTER TABLE Usuarios add tipo int NOT NULL DEFAULT(0);

EXEC sp_rename 'dbo.Usuarios.id_doctor','id_usuario','COLUMN';