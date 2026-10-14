# ServicioSAT: ejemplo práctico de la Semana 12

Parte del Proyecto 3 de IPC2: **actualización del RTU de punta a punta**.

| Proyecto | Qué es | URL |
|---|---|---|
| `ApiSAT` | Web API (backend, "Servicio 2"). Valida NITs y guarda el RTU en `Datos/rtu.json` | http://localhost:5090 |
| `FrontendSAT` | MVC con vistas Razor (frontend, "Programa 1"). Consume la API con `HttpClient` | http://localhost:5080 |

## Cómo correrlo

Se necesitan **dos terminales**, y la API se arranca primero:

```
cd ApiSAT
dotnet run
```

```
cd FrontendSAT
dotnet run
```

Los archivos de prueba están en `../datos-prueba/`. Para probar la API sin el frontend, use
`ApiSAT/ApiSAT.http` (Visual Studio o VS Code con REST Client) o Postman.

## Endpoints

| Verbo | Ruta | Respuesta |
|---|---|---|
| POST | `/api/rtu` | 200 `{"CONTRIBUYENTES_NUEVOS", "CONTRIBUYENTES_ACTUALIZADOS", "NITS_INVALIDOS"}` · 400 |
| GET | `/api/rtu` | 200 con la lista del RTU |
| GET | `/api/rtu/{nit}` | 200 · 404 |
| POST | `/api/sistema/inicializar` | 200 (deja el sistema sin datos) |
