# Tests DevQuest

Ce dossier contient tous les tests unitaires pour l'application DevQuest, organisés selon la structure de clean architecture.

## Structure

```
Test/
??? DevQuest.Domain.Tests/           # Tests pour la couche Domain
?   ??? Identity/
?   ?   ??? UserTests.cs            # Tests pour l'entité User
?   ??? GlobalUsings.cs             # Usings globaux
?   ??? DevQuest.Domain.Tests.csproj
??? DevQuest.Application.Tests/      # Tests pour la couche Application
?   ??? Identity/
?   ?   ??? CreateUser/
?   ?   ?   ??? CreateUserCommandHandlerTests.cs
?   ?   ??? GetUserById/
?   ?       ??? GetUserByIdQueryHandlerTests.cs
?   ??? GlobalUsings.cs             # Usings globaux
?   ??? DevQuest.Application.Tests.csproj
??? DevQuest.Infrastructure.Tests/   # Tests pour la couche Infrastructure
    ??? Identity/
    ?   ??? InMemoryUserRepositoryTests.cs
    ??? GlobalUsings.cs             # Usings globaux
    ??? DevQuest.Infrastructure.Tests.csproj
```

## Technologies utilisées

- **xUnit** : Framework de test principal
- **FluentAssertions** : Assertions fluides et expressives
- **Moq** : Framework de mocking pour les dépendances
- **.NET 9** : Version cible du framework

## Types de tests

### DevQuest.Domain.Tests
Tests pour les entités du domaine :
- Tests du constructeur de l'entité User
- Validation des propriétés
- Tests de génération d'ID uniques

### DevQuest.Application.Tests
Tests pour les handlers de commandes et requêtes :
- **CreateUserCommandHandler** : Validation, vérification des doublons, hachage des mots de passe
- **GetUserByIdQueryHandler** : Récupération d'utilisateurs existants et inexistants

### DevQuest.Infrastructure.Tests
Tests pour les implémentations d'infrastructure :
- **InMemoryUserRepository** : CRUD operations, gestion des cas d'erreur, insensibilité à la casse

## Exécution des tests

```bash
# Exécuter tous les tests
dotnet test

# Exécuter les tests avec plus de détails
dotnet test --verbosity normal

# Exécuter les tests d'un projet spécifique
dotnet test Test/DevQuest.Application.Tests

# Exécuter un test spécifique
dotnet test --filter "FullyQualifiedName~CreateUserCommandHandlerTests"
```

## Couverture de test

Les tests couvrent :
- ? Création d'utilisateurs (validation, doublons, hachage)
- ? Récupération d'utilisateurs par ID
- ? Repository en mémoire (CRUD complet)
- ? Entité User (constructeur, propriétés)

## Conventions

- Un fichier de test par classe testée
- Nommage : `{ClasseTestée}Tests.cs`
- Organisation des dossiers qui reflète la structure du code source
- Utilisation de `GlobalUsings.cs` pour réduire la duplication des usings
- Tests nommés selon le pattern : `{Méthode}_Should_{Comportement}_When_{Condition}`