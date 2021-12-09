using pyramid.Helpers;
using pyramid.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pyramid
{
    class Program
    {

        static void Main(string[] args)
        {
            List<User> finalList = new List<User>();

            if(args != null)
            {
                if (args[0] != null && File.Exists(Path.Combine(args[0])))
                {
                    List<User> values = File.ReadAllLines(Path.Combine(args[0].ToString()))
                        .Skip(1)
                        .Select(v => v.StringToUser())
                        .ToList();

                    Console.WriteLine("File opened and converted to a list.");

                    foreach (var value in values)
                    {
                        Console.WriteLine($"Working on ID {value.ID}...");
                        GetListByUser(values, value.ID, ref finalList, MasterSponsor: value.ID);
                    }

                    Console.WriteLine("Gathered values, started to write on the csv.");

                    var splittedLists = SplitList(finalList, 100000).ToList();

                    var filenumber = 1;

                    foreach (var list in splittedLists)
                    {
                        using (var file = File.CreateText(Path.GetDirectoryName(args[0]) + $"\\output{filenumber}.csv"))
                        {
                            file.WriteLine($"{nameof(User.ID)},{nameof(User.Sponsor)},{nameof(User.Level)},{nameof(User.MasterSponsor)}");
                            foreach (var line in list)
                            {
                                file.WriteLine($"{line.ID},{line.Sponsor},{line.Level},{line.MasterSponsor}");
                            }
                        }
                        filenumber++;
                    }

                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Error en parametros");
                }
            }

            else
            {
                Console.WriteLine("Faltan parametros");
            }

        }

        private static void GetListByUser(List<User> values, int CurrentSponsor, ref List<User> finalList, int level = 0, int MasterSponsor = 0)
        {

            level++;

            var listForLevel = values.Where(x => x.Sponsor == CurrentSponsor).ToList();

            foreach (var value in listForLevel)
            {
                value.MasterSponsor = MasterSponsor;
                value.Level = level;
                finalList.Add(value);

                GetListByUser(values, value.ID, ref finalList, level, MasterSponsor);
            }
        }

        public static IEnumerable<List<T>> SplitList<T>(List<T> locations, int nSize = 30)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }
    }
}
