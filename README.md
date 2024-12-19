# Introducción

Este es el README del proyecto integrador titulado: Aplicación Android con Realidad Aumentada para la Identificación Eficiente e Interactiva de Productos del Hogar. Específicamente, este trabajo describe la elaboración de una aplicación móvil realizada en Unity para dispositivos Android. Dicha aplicación permite al usuario crear listas de productos del hogar y escanearlos con realidad aumentada para detectar cuáles productos hacen falta. Además, permite compartir las listas con los productos y sus precios totales, de manera que los usuarios puedan visualizarlas desde sus diferentes dispositivos. Dentro de este repositorio se pueden encontrar los códigos utilizados

# Estructura del Proyecto

La estructura del proyecto en carpetas es la siguiente:

<pre>
📁 Proyecto Integrador
|----------------------
|
|--- 📁 <a href="https://github.com/Juan20011223/ProyectoIntegrador/tree/main/Proyecto%20Integrador/Modelo">Modelo</a>
|    |--- InventoryManager.cs
|    |--- SavingManager.cs  
|    |--- ImageTargetManager.cs
|    |--- Item.cs
|    |--- ItemList.cs
|    |--- Modelo.txt
|
|--- 📁 <a href="https://github.com/Juan20011223/ProyectoIntegrador/tree/main/Proyecto%20Integrador/Vista">Vista</a>
|    |--- UIComponents.txt
|
|--- 📁 <a href="https://github.com/Juan20011223/ProyectoIntegrador/tree/main/Proyecto%20Integrador/Controlador">Controlador</a>
|    |--- CameraManager.cs
|    |--- EventHandlers
|    |--- EmailSender.cs
|    |--- CanvasManager.cs
|    |--- ARButtons.cs
|    |--- Controlador.txt
|
|----------------------
</pre>


# Descripcion del Proyecto 

## **Modelo:**   
El modelo se encarga de gestionar la lógica de negocio y los datos de la aplicación. Aquí se definen las clases que representan los objetos de datos, junto con los métodos para manejar estos datos. También puede incluir la lógica para guardar y cargar datos desde archivos o bases de datos.

Responsabilidades:

Definir estructuras de datos y objetos (como Item o ItemList).
Administrar el estado interno y la lógica de persistencia (guardar/cargar información).
Ejemplos en el proyecto:

InventoryManager.cs: Administra el inventario de los objetos de la aplicación.  

SavingManager.cs: Se encarga de guardar y cargar datos de la aplicación.  

ImageTargetManager.cs: Gestiona el reconozimiento de imagenes con Vuforia  

Item.cs y ItemList.cs: Definen los datos y estructuras básicas del inventario (por ejemplo, los objetos y sus listas).

## **Vista**  
La vista maneja todo lo relacionado con la presentación de la información al usuario. Aquí se define cómo se ven y se comportan las interfaces de usuario. No contiene lógica de negocio, solo lógica de interfaz.

Responsabilidades:

Mostrar datos que provienen del modelo.
Proveer una interfaz interactiva que el usuario puede manipular.
Ejemplos en el proyecto:

UI Components: Aquí estarían los elementos de la interfaz de usuario, como botones, menús, y pantallas.  

## **Controlador**  
El controlador actúa como un intermediario entre el modelo y la vista. Recibe las entradas del usuario desde la vista, procesa esas entradas (generalmente usando datos del modelo), y actualiza la vista según sea necesario.

Responsabilidades:

Manejar la lógica de aplicación.  
Responder a los eventos del usuario y actualizar el modelo y/o la vista.  
Coordinar las interacciones entre el modelo y la vista.  
Ejemplos en el proyecto:  

CameraManager.cs: Gestiona la cámara, posiblemente para la funcionalidad de AR.  
EventHandlers: Maneja eventos específicos en la aplicación.  
EmailSender.cs: Envía correos electrónicos cuando es necesario.  
CanvasManager.cs: Controla los elementos del lienzo de la interfaz de usuario.  
ARButtons.cs: Gestiona la lógica detrás de los botones utilizados en la funcionalidad de realidad aumentada.  


#   Requisitos del Proyecto  

1. **Entorno de Desarrollo:**
   - Unity 2019.4.0 o superior
   - Visual Studio 2019 o superior

2. **Lenguaje:**
   - C#
   - .NET Framework

3. **Vuforia Engine:**
   - Configuracion de Vuforia Engine dentro de Unity

# Instalación

Sigue estos pasos para configurar el entorno de desarrollo necesario para el proyecto:

## 1. Instalar Unity
1. Descarga e instala Unity Hub desde su sitio oficial: [Unity Hub](https://unity.com/download).
2. Dentro de Unity Hub:
   - Ve a la sección **Installs**.
   - Haz clic en **Add** y selecciona la versión **2019.4.0** o superior.
   - Asegúrate de incluir los módulos necesarios como:
     - **Android Build Support** (incluye herramientas como SDK y NDK).

## 2. Instalar Visual Studio
1. Descarga e instala Visual Studio 2019 o superior desde: [Visual Studio](https://visualstudio.microsoft.com/).
2. Durante la instalación:
   - Selecciona la carga de trabajo **Desarrollo de juegos con Unity**.
   - Incluye el soporte para **.NET Framework**.

## 3. Configuración de Unity y Vuforia
1. Activa Vuforia en tu proyecto siguiendo estos pasos:
   - Descarga el paquete de Vuforia Engine desde su sitio oficial: [Vuforia Engine Unity](https://developer.vuforia.com/downloads/sdk).
   - Antes de descargar, deberás crear una cuenta en el portal de Vuforia si aún no tienes una. Esto es necesario para acceder a las herramientas de desarrollo.
   - Importa el paquete descargado en Unity:
     - Ve a **Assets > Import Package > Custom Package**, selecciona el archivo del paquete y sigue los pasos para importarlo.

2. Configura la cámara para trabajar con Vuforia:
   - Elimina la cámara principal del proyecto.
   - Agrega una cámara de Vuforia:
     - Ve a **GameObject > Vuforia Engine > AR Camera**.
   - Asegúrate de que los componentes necesarios, como el **Vuforia Behaviour**, se hayan añadido correctamente.

## 4. Instalar Proyecto
- Instala el paquete de unity [Paquete Unity](https://github.com/Juan20011223/ProyectoIntegrador/tree/main/Proyecto%20Integrador).
- Una vez instalado has click derecho en importar paquete y descarga todas las dependencias para instalar el proyecto

# Uso 

Una vez instalado todo lo requerido:

- Preciona play en el editor de unity y prueba la aplicacion
- Si deseas hacer un build ingresa a:
  - **Build Settings > Build**

## En caso de que persista un error o para mas informacion:

Correo : juanromero505@gmail.com
  
