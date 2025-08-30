# Tests DevQuest

Ce dossier contient tous les tests unitaires pour l'application DevQuest, organis�s selon la structure de clean architecture.

## Structure

```
Test/
??? DevQuest.Domain.Tests/           # Tests pour la couche Domain
?   ??? Identity/
?   ?   ??? UserTests.cs            # Tests pour l'entit� User
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

## Technologies utilis�es

- **xUnit** : Framework de test principal
- **FluentAssertions** : Assertions fluides et expressives
- **Moq** : Framework de mocking pour les d�pendances
- **.NET 9** : Version cible du framework

## Types de tests

### DevQuest.Domain.Tests
Tests pour les entit�s du domaine :
- Tests du constructeur de l'entit� User
- Validation des propri�t�s
- Tests de g�n�ration d'ID uniques

### DevQuest.Application.Tests
Tests pour les handlers de commandes et requ�tes :
- **CreateUserCommandHandler** : Validation, v�rification des doublons, hachage des mots de passe
- **GetUserByIdQueryHandler** : R�cup�ration d'utilisateurs existants et inexistants

### DevQuest.Infrastructure.Tests
Tests pour les impl�mentations d'infrastructure :
- **InMemoryUserRepository** : CRUD operations, gestion des cas d'erreur, insensibilit� � la casse

## Ex�cution des tests

```bash
# Ex�cuter tous les tests
dotnet test

# Ex�cuter les tests avec plus de d�tails
dotnet test --verbosity normal

# Ex�cuter les tests d'un projet sp�cifique
dotnet test Test/DevQuest.Application.Tests

# Ex�cuter un test sp�cifique
dotnet test --filter "FullyQualifiedName~CreateUserCommandHandlerTests"
```

## Couverture de test

Les tests couvrent :
- ? Cr�ation d'utilisateurs (validation, doublons, hachage)
- ? R�cup�ration d'utilisateurs par ID
- ? Repository en m�moire (CRUD complet)
- ? Entit� User (constructeur, propri�t�s)

## Conventions

- Un fichier de test par classe test�e
- Nommage : `{ClasseTest�e}Tests.cs`
- Organisation des dossiers qui refl�te la structure du code source
- Utilisation de `GlobalUsings.cs` pour r�duire la duplication des usings
- Tests nomm�s selon le pattern : `{M�thode}_Should_{Comportement}_When_{Condition}`