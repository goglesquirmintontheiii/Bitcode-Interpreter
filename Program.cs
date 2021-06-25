//The idea of this system is that the red value of the color represents a "channel".
//For example, characters use channel 1, builtin methods use 2, variables use 3, user-defined methods use 4
//The system is basically [channel,arg1,arg2] or [channel,arg,sub-arg] 
//Using this method, 255 different channels can be made.. more than enough.
//Image langs are virtually infinitely expandable, with over 16M different color combinations possible, so you'll never run out of commands.
//Unicode has 143,859 characters, a tiny fraction of how many is possible.


//Characters will use Red-1, Green-charnum, Blue-capital
//Example: [1,3,1] - C / [1,7,0] - g / [1,30,0] - 3 
//Builtin methods will use Red-2, Green-methodnum, Blue-arg




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;
using System.IO;
using System.Diagnostics;

namespace Bitcode
{
    class Program
    {
        static int ms = 0;
        static void Main(string[] args)
        {

            void interpret(string fnme)
            {
                Console.WriteLine("Attempting interpretation..");
                if (fnme.EndsWith(".bmp"))
                {
                    int x = 0;
                    int y = 0;
                    Bitmap img = new Bitmap(Bitmap.FromFile(fnme));
                    bool instr = false;
                    string str = "";
                    Dictionary<Color, string> chars = new Dictionary<Color, string>();
                    string chls = "abcdefghijklmnopqrstuvwxyz 1234567890~!@#$%^&*()_+=-`{}[]\\|:;\"'<>,.?/";
                    int index = 1;
                    foreach (char nc in chls)
                    {
                        string c = nc.ToString();
                        chars[Color.FromArgb(1, index, 0)] = c;
                        chars[Color.FromArgb(1, index, 1)] = c.ToUpper();
                        index++;
                    }
                    while (y < img.Height)
                    {
                        while (x < img.Width)
                        {
                            try
                            {
                                Color pix = img.GetPixel(x, y);
                                //Console.WriteLine($"{pix.R} {pix.G} {pix.B}");
                                if (instr)
                                {
                                    if (pix == Color.FromArgb(1, 0, 0))
                                    {
                                        instr = false;
                                        Console.WriteLine(str);
                                        str = "";
                                    }
                                    else
                                    {
                                        try
                                        {
                                            str = str + chars[pix];
                                        }
                                        catch
                                        {
                                            Console.WriteLine($"Error at pix [{x},{y}] - invalid character. Looking for problems..");
                                            if (pix.B > 1)
                                            {
                                                Console.WriteLine("Pixel sub-arg muss be 1.");
                                            }
                                            if (pix.G > chls.Length + 1)
                                            {
                                                Console.WriteLine($"Character out of range. Must be lower than {chls.Length + 2} and greater than 0");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (pix == Color.FromArgb(1,0,0)) //print statement
                                    {
                                        instr = true;
                                    }
                                    else if (pix == Color.FromArgb(0, 0, 0))
                                    {

                                    }
                                }
                            }
                            catch
                            {

                            }
                            x++;
                        }
                        x = 0;
                        y++;
                    }
                }
            }

            Console.WriteLine("Initial boot complete..");
            Console.WriteLine("Running image-text protocol speed test..");
            Timer t = new Timer();
            t.Interval = 1;
            t.Elapsed += T_Elapsed;
            int r = 0;
            int g = 0;
            int b = 0;
            while (r < 255)
            {
                if (b == 255)
                {
                    b = 0;
                    g++;
                }
                if (g == 255)
                {
                    g = 0;
                    r++;
                }
                b++;
            }
            t.Stop();
            Console.WriteLine("Protocol complete.");
            Console.WriteLine($"Took {ms}ms");
            Console.WriteLine("Healthy speed is 1ms or under.");
            
            if (args.Length == 0)
            {
                Console.WriteLine("No image attached. Looking for image named \"code.bmp\".");
                string dir = Environment.CurrentDirectory;
                if (File.Exists(dir + "\\code.bmp"))
                {
                    interpret(dir + "\\code.bmp");
                }
                else
                {
                    Console.WriteLine("No file named code.bmp found, file created.");
                    FileStream f = File.Create(dir + "\\code.bmp");
                    f.Close();
                    Process.Start(dir + "\\code.bmp");
                }
            }
            else
            {
                
            }
            Console.ReadLine();
        }

        private static void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            ms++;
        }
    }
}
