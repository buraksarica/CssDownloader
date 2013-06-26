using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CssDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage :  ");
                Console.WriteLine("cssd http://example.com/styles.css");
                return;
            }
            // No options. Just URL of the css. 
            string cssFile = args[0];

            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                Uri furi = new Uri(cssFile);
                string content = wc.DownloadString(furi);
                Console.WriteLine("File downloaded.. Looking for url() references to download.");
                DirectoryInfo di = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, furi.Segments[furi.Segments.Length - 1]));
                UriBuilder ub = new UriBuilder();
                ub.Host = furi.Host;
                ub.Scheme = furi.Scheme;
                System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex("url(\\((.*?)\\))");
                MatchCollection matches = rg.Matches(content);
                int i = 0;
                foreach (Match item in matches)
                {
                    string url = item.Groups[2].Value;
                    ub.Path = url;
                    wc.DownloadFile(ub.Uri, System.IO.Path.Combine(di.FullName, ub.Uri.Segments[ub.Uri.Segments.Length - 1]));
                    content = content.Replace(url, ub.Uri.Segments[ub.Uri.Segments.Length - 1]);
                    DrawProgressBar(i, matches.Count, 40, '=');
                    i++;
                }
                wc.Dispose();

                DrawProgressBar(matches.Count, matches.Count, 40, '=');
                Console.WriteLine("");
                Console.WriteLine("Complete!");
                System.Threading.Thread.Sleep(300);
                System.IO.File.WriteAllText(System.IO.Path.Combine(di.FullName, furi.Segments[furi.Segments.Length - 1]), content);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }
        }


        /// <summary>
        /// http://markmintoff.com/2012/09/c-console-progress-bar/ 
        /// </summary>
        /// <param name="complete"></param>
        /// <param name="maxVal"></param>
        /// <param name="barSize"></param>
        /// <param name="progressCharacter"></param>
        private static void DrawProgressBar(int complete, int maxVal, int barSize, char progressCharacter)
        {
            Console.CursorVisible = false;
            int left = Console.CursorLeft;
            decimal perc = (decimal)complete / (decimal)maxVal;
            int chars = (int)Math.Floor(perc / ((decimal)1 / (decimal)barSize));
            string p1 = String.Empty, p2 = String.Empty;

            for (int i = 0; i < chars; i++) p1 += progressCharacter;
            for (int i = 0; i < barSize - chars; i++) p2 += progressCharacter;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(p1);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(p2);

            Console.ResetColor();
            Console.Write(" {0}%", (perc * 100).ToString("N2"));
            Console.CursorLeft = left;
        }
    }
}
