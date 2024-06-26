# API de un carrito de compras hecho en .NET

## Introducción

Este proyecto es parte de una práctica personal para asimilar los conceptos relacionados al lenguaje de C#.

## Temas practicados

1. Uso de controladores
   - Manejo de las solicitudes HTTP.
     
3. Uso de servicios
   - Manejo de la logica de negocio y capa intermedia entre la capa de persistencia y la capa de controladores.
     
5. Uso de repositorios
   - Capa que se comunica con el acceso a los datos y con EF.

6. Injeccion de dependecias
7. Uso de interfaces y su posterior implementacion. Dentro de ellas, uso de genericos para reutilizar la misma interfaz e implementacion
8. Creacion de modelos
9. Creacion de DTO y DTI para el manejo en las solicitudes y respuestas HTTP.
10. Uso de Entity Framework para la persistencia de datos, acompañado por una base de datos SQL Server Local. Utilizacion de relaciones entre entidades.
11. AutoMapper para el mapeo de modelos en el control de las respuestas y solicitudes HTTP.
12. Sistema de autenticacion y autorizacion de usuarios
    - La autenticacion se realiza a traves de un sistema de tokens JWT. El sistema consiste en asignar un token de acceso para el manejo de solicitudes posteriores a corto plazo, y un token de refresco para evitar que el usuario deba loguearse en un mediano plazo reiteradas veces.
    - La autorizacion y acceso a recursos se hace a traves de un sistema de roles manejado por Identity de Entity Framework.
