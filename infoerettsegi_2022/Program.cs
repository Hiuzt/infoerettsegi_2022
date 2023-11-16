using System;
using System.Formats.Asn1;
using System.Text;

namespace infoerettsegi_2022
{

    public class Sites
    {
        public int Owner { get; set; }

        public string StreetName {get; set; }
        public string Address { get; set; }
        public char Class { get; set; }
        public int Area { get; set; }

        public Sites(int Owner, string StreetName, string Address, char Class, int Area)
        {
            this.Owner = Owner;
            this.StreetName = StreetName;
            this.Address = Address;
            this.Class = Class;
            this.Area = Area;

        }
    }

    internal class Program
    {

        private static List<Sites> siteList = new List<Sites>();
        private static int[] taxList = new int[3];

        static void Main(string[] args)
        {
            Task1();
            Task2();
            Task3();
            Task5();
            Task6();
            Task7();

            Console.ReadKey();
        }

        public static void Task7()
        {
            using (StreamWriter newFile = new StreamWriter("fizetendo.txt"))
            {
                var groupedByOwner = siteList.GroupBy(area => area.Owner).Select(group => group.ToList()).ToList();

                foreach (var group in groupedByOwner)
                {
                    int taxAmmount = 0;
                    int ownerValue = 0;
                    foreach (Sites site in group)
                    {
                        ownerValue = site.Owner;
                        taxAmmount += taxValue(site.Class, site.Area);
                    }
                    newFile.WriteLine("{0} {1}", ownerValue, taxAmmount);
                }

                newFile.Close();
            }
        }
        

        public static void Task6()
        {
            Console.WriteLine("6. feladat. ");

            var groupedByStreet = siteList.GroupBy(area => area.StreetName).Select(group => group.ToList()).ToList();
            
            foreach(var group in groupedByStreet)
            {
                foreach (Sites site in group)
                {
                    char firstClass = group[0].Class;

                    if (firstClass != site.Class)
                    {
                        Console.WriteLine(site.StreetName);
                        break;
                    }
                    
                }
            }
        }

        public static void Task5()
        {
            Console.WriteLine("5. feladat. ");

            int[,] newArray = { {0, 0 }, {0, 0}, {0, 0} };



            foreach (Sites site in siteList)
            {
                int characterIndex = Convert.ToInt32(Convert.ToByte(site.Class)) - 65;
                int currentTax = taxValue(site.Class, site.Area);

                if (currentTax < 10000)
                {
                    currentTax = 0;
                }

                newArray[characterIndex, 0]++;
                newArray[characterIndex, 1] += currentTax;
            }

            for (int i = 0; i < 3; i++)
            {
                char className = Convert.ToChar((Byte)i + 65);

                Console.WriteLine("{0} sávba {1} telek esik, az adó {2} Ft", className, newArray[i, 0], newArray[i, 1]);
            }
        }

        public static int taxValue(char taxClass, int areaValue)
        {
            int characterIndex = Convert.ToInt32(Convert.ToByte(taxClass)) - 65;

            

            return (areaValue * taxList[characterIndex]);
        }

        public static void Task3()
        {
            Console.Write("3. feladat. ");
            Console.Write("Egy tulajdonos adószáma: ");

            int ownerValue = int.Parse(Console.ReadLine());
            bool ownerFound = false;

            foreach (Sites site in siteList)
            {
                if (site.Owner == ownerValue)
                {
                    ownerFound = true;
                    Console.WriteLine("{0} utca {1}", site.StreetName, site.Address);
                }
            }
            
            if (!ownerFound)
            {
                Console.WriteLine("Nem szerepel az adatállományban!");
            }
            
        }

        public static void Task2()
        {
            Console.Write("2. feladat. ");

            Console.WriteLine("A mintában {0} telek szerepel.", siteList.Count);
        }

        public static void Task1()
        {
            using (StreamReader openedFile = new StreamReader("utca.txt"))
            {
                string[] taxString = openedFile.ReadLine().Split(" "); // 1. sor

                for (int i = 0; i < taxString.Length; i++)
                {
                    taxList[i] = Convert.ToInt32(taxString[i]);
                }

                while (!openedFile.EndOfStream)
                {
                    string[] splitString = openedFile.ReadLine().Split(" ");

                    siteList.Add(new Sites(Convert.ToInt32(splitString[0]), splitString[1], splitString[2], Convert.ToChar(splitString[3]), Convert.ToInt32(splitString[4])));
                }
            }
        }
    }
}