
# Résolution de systèmes matriciels — PIF1006

Application console en C# implémentant trois méthodes de résolution de systèmes d'équations linéaires : Cramer, Gauss et matrice inverse.

## Fonctionnalités

- Résolution par la règle de Cramer
- Résolution par élimination de Gauss
- Résolution par matrice inverse (comatrice + déterminant)
- Opérations matricielles : transposée, déterminant, comatrice, inverse, multiplication
- Affichage formaté des matrices et des équations
- Validation de la conformité des matrices avant calcul

## Concepts démontrés

- Algèbre linéaire appliquée
- Calcul récursif du déterminant par développement de cofacteurs
- Programmation orientée objet en C#

## Technologies

- C# / .NET 6+

## Prérequis

- .NET 6+

## Lancer le projet

```bash
dotnet run
```

Les matrices sont définies directement dans `Program.cs` — modifier les valeurs de `MatriceUN` et `MatriceDEUX` pour tester d'autres systèmes.

## Structure

```
Program.cs   — point d'entrée et menu interactif
System.cs    — résolution Cramer, Gauss, inverse
Matrix2D.cs  — opérations matricielles (déterminant, inverse, transposée...)
```

---

Projet universitaire solo — cours PIF1006, UQTR.
