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
7. Aplicacion de herencias
8. Uso de interfaces y su posterior implementacion. Dentro de ellas, uso de genericos para reutilizar la misma interfaz e implementacion
9. Creacion de modelos
    - Data Annotations para la validacion de modelos.
11. Creacion de DTO y DTI para el manejo en las solicitudes y respuestas HTTP.
12. Uso de Entity Framework para la persistencia de datos, acompañado por una base de datos SQL Server Local. Utilizacion de relaciones entre entidades.
13. AutoMapper para el mapeo de modelos en el control de las respuestas y solicitudes HTTP.
14. Sistema de autenticacion y autorizacion de usuarios
    - La autenticacion se realiza a traves de un sistema de tokens JWT. El sistema consiste en asignar un token de acceso para el manejo de solicitudes posteriores a corto plazo, y un token de refresco para evitar que el usuario deba loguearse en un mediano plazo reiteradas veces.
    - La autorizacion y acceso a recursos se hace a traves de un sistema de roles manejado por Identity de Entity Framework. Ciertos recursos de alto riesgo pueden ser manejados solo por usuarios con roles de alto rango.

15. Configuracion de Docker para crear imagen del proyecto y su posterior implementacion en un contenedor.


## Distribucion 

El proyecto cuenta con 4 capas:

1. Capa.Datos: en esta capa encontramos las entidades y modelos de utilidad para el correcto funcionamiento de la aplicacion. (Class Library)
2. Capa.Aplicacion: en esta capa encontramos las configuracion de utilidades (como el Mapper), las interfaces de servicios y repositorios, y los DTO / DTI. (Class Library)
3. Capa.Infraestructura: en esta capa encontramos la implementacion de las interfaces, un archivo de registro que es llamado por el archivo principal del programa Program.cs, y la configuracion del dbContext para Entity Framework. (Class Library)
4. CarritoDeCompras: en esta capa encontramos la capa de presentacion donde estan nuestros controladores, y donde controlamos las peticiones a la API, ademas del DockerFile y el Program.cs principal (Proyecto web API).

### Muchas gracias!
