using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

//************************************************************************
//*                                Yannick Poirier                       *
//*                                 POIY04109403                         *
//************************************************************************

namespace PIF1006_tp2
{
    public class Matrix2D
    {
        public double[,] Matrice { get; private set; }
        public string Name { get; private set; }

        public Matrix2D(string name, int lines, int columns)
        {
            Matrice = new double[lines, columns];
            Name = name;

        }

        // Méthode pour assigner une valeur à un élément de la matrice à defaut de ne pas avoir de fichier JSON
        public void SetValue(int row, int column, double value)
        {
            if (row >= 0 && row < Matrice.GetLength(0) && column >= 0 && column < Matrice.GetLength(1))
            {
                Matrice[row, column] = value;
            }
        }

        // Méthode pour transposé la matrice
        public Matrix2D Transpose()
        {
            // Iici c'est pour obtenir le nombre de lignes et de colonnes de la matrice
            int rows = Matrice.GetLength(0);
            int cols = Matrice.GetLength(1);

            // Créer une nouvelle matrice pour stocker la transposée
            Matrix2D transposedMatrix = new Matrix2D($"{Name} transposée", cols, rows);

            // Parcourir chaque élément de la matrice actuelle pour pouvoir effectuer la transposée
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // Swap les éléments pour obtenir la transposée
                    transposedMatrix.Matrice[j, i] = Matrice[i, j];
                }
            }

