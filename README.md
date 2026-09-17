🛍️ Perfumería Anita - Sistema de Gestión y POS
---
## 🎯 Objetivo del Sistema
Aplicación web desarrollada desde cero para la gestión integral de una perfumería. El objetivo principal fue digitalizar y automatizar el control de caja, el inventario y la emisión de reportes mediante una interfaz rápida y a prueba de errores para usuarios no técnicos.

## 🛠️ Stack Tecnológico
El sistema fue construido utilizando el ecosistema moderno de Microsoft, priorizando la robustez y el rendimiento:
* **Framework:** C# 12 y **.NET 10**
* **Frontend:** ASP.NET Core Blazor (Single Page Application para una experiencia fluida sin recargas)
* **UI/UX:** MudBlazor (Material Design) y MudBlazor Charts para reportes gráficos
* **Base de Datos:** **SQLite** 
* **ORM:** Entity Framework Core (Enfoque Code-First)
* **Hardware Interop:** QuaggaJS integrado mediante JS Interop para usar la cámara como escáner de código de barras.

## 🚀 Funcionalidades Clave implementadas
* **Punto de Venta (POS):** Facturación en tiempo real, cálculo de totales, soporte para múltiples métodos de pago (Efectivo, MercadoPago, Transferencias) y control para evitar ventas vacías.
* **Gestión de Inventario (CRUD):** Altas, bajas y modificaciones con buscador predictivo y edición rápida de precios.
* **Sistema de Alertas:** Notificaciones visuales automáticas cuando un producto llega a 3 unidades o quiebra stock (0 unidades).
* **Módulo de Reportes:** Trazabilidad financiera con reportes de ventas diarias, semanales y estado general del patrimonio en stock.
* **Seguridad:** Autenticación de usuarios mediante ASP.NET Core Identity con separación de roles (Administrador y Vendedor).

## 📐 Lógica de Negocio y Arquitectura
Para garantizar la integridad de los datos comerciales, se implementaron reglas estrictas a nivel de base de datos y backend:
* **Integridad de Stock:** Validaciones para impedir transacciones que generen stock negativo.
* **Borrado Lógico:** Los productos que ya poseen ventas registradas en el sistema no pueden ser eliminados físicamente de la base de datos para no romper el historial financiero; en su lugar, se cambia su estado a "Inactivo".
* **Unicidad:** Restricciones de base de datos para evitar la duplicidad de códigos de barras.

---
**Desarrollado por:** Lucas Ezequiel Flores
