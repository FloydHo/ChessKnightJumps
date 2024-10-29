using System.Threading;

namespace ChessKnightJumps
{
    internal class Program
    {
        private record struct PositionKey(int X, int Y);
        public static int sleepTime = 100;

        static void Main(string[] args)
        {
            //List<string> list = new List<string>();
            //for (int i = 1; i < 8; i++)
            //{
            //    for (int j = 1; j < 8; j++)
            //    {
            //        try
            //        {
            //            KnightJumps((char)(i + 65), j);
            //        }
            //        catch (Exception e)
            //        {
            //            Console.WriteLine(e.Message);
            //            list.Add($"ERROR: {(char)(i + 65)} : {j}");
            //        }
            //    }
            //}

            //foreach (string str in list) Console.WriteLine(str);


            try
            {
                KnightJumps('D', 8);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void KnightJumps(char Xs, int Ys)
        {
            int X = Xs - 65; //Ascii
            int Y = Ys - 1; //-1 weil wegen index 0 und so
            if (X < 0 || X > 7 || Y < 0 || Y > 7) throw new ArgumentOutOfRangeException();      // Feld ist ja nur 8x8 groß 

            DrawField(); //Selbsterklärend oder?

            int count = 1;                                                                      // Erste Feld ist belegt also direkt bei 1 anfangen
            Dictionary<PositionKey, int> field = new Dictionary<PositionKey, int>();            // Dictionary das die schon genutzten Felder speichert + die Position in der Reihenfolge
            PriorityQueue<PositionKey, int> checking = new PriorityQueue<PositionKey, int>();   // Queue in der die zu checkenden Felder zwischengespeichert werden. Warum Priority? Weil beim Dequeue direkt low zu high sortiert.

            PositionKey current = new PositionKey(X, Y);                                        //Startposition (struct record)
            field.Add(current, count);                                                          //Starposition zum Dictionary hinzufügen.
            DrawPosition(current, count);                                                       //Zeichnet die StartPosition
            while (field.Count <= 63)                                                           //Solange iterieren bis alle 64 Felder voll sind.
            {
                PositionKey[] possibleMoves = CheckPossibilities(current, field);               //Sucht alle freien Felder

                foreach (PositionKey possibleMove in possibleMoves)
                {
                    checking.Enqueue(possibleMove, CheckPossibilities(possibleMove, field).Length); //Speichert die Freien Felder mit deren Anzahl an freien Feldern in einer PriorityQueue
                }
                if (!checking.TryDequeue(out PositionKey pos, out int p))                       //Nimmt den Eintrag mit der kleinsten Prioirity (Anzahl an freien Feldern) raus und speichert es die Koordinaten als akteulle Position.
                {
                    throw new NullReferenceException("Q is empty");
                }
                current = pos;                                                   
                field.Add(current, ++count);                                                    //Speichert das aktuelle Feld mit der Position in der Reihenfolge. 
                DrawPosition(current, count);
                checking.Clear();                                                               //Reset der PriorityQueue
            }
        }

        private static PositionKey[] CheckPossibilities(PositionKey pos, Dictionary<PositionKey, int> dict)
        {
            List<PositionKey> valid = new List<PositionKey>();

            List<PositionKey> checkMe = new List<PositionKey>();
            checkMe.Add(new PositionKey(pos.X - 2, pos.Y + 1)); // links unten
            checkMe.Add(new PositionKey(pos.X + 1, pos.Y - 2)); // oben rechts
            checkMe.Add(new PositionKey(pos.X - 1, pos.Y + 2)); // unten links
            checkMe.Add(new PositionKey(pos.X + 1, pos.Y + 2)); // unten rechts
            checkMe.Add(new PositionKey(pos.X + 2, pos.Y - 1)); // rechts oben
            checkMe.Add(new PositionKey(pos.X + 2, pos.Y + 1)); // rechts unten
            checkMe.Add(new PositionKey(pos.X - 2, pos.Y - 1)); // links oben
            checkMe.Add(new PositionKey(pos.X - 1, pos.Y - 2)); // oben links

            foreach (PositionKey checkPos in checkMe)
            {
                if (checkPos.X < 0 || checkPos.X > 7 || checkPos.Y < 0 || checkPos.Y > 7 || dict.ContainsKey(checkPos))
                {
                    continue;
                }
                else
                {
                    valid.Add(checkPos);
                }
            }
            return valid.ToArray();
        }

        private static void DrawField()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("   ┌──────┬──────┬──────┬──────┬──────┬──────┬──────┬──────┐");
            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine("   │      │      │      │      │      │      │      │      │"); ;
                Console.WriteLine("   │      │      │      │      │      │      │      │      │");
                Console.WriteLine("   ├──────┼──────┼──────┼──────┼──────┼──────┼──────┼──────┤ ");
            }
            Console.SetCursorPosition(0, 24);
            Console.WriteLine("   └──────┴──────┴──────┴──────┴──────┴──────┴──────┴──────┘");
            Console.WriteLine("      A      B      C      D      E      F      G      H");
            for (int i = 0; i < 8; i++)
            {
                Console.SetCursorPosition(1, 3 * i + 2);
                Console.Write(i + 1);
            }
            //foreach (var k in field)
            //{
            //    Console.SetCursorPosition(7 * k.Key.X + 6, 3 * k.Key.Y + 2);
            //    Console.Write($"{k.Value,-2}");
            //}
            Console.SetCursorPosition(0, 26);
        }

        private static void DrawPosition(PositionKey pos, int v)
        {
            Thread.Sleep(sleepTime);
            Console.SetCursorPosition(7 * pos.X + 6, 3 * pos.Y + 2);
            Console.Write($"{v,-2}");
            Console.SetCursorPosition(0, 26);
        }
    }
}
