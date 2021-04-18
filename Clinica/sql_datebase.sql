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
    id_medicamento serial primary key not null,
    nombre varchar(50) not null,
    dosis varchar(10) not null,
    precio double not null,
    indicaciones varchar(100) not null
)

Create Table Doctores(
    id_doctor serial primary key not null,
    nombre varchar(30) not null,
    ape_pat varchar(30) not null,
    ape_mat varchar(30) not null,
    usuario varchar(25) not null,
    password varchar(20) not null
)

Create Table Pacientes(
    id_paciente serial primary key not null,
    nombre varchar(30) not null,
    ape_pat varchar(30) not null,
    ape_mat varchar(30) not null,
    usuario varchar(25) not null,
    password varchar(20) not null
)

Create Table Citas(
    id_cita serial primary key not null,
    id_paciente integer not null,
    id_doctor integer not null,
    fecha date not null,
    hora time not null,
    status int not null,
    observacion varchar(100) not null
)


Create Table Recetas(
    id_receta serial primary key not null,
    id_cita integer not null,
    nombre varchar not null,
    ids_medicamentos varchar(50) not null,
    fecha date not null,
    observacion varchar(100) not null,
    instruccion varchar(100) not null
)