using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;


namespace Assignment1
{
    internal class Program
    {
        static List<Recording> LoadRecordings(string path)
        {
            var recordings = new List<Recording>();
            var composers = new Dictionary<string, Composer>();
            var pieces = new Dictionary<string, Piece>();

            foreach (var line in File.ReadLines(path))
            {
                var fields = line.Split(',');

                string productCode = fields[0];
                string lastName = fields[1];
                string firstName = fields[2];
                string title = fields[3];
                string catalogue = fields[4];

                // Создаем или находим композитора
                string composerKey = $"{firstName} {lastName}";
                if (!composers.ContainsKey(composerKey))
                {
                    composers[composerKey] = new Composer(firstName, lastName);
                }
                var composer = composers[composerKey];

                // Создаем или находим произведение
                if (!pieces.ContainsKey(title))
                {
                    pieces[title] = new Piece(title, composer, catalogue);
                }
                var piece = pieces[title];

                // Создаем запись и добавляем её в список
                recordings.Add(new Recording(piece, productCode));
            }

            return recordings;
        }


        static List<Purchase> LoadPurchases(string path)
        {
            var purchases = new List<Purchase>();
            string recordingsPath = path.Replace("purchases", "recordings");
            var recordings = LoadRecordings(recordingsPath);

            var recordingDict = recordings.ToDictionary(r => r.GetCode(), r => r);

            foreach (var line in File.ReadLines(path))
            {
                var fields = line.Split(',');

                string productCode = fields[0];
                double price = double.Parse(fields[1]);
                int amount = int.Parse(fields[2]);
                DateTime time = DateTime.Parse(fields[3]);

                if (recordingDict.TryGetValue(productCode, out Recording recording))
                {
                    purchases.Add(new Purchase(recording, price, amount, time));
                }
            }

            return purchases;
        }

        static List<string> GetAllTitles(string path)
        {
            var titles = new HashSet<string>();
            foreach (var line in File.ReadLines(path))
            {
                var fields = line.Split(',');
                titles.Add(fields[3]); // Название произведения
            }

            return titles.ToList();
        }


        static string FindMostPopularPiece(string path)
        {
            var purchases = LoadPurchases(path);
            var pieceSales = new Dictionary<string, int>();

            foreach (var purchase in purchases)
            {
                string pieceTitle = purchase.GetRecording().GetPiece().GetTitle();
                if (!pieceSales.ContainsKey(pieceTitle))
                {
                    pieceSales[pieceTitle] = 0;
                }
                pieceSales[pieceTitle] += purchase.GetAmount();
            }

            return pieceSales.OrderByDescending(p => p.Value).First().Key;
        }


        static string FindMostPopularComposer(string path)
        {
            var purchases = LoadPurchases(path);
            var composerSales = new Dictionary<string, int>();

            foreach (var purchase in purchases)
            {
                string composerName = purchase.GetRecording().GetPiece().GetComposer().GetName();
                if (!composerSales.ContainsKey(composerName))
                {
                    composerSales[composerName] = 0;
                }
                composerSales[composerName] += purchase.GetAmount();
            }

            return composerSales.OrderByDescending(c => c.Value).First().Key;
        }


        static string GetBestSellDay(string path)
        {
            var purchases = LoadPurchases(path);
            var daySales = new Dictionary<DayOfWeek, double>();

            foreach (var purchase in purchases)
            {
                DayOfWeek day = purchase.GetPurchaseDay().DayOfWeek;
                if (!daySales.ContainsKey(day))
                {
                    daySales[day] = 0.0;
                }
                daySales[day] += purchase.GetTotal();
            }

            return daySales.OrderByDescending(d => d.Value).First().Key.ToString();
        }


        static double GetAveragePiecePrice(string path, string title)
        {
            var purchases = LoadPurchases(path);
            double totalSum = 0.0;
            int totalCount = 0;

            foreach (var purchase in purchases)
            {
                string pieceTitle = purchase.GetRecording().GetPiece().GetTitle();
                if (pieceTitle == title)
                {
                    totalSum += purchase.GetTotal();
                    totalCount += purchase.GetAmount();
                }
            }

            return totalCount > 0 ? totalSum / totalCount : 0.0;
        }


        static void Main(string[] args)
        {
            StructureTest sTest = StructureTest.GetInstance();
            sTest.CheckClasses();
        }
    }
}