# Projet de Cryptographie d'Image .netImageSteganography

Ce projet permet de cacher des messages secrets dans des images en utilisant une technique de stéganographie simple. Il modifie légèrement les pixels de l'image pour y incorporer le texte, de manière imperceptible à l'œil nu.

## Fonctionnalités

- Cryptage : Cache un message dans une image
- Décryptage : Récupère le message caché en comparant avec l'image originale
- Interface console interactive
- Prise en charge des images par défaut et messages personnalisés

## Installation

1. Clonez le projet :

```bash
git clone [url-du-projet]
cd [nom-du-dossier]
```

2. Ajoutez la référence au package dans le fichier .csproj :

```xml
<ItemGroup>
    <PackageReference Include="System.Drawing.Common" Version="6.0.0" />
</ItemGroup>
```

3. Restaurez les packages :

```bash
dotnet restore
```

## Utilisation

1. Placez une image au format PNG dans le dossier du projet (par défaut : "image.png")

2. Lancez le programme :

```bash
dotnet run
```

3. Choisissez une option dans le menu :
   - 1: Crypter une image
   - 2: Décrypter une image
   - 3: Quitter

### Cryptage

- Accepte l'image par défaut ou un chemin personnalisé
- Accepte le message par défaut ou un message personnalisé
- Crée une nouvelle image "image_cryptee.png"

### Décryptage

- Nécessite l'image originale et l'image cryptée
- Compare les deux images pour extraire le message
- Affiche le message récupéré

## Structure du projet

```
ProjetCrypto/
├── Program.cs           # Code source principal
├── image.png           # Image source par défaut
├── image_cryptee.png   # Image générée avec le message
└── ProjetCrypto.csproj # Configuration du projet
```

## Fonctionnement technique

### Cryptage

1. Charge l'image source
2. Pour chaque pixel :
   - Prend un caractère du message
   - Ajoute sa valeur ASCII aux composantes RGB du pixel
   - Applique un modulo 255 pour rester dans les limites valides
3. Sauvegarde la nouvelle image

### Décryptage

1. Compare les pixels des deux images
2. Calcule la différence pour retrouver les caractères
3. Reconstruit le message en éliminant les répétitions

## Limitations

- Fonctionne uniquement avec des images PNG
- Le message se répète dans l'image si celle-ci est trop grande
- Les images doivent avoir exactement les mêmes dimensions
- Modification visible si l'image est analysée numériquement
