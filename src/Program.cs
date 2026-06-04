using PIF1006_tp2;
using System;

//************************************************************************
//*                                Yannick Poirier                       *
//*                                 POIY04109403                         *
//************************************************************************

namespace PIF1006_tp2
{

    class Program
    {
        static void Main(string[] args)
        {

            // Je créer localement une matrice ici. Possible d'adapter pour un fichier JSON à la place 
            Matrix2D MatriceUN = new Matrix2D("Première matrice", 3, 3); // on peut changer le nombre de lignes et colonnes de la matriceUN ici
            Matrix2D MatriceDEUX = new Matrix2D("Deuxième matrice", 3, 3); // on peut changer le nombre de lignes et colonnes de la matriceDEUX ici

            // Insére des valeurs dans MatriceUN
            MatriceUN.SetValue(0, 0, 1);
            MatriceUN.SetValue(0, 1, 7);
            MatriceUN.SetValue(0, 2, 3);
            MatriceUN.SetValue(1, 0, 4);
            MatriceUN.SetValue(1, 1, 1);
            MatriceUN.SetValue(1, 2, 2);
            MatriceUN.SetValue(2, 0, 8);
            MatriceUN.SetValue(2, 1, 2);
            MatriceUN.SetValue(2, 2, 5);

            // Insére des valeurs dans MatriceDEUX
            MatriceDEUX.SetValue(0, 0, 5);
            MatriceDEUX.SetValue(0, 1, 7);
            MatriceDEUX.SetValue(0, 2, 8);
            MatriceDEUX.SetValue(1, 0, 2);
            MatriceDEUX.SetValue(1, 1, 3);
            MatriceDEUX.SetValue(1, 2, 9);
            MatriceDEUX.SetValue(2, 0, 4);
            MatriceDEUX.SetValue(2, 1, 3);
            MatriceDEUX.SetValue(2, 2, 5);

            System system = new System(MatriceUN, MatriceDEUX);

            // Résolution de la matrice selon les trois méthodes (Cramer, Gauss et inverse)
            Matrix2D matriceResolue;
            matriceResolue = system.SolveUsingCramer();
            matriceResolue = system.SolveUsingGauss();
            matriceResolue = system.SolveUsingInverseMatrix();

            bool quitter = false;

            // Ici commence la boucle principale du menu
            while (!quitter)
            {
                // Affichage des options du menu
                Console.WriteLine("Bienvenue dans le menu de l'utilisateur :");
                Console.WriteLine("\n Merci de bien vouloir choisir une option :");
                Console.WriteLine("1- Quitter le menu");
                Console.WriteLine("2- Afficher le système");
                Console.WriteLine("3- Résoudre avec Cramer");
                Console.WriteLine("4- Résoudre avec la méthode de la matrice inverse");
                Console.WriteLine("5- Résoudre avec Gauss");
                Console.WriteLine("6- Résoudre\n");

                int choix;
                if (!int.TryParse(Console.ReadLine(), out choix))
                {
                    Console.WriteLine("\n Veuillez saisir un numéro valide. Les options possibles sont inclusivement de 1 à 6.");
                    continue;
                }

                switch (choix)
                {
                    case 1:
                        // Quitter le menu
                        quitter = true;
                        break;

                        // Afficher les matrices
                    case 2:
                        Console.WriteLine("Sélectionnez une matrice (UN ou DEUX) pour afficher:");
                        int selectedMatrice;
                        if (system.IsValid())
                        {
                            if (int.TryParse(Console.ReadLine(), out selectedMatrice) && (selectedMatrice == 1 || selectedMatrice == 2))
                            {
                                Console.WriteLine(selectedMatrice == 1 ? MatriceUN.ToString() : MatriceDEUX.ToString());
                            }
                            else
                            {
                                Console.WriteLine("Matrice non valide");
                            }
                        }
                        break;

                    case 3:
                        // Résoudre avec Cramer
                        if (system.IsValid())
                        {
                            var solution = system.SolveUsingCramer();
                            AfficherSolution(solution);
                        }
                        else
                        {
                            Console.WriteLine("Le système n'est pas valide pour la résolution avec Cramer");
                        }
                        break;

                    case 4:
                        // Résoudre avec la matrice inverse
                        if (system.IsValid())
                        {
                            var solution = system.SolveUsingInverseMatrix();
                            AfficherSolution(solution);
                        }
                        else
                        {
                            Console.WriteLine("Le système n'est pas valide pour la résolution avec la méthode de la matrice inverse");
                        }
                        break;

                    case 5:
                        // Résoudre avec Gauss
                        if (system.IsValid())
                        {
                            var solution = system.SolveUsingGauss();
                            AfficherSolution(solution);
                        }
                        else
                        {
                            Console.WriteLine("Le système n'est pas valide pour la résolution avec Gauss");
                        }
                        break;

                    case 6:
                        // Afficher l'équation
                        Console.WriteLine(system.ToString());
                        break;

                    default:
                        Console.WriteLine("Option non valide. Merci de bien vouloir choisir à nouveau!");
                        break;
                }
            }
        }

        // Méthode pour afficher la solution
        static void AfficherSolution(Matrix2D solution)
        {
            if (solution != null)
            {
                Console.WriteLine("La solution est :");
                Console.WriteLine(solution);
            }
            else
            {
                Console.WriteLine("Aucune solution trouvée :'(");
            }
        }
    }
}

