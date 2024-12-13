# Introduccion

El presente trabajo describe la elaboracion de una applicacion movil realizada en unity para dispositivos android. Dicha aplicacion permite al usuario crear listas de productos de la casa y escanearlos con realidad aumentada para detectar cuales productos hacen falta. Esta permite compartir las listas con los productos y sus precios totales para que asi los usuarios puedan vizualisarlas desde sus difrereentes dispositivos.





Este es el READ ME del proyecto integrador titulado: Aplicación Android con Realidad Aumentada para la Identificación Eficiente e Interactiva de Productos del Hogar

A continuacion se da una breve explicacion acerca de los codigos implementados.

Item.cs-------------------------------------------------------------

La clase Item es una clase simple y serializable que representa un artículo con propiedades esenciales como nombre, descripción, precio e imagen. Su diseño es ideal para sistemas de inventario, catálogos de productos o cualquier funcionalidad donde necesites representar objetos en Unity.

Variables 

itemName: El nombre del artículo.
itemDescription: Una breve descripción del artículo.
price: El precio del artículo como un valor flotante.
imagePath: La ruta a la imagen asociada al artículo (puede ser un archivo en el sistema o un recurso en el proyecto Unity).

ItemList.cs---------------------------------------------------------

La clase ItemList es un contenedor serializable que organiza y agrupa múltiples objetos de tipo Item bajo un nombre específico (listName). Es útil para gestionar colecciones de artículos dentro de un inventario, una tienda, o cualquier sistema similar.

Variables

listName: Un nombre descriptivo para identificar la lista de artículos
items: Una lista genérica de objetos Item.


CanvasManager.cs----------------------------------------------------

Canvas Manager es un script que me permite controlar que paneles del Canvas se pueden ver y cuales no Tambien controlando avisos que salen cuando el ususario ingresa mal un dato o cuando no ingresa datos en los campos necesarios.

Funciones:

Start():

Inicializa la variable puedeSeguir con el valor true cuando el objeto al que está asociado este script se activa al inicio del juego.

SetPuedeSeguir(bool booleano):

Permite modificar el valor de la variable puedeSeguir desde otra parte del código.
Usando este método, se puede habilitar o deshabilitar dinámicamente el control para seguir activando o desactivando paneles.

ActivatePanel(GameObject panelToActivate):

Activa un panel específico (GameObject) pasado como parámetro si:
El panel no es null.
La variable puedeSeguir es true.
Esto asegura que solo se puedan activar paneles cuando puedeSeguir lo permita.

DeactivatePanel(GameObject panelToDeactivate):

Desactiva un panel específico (GameObject) pasado como parámetro si:
El panel no es null.
La variable puedeSeguir es true.
Después de desactivar el panel, establece puedeSeguir de nuevo a true. Esto implica que se podrá activar o desactivar otros paneles nuevamente.

DeactivateAviso(GameObject panelToDeactivate):

Similar a DeactivatePanel, desactiva el panel pasado como parámetro, pero sin depender del valor de puedeSeguir.
Esto permite desactivar el panel directamente, sin restricciones.


CameraManager.cs----------------------------------------------------

Variables 

GameObject photoPanel:
Panel de la UI que contiene la interfaz relacionada con la captura de fotos.

Texture2D capturedPhoto:
Almacena la foto capturada en formato de textura.

RawImage capturedPhotoImage:
Muestra la foto capturada en la interfaz de usuario.

Botones (takePhotoButton, backButton):

takePhotoButton: Botón para capturar una foto.

backButton: Botón para volver a la pantalla anterior.

InputField itemNameInputField:
Campo de entrada donde el usuario puede escribir un nombre para la foto.

string capturedPhotoPath:
Ruta donde se guardará la foto capturada en el sistema de archivos.

Camera arCamera:
Cámara de AR utilizada para capturar la vista.

Funciones:

Start()
Esconde inicialmente el panel de la foto.
Asigna los eventos de clic a los botones:
takePhotoButton: Llama a CapturePhoto pasando el texto del campo de entrada como nombre de la foto.
backButton: Llama a BackToPreviousScreen para volver atrás.


StartCamera() y StopCamera()
StartCamera:
Activa la cámara AR y el panel de la foto, iniciando el modo de captura.
StopCamera:
Esconde el panel de la foto (la cámara AR permanece activa aunque está comentada en este código).


CapturePhoto(string itemName)
Inicia la captura de una foto llamando a CapturePhotoCoroutine, pasando el nombre del item ingresado como parámetro.


CapturePhotoCoroutine (Corutina)
Captura una foto de la vista actual de la cámara AR utilizando un RenderTexture.
Pasos:
Esperar a que termine el frame actual para asegurarse de que la vista está completamente renderizada.
Crear un RenderTexture para almacenar temporalmente lo que ve la cámara AR.
Aplicar el RenderTexture a la cámara y capturar la vista.
Convertir la imagen capturada en una textura (Texture2D) y copiar los píxeles desde el RenderTexture.
Mostrar la foto capturada en el elemento capturedPhotoImage.
Guardar la foto llamando a SavePhoto.
Limpiar el RenderTexture para que no interfiera con el renderizado posterior de la cámara.