            return transposedMatrix;
        }

        // Méthode pour vérifier si la matrice est carrée
        public bool IsSquare()
        {
            // Récupere le nombre de lignes et de colonnes de la matrice
            int rows = Matrice.GetLength(0);
            int cols = Matrice.GetLength(1);

            // Vérifier si le nombre de lignes est égal au nombre de colonnes
            // Si c'est le cas, la matrice est carrée, dans le cas inverse, la matrice n'est pas carrée
            return rows == cols;
        }

        // Méthode pour calculer le déterminant de la matrice
        public double Determinant()
        {
            if (!IsSquare()) // On s'assure que la matrice est carrée
            {
                throw new InvalidOperationException("il faut que la matrice soit carrée afin de pouvoir calculer le déterminant.");
            }

            int dimension = Matrice.GetLength(0);

            if (dimension == 1)
            {
                // Pour une matrice 1x1, le déterminant est leseul élément de la matrice
                return Matrice[0, 0];
            }
            else if (dimension == 2)
            {
                // Pour une matrice 2x2, le calcul du déterminant peut se faire selon la formule ad-bc
                return Matrice[0, 0] * Matrice[1, 1] - Matrice[0, 1] * Matrice[1, 0];
            }
            else
            {
                double determinant = 0;

                // Calcul récursif du déterminant pour les matrices de taille supérieure à 2x2
                for (int i = 0; i < dimension; i++)
                {
                    determinant += Matrice[0, i] * Minor(0, i).Determinant() * (i % 2 == 0 ? 1 : -1);
                }

                return determinant;
            }
        }

        // Il faut une méthode pour obtenir le mineur si on veut calculer une matrice plus élevé que 2x2
        private Matrix2D Minor(int row, int col)
        {
            int dimension = Matrice.GetLength(0);
            Matrix2D minor = new Matrix2D($"{Name} minor", dimension - 1, dimension - 1);

            int minorRow = 0;
            int minorCol = 0;

            // Parcours de la matrice d'origine pour construire le mineur
            for (int i = 0; i < dimension; i++)
            {
                if (i != row)  // Exclure la ligne donnée pour le mineur
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        if (j != col)  // Exclure la colonne donnée pour le mineur
                        {
                            // Remplissage du mineur en sautant la ligne et la colonne trouver auparavant
                            minor.Matrice[minorRow, minorCol] = Matrice[i, j];
                            minorCol++;
                        }
                    }
                    minorRow++;
                    minorCol = 0;
                }
            }

            return minor;  // Retourne le mineur de la matrice
        }
    

    // Méthode pour calculer la comatrice de la matrice
    public Matrix2D Comatrix()
    {
            // On s'assure que la matrice est carrée
            if (!IsSquare())
            {
                throw new InvalidOperationException("La comatrice est possible uniquement pour les matrices carrées. Cette matrice n'est pas carrée.");
            }

            int dimension = Matrice.GetLength(0);
            Matrix2D comatrix = new Matrix2D($"{Name} comatrice", dimension, dimension);

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    // Calcul de la comatrice en utilisant les cofacteurs et en obtenant le mineur pour calculer son déterminant
                    double minorDeterminant = Minor(i, j).Determinant();

                    // On utilise le facteur (-1)^(i+j) pour obtenir le cofacteur
                    comatrix.Matrice[i, j] = minorDeterminant * ((i + j) % 2 == 0 ? 1 : -1);
                }
            }

            return comatrix; 
    }

        // Méthode pour calculer l'inverse de la matrice
        public Matrix2D Inverse()
        {
            if (!IsSquare()) // La matrice doit être carrée pour qu'un inverse soit possible
            {
                return null;
            }

            double determinant = Determinant(); // On va chercher la valeur du déterminant 

            if (determinant == 0)
            {
                // Si le déterminant est nul, l'inverse n'existe pas
                return null;
            }

            int dimension = Matrice.GetLength(0);
            Matrix2D inverse = new Matrix2D($"{Name} inverse", dimension, dimension);

            Matrix2D comatrix = Comatrix(); // On va chercher la comatrice

            // Le calcul de la matrice inverse est = A^-1 = (1 / det(A)) * adj(A) donc :
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    inverse.Matrice[i, j] = comatrix.Matrice[i, j] / determinant;
                }
            }

            return inverse;
        }

        // Méthode pour obtenir une représentation propre de la matrice
        public override string ToString()
        {
            StringBuilder matriceString = new();
            matriceString.AppendLine($"{Name}:"); // Ajout du nom de la matrice 

            // Vérification de la conformité de la matrice
            if (Matrice == null || Matrice.GetLength(0) == 0 || Matrice.GetLength(1) == 0)
            {
                matriceString.AppendLine("Matrice non conforme."); 
                return matriceString.ToString();
            }

            int rows = Matrice.GetLength(0);
            int columns = Matrice.GetLength(1);

            // Parcours les lignes 
            for (int i = 0; i < rows; i++)
            {
                matriceString.Append("| "); // Début d'une ligne de la matrice

                for (int j = 0; j < columns; j++)
                {
                    matriceString.Append($"{Matrice[i, j]:F2} "); // Formater les valeurs avec deux chiffres après la virgule
                }

                matriceString.AppendLine("|"); // Fin d'une ligne de la matrice
            }

            return matriceString.ToString();
        }

        // Méthode pour la multiplication de matrice
        public Matrix2D Multiply(Matrix2D otherMatrice)
        {
            // Obtention des dimensions des matrices
            int rowsA = Matrice.GetLength(0); // Nombre de lignes de la matrice actuelle (A)
            int colsA = Matrice.GetLength(1); // Nombre de colonnes de la matrice actuelle (A)
            int rowsB = otherMatrice.Matrice.GetLength(0); // Nombre de lignes de l'autre matrice (B)
            int colsB = otherMatrice.Matrice.GetLength(1); // Nombre de colonnes de l'autre matrice (B)

            // Vérification si les dimensions permettent la multiplication de matrices
            if (colsA != rowsB)
            {
                throw new InvalidOperationException("Les dimensions des matrices ne permettent pas la multiplication.");
            }

            // Création de la matrice résultante avec les bonnes dimensions
            Matrix2D matriceMultiplie = new Matrix2D($"{Name} multiplier", rowsA, colsB);

            // Boucles pour calculer chaque élément de la matrice résultante
            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsB; j++)
                {
                    double sum = 0;
                    // Calcul de chaque élément de la matrice résultante
                    for (int k = 0; k < colsA; k++)
                    {
                        // Multiplication et aussi la sommation des produits des éléments qui sont correspondants
                        sum += Matrice[i, k] * otherMatrice.Matrice[k, j];
                    }
                    // Assignation de la valeur calculée à la position correspondante dans la matrice résultante (matriceMultiplie)
                    matriceMultiplie.Matrice[i, j] = sum;
                }
            }

            return matriceMultiplie;
        }
    }
}
