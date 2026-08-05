# Manual de Configuración y Comandos Esenciales para Proyectos C# (.NET)

Este manual proporciona una guía paso a paso para configurar la variable de entorno `PATH`, crear, compilar y ejecutar proyectos de consola en C# utilizando la interfaz de línea de comandos de .NET (**.NET CLI**) en Visual Studio Code o cualquier terminal.

---

## Tabla de Contenidos


1. [Requisitos Previos](#1-requisitos-previos)
2. [Configuración de la Variable de Entorno PATH](#2-configuración-de-la-variable-de-entorno-path)
3. [Comandos Esenciales de la .NET CLI](#3-comandos-esenciales-de-la-net-cli)
4. [Flujo de Trabajo Paso a Paso (Paso a Paso)](#4-flujo-de-trabajo-paso-a-paso)
5. [Estructura del Proyecto Generado](#5-estructura-del-proyecto-generado)
6. [Configuración de Extensiones en Visual Studio Code](#6-configuración-de-extensiones-en-visual-studio-code)
7. [Preguntas Frecuentes y Solución de Problemas](#7-preguntas-frecuentes-y-solución-de-problemas)

---

## 1. Requisitos Previos

Antes de comenzar, asegúrate de tener instalados los siguientes componentes:

- **.NET SDK (Software Development Kit):** Se recomienda la versión LTS más reciente (disponible en [dotnet.microsoft.com](https://dotnet.microsoft.com/download)).
- **Visual Studio Code:** Editor de código recomendado.

---

## 2. Configuración de la Variable de Entorno PATH

Si al ejecutar el comando `dotnet` en la terminal obtienes el error:
> *"dotnet no se reconoce como un comando interno o externo, programa o archivo por lote ejecutable"*

Significa que la ruta de instalación de .NET no se encuentra en las variables de entorno de tu sistema operativo.

### Windows (Interfaz Gráfica)
1. Presiona `Win + R`, escribe `sysdm.cpl` y presiona **Enter**.
2. Ve a la pestaña **Opciones avanzadas** y haz clic en **Variables de entorno...**.
3. En la sección **Variables del sistema**, selecciona `Path` y haz clic en **Editar...**.
4. Haz clic en **Nuevo** y agrega la ruta predeterminada de .NET:
   ```text
   C:\Program Files\dotnet
   ```
5. Haz clic en **Aceptar** en todas las ventanas.
6. **Importante:** Reinicia cualquier terminal abierta (CMD, PowerShell o VS Code).

### Windows (PowerShell - Administrador)
```powershell
[Environment]::SetEnvironmentVariable("Path", $env:Path + ";C:\Program Files\dotnet", [EnvironmentVariableTarget]::Machine)
```

### macOS / Linux
Edita tu archivo de configuración del shell (`~/.bashrc` o `~/.zshrc`):

```bash
# Agregar al final de ~/.zshrc o ~/.bashrc
export PATH=$PATH:/usr/share/dotnet
```

Aplica los cambios en la sesión actual:
```bash
source ~/.zshrc    # o source ~/.bashrc
```

---

## 3. Comandos Esenciales de la .NET CLI

| Comando | Descripción | Ejemplo |
| :--- | :--- | :--- |
| `dotnet --version` | Muestra la versión activa del SDK de .NET. | `dotnet --version` |
| `dotnet --info` | Muestra información detallada de los SDKs y Runtimes instalados. | `dotnet --info` |
| `dotnet new <template>` | Crea un nuevo proyecto basado en la plantilla especificada. | `dotnet new console` |
| `dotnet build` | Compila el proyecto y genera los binarios (.exe / .dll). | `dotnet build` |
| `dotnet run` | Compila y ejecuta la aplicación de forma inmediata. | `dotnet run` |
| `dotnet clean` | Limpia los archivos de compilación generados previamente (`bin/` y `obj/`). | `dotnet clean` |
| `dotnet restore` | Restaura las dependencias y paquetes NuGet del proyecto. | `dotnet restore` |

---

## 4. Flujo de Trabajo Paso a Paso

Sigue esta secuencia para crear y ejecutar un proyecto desde cero en **Visual Studio Code**.

### Paso 1: Verificar la instalación
Abre la terminal integrada en VS Code (`Ctrl + ~`) o tu terminal de preferencia y ejecuta:

```bash
dotnet --version
```

### Paso 2: Crear la carpeta y el proyecto
Usa el comando `dotnet new console` especificando el nombre del proyecto con la bandera `-n` (o `-o` para definir la carpeta de salida):

```bash
# Crear un proyecto de consola llamado "MiPrimerProyecto"
dotnet new console -n MiPrimerProyecto
```

### Paso 3: Navegar al directorio del proyecto
```bash
cd MiPrimerProyecto
```

### Paso 4: Abrir el proyecto en Visual Studio Code
```bash
code .
```

### Paso 5: Editar el código
Abre el archivo `Program.cs` y reemplaza el contenido con tu código C#:

```csharp
using System;

namespace MiPrimerProyecto
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine(" ¡Proyecto C# configurado exitosamente!");
            Console.WriteLine("========================================");
        }
    }
}
```

### Paso 6: Ejecutar el proyecto
Asegúrate de estar ubicado en la carpeta raíz del proyecto (donde se encuentra el archivo `.csproj`) y ejecuta:

```bash
dotnet run
```

---

## 5. Estructura del Proyecto Generado

Cuando ejecutas `dotnet new console`, se generan los siguientes archivos principales:

```text
MiPrimerProyecto/
│
├── bin/                   # Contiene los archivos ejecutables compilados (.dll, .exe)
├── obj/                   # Archivos temporales de compilación y restauración de NuGet
├── MiPrimerProyecto.csproj # Archivo de configuración del proyecto (.NET versión, dependencias)
└── Program.cs             # Punto de entrada principal del código C#
```

---

## 6. Configuración de Extensiones en Visual Studio Code

Para tener autocompletado (IntelliSense), sintaxis coloreada y herramientas de depuración (debugging), instala las siguientes extensiones desde el Marketplace de VS Code:

1. **C#** (Desarrollado por Microsoft) — `ms-dotnettools.csharp`
2. **C# Dev Kit** (Opcional, entorno avanzado) — `ms-dotnettools.csdevkit`

---

## 7. Preguntas Frecuentes y Solución de Problemas

### ¿Cómo paso argumentos a la aplicación con `dotnet run` descarga?
Puedes pasar parámetros separándolos con `--`:
```bash
dotnet run -- argumento1 argumento2
```

### ¿Cómo ejecuto un proyecto si tengo múltiples archivos `.csproj`?
Debes especificar la ruta directa del archivo de proyecto:
```bash
dotnet run --project MiPrimerProyecto.csproj
```