SavePhoto(Texture2D photo, string itemName)
Convierte la textura capturada en formato PNG y la guarda en el sistema de archivos.
Usa un nombre único generado con el texto ingresado (itemName) y un timestamp.
La foto se guarda en la ruta Application.persistentDataPath, que es un directorio persistente en la plataforma donde se ejecuta el juego.


BackToPreviousScreen()
Detiene la cámara AR (solo esconde el panel de la foto).


OnDestroy()
Llama a StopCamera para asegurarse de que la cámara se desactiva al destruir el objeto.

SavingManager.cs----------------------------------------------------

Variables

filePath:
Es la ruta del archivo donde se guardarán o cargarán los datos.


Funciones

SaveItemLists()
Guarda dos listas de objetos ItemList (llamadas firstItemList y secondItemList) en un archivo.

LoadFirstItemList() y LoadSecondItemList()
Carga solo la primera lista (firstItemList o secondItemList) desde el archivo JSON.

ImageTargetManager.cs-----------------------------------------------

Variables

arCamera: Cámara AR utilizada para seguimiento.

jsonFilePath: Ruta del archivo JSON que contiene datos de los objetos.

itemImageTextures: Lista de texturas cargadas de las imágenes de los objetos.

instantiatedImageTargets: Lista de objetos de imagen generados.

currentItemList: Lista actual de objetos seleccionados.

inventoryManager: Referencia al administrador de inventario.

canvasPrefab: Prefab del Canvas para mostrar información.

Funciones:

Start()
Inicializa Vuforia y configura la ruta del archivo JSON.

OnVuforiaStarted()
Indica que Vuforia ha sido inicializado correctamente.

LoadImagesFromJson()
Carga los datos del archivo JSON y crea objetivos de imagen a partir de las imágenes especificadas.

SetItemList(ItemList itemList)
Asigna una lista de objetos actual y garantiza nombres únicos para las listas compartidas.

CreateImageTargetFromPath(string path, string itemName, float price, string currentlist)
Crea un ImageTarget desde un archivo de imagen, genera un Canvas asociado y muestra información como el nombre, precio y lista actual.

OnDestroy()
Limpia los objetivos de imagen instanciados al destruir el objeto.

DestroyAllImageTargets()
Destruye todos los objetivos de imagen generados y limpia la lista.

OnTargetStatusChanged(ObserverBehaviour observer, TargetStatus status)
Verifica el estado del seguimiento del objetivo y muestra información en la consola.

EmailSender.cs------------------------------------------------------

Variables

InputField recipientEmailInput:
Campo de entrada para ingresar el correo electrónico del destinatario.

InventoryManager inventoryManager:
Referencia al gestor de inventario que maneja las listas de ítems.

Text titleText:
Elemento de texto de la UI usado para mostrar el título de la lista seleccionada.

ItemList temporaryList:
Mantiene una lista temporal de compras derivada del gestor de inventario.

List<Sprite> photoSprites:
Lista de imágenes (sprites) que representan las fotos de los productos para incrustar en el HTML generado.

string fromEmail:
Dirección de correo electrónico del remitente (AppCompras2024@gmail.com).

string smtpServer:
Servidor SMTP para enviar correos electrónicos (smtp.gmail.com).

int smtpPort:
Número de puerto para SMTP (587).

string smtpUsername y string smtpPassword:
Credenciales de correo electrónico para autenticación.

string toEmail:
Dirección de correo electrónico del destinatario (recuperada desde recipientEmailInput).

float totalPrice:
Precio total de los ítems en la lista de compras (calculado en GenerateShoppingListHtml y GenerateShoppingListTxt).

Funciones

SendEmailWithAttachment()
Envía un correo electrónico al destinatario con la lista de compras adjunta como archivo (HTML o TXT).
Maneja la configuración SMTP, la creación del correo electrónico y la eliminación del archivo después de enviarlo.

GenerateShoppingListHtml()
Crea un archivo HTML con una lista de compras estilizada que incluye los nombres de los productos, precios, precio total e imágenes incrustadas.

GenerateShoppingListTxt()
Crea un archivo de texto plano que contiene la lista de compras con los nombres de los productos, precios y precio total.

findList()
Busca una lista de compras en inventoryManager.itemListsShareable basado en titleText.text.
Copia los datos de la lista a temporaryList para su posterior procesamiento.
Registra los detalles de la lista encontrada o muestra una advertencia si no se encuentra.

ARButtons.cs--------------------------------------------------------

Variables

Text name:
Componente de texto de la UI que contiene el nombre del ítem que se va a agregar.

Text price:
Componente de texto de la UI que contiene el precio del ítem que se va a agregar.

Text listname:
Componente de texto de la UI que contiene el nombre de la lista (aunque no se usa actualmente en el script).

