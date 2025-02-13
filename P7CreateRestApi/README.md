# P7 Findexium - API de gestion financière

## Prérequis
- .NET 6 ou plus
- SQL Server (LocalDB ou autre)
- Visual Studio (ou tout IDE compatible avec .NET)

## Configuration

### Base de données
Deux bases sont utilisées :
- Findexium → Contient les entités financières.
- FindexiumIdentity → Contient les utilisateurs et l'authentification (Identity).

Avant de démarrer, exécuter les migrations :
```
Update-Database -Context LocalDbContext
Update-Database -Context ApplicationDbContext
```

### Configuration des secrets (dotnet user-secrets)
L'application utilise dotnet user-secrets pour stocker les informations sensibles.

Exécuter ces commandes pour définir les valeurs nécessaires :
```
dotnet user-secrets set "AdminSettings:Password" "AdminUserOnePassword123!"
dotnet user-secrets set "Jwt:Key" "F25073BA-68BB-4CD7-82CE-2339C45BAA9F"
```

## Lancer l'application
Démarrer l'API avec :
```
dotnet run
```

L'API est accessible via Swagger :
```
http://localhost:7210/swagger/index.html
```

## Tester l'authentification dans Swagger
1. S'enregistrer ou se connecter
   - Pour tester l'authentification, il faut soit s'enregistrer, soit se connecter en tant qu'admin.
   - Pour se connecter en admin :
     - UserName : AdminUserOne
     - Password : AdminUserOnePassword123!

2. Récupérer et valider le token
   - Après s'être connecté, récupérer le token JWT dans la réponse.
   - En haut de Swagger, cliquer sur Authorize, entrer Bearer <votre_token> et valider.
   - Cela permet de déverrouiller les routes protégées ([Authorize]).

3. Tester les endpoints
   - GET /User → Affiche tous les utilisateurs (nécessite d'être connecté).
   - GET /BidList → Récupère toutes les enchères.
   - POST /BidList → Ajoute une enchère (authentification requise).
   - DELETE /BidList/{id} → Supprime une enchère (admin requis).

## Tests unitaires
Les tests unitaires couvrent :
- BidListControllerTests → Vérifie les CRUD des entités financières
- LoginControllerTests → Vérifie l'authentification et la génération des tokens JWT

Exécuter les tests avec :
```
dotnet test
```
