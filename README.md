# ImportCost Pro

Sistema de gestión de importaciones y cálculo automatizado de **Landed Cost** (costo real final de mercancías puestas en almacén local). Proyecto académico diseñado bajo los requerimientos y la rúbrica de evaluación de 960 puntos.

---

## 👥 Equipo de Desarrollo
*   **Aliandy Jimenez | 2025-1018** 
*   **Waldin Ceballos | 2025-1112** 
*   **Yailyn Santana    | 2025-1111**

---

## 🛠️ Tecnologías y Herramientas
*   **Framework principal:** .NET 9 (ASP.NET Core MVC)
*   **Persistencia de datos:** Entity Framework Core (Enfoque Code-First)
*   **Motor de Base de Datos:** SQL Server
*   **Diseño Visual:** Bootstrap CSS

---

## 🏗️ Reglas de Arquitectura
El proyecto se divide estrictamente en **3 capas independientes**:

1.  **`ImportCostPro.Web` (Presentación):** Controladores MVC y vistas.
2.  **`ImportCostPro.Core` (Lógica de Negocio):** Clases de servicio puros, DTOs y lógica matemática (prorrateos, tasas e impuestos).
3.  **`ImportCostPro.Data` (Acceso a Datos):** El `DbContext`, las entidades de la base de datos y las migraciones de EF Core.

> ⚠️ **Nota Crítica:** Todos los valores monetarios, porcentajes y cálculos financieros deben utilizar obligatoriamente el tipo de dato `decimal`. Queda prohibido el uso de `float` o `double`.

---

## 🚀 Configuración Inicial del Entorno

Sigue estos pasos en tu terminal para clonar y ejecutar el proyecto localmente:

```bash
# 1. Clonar el repositorio privado
git clone https://github.com/AliaJimenez/ImportCost-Pro.git
cd ImportCost-Pro

# 2. Restaurar los paquetes Nuget de la solución
dotnet restore

# 3. Aplicar las migraciones para crear la base de datos local
cd ImportCostPro.Web
dotnet ef database update
