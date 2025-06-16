# Days Off Project

## Installation du projet
Pour installer ce projet, il vous faudra cloner ce repository sur votre poste de travail pour ouvrir le projet dans Visual Studio, de préférence dans sa version 2022 et avoir .Net 8 d'installé.

Ouvrez la solution `DaysOffManager.sln`.

### Base de données

Dans le fichier de configurations `appsettings.json` dans la couche `Application` remplacez la `DayOffManagerDbConnection` par votre chaîne de connexion locale.

Au démarrage de l'application la base de données se créera automatiquement.

## Démarrage du projet
Le projet dispose d'une configuration de démarrage nommée `https`. Cette configuration permet de démarrer la couche `Application` et d'ouvrir automatiquement dans votre navigateur par défaut un Swagger avec l'ensemble des routes accessibles.

## Contexte du projet
Ce projet intègre deux User stories :
 - Soumission d'une demande de congé
 - Approbation ou rejet d'une demande de congé

Ce projet dispose de tests unitaires réalisés avec `xUnit` afin de tester la couche `Domain`.