using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//************************************************************************
//*                                Yannick Poirier                       *
//*                                 POIY04109403                         *
//************************************************************************

namespace PIF1006_tp2
{
    public class System
    {
        public Matrix2D MatriceUN { get; private set; }
        public Matrix2D MatriceDEUX { get; private set; }

        public System(Matrix2D a, Matrix2D b)
        {
            MatriceUN = a;
            MatriceDEUX = b;
        }

        // Permet de s'assurer que la matrice est conforme avant de faire des manipulations
        public bool IsValid()
        {
            // Vérifier si MatriceUN est carrée
            bool isSquare = MatriceUN.IsSquare();

            // Vérifier si MatriceDEUX a le même nombre de lignes et colonnes que MatriceUN
            bool isValidDEUX = MatriceDEUX.Matrice.GetLength(0) == MatriceUN.Matrice.GetLength(0) &&
                               MatriceDEUX.Matrice.GetLength(1) == MatriceUN.Matrice.GetLength(1);

            // Retourner vrai si MatriceUN est carrée et que MatriceDEUX a le même nombre de lignes et colonnes que MatriceUN
            return isSquare && isValidDEUX;
        }

        //Méthode Cramer
        public Matrix2D SolveUsingCramer()
        {
            // Vérifie la validité de la matrice avant de procéder
            if (!IsValid())
            {
                throw new InvalidOperationException("Cette matrice ne peut pas être utilisée pour la résolution avec Cramer.");
            }

            int dimension = MatriceUN.Matrice.GetLength(0);
            double mainDeterminant = MatriceUN.Determinant(); // Calcul du déterminant de la matrice principale

            Matrix2D matriceResolueCramer = new Matrix2D("Solution avec Cramer", dimension, 1);

            // Utilisation d'une seule matrice temporaire pour éviter la duplication
            Matrix2D tempMatrice = new Matrix2D("TempMatrice", dimension, dimension);

            for (int i = 0; i < dimension; i++)
            {
                // Copie des données de MatriceUN vers la matrice temporaire
                for (int m = 0; m < dimension; m++)
                {
                    for (int n = 0; n < dimension; n++)
                    {
                        tempMatrice.Matrice[m, n] = MatriceUN.Matrice[m, n];
                    }
                }

                // Remplace la colonne i par la colonne des termes indépendants (MatriceDEUX)
                for (int j = dimension - 1; j >= 0; j--)
                {
                    tempMatrice.Matrice[j, i] = MatriceDEUX.Matrice[j, 0];
                }

                // Calcul du déterminant de cette nouvelle matrice et division par le déterminant principal
                double determinant = tempMatrice.Determinant();
                double result = determinant / mainDeterminant;

                matriceResolueCramer.Matrice[i, 0] = result; // Stockage du résultat dans la matrice de résolution
            }

            return matriceResolueCramer; 
        }


        public Matrix2D SolveUsingInverseMatrix()
        {
            // Vérifie la validité de la matrice avant de procéder
            if (!IsValid())
            {
                return null;
            }

            // Calcul de l'inverse de la matrice A
            Matrix2D inverseA = MatriceUN.Inverse();

            // Vérification si l'inverse existe
            if (inverseA == null) // Si l'inverse n'existe pas (déterminant de A est nulle)
            {
                return null;
            }

            // Multiplication de l'inverse de A par la matrice B (nouvelle méthode créer Multiply définie dans Matrix.cs)
            Matrix2D matriceResolueInverse = inverseA.Multiply(MatriceDEUX);

            return matriceResolueInverse;
        }

        // Méthode Gauss
        public Matrix2D SolveUsingGauss()
        {
            if (!IsValid()) // S'assure que la matrice est valide
            {
                throw new InvalidOperationException("Cette matrice n'est pas compatible pour la résolution avec la méthode de Gauss.");
            }

            // Création d'une copie de la matrice B
            Matrix2D augmentedMatrice = new Matrix2D("Matrice augmenté", MatriceDEUX.Matrice.GetLength(0), MatriceDEUX.Matrice.GetLength(1));
            for (int i = 0; i < MatriceDEUX.Matrice.GetLength(0); i++)
            {
                for (int j = 0; j < MatriceDEUX.Matrice.GetLength(1); j++)
                {
                    augmentedMatrice.Matrice[i, j] = MatriceDEUX.Matrice[i, j];
                }
            }

            // Algorithme de Gauss pour la résolution
            int rows = augmentedMatrice.Matrice.GetLength(0);
            int cols = augmentedMatrice.Matrice.GetLength(1);

            for (int i = 0; i < rows - 1; i++)
            {
                for (int k = i + 1; k < rows; k++)
                {
                    double factor = augmentedMatrice.Matrice[k, i] / augmentedMatrice.Matrice[i, i];
                    for (int j = i; j < cols; j++)
                    {
                        augmentedMatrice.Matrice[k, j] -= factor * augmentedMatrice.Matrice[i, j];
                    }
                }
            }

            // Résolution des inconnus à partir de la matrice triangulaire obtenue
            Matrix2D matriceResolueGauss = new Matrix2D("Solution matrice avec Gauss", rows, 1);
            for (int i = rows - 1; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < cols; j++)
                {
                    sum += augmentedMatrice.Matrice[i, j] * matriceResolueGauss.Matrice[j, 0];
                }
                matriceResolueGauss.Matrice[i, 0] = (augmentedMatrice.Matrice[i, cols - 1] - sum) / augmentedMatrice.Matrice[i, i];
            }

            return matriceResolueGauss;
        }

        // Méthode pour afficher l'équation de la matrice de facon propre
        public override string ToString()
        {
            // Vérifier la validité
            if (!IsValid())
            {
                return "Non valide pour l'affichage de l'équation";
            }

            int rows = MatriceUN.Matrice.GetLength(0);
            int cols = MatriceUN.Matrice.GetLength(1);

            StringBuilder equationString = new();

            // Parcourir les lignes de la matrice MatriceUN
            for (int i = 0; i < rows; i++)
            {
                // Parcourir les colonnes de la matrice MatriceUN
                for (int j = 0; j < cols - 1; j++)
                {
                    // Ajouter les termes des équations, sauf le dernier
                    equationString.Append($"{MatriceUN.Matrice[i, j].ToString("0.##")}x{j + 1} + ");
                }

                // Ajouter le dernier terme de l'équation avec le résultat
                equationString.Append($"{MatriceUN.Matrice[i, cols - 1]}x{cols} = {MatriceDEUX.Matrice[i, 0]}\n");
            }

            return equationString.ToString();
        }

    }
}