InventoryManager inventoryManager:
Referencia a la clase InventoryManager, utilizada para gestionar el inventario y agregar ítems a la lista actual.

float totalPrice:
Variable para hacer seguimiento del precio total de los ítems (actualmente no se usa en el script, pero podría ser útil para cálculos futuros).

Functions

Start()
Se llama cuando el script comienza. Encuentra el objeto InventoryManager en la escena y lo asigna a la referencia inventoryManager.

Anadir()
Esta función se llama cuando se presiona el botón "Anadir". Crea un nuevo Item con el nombre y el precio proporcionados por los componentes de texto de la UI (name.text y price.text), y lo agrega a la lista actual de inventario utilizando inventoryManager.AddItemToCurrentList(newItem).

NoAnadir()
Esta función se llama cuando se presiona el botón "NoAnadir". Registra un mensaje indicando que no se agregó ningún ítem a la lista.

GetActiveTargetName()
Esta función auxiliar obtiene el nombre del objetivo AR actualmente activo. Revisa todos los objetos ObserverBehaviour activos en la escena y devuelve el nombre del objetivo rastreado. Si no se encuentra ningún objetivo activo, devuelve null.

InventoryManager.cs-------------------------------------------------

Script mas importante, conecta es a base que ocnecta el modelo y el view y Tambien maneja los datos de las listas. A continuacion e presentaran las variables y funciones principals e este.

Variables

InputField listNameInputField, itemNameInputField, itemDescriptionInputField, itemPriceInputField, itemNameDefault, itemDescriptionDefault, itemPriceDefault:
Campos de entrada de texto para ingresar el nombre de las listas y los detalles de los items (nombre, descripción, precio).

GameObject itemListPrefab, itemEntryPrefab, itemShareableListPrefab, itemScanListPrefab, itemEditListPrefab:
Prefabs de UI que se instancian para mostrar listas de items, entradas de items, y otros elementos de la interfaz.

Transform itemListContainer, itemDisplayContainer, itemShareableContainer, itemEditListContainer, itemScanListContainer:
Contenedores donde se colocan los items o listas en la interfaz de usuario.

Text itemListTitleText, itemShareableListTitleText, totalPrecioTxt:
Elementos de texto para mostrar el título de las listas y el precio total de los items.

GameObject itemCreationPanel, itemListDisplayPanel, itemEditListDisplayPanel, itemScanListDisplayPanel, panelScanner, panelAviso:
Paneles que se muestran en la UI para crear items, visualizar listas, y mostrar advertencias.

List<ItemList> itemLists, itemListsShareable:
Listas de objetos ItemList que contienen las listas de items regulares y compartibles.

ItemList currentItemList, currentItemListShareable, temporaryList:
Variables para mantener la lista de items actual, la lista compartible actual y una lista temporal para manipulaciones.

SavingManager savingManager:
Componente encargado de guardar y cargar las listas de items.

GameObject scannerCamera:
Referencia a la cámara del escáner.

CameraManager cameraManager:
Referencia al componente que maneja la cámara.

ImageTargetManager imageTargetManager:
Referencia a un componente que maneja los objetos detectados por el escáner (probablemente AR).

CanvasManager canvasManager:
Componente encargado de manejar el estado de la interfaz de usuario y sus interacciones.

Funciones

Start():
Carga las listas de items guardadas al inicio del juego mediante 
savingManager.LoadFirstItemList() y savingManager.LoadSecondItemList()
Actualiza la interfaz de usuario (UI) con las listas de items.

CreateNewShareableItemList(string name, string baseName):
Crea una nueva lista de items compartible basada en un nombre y una lista base.
Clona los items de la lista base en la nueva lista compartible.

AddItemListToShareableUI(ItemList itemList):
Añade un nuevo item a la interfaz de usuario de listas compartibles, instanciando un prefab para la lista y asignando el nombre.

ViewItemListShareable(ItemList itemList):
Muestra los items de una lista compartible en la UI.
Calcula el precio total de los items y lo muestra en la interfaz.

CompareList(string listName):
Compara el nombre de una lista proporcionada con las listas compartibles almacenadas, devuelve true si encuentra una coincidencia.

AddItemToCurrentList(Item newItem):
Añade un nuevo item a la lista actual de items compartibles.

DiscardItems():
Descarta los items que ya existen en la lista temporal de items y los elimina de la lista base.

CreateNewItemList():
Crea una nueva lista de items con el nombre proporcionado desde un campo de entrada.
Guarda la lista creada en itemLists y actualiza la UI.

AddItemListToUI(ItemList itemList):
Añade una nueva lista de items a la interfaz de usuario general, instanciando un prefab para cada lista.

AddItemScanListToUI(ItemList itemList):
Añade una lista de items a la interfaz de usuario de escaneo, permitiendo acceder a la lista desde el escáner.

OpenScanner():
Abre el escáner y asocia la lista de items seleccionada con el ImageTargetManager.

AddItemEditListToUI(ItemList itemList):
Añade un item de lista editable en la UI para editar o borrar listas existentes.
