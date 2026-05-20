# ImportCost Pro

Sistema de gestión de importaciones y cálculo automatizado de **Landed Cost** (costo real final de mercancías puestas en almacén local)[cite: 2]. Proyecto académico diseñado bajo los requerimientos y la rúbrica de evaluación de 960 puntos[cite: 1].

---

## 👥 Equipo de Desarrollo
*   **Aliandy Jimenez** 
*   **Waldin Ceballos** 
*   **Yailin Santana**

---

## 🛠️ Tecnologías y Herramientas
*   **Framework principal:** .NET 9 (ASP.NET Core MVC)[cite: 2]
*   **Persistencia de datos:** Entity Framework Core (Enfoque Code-First)[cite: 2]
*   **Motor de Base de Datos:** SQL Server[cite: 2]
*   **Diseño Visual:** Bootstrap CSS[cite: 2]

---

## 🏗️ Reglas de Arquitectura
El proyecto se divide estrictamente en **3 capas independientes**[cite: 2]. Está totalmente penalizado por la rúbrica mezclar responsabilidades[cite: 2]:

1.  **`ImportCostPro.Web` (Presentación):** Controladores MVC y vistas[cite: 2]. *Regla de oro:* Los controladores solo reciben peticiones y devuelven vistas; **no hacen cálculos ni validaciones complejas de negocio**[cite: 2].
2.  **`ImportCostPro.Core` (Lógica de Negocio):** Clases de servicio puros, DTOs y lógica matemática (prorrateos, tasas e impuestos)[cite: 2].
3.  **`ImportCostPro.Data` (Acceso a Datos):** El `DbContext`, las entidades de la base de datos y las migraciones de EF Core[cite: 2].

> ⚠️ **Nota Crítica:** Todos los valores monetarios, porcentajes y cálculos financieros deben utilizar obligatoriamente el tipo de dato `decimal`. Queda prohibido el uso de `float` o `double`[cite: 2].

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
