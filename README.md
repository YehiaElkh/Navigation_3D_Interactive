<div align="center">

# 🏭 Navigation 3D Interactive

### Conception d'un atelier virtuel et développement des scripts de contrôle sous Unity 3D

![Unity](https://img.shields.io/badge/Unity-6000.3.26f1_LTS-000000?logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-MonoBehaviour-239120?logo=csharp&logoColor=white)
![Render Pipeline](https://img.shields.io/badge/Render_Pipeline-URP-blue)
![Platform](https://img.shields.io/badge/Platform-Linux%20%7C%20Windows%20%7C%20macOS-lightgrey)
![Status](https://img.shields.io/badge/Statut-Terminé-success)

**TP 2 – Module Réalité Virtuelle et Réalité Augmentée**
Filière Robotique et Objets Connectés (ROC3) · Université Mohammed Premier · ENIAD Berkane

</div>

---

## 📑 Table des matières

1. [Présentation](#-présentation)
2. [Démonstration](#-démonstration)
3. [Objectifs du TP](#-objectifs-du-tp)
4. [Fonctionnalités](#-fonctionnalités)
5. [Contrôles](#-contrôles)
6. [Architecture de la scène](#-architecture-de-la-scène)
7. [Implémentation technique](#-implémentation-technique)
8. [Structure du projet](#-structure-du-projet)
9. [Installation et exécution](#-installation-et-exécution)
10. [Paramètres configurables](#-paramètres-configurables)
11. [Tests et validation](#-tests-et-validation)
12. [Difficultés rencontrées](#-difficultés-rencontrées)
13. [Perspectives d'amélioration](#-perspectives-damélioration)
14. [Informations académiques](#-informations-académiques)

---

## 📖 Présentation

Ce projet est la réalisation du **TP 2 : Navigation 3D interactive**. Il consiste à concevoir un **atelier virtuel de base** sous Unity et à développer les scripts de contrôle permettant à un **opérateur de se déplacer et d'observer son espace de travail à l'écran**, en vue subjective (*First-Person*).

L'environnement est une pièce 3D simplifiée comprenant un sol, quatre murs et un objet central symbolisant une machine ou un poste robotisé. La navigation repose sur deux mécanismes complémentaires :

- le **déplacement** du joueur au clavier (touches **Z, Q, S, D**) avec gestion des collisions ;
- l'**orientation du regard** à la souris, avec rotation fluide et limitation de l'angle vertical.

> Ce type de navigation est une base pour la simulation industrielle, la supervision de cellules robotisées et la formation technique en environnement virtuel.

---

## 🎬 Démonstration

La démonstration vidéo du projet est disponible ici :

👉 **[`docs/demo.webm`](docs/demo.webm)**

<!--
  Pour afficher la vidéo directement dans ce README :
  modifiez le README sur GitHub (icône ✏️) puis glissez-déposez demo.webm dans l'éditeur.
  GitHub génère un lien que vous pouvez coller à la place du lien ci-dessus.
-->

### Aperçu

| Vue d'ensemble de l'atelier | Vue à la première personne |
|:---:|:---:|
| ![Vue d'ensemble](docs/images/vue-ensemble.png) | ![Vue subjective](docs/images/vue-subjective.png) |

---

## 🎯 Objectifs du TP

Conformément à la consigne, le travail est organisé en quatre étapes :

| # | Étape | Description |
|:-:|-------|-------------|
| 1 | **Création et configuration** | Projet 3D sous Unity avec une arborescence structurée et un dossier `Scripts` |
| 2 | **Modélisation de l'atelier** | `Plane` pour le sol, `Cube` pour les murs, objet central (machine / poste robotisé) |
| 3 | **Déplacement au clavier** | Objet joueur avec `Character Controller` et script C# commenté, touches Z, Q, S, D, collisions avec les murs |
| 4 | **Orientation à la souris** | Caméra subjective `PlayerCamera` et script C# avec rotation fluide, continue, sans blocage et avec limitation de l'angle vertical |

---

## ✨ Fonctionnalités

- ✅ Atelier virtuel : sol, 4 murs, machine centrale
- ✅ Déplacement avant / arrière / latéral et **diagonal** (ZQSD)
- ✅ Mouvement **fluide** grâce à une accélération progressive
- ✅ Gravité appliquée au joueur
- ✅ **Collisions** avec les murs et la machine (`Character Controller`)
- ✅ Caméra à la première personne pilotée à la souris
- ✅ Rotation horizontale (*yaw*) et verticale (*pitch*) **séparées**
- ✅ Angle vertical limité à **−45° / +45°**
- ✅ Lissage de la rotation
- ✅ Gestion du curseur : verrouillé en jeu, libéré avec `Échap`, reverrouillé par clic gauche

---

## 🎮 Contrôles

| Touche / Action | Effet |
|:---------------:|-------|
| `Z` | Avancer |
| `S` | Reculer |
| `Q` | Déplacement latéral gauche |
| `D` | Déplacement latéral droit |
| `Z` + `Q` / `Z` + `D` / `S` + `Q` / `S` + `D` | Déplacements diagonaux |
| **Souris (axe X)** | Regarder à gauche / à droite |
| **Souris (axe Y)** | Regarder en haut / en bas (limité à ±45°) |
| `Échap` | Libérer le curseur |
| **Clic gauche** | Reverrouiller le curseur et reprendre la navigation |

> Les touches correspondent à la disposition de clavier **AZERTY**.

---

## 🧱 Architecture de la scène

Hiérarchie des `GameObjects` dans la scène `SampleScene` :

```
SampleScene
├── Directional Light
├── Global Volume
├── Sol                      (Plane – surface de déplacement)
│   ├── BackWall             (Cube)
│   ├── FrontWall            (Cube)
│   ├── LeftWall             (Cube)
│   └── RightWall            (Cube)
├── Machine_Base             (objet central – machine / poste robotisé)
│   └── Machine_Body
└── Player                   (Character Controller + PlayerMovement + MouseLook)
    └── PlayerCamera         (pivot de la rotation verticale → Main Camera)
```

| Objet | Rôle |
|-------|------|
| **Sol** | Surface principale de déplacement |
| **Murs** | Limites de l'atelier, obstacles fixes avec collision |
| **Machine_Base** | Machine centrale, obstacle supplémentaire |
| **Player** | Représente l'utilisateur, porte le contrôleur et les scripts |
| **PlayerCamera** | Point de vue de l'utilisateur, enfant du joueur |

---

## ⚙️ Implémentation technique

### 🚶 `PlayerMovement.cs` — Déplacement

Script attaché à l'objet `Player` (`[RequireComponent(typeof(CharacterController))]`).

1. Lecture des touches Z, Q, S, D → vecteur de direction (`horizontal`, `vertical`).
2. Normalisation du vecteur (`Vector3.ClampMagnitude`) pour éviter une vitesse supérieure en diagonale.
3. Calcul de la vitesse cible (`direction * speed`) et **accélération progressive** via `Vector3.MoveTowards`.
4. Application de la **gravité** (vitesse verticale, réinitialisée quand le joueur est au sol).
5. Déplacement final par `CharacterController.Move(movement * Time.deltaTime)`, qui prend en charge les collisions.

### 🖱️ `MouseLook.cs` — Orientation de la caméra

Script attaché à `Player`, avec une référence au pivot `PlayerCamera` (`cameraPivot`).

- **Yaw** (horizontal) : appliqué à l'objet `Player` autour de l'axe Y.
- **Pitch** (vertical) : appliqué au pivot `PlayerCamera` autour de l'axe X.
- Limitation de l'angle vertical avec `Mathf.Clamp(pitch, minVerticalAngle, maxVerticalAngle)`.
- Lissage des rotations avec `Mathf.SmoothDampAngle` / `Mathf.SmoothDamp`.
- Gestion du curseur via `Cursor.lockState` et `Cursor.visible`.

> La séparation *yaw / pitch* (corps / pivot de caméra) reproduit le mouvement naturel de la tête et évite les rotations parasites.

---

## 📂 Structure du projet

```
Navigation_3D_Interactive/
├── Assets/
│   ├── Scenes/
│   │   ├── SampleScene.unity        # Scène principale (atelier virtuel)
│   │   └── Scripts/
│   │       ├── PlayerMovement.cs    # Déplacement ZQSD + gravité + collisions
│   │       └── MouseLook.cs         # Orientation de la caméra à la souris
│   ├── Settings/                    # Paramètres URP
│   └── InputSystem_Actions.inputactions
├── Packages/                        # Dépendances Unity
├── ProjectSettings/                 # Configuration du projet
├── docs/
│   ├── rapport-TP2.pdf              # Rapport de travaux pratiques
│   ├── demo.webm                    # Démonstration vidéo
│   └── images/                      # Captures d'écran
├── .gitignore
└── README.md
```

---

## 🚀 Installation et exécution

### Prérequis

- **Unity Hub** et **Unity 6000.3.26f1 (LTS)** (ou version compatible)
- Un clavier **AZERTY** (ou adapter les touches dans `PlayerMovement.cs`)

### Étapes

```bash
# 1. Cloner le dépôt
git clone https://github.com/<votre-utilisateur>/unity-tp2-navigation-3d.git
cd unity-tp2-navigation-3d
```

1. Ouvrir **Unity Hub** → **Add** → sélectionner le dossier du projet.
2. Ouvrir le projet avec la version d'Unity indiquée ci-dessus.
3. Ouvrir la scène `Assets/Scenes/SampleScene.unity`.
4. Cliquer sur **▶ Play**, puis cliquer dans la fenêtre *Game* pour verrouiller le curseur.

### ⚠️ Système d'entrée

Les scripts utilisent l'ancien **Input Manager** (`Input.GetKey`, `Input.GetAxis`). Si Unity affiche une erreur d'entrée, vérifier :

**Edit → Project Settings → Player → Other Settings → Active Input Handling**
→ choisir **Input Manager (Old)** ou **Both**, puis redémarrer l'éditeur.

---

## 🔧 Paramètres configurables

Les paramètres sont exposés dans l'**Inspector** de Unity.

### `PlayerMovement`

| Paramètre | Valeur par défaut | Description |
|-----------|:-----------------:|-------------|
| `speed` | `4` | Vitesse de déplacement |
| `acceleration` | `10` | Rapidité de mise en vitesse |
| `gravity` | `-9.81` | Intensité de la gravité |

### `MouseLook`

| Paramètre | Valeur par défaut | Description |
|-----------|:-----------------:|-------------|
| `mouseSensitivity` | `150` | Sensibilité de la souris |
| `smoothTime` | `0.05` | Temps de lissage de la rotation |
| `minVerticalAngle` | `-45` | Angle minimal (regard vers le haut) |
| `maxVerticalAngle` | `45` | Angle maximal (regard vers le bas) |

### `Character Controller`

| Paramètre | Valeur |
|-----------|:------:|
| `Slope Limit` | 45 |
| `Step Offset` | 0.3 |
| `Skin Width` | 0.08 |
| `Radius` | 0.5 |

---

## 🧪 Tests et validation

| Fonctionnalité | Critère de validation | Résultat |
|----------------|----------------------|:--------:|
| Environnement 3D | Présence du sol, des murs et de l'objet central | ✅ |
| Déplacement avant / arrière | Touches Z et S | ✅ |
| Déplacement latéral | Touches Q et D | ✅ |
| Déplacement diagonal | Combinaison de touches | ✅ |
| Collisions | Absence de traversée des murs et de la machine | ✅ |
| Orientation horizontale | Rotation gauche / droite à la souris | ✅ |
| Orientation verticale | Rotation haut / bas à la souris | ✅ |
| Limitation de la caméra | Absence de rotation verticale excessive (±45°) | ✅ |
| Navigation globale | Exploration cohérente de l'atelier | ✅ |

---

## 🛠️ Difficultés rencontrées

| Difficulté | Solution apportée |
|------------|-------------------|
| Prise en main de la hiérarchie et des composants Unity | Construction progressive de la scène avec les primitives, ajustement des `Transform` |
| Traversée des murs par le joueur | Utilisation du `Character Controller` et de colliders adaptés |
| Orientation incorrecte de la caméra et mouvements peu naturels | Séparation du *yaw* (joueur) et du *pitch* (pivot de caméra) |
| Retournement de la vue en rotation verticale | Limitation de l'angle avec `Mathf.Clamp` |

---

## 🔭 Perspectives d'amélioration

- 🤖 Animation des articulations du poste robotisé
- 🕹️ Commande interactive du robot
- 📡 Intégration de capteurs simulés
- 🖥️ Interface de supervision (HUD)
- 🎨 Textures, matériaux et éclairage plus réalistes
- ⌨️ Migration vers le nouveau **Input System** de Unity

---

## 🎓 Informations académiques

| | |
|---|---|
| **Établissement** | École Nationale de l'Intelligence Artificielle et du Digital – Berkane |
| **Université** | Université Mohammed Premier |
| **Filière** | Robotique et Objets Connectés (ROC3) |
| **Module** | Réalité virtuelle et Réalité augmentée |
| **Réalisé par** | ELKHOULATI Yahya |
| **Encadré par** | Pr. SEJAI Mohammed |
| **Année universitaire** | 2026 – 2027 |

---

## 📚 Références

- [Unity Manual – Character Controller](https://docs.unity3d.com/Manual/class-CharacterController.html)
- [Unity Scripting API – CharacterController.Move](https://docs.unity3d.com/ScriptReference/CharacterController.Move.html)
- [Unity Scripting API – Mathf.Clamp](https://docs.unity3d.com/ScriptReference/Mathf.Clamp.html)

---

<div align="center">

⭐ Projet réalisé dans un cadre pédagogique — Université Mohammed Premier, 2026 – 2027

</div>
