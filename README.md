# GreenForest API

## Descripción del proyecto

GreenForest API es una aplicación backend desarrollada en C# utilizando ASP.NET Core 8.

El proyecto tiene como objetivo gestionar información relacionada con procesos de reforestación, permitiendo administrar usuarios, proyectos, árboles, especies, organizaciones y reportes.

La API implementa operaciones CRUD para las diferentes entidades, utiliza Entity Framework Core para la conexión con la base de datos SQLite y cuenta con una integración con Google Gemini para realizar consultas mediante inteligencia artificial.

---

## Objetivo general

Desarrollar una API REST que permita gestionar proyectos de reforestación mediante una estructura organizada utilizando programación orientada a objetos, Entity Framework Core y servicios en ASP.NET Core.

---

## Objetivos específicos

- Crear una API REST utilizando ASP.NET Core 8.
- Implementar operaciones CRUD para las entidades principales.
- Utilizar Entity Framework Core para manejar la base de datos.
- Trabajar con una base de datos SQLite.
- Aplicar DTOs para manejar la transferencia de información.
- Implementar relaciones entre entidades.
- Crear servicios para manejar lógica de negocio.
- Integrar un servicio de inteligencia artificial utilizando Google Gemini.
- Documentar y probar los endpoints mediante Swagger.

---

# Tecnologías utilizadas

- C#
- ASP.NET Core 8
- Entity Framework Core
- SQLite
- Swagger / OpenAPI
- Google Gemini API
- Git y GitHub

---

# Estructura del proyecto


GreenForest-API
│
├── AI
│ ├── GeminiRequest.cs
│ ├── GeminiResponse.cs
│ └── GeminiService.cs
│
├── Controllers
│ ├── ArbolController.cs
│ ├── EspecieController.cs
│ ├── IAController.cs
│ ├── OrganizacionController.cs
│ ├── ProyectoController.cs
│ ├── ReporteController.cs
│ └── UsuarioController.cs
│
├── DTO
│ ├── ArbolDTO.cs
│ ├── EspecieDTO.cs
│ ├── OrganizacionDTO.cs
│ ├── ProyectoDTO.cs
│ ├── ReporteDTO.cs
│ └── UsuarioDTO.cs
│
├── Data
│ ├── GreenForestContext.cs
│ └── DbSeeder.cs
│
├── Interfaces
│
├── Models
│
├── Services
│
├── Program.cs
├── appsettings.json
└── GreenForest.csproj


---

# Descripción de las carpetas

## Models

Contiene las clases principales del sistema que representan las entidades de la base de datos.

Las entidades principales son:

- Usuario
- Proyecto
- Árbol
- Especie
- Organización
- Reporte

---

## Controllers

Contiene los controladores encargados de recibir las solicitudes HTTP y devolver las respuestas de la API.

Los controladores implementados son:

- ArbolController
- EspecieController
- OrganizacionController
- ProyectoController
- ReporteController
- UsuarioController
- IAController

---

## DTO

Los DTO (Data Transfer Object) se utilizan para controlar la información que recibe y envía la API.

Esto permite evitar trabajar directamente con las entidades de la base de datos en algunas operaciones.

---

## Data

Contiene la configuración relacionada con la base de datos.

Incluye:

- GreenForestContext: conexión con Entity Framework Core.
- DbSeeder: creación de datos iniciales para pruebas.

---

## Services

Contiene la lógica de negocio del sistema.

Actualmente incluye servicios para manejar operaciones relacionadas con usuarios y la conexión con Gemini.

---

## AI

Contiene la implementación del servicio de inteligencia artificial.

Se utiliza Google Gemini para responder preguntas relacionadas con el proyecto mediante un endpoint propio.

---

# Base de datos

El proyecto utiliza SQLite como base de datos.

La conexión se encuentra configurada en el archivo:


appsettings.json


La base de datos almacena información de:

- Usuarios.
- Proyectos.
- Árboles.
- Especies.
- Organizaciones.
- Reportes.

---

# Relaciones entre entidades

Las principales relaciones del sistema son:

- Un usuario puede tener varios proyectos.
- Una organización puede estar relacionada con varios proyectos.
- Un proyecto puede contener varios árboles.
- Un proyecto puede tener varios reportes.
- Una especie puede estar relacionada con varios árboles.

Representación:


Usuario
|
|
Proyecto
|
|------ Organización
|
|------ Árbol
|
|
Especie
|
|------ Reporte


---

# Endpoints principales

## Usuarios


GET /api/Usuario
GET /api/Usuario/{id}
POST /api/Usuario
PUT /api/Usuario/{id}
DELETE /api/Usuario/{id}


Permite crear, consultar, actualizar y eliminar usuarios.

---

## Proyectos


GET /api/Proyecto
GET /api/Proyecto/{id}
POST /api/Proyecto
PUT /api/Proyecto/{id}
DELETE /api/Proyecto/{id}


Permite administrar los proyectos de reforestación.

---

## Árboles


GET /api/Arbol
GET /api/Arbol/{id}
POST /api/Arbol
PUT /api/Arbol/{id}
DELETE /api/Arbol/{id}


Permite registrar y administrar árboles asociados a proyectos.

---

## Especies


GET /api/Especie
GET /api/Especie/{id}
POST /api/Especie
PUT /api/Especie/{id}
DELETE /api/Especie/{id}


Permite manejar las especies disponibles.

---

## Organizaciones


GET /api/Organizacion
GET /api/Organizacion/{id}
POST /api/Organizacion
PUT /api/Organizacion/{id}
DELETE /api/Organizacion/{id}


Permite administrar las organizaciones relacionadas con los proyectos.

---

## Reportes


GET /api/Reporte
GET /api/Reporte/{id}
POST /api/Reporte
PUT /api/Reporte/{id}
DELETE /api/Reporte/{id}


Permite generar y consultar reportes de los proyectos.

---

# Inteligencia artificial

El proyecto cuenta con un servicio conectado a Google Gemini.

Endpoint:


POST /api/IA/chat
 # Video de youtube
 ## https://youtu.be/EyEWva4IqYE

Ejemplo de solicitud:

```json
{
  "pregunta": "¿Qué árboles son recomendados para una zona tropical?"
}

El servicio recibe la pregunta, la envía a Gemini y devuelve la respuesta generada.

Instalación y ejecución
Requisitos

Tener instalado:

.NET SDK 8
Visual Studio Code o Visual Studio
Git
Clonar el repositorio
git clone https://github.com/RM0135/GreenForest-API.git
Entrar al proyecto
cd GreenForest-API
Restaurar dependencias
dotnet restore
Ejecutar proyecto
dotnet run
Swagger

La API cuenta con Swagger para realizar pruebas de los endpoints.

Después de ejecutar el proyecto se puede ingresar desde:

https://localhost:puerto/swagger

Desde Swagger se pueden probar las operaciones GET, POST, PUT y DELETE.

Conceptos aplicados

Durante el desarrollo del proyecto se aplicaron conceptos como:

Programación orientada a objetos.
Clases y objetos.
Encapsulamiento.
Relaciones entre entidades.
Inyección de dependencias.
Servicios.
Entity Framework Core.
Manejo de bases de datos.
APIs REST.
Posibles mejoras

Algunas mejoras futuras pueden ser:

Implementar autenticación con JWT.
Manejar roles de usuario.
Mejorar la seguridad de contraseñas.
Implementar pruebas unitarias.
Agregar paginación y filtros.
Realizar despliegue en un servidor.
