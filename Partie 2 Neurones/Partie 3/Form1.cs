﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Partie_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static Bitmap bmp;
         Random rnd = new Random();

        Reseau reseau;

        private void button1_Click(object sender, EventArgs e)
        {
            // Initialisation d'un réseau de neurones avec le nombre d'entrées, 
            // le nombre de couches et le nbre de neurones par couches
            reseau = new Reseau(Convert.ToInt32(textBoxnbentrees.Text),
                                        Convert.ToInt32(textBoxnbcouches.Text),
                                        Convert.ToInt32(textBoxnbneurcouche.Text));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // En entrée on a une liste de k valeurs réelles correspondant aux k neurones
            // de la 1ère couche dite couche des entrées ou entrées tout court
            // On dispose en général d'une base de données de vecteurs d'entrées
            // c'est pour cela qu'on a une liste de vecteurs, donc une liste de liste 
            List<List<double>> lvecteursentrees = new List<List<double>>();
            List<List<double>> lve = new List<List<double>>();

            // On ouvre le fichier de données en lecture
            StreamReader reader = new StreamReader("..//..//datasetclassif.txt");


            // On a 1 seule sortie associée à chaque vecteur d'entrée
            // donc on a seulement 1 liste de réels
            // Attention, on suppose ici que le nième élément de cette liste est
            // la sortie désirée du nième vecteur de levecteurentrees
             List<double> lsortiesdesirees = new List<double>();
             List<double> ls = new List<double>();

            // Initialisation des coordonnées du point
             double x = 0;
             double y = 0;

             for (int i = 0; i < 3000; i++)
             {
                     // On lit les lignes 3 par 3 pour créer les couples de données
                 for (int j = 0; j < 3; j++)
                 {
                     string line = reader.ReadLine();

                     if (j == 1)
                         x = double.Parse(line);
                     else if (j == 2)
                         y = double.Parse(line);
                 }

                 List<double> vect = new List<double>();

                 // Ajout des valeurs au vecteur d'entrée
                 vect.Add(x/800.0);
                 vect.Add(y/800.0);
                 lve.Add(vect);

                 //Création des sorties désirées
                 if (i < 1500)
                     ls.Add(0.2);
                 else
                     ls.Add(0.8);
             }


             for (int i = 0; i < 3000; i++)
             {
                 int k = rnd.Next( lve.Count());
                 lvecteursentrees.Add(lve[k]);
                 lve.RemoveAt(k);
                 lsortiesdesirees.Add(ls[k]);
                 ls.RemoveAt(k);

             }

             reseau.backprop(lvecteursentrees, lsortiesdesirees,
                                Convert.ToDouble(textBoxalpha.Text),
                                Convert.ToInt32(iterations.Text));
             Affichage affichage = new Affichage(lvecteursentrees, lsortiesdesirees, reseau);
             affichage.Show();

             reader.Close();
             /*Tests( g, bmp);
             pictureBox1.Invalidate();*/
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*bmp = (Bitmap)pictureBox1.Image;*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*listBox1.Items.Clear();
            reseau.AfficheInfoNeurone(Convert.ToInt32(textBoxnumcouche.Text),
                                       Convert.ToInt32(textBoxnumneur.Text),
                                       listBox1);*/

        }
        /*****************************************************************/
        // Attention, la fonction à apprendre doit fournir des valeurs entre 0 et 1 !!!
        double fonctionmodele(double x)
        {
            // return Math.Sin(x * 20) / 2.5 + 0.5;
            if (x < 0.2 || x > 0.8) return 0.8;
            else return 0.2;
        }

        /**********************************************************************/
        public void Tests(Graphics g, Bitmap bmp)
        {
            int x, z, zdesire;
            double x2, z2;
            for (x = 0; x < bmp.Width; x++)
                for (z = 0; z < bmp.Height; z++)
                    bmp.SetPixel(x, z, Color.Black);

            List<List<double>> lvecteursentrees = new List<List<double>>();
            List<double> lsortiesdesirees = new List<double>();
            List<double> lsortiesobtenues; 

            // EN général, on reprend ici les données récupérées du fichier base de données
            // mais pour illustrer le fonctionnement, on se propose ici de tester 200 valeurs
            // de x (dimension 1 pour les entrées ici) entre 0 et 1, ramenées entre 0 et 200
            // idem pour la sortie, pour permettre l'affichage dans une image.
            for (x = 0; x < 200; x++)
            {
                x2 = x /200.0;
                // Initialisation des activations  ai correspondant aux entrées xi
                // Le premier neurone est une constante égale à 1
                List<double> vect = new List<double>();
                vect.Add(x2); // Une seule valeur ici pour ce vecteur 
                lvecteursentrees.Add(vect);
                lsortiesdesirees.Add( fonctionmodele(x2) );
            }

            lsortiesobtenues = reseau.ResultatsEnSortie( lvecteursentrees );
            
            // Affichage
             for (x = 0; x < 200; x++)
             {
                 z2 = lsortiesobtenues[x];
                
                // z2 valeur attendu entre 0 et 1 ; conversion pour z qui est retenu pour l'affichage
                 z = (int)(z2 * 200);
                 zdesire = (int)(lsortiesdesirees[x] * 200);
                bmp.SetPixel(x, bmp.Height - z - 1, Color.Yellow);
                
                bmp.SetPixel(x, bmp.Height - zdesire - 1, Color.White);
            }

        }

        private void textBoxnbiter_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
        
    }
}
