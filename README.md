\# TallerArchivosPlanos



\## Presentado por



Yokcel Alejandro Ibarra Gaviria



\## Descripción



Aplicación de consola desarrollada en C# para el Taller #6 de Archivos Planos.



El proyecto permite gestionar personas mediante archivos .txt, sin utilizar base de datos. Incluye autenticación de usuarios, bloqueo por intentos fallidos, validaciones, registro de operaciones e informe agrupado por ciudad.



\## Archivos principales



\- Program.cs: contiene la lógica principal del aplicativo.

\- Users.txt: almacena usuarios, contraseñas y estado activo o inactivo.

\- People.txt: almacena las personas registradas.

\- log.txt: registra las operaciones realizadas por cada usuario.



\## Funcionalidades



\- Inicio de sesión desde archivo plano.

\- Bloqueo de usuario después de tres intentos fallidos.

\- Mostrar personas registradas.

\- Agregar persona con validaciones.

\- Editar persona.

\- Eliminar persona con confirmación.

\- Generar informe por ciudad.

\- Guardar cambios en archivo plano.

\- Registrar acciones en archivo de log.



\## Ejecución



Para ejecutar el proyecto:



dotnet run



Usuario de prueba:



admin

Admin123\*

