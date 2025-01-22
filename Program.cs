using System;
using System.Drawing;
using System.Text;
using System.IO;
using System.Collections.Generic;

// Programme qui permet de cacher et retrouver des messages dans une image
class Program
{
    // Noms des fichiers par défaut
    private const string DEFAULT_IMAGE = "image.png";          // Image de départ
    private const string DEFAULT_CRYPTED_IMAGE = "image_cryptee.png";  // Image avec le message caché
    private const string DEFAULT_MESSAGE = "La cryptographie est l'art de protéger des informations en les transformant de manière à les rendre illisibles sans une clé secrète. Ce message est caché dans cette image !";

    // Démarre le programme et affiche le menu principal
    static void Main(string[] args)
    {
        bool continuer = true;
        while (continuer)
        {
            Console.Clear();
            AfficherMenu();

            string choix = Console.ReadLine();
            switch (choix)
            {
                case "1":
                    CrypterImage();     // Option 1: Cacher un message
                    break;
                case "2":
                    DecrypterImage();   // Option 2: Retrouver le message
                    break;
                case "3":
                    continuer = false;   // Option 3: Quitter
                    break;
                default:
                    Console.WriteLine("\nOption invalide. Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    // Affiche les options disponibles
    static void AfficherMenu()
    {
        Console.WriteLine("=== Programme de Cryptographie ===");
        Console.WriteLine($"Image par défaut : {DEFAULT_IMAGE}");
        Console.WriteLine($"Image cryptée par défaut : {DEFAULT_CRYPTED_IMAGE}");
        Console.WriteLine("\n1. Crypter une image");
        Console.WriteLine("2. Décrypter une image");
        Console.WriteLine("3. Quitter");
        Console.Write("\nChoisissez une option (1-3) : ");
    }

    // Cache un message dans l'image en modifiant légèrement ses pixels
    static void CrypterImage()
    {
        Console.Clear();
        Console.WriteLine("=== Cryptage d'Image ===\n");

        try
        {
            // Demande le nom de l'image ou utilise celle par défaut
            Console.Write($"Entrez le nom de l'image (appuyez sur Entrée pour utiliser {DEFAULT_IMAGE}): ");
            string imagePath = Console.ReadLine().Trim();

            if (string.IsNullOrWhiteSpace(imagePath))
            {
                imagePath = DEFAULT_IMAGE;
                Console.WriteLine($"Utilisation de l'image par défaut : {DEFAULT_IMAGE}");
            }

            // Vérifie si l'image existe
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException($"L'image '{imagePath}' n'existe pas dans le dossier du projet.");
            }

            // Demande le message ou utilise celui par défaut
            Console.Write("\nEntrez le message à crypter (appuyez sur Entrée pour utiliser le message par défaut): ");
            string messageToHide = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(messageToHide))
            {
                messageToHide = DEFAULT_MESSAGE;
                Console.WriteLine($"Utilisation du message par défaut : {DEFAULT_MESSAGE}");
            }

            // Ouvre l'image et prépare la nouvelle image
            using (Bitmap sourceBmp = new Bitmap(imagePath))
            using (Bitmap resultBmp = new Bitmap(sourceBmp.Width, sourceBmp.Height))
            {
                Console.WriteLine($"\nTraitement en cours...");
                int messageLength = messageToHide.Length;
                int currentCharIndex = 0;

                // Pour chaque pixel de l'image
                for (int x = 0; x < sourceBmp.Width; x++)
                {
                    for (int y = 0; y < sourceBmp.Height; y++)
                    {
                        // Prend un pixel et un caractère du message
                        Color sourcePixel = sourceBmp.GetPixel(x, y);
                        char currentChar = messageToHide[currentCharIndex];
                        int asciiValue = (int)currentChar;

                        // Modifie les couleurs du pixel avec le code ASCII du caractère
                        int newR = (sourcePixel.R + asciiValue) % 255;
                        int newG = (sourcePixel.G + asciiValue) % 255;
                        int newB = (sourcePixel.B + asciiValue) % 255;

                        // Enregistre le nouveau pixel
                        resultBmp.SetPixel(x, y, Color.FromArgb(sourcePixel.A, newR, newG, newB));
                        currentCharIndex = (currentCharIndex + 1) % messageLength;  // Passe au caractère suivant
                    }
                }

                // Sauvegarde la nouvelle image
                resultBmp.Save(DEFAULT_CRYPTED_IMAGE);

                // Affiche un résumé
                Console.WriteLine("\nCryptage terminé !");
                Console.WriteLine($"Image source : {imagePath}");
                Console.WriteLine($"Image cryptée sauvegardée : {DEFAULT_CRYPTED_IMAGE}");
                Console.WriteLine($"Message crypté : {messageToHide}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErreur : {ex.Message}");
        }

        Console.WriteLine("\nAppuyez sur une touche pour retourner au menu...");
        Console.ReadKey();
    }

    // Retrouve le message caché en comparant l'image originale et l'image cryptée
    static void DecrypterImage()
    {
        Console.Clear();
        Console.WriteLine("=== Décryptage d'Image ===\n");

        try
        {
            // Demande les noms des images ou utilise ceux par défaut
            Console.Write($"Entrez le nom de l'image originale (appuyez sur Entrée pour utiliser {DEFAULT_IMAGE}): ");
            string originalImagePath = Console.ReadLine().Trim();
            if (string.IsNullOrWhiteSpace(originalImagePath))
            {
                originalImagePath = DEFAULT_IMAGE;
                Console.WriteLine($"Utilisation de l'image originale par défaut : {DEFAULT_IMAGE}");
            }

            Console.Write($"Entrez le nom de l'image cryptée (appuyez sur Entrée pour utiliser {DEFAULT_CRYPTED_IMAGE}): ");
            string encryptedImagePath = Console.ReadLine().Trim();
            if (string.IsNullOrWhiteSpace(encryptedImagePath))
            {
                encryptedImagePath = DEFAULT_CRYPTED_IMAGE;
                Console.WriteLine($"Utilisation de l'image cryptée par défaut : {DEFAULT_CRYPTED_IMAGE}");
            }

            // Vérifie si les images existent
            if (!File.Exists(originalImagePath))
            {
                throw new FileNotFoundException($"L'image originale '{originalImagePath}' n'existe pas.");
            }
            if (!File.Exists(encryptedImagePath))
            {
                throw new FileNotFoundException($"L'image cryptée '{encryptedImagePath}' n'existe pas.");
            }

            // Ouvre les deux images
            using (Bitmap originalBmp = new Bitmap(originalImagePath))
            using (Bitmap encryptedBmp = new Bitmap(encryptedImagePath))
            {
                // Vérifie que les images ont la même taille
                if (originalBmp.Width != encryptedBmp.Width || originalBmp.Height != encryptedBmp.Height)
                {
                    throw new Exception("Les deux images doivent avoir les mêmes dimensions.");
                }

                // Liste pour stocker les caractères trouvés
                List<char> foundChars = new List<char>();

                // Compare chaque pixel des deux images
                Console.WriteLine("\nAnalyse des différences...");
                for (int x = 0; x < originalBmp.Width; x++)
                {
                    for (int y = 0; y < originalBmp.Height; y++)
                    {
                        Color originalPixel = originalBmp.GetPixel(x, y);
                        Color encryptedPixel = encryptedBmp.GetPixel(x, y);

                        // Calcule la différence pour chaque couleur
                        int diffR = (encryptedPixel.R - originalPixel.R + 255) % 255;
                        int diffG = (encryptedPixel.G - originalPixel.G + 255) % 255;
                        int diffB = (encryptedPixel.B - originalPixel.B + 255) % 255;

                        // Si toutes les différences sont égales, on a trouvé un caractère
                        if (diffR == diffG && diffG == diffB && diffR > 0)
                        {
                            char recoveredChar = (char)diffR;
                            if (char.IsLetterOrDigit(recoveredChar) ||
                                char.IsPunctuation(recoveredChar) ||
                                char.IsWhiteSpace(recoveredChar))
                            {
                                foundChars.Add(recoveredChar);
                            }
                        }
                    }
                }

                // Affiche le résultat
                Console.WriteLine("\nRésultats du décryptage :");
                Console.WriteLine("---------------------------");

                if (foundChars.Count > 0)
                {
                    // Convertit les caractères en texte et trouve le message unique
                    string fullMessage = new string(foundChars.ToArray());
                    string uniqueMessage = TrouverMotifRepete(fullMessage);

                    Console.WriteLine($"\nMessage trouvé : {uniqueMessage}");
                    Console.WriteLine($"Longueur du message : {uniqueMessage.Length} caractères");
                }
                else
                {
                    Console.WriteLine("Aucun message valide n'a été trouvé.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErreur : {ex.Message}");
        }

        Console.WriteLine("\nAppuyez sur une touche pour retourner au menu...");
        Console.ReadKey();
    }

    // Cherche la partie du message qui se répète
    static string TrouverMotifRepete(string texte)
    {
        if (string.IsNullOrEmpty(texte)) return texte;

        // Teste différentes longueurs de texte
        for (int longueurFenetre = 5; longueurFenetre <= texte.Length / 2; longueurFenetre++)
        {
            // Prend une partie du début du texte
            string sousChaine = texte.Substring(0, longueurFenetre);

            // Compte combien de fois on trouve cette partie
            int occurrences = CompterOccurrences(texte, sousChaine);

            // Si on trouve la partie plusieurs fois
            if (occurrences > 1)
            {
                // Vérifie si elle se répète juste après
                int prochainIndex = texte.IndexOf(sousChaine, longueurFenetre);
                if (prochainIndex == longueurFenetre)
                {
                    // On a trouvé le message qui se répète
                    Console.WriteLine($"\nDétection d'un motif qui se répète {occurrences} fois");
                    return sousChaine;
                }
            }
        }

        // Si on ne trouve pas de répétition, prend le début du texte
        int longueurRaisonnable = Math.Min(texte.Length, 200);
        return texte.Substring(0, longueurRaisonnable);
    }

    // Compte combien de fois on trouve une partie dans le texte
    static int CompterOccurrences(string texte, string sousChaine)
    {
        if (string.IsNullOrEmpty(sousChaine)) return 0;

        int count = 0;
        int position = 0;

        // Cherche toutes les occurrences
        while ((position = texte.IndexOf(sousChaine, position)) != -1)
        {
            count++;
            position += 1;
        }

        return count;
    }
}