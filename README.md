# BennyStock - Application de gestion de stock

## Description
Application web ASP.NET Core développée pour gérer les stocks dans un environnement de restauration multi-sites (Rôtisseries Benny).

## Fonctionnalités principales

- Gestion des stocks par restaurant
- Visualisation des disponibilités entre restaurants
- Transferts de stock avec validation automatique
- Historique des mouvements de stock
- Réception fournisseur avec ajout de facture (PDF, image)
- Import des ventes via fichier CSV (simulation Veloce)
- Export automatique de l’inventaire en Excel
- Système de connexion avec gestion des rôles (Admin / Manager)

## Technologies utilisées

- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- EPPlus (Excel)
- Bootstrap
- Docker (configuration incluse)

## Comptes de test

Admin :
admin@benny.com / 1234

Manager Boucherville :
boucherville@benny.com / 1234

Manager La Prairie :
laprairie@benny.com / 1234

## Lancer le projet

1. Configurer PostgreSQL local
2. Appliquer les migrations
3. Lancer l'application via Visual Studio

## Déploiement

Le projet est préparé pour être déployé avec Docker et docker-compose.

## Améliorations futures

- Intégration API Veloce
- Scan automatique des factures (OCR)
- Déploiement Kubernetes
- CI/CD avec GitHub Actions
