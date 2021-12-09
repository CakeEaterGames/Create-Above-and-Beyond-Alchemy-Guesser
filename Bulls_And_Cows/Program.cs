using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulls_And_Cows
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            Console.ReadLine();
        }

        bool autoMode = false;
        List<string> allVariants = new List<string>();
        HashSet<string> variants = new HashSet<string>();
        public Program()
        {
            Console.WriteLine("Enter the names of reagents separated by commas. Example:");
            Console.WriteLine("stone, andes, gabro, diori, grani, basal");

            string[] pseudonims = Console.ReadLine().Replace(" ", "").Split(',');
            Console.WriteLine();

            //GenAll();
            // for (int ti = 0; ti < allVariants.Count; ti++)
            {
                GenAll();


                //var answer = Console.ReadLine();
                var answer = "aaaa";
                //var answer = allVariants[ti];
                if (autoMode)
                {
                    answer = Console.ReadLine();
                }

                int turns = 0;
                while (true)
                {
                    string v = "";

                    if (turns == 0)
                    {
                        v = "1122";
                    }
                    else
                    {
                        v = bestGuess(variants);
                    }
                    variants.Remove(v);

                    string vi = numsToNames(v, pseudonims);

                    turns++;
                    var bc = getBC(v, answer);
                    var b = bc.b;
                    var c = bc.c;

                    if (!autoMode)
                    {
                        Console.WriteLine("Try {0}", vi);
                        Console.WriteLine("How many Glowstone and Redstone? Example:0 1");
                        var inp = Console.ReadLine().Split(' ');
                        b = int.Parse(inp[0]);
                        c = int.Parse(inp[1]);
                    }
                    Console.WriteLine("{0}, Glowstone {1} Redstone {2}", vi, b, c);

                    if (b == 4 && c == 0)
                    {
                        Console.WriteLine("Victory in {0} turns! Answer {1}", turns, vi);
                        break;
                    }

                    HashSet<string> toRem = new HashSet<string>();
                    foreach (var item in variants)
                    {
                        if (!CanBeAnswer(item, v, b, c))
                        {
                            toRem.Add(item);
                        }
                    }
                    variants.ExceptWith(toRem);

                    Console.WriteLine("Variants left: " + variants.Count);
                    if (variants.Count == 0)
                    {
                        Console.WriteLine("You've made a mistake somewhere");
                        break;
                    }

                    Console.WriteLine();
                }
            }
            Console.ReadLine();
        }

        string numsToNames(string nums, string[] names)
        {
            string res = "";
            foreach (var n in nums)
            {
                res += names[int.Parse("" + n) - 1] + " ";
            }
            return res;
        }

        struct BC
        {
            public int b;
            public int c;

            public BC(int b, int c)
            {
                this.b = b;
                this.c = c;
            }
            public override string ToString()
            {
                return b + " " + c;
            }
        }

        public string bestGuess(HashSet<string> vars)
        {
            HashSet<BC> outcomes = new HashSet<BC>();
            foreach (var guess in vars)
            {
                foreach (var answ in vars)
                {
                    outcomes.Add(getBC(guess, answ));
                }
            }

            string best = allVariants[0];
            int max = 0;
            foreach (var guess in vars)
            {
                int min = 9999;
                foreach (var outc in outcomes)
                {
                    int cnt = 0;
                    foreach (var answ in vars)
                    {
                        if (!CanBeAnswer(answ, guess, outc.b, outc.c))
                        {
                            cnt++;
                        }
                    }
                    min = Math.Min(min, cnt);

                    if (min < max)
                    {
                        break;
                    }
                }
                if (min >= max)
                {
                    max = min;
                    best = guess;
                }

            }

            return best;
        }

        public bool CanBeAnswer(string v1, string v2, int b, int c)
        {
            var a = getBC(v1, v2);

            return (a.c == c && a.b == b);
        }

        BC getBC(string a, string b)
        {
            char[] useda = "....".ToCharArray();
            char[] usedb = "....".ToCharArray();

            int resB = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == b[i])
                {
                    resB++;
                    useda[i] = '+';
                    usedb[i] = '+';
                }
            }
            int resC = 0;
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    if (a[i] == b[j] && useda[i] == '.' && usedb[j] == '.')
                    {
                        resC++;
                        useda[i] = '+';
                        usedb[j] = '+';
                        break;
                    }
                }

            }

            return new BC(resB, resC);
        }

        public void GenAll()
        {
            if (allVariants.Count == 0)
            {
                for (int a = 1; a <= 6; a++)
                    for (int b = 1; b <= 6; b++)
                        for (int c = 1; c <= 6; c++)
                            for (int d = 1; d <= 6; d++)
                            //if (a != b && b != c && c != d && a != c && b != d && a != d)
                            {
                                allVariants.Add("" + a + b + c + d);
                            }
            }


            variants.Clear();
            variants = new HashSet<string>(allVariants);

        }

    }
}

