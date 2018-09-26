# Readme

Les dossiers Assets, PLStream et PythonScripts sont stocké sur le github
Le dossier "Polhemus" est le dossier de sources de Polhemus qui permet de recompiler le fichier "UnityExport.exe" 

Il y a un fichier "logs" sur le casque ici :
	"User Files \ LocalAppData \ HololensProject_1.0.10.0_x86__pzq3xp76mxafg \ TempState \ UnityPlayer.log"

* Version unity : 2018.1.1.f1 personal
* Version visual studio : Community 2017 - 15.7.4

##### Pour le bon fonctionnement appli : 
* 1) Executer "UnityExport.exe" dans le dossier "PlStream" : sans argument <=> ouvre port sur 192.168.137.1 : 5124 cette @IP
    est celle du point d'accès wifi sur lequel se connect l'hololens. Pour changer l'adresse IP :
    UnityExport @ port (voir message au lancement)
* 2) Executer "ServerEnvoieFichier" dans "Python Scripts" (aucun argument)
* 3) Lancer IP Config puis Service transfert fichier, puis IP config a nouveau (en changeant l'ip), puis StartApp
## I) Assets: 

### 1) HoloToolkit : 
	Pas touché.

### 2) HoloToolkit-Examples : 
	Pas touché.

### 3) Materials :
	les materiaux pour les objets de la boite (green glassred etc).

### 4) Prefabs : 
	Objets fabriqués pour la scène.
    
### 5) Ressources : 
	Contient des prefabs necessaires à la copie sur le casque, ce sont les objets instanciés dynamiquement dans l'appli par unity, et pas "inclus" au départ au lancement.
### 6) Streaming Assets : 
	Comme les ressources.

### 7) Scene: 
	La scene, fabriqué avec unity 

### 8) Scripts :

#### a) Autres
	Contient tous les scripts non trié

##### CheckForPlacebyTapOnChildreen :
	Verifie que les enfants de l'objet sur lequel ce script est attaché ont bien le script "PlaceByTap" attaché sur eux
    
##### GLTFComponentPerso : 
	Copie modifié du fichier "GLTFComponent" fourni par "Khronos", le groupe à l'origine du format GLTF, Blender etc...
    
##### DebugHelper :
	Sert à debug : affiche du texte sur la zone de debug, (touche 'p')
    
##### PlStreamCustom :
	Réécriture et amélioration du script "PlStream" fourni par polhemus pour qu'il soit plus adapté au projet

##### ReceiveAndWriteFile :
	Reception du fichier gltf envoyé via le serveur et écriture sur le hololens dans le dossier dédié
	
##### TransformFromUserInput :
	Permet de déplacer / orienté / reduire - augmenter la taille de l'objet 3D (la boite, modèle 3D chargé etc....)

##### UpdateDecallageWithAiguille: 
	Met à jour le décallage de l'axis viewer par rapport à celui de l'aiguille -> les 2 sont parfaitement synchronisé

##### UpdatePosOrientAiguille :
	Utilise le PLStreamCustom pour recupérer les informations de déplacement du capteur et les appliqués à l'aiguille

#### b) Calibrage :
	Contient les fichiers scripts necessaire au calibrage
    
###### 1) CalibrageClickAnnex :
	Permet d'indiquer que l'on a bien cliquer sur l'élément pour le calibrage

###### 2) CalibrageClicker :
	Contient les formules pour le calibrage

#### c) DragByGaze -> Obsolète
	Il y a un script fourni par microsoft qui fait ce que le dossier contient et qui est plus clair et plus simple

#### d) UI
	Contient les script liés à l'UI

##### CreateElementsFromFileList :
	Permet de créer des boutons à partir de la liste des fichiers de "ShowFileInFolder"

##### IpConfiguratorTriggers :
	Contient tous les déclencheurs de l'IpConfigurator, tout les code utilisé par les boutons ou autres sont ici

##### ShowFileInFolder :
	Permet de récupèrer les liste des dossiers / fichiers d'un dossier voulu

##### UITriggers :
	Contient tous les déclencheurs de l'interface utilisateur, tout les code utilisé par les boutons ou autres sont ici

### 9) Sounds :
	Contient les sons ajouté en plus de ceux du holotoolkit dans le projet 

D:\UnityProjects\HololensProject\Assets\StreamingAssets
\3DModels  : inutilisé mais utile pour des tests (on est sûr que les données seront là)
\shaders INDISPENSABLES


## II) PLStream: 
	Dossier contenant les fichiers scripts du Polhemus (fournit par Polhemus), pas modifié mais utilisé comme exemple pour PlstreamCustom
##### UnityExport.exe
	Fichier très important -> permet l'envoie de donnée du capteur polhemus à l'application : a été recompilé pour correspondre aux besoins du projet :
    Les sources sont disponible dans le dossier "Polhemus"


## III) PythonScripts :

Dossier contenant les scripts python

#### Blender -> Inutilisé
	Fichiers scripts de test pour blender

#### Forwards -> Obsolète
	Fichiers permettant le forward des données du capteur polhemus (avant le changement) 

#### ServeurClientTest -> A titre de test et verification uniquement
	Fichiers de test pour un client / serveur d'envoie de fichier en python

##### ServerEnvoieFichier.py 
	Serveur d'envoie de fichier pour l'application