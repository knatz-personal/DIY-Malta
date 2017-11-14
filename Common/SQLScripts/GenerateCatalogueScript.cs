using System;

namespace Common.SQLScripts
{
    class GenerateCatalogueScript
    {
        /*
        void Main()
        {
            Console.WriteLine("--START");
            var catIDList = (from c in Entity.Categorys select c.ID);
            Random rand = new Random(100);
            int count = 1;
            foreach (var item in catIDList)
            {
                for (int i = 1; i <= 150; i++)
                {
                    var prodId = Guid.NewGuid();
                    Console.WriteLine("--Record " + count);
                    Console.Write("insert into Products");
                    Console.WriteLine("\t(ID, Name, Description, CategoryID, SaleID, VAT,  ImagePath,  Stock, Active)");
                    Console.WriteLine("\tvalues \n(");
                    Console.WriteLine("\t\t'" + prodId + "'");
                    Console.WriteLine("\t\t, '" + string.Format("{0:0000} Test", count) + "'");
                    Console.WriteLine("\t\t, '" + string.Format("{0:0000} Description lorem ipsum dolor", count) + "'");
                    Console.WriteLine("\t\t, " + item);
                    Console.WriteLine("\t\t, null, " + 18 + ", '/img/catalogue/product0.jpg', 1000, 1");
                    Console.WriteLine(")");
                    var money = rand.Next(5, 151);
                    Console.WriteLine("--Price Types ");
                    for (int j = 1; j < 5; j++)
                    {
                        Console.WriteLine("insert into PriceTypes (ProductID, UserTypeID, UnitPrice)");
                        Console.WriteLine("\tvalues \n(");
                        Console.WriteLine("\t\t'" + prodId + "'");
                        Console.WriteLine("\t\t, " + j);
                        Console.WriteLine("\t\t, " + (money + (j * 2)));
                        Console.WriteLine(")");
                    }
                    Console.WriteLine("--Record " + count + " inserted");
                    count++;
                }

            }

            Console.WriteLine("--END");
        }
        */
    }
}
