<h1 align="center">📦 ImportCost Pro</h1>

<p align="center">
  <b>Import management system that calculates the real <i>Landed Cost</i> of goods delivered to a local warehouse.</b><br/>
  <i>Sistema de gestión de importaciones con cálculo automatizado del costo real (Landed Cost) de la mercancía puesta en almacén.</i>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-9-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 9"/>
  <img src="https://img.shields.io/badge/ASP.NET_Core-MVC-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="ASP.NET Core MVC"/>
  <img src="https://img.shields.io/badge/EF_Core-Code_First-6DB33F?style=for-the-badge" alt="EF Core"/>
  <img src="https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" alt="SQL Server"/>
  <img src="https://img.shields.io/badge/Bootstrap-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white" alt="Bootstrap"/>
</p>

---

## ✨ Features · Funcionalidades

- **Catalogs · Catálogos:** currencies & exchange rates, countries, suppliers, importers, products and tariff categories.
- **Import orders · Órdenes de importación:** products, quantities and associated expenses (freight, insurance, customs, etc.).
- **Tax configuration · Impuestos:** configurable tariff and tax rates.
- **Landed Cost engine · Motor de Landed Cost:** prorates every expense across the order's products and computes the real unit cost, with all money handled as `decimal` for financial precision.

## 🏗️ Architecture · Arquitectura

| Layer · Capa | Responsibility · Responsabilidad |
|---|---|
| `ImportCostPro.Web` | MVC controllers and Razor views · Controladores y vistas |
| `ImportCostPro.Core` | Business services, DTOs and cost calculations · Servicios, DTOs y cálculos |
| `ImportCostPro.Data` | `DbContext`, entities and EF Core migrations · Acceso a datos y migraciones |

## 🚀 Run locally · Ejecutar localmente

**Requirements · Requisitos:** .NET 9 SDK and SQL Server (local instance or Docker).

```bash
git clone https://github.com/AliaJimenez/ImportCost-Pro.git
cd ImportCost-Pro/ImportCostPro.Web
dotnet run
```

The database is created and migrated automatically on first run. · *La base de datos se crea y migra automáticamente al iniciar.*

**Windows:** the default connection string uses your local SQL Server (`Server=.` with Windows authentication).

**macOS / Linux:** start SQL Server with Docker and point the app to it:

```bash
docker compose up -d
export ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=ImportCostProDb;User Id=sa;Password=ImportCost_Dev123!;TrustServerCertificate=True"
cd ImportCostPro.Web && dotnet run
```

## 👥 Team · Equipo

- **Aliandy Jimenez** — [@AliaJimenez](https://github.com/AliaJimenez)
- **Waldin Ceballos**
- **Yailyn Santana**

*Academic project · Proyecto académico — ITLA.*
