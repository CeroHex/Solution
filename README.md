# Sistema de Gestión y Distribución de Ítems de Trabajo

## Descripción

Aplicación backend desarrollada bajo una arquitectura basada en microservicios utilizando **ASP.NET Core Web API con C#**.

El sistema permite gestionar usuarios e ítems de trabajo, implementando un algoritmo automático de distribución de tareas considerando:

- Fecha de vencimiento.
- Nivel de relevancia del ítem.
- Cantidad de ítems pendientes asignados a cada usuario.
- Saturación de usuarios por cantidad de ítems altamente relevantes.

Para la persistencia de datos se utilizan archivos JSON como base de datos simulada.

---

# Arquitectura

El proyecto está compuesto por dos microservicios independientes:


Solution │ UsersService │ WorkItemsService


## Microservicios

### UsersService

Responsable de la gestión y consulta de usuarios.

Puerto:


https://localhost:7001


Responsabilidades:

- Consultar usuarios existentes.
- Crear usuarios.
- Eliminar usuarios.
- Proveer información al microservicio de ítems mediante HTTP.


---

### WorkItemsService

Responsable de la gestión y distribución automática de ítems de trabajo.

Puerto:


https://localhost:7002


Responsabilidades:

- Crear ítems de trabajo.
- Consultar ítems.
- Eliminar ítems.
- Asignar automáticamente los ítems al usuario adecuado.


---

# Arquitectura interna de cada microservicio

Cada microservicio implementa una arquitectura de tres capas:


Controller
|
|
Service
|
|
Repository
|
|
JSON Database


## Controller

Responsable de:

- Recibir solicitudes HTTP.
- Validar parámetros básicos.
- Retornar respuestas HTTP.


## Service

Responsable de:

- Contener lógica de negocio.
- Coordinar operaciones entre controlador y repositorio.
- Ejecutar algoritmo de distribución de ítems.


## Repository

Responsable de:

- Acceso a datos.
- Lectura y escritura de archivos JSON.


---

# Persistencia de datos

Debido a que no se utiliza una base de datos real, la información se almacena en archivos JSON.


Ejemplo:


Solution │ users.json │ items.json


---

# Endpoints API

## UsersService

El microservicio **UsersService** permite la gestión de usuarios mediante los siguientes endpoints:

### GET /api/users

Obtiene la lista completa de usuarios registrados.

Request:
GET https://localhost:7001/api/users

Response:

[
  {
    "id": "9b9b4d2d-3df7-4b5c-9f7f-8a4dbef7c001",
    "username": "usuario A"
  },
  {
    "id": "2d7d49c8-5f5d-4c73-b7f5-0b63e7d4c002",
    "username": "usuario B"
  }
]


### POST /api/users

Permite crear un nuevo usuario.

Request:

{
  "username": "usuario C"
}

Response:

{
  "id": "c1d5fd42-77df-4fb6-a60c-52b27db4c003",
  "username": "usuario C"
}


### DELETE /api/users/{id}

Elimina un usuario mediante su UUID.

Request:

DELETE https://localhost:7001/api/users/9b9b4d2d-3df7-4b5c-9f7f-8a4dbef7c001

Response:

200 OK



# WorkItemsService

El microservicio **WorkItemsService** permite gestionar los ítems de trabajo y realizar la distribución automática según las reglas definidas.

### GET /api/workitems

Obtiene todos los ítems de trabajo registrados con la información del usuario asignado.

Request:

GET https://localhost:7002/api/workitems

Response:

[
  {
    "id": "3ce41d60-5c03-4ede-be3f-e57a7157cb42",
    "title": "TAREA RELEVANTE 1",
    "description": "COMPLETAR TAREA",
    "priority": "HIGH",
    "status": "PENDING",
    "dueDate": "2026-07-10T05:50:38.557Z",
    "assignedUserId": "9b9b4d2d-3df7-4b5c-9f7f-8a4dbef7c001",
    "assignedUsername": "usuario A"
  },
  {
    "id": "e4a1408f-105e-45f1-9f35-e0563c617690",
    "title": "TAREA NO RELEVANTE 1",
    "description": "COMPLETAR TAREA",
    "priority": "LOW",
    "status": "PENDING",
    "dueDate": "2026-09-08T05:50:38.557Z",
    "assignedUserId": "2d7d49c8-5f5d-4c73-b7f5-0b63e7d4c002",
    "assignedUsername": "usuario B"
  }
]


### POST /api/workitems

Crea un nuevo ítem de trabajo y ejecuta automáticamente el algoritmo de distribución para asignarlo al usuario correspondiente.

Reglas aplicadas:

- Si la fecha de entrega es menor a tres días, se asigna al usuario con menor cantidad de ítems sin importar la prioridad.
- Los ítems HIGH se asignan primero a usuarios con menor cantidad de pendientes.
- Usuarios con más de tres ítems HIGH pendientes son considerados saturados.
- Los ítems LOW se asignan al usuario con menor cantidad de pendientes.

Request:

{
  "title": "NUEVA TAREA",
  "description": "COMPLETAR TAREA",
  "priority": "HIGH",
  "dueDate": "2026-08-10T05:50:38.557Z"
}

Response:

{
  "id": "f8a7c9d1-22b3-4e56-9f10-a12345678900",
  "title": "NUEVA TAREA",
  "description": "COMPLETAR TAREA",
  "priority": "HIGH",
  "status": "PENDING",
  "dueDate": "2026-08-10T05:50:38.557Z",
  "assignedUserId": "2d7d49c8-5f5d-4c73-b7f5-0b63e7d4c002",
  "assignedUsername": "usuario B"
}


### DELETE /api/workitems/{id}

Elimina un ítem de trabajo mediante su UUID.

Request:

DELETE https://localhost:7002/api/workitems/3ce41d60-5c03-4ede-be3f-e57a7157cb42

Response:

200 OK


La comunicación entre microservicios se realiza mediante HTTP, donde WorkItemsService consume la información proporcionada por UsersService para obtener los usuarios disponibles y ejecutar la asignación automática de ítems.