Comandos Perfumeria Anita:

1. Entorno de Desarrollo (Docker / Dev Containers)
(Se hace apenas abres VS Code para entrar al entorno seguro de Linux con .NET 9)

	Presionar F1 -> Buscar y seleccionar Dev Containers: Reopen in Container

2. Ejecutar y Compilar el Proyecto

	dotnet watch (Tu comando principal. Arranca la web y actualiza la p�gina autom�ticamente cada vez que guardas un cambio).

	dotnet build (Verifica si el c�digo tiene errores, �til para saber si todo est� verde antes de probar).

	dotnet clean (Borra la "basura" temporal. �salo si VS Code te marca errores extra�os que no tienen sentido).

3. Base de Datos (Entity Framework Core & SQLite)
	
	dotnet ef migrations add NombreDescriptivo (Ejecuta esto cada vez que modifiques o crees un Modelo nuevo, por ejemplo: add AgregadoStock).

	dotnet ef database update (Aplica la migraci�n al archivo .db. Tambi�n sirve para crear la base de datos vac�a cuando pasas a la otra computadora).

4. Rutina Git (Para sincronizar la PC Principal y la Notebook)
	
	git pull origin main (?? Ejecuta esto SIEMPRE apenas te sientes a programar para descargar los cambios de tu otra compu).

	git add . (Prepara todos los archivos que modificaste en tu sesi�n de hoy).

	git commit -m "Mensaje de lo que hiciste" (Guarda los cambios localmente).

	git push origin main (Sube tu trabajo a la nube para que est� listo cuando prendas la otra computadora).

5. Limpieza de Emergencia (En Windows)
(Si alguna vez el proyecto se traba por cambiar de computadora y el contenedor no arranca, ejecuta esto en la terminal normal de PowerShell antes de presionar F1):

	Remove-Item -Recurse -Force bin

	Remove-Item -Recurse -Force obj