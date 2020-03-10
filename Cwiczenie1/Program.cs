using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Cwiczenie1
{
    class Program
    {
        static void Main(string[] args)
        {
            int mode = 0;
            Stream stream = null;
            Stream output = null;
            BitFormatter formatter = null;
            CodingStream codingStream = null;
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-encode-single":
                        mode = 0;
                        break;
                    case "-encode-double":
                        mode = 1;
                        break;
                    case "-decode":
                        mode = 2;
                        break;
                    case "-seq":
                        if (args[i + 1].TryReadBinaryByte(out byte b))
                        {
                            stream = new MemoryStream(new byte[1] {b});
                        }
                        else
                        {
                            Console.WriteLine("Sequence not specified");
                        }
                        break;
                    case "-byte":
                        if (args.Length > i)
                        {
                            stream = new MemoryStream(new byte[1] {Convert.ToByte(args[i + 1])});
                        }
                        else
                        {
                            Console.WriteLine("Byte not specified");
                        }
                        break;
                    case "-infile":
                        if (args.Length > i && File.Exists(args[i+1]))
                        {
                            stream = new FileStream(args[i+1], FileMode.Open, FileAccess.Read);
                        }
                        else
                        {
                            Console.WriteLine("Path not specified or does not exists");
                        }
                        break;
                    case "-outfile":
                        if (args.Length > i )
                        {
                            output = new FileStream(args[i+1], FileMode.Create, FileAccess.ReadWrite);
                        }
                        else
                        {
                            Console.WriteLine("Path not specified or does not exists");
                        }
                        break;
                    case "-outconsole":
                        output = Console.OpenStandardOutput();
                        break;
                    case "-bitprint":
                        formatter = new BitFormatter();
                        break;
                    case "-binaryprint":
                        formatter = new RawBinaryFormatter();
                        break;
                }
            }

            if (stream == null)
            {
                throw new Exception();
            }

            var len = stream.Length;
            for (int i = 0; i < len; i++)
            {
                byte b = (byte)stream.ReadByte();
                byte[] encoded;
                if (mode == 0)
                {
                    encoded = ECC.Single.Encode(b).ToVector().ToByteArray(true);
                }
                else if (mode == 2)
                {

                    var f = new BitMatrix(b).MergeHorizontally(new BitMatrix((byte)stream.ReadByte()));
                    i++;
                    if (!ECC.Double.CheckCorrectness(f))
                    {
                        ECC.Double.Correct(f);
                    }

                    encoded = ECC.Double.Decode(f).ToVector().ToByteArray();

                }
                else
                {
                    encoded = ECC.Double.Encode(b).ToVector().ToByteArray();
                }
                formatter.Write(output, encoded);
            }
            stream.Close();
            output.Close();
            // Console.WriteLine("Wybierz tryb kodowania: ([1]/[2] błedy)");
            // int input = Console.Read();

            // File.WriteAllBytes("test.bin", new []{res});
            // int[,] x = new int[,]
            // {
            //     {1, 1},
            //     {0, 1}
            // };
            // BitMatrix m = new BitMatrix(x);
            // BitMatrix.Print(m);
            // Console.WriteLine(m[0]);
            // byte b = 129; // 1000 0001
            // // BitMatrix.Print(new BitMatrix(b));
            // var encoded = ECC.Single.Encode(b);
            // // encoded[0, 5] = true;
            // encoded[0, 4] = true;
            // var c = encoded.ToVector().ToByteArray(true);
            // BitMatrix.Print(encoded);
            // Console.WriteLine($"{c[0]} {c[1]}");
            // File.WriteAllBytes("encoded.bin", c);
            // bool error = ECC.Single.CheckCorrectness(encoded);
            // Console.WriteLine(error);
            // ECC.Single.Correct(encoded);
            // var decoded = ECC.Single.Decode(encoded);
            // BitMatrix.Print(decoded);
            //
            // var encoded1 = ECC.Double.Encode(b);
            // BitMatrix.Print(encoded1);
            // encoded1[0, 2] = true;
            // encoded1[0, 6] = true;
            // // encoded1.ToVector().ToByteArray();
            // BitMatrix.Print(encoded1);
            // bool error1 = ECC.Double.CheckCorrectness(encoded1);
            // Console.WriteLine(error1);
            // ECC.Double.Correct(encoded1);
            // var decoded1 = ECC.Double.Decode(encoded1);
            // BitMatrix.Print(decoded1);
            // var ba = decoded1.ToVector().ToByteArray();
            // Console.WriteLine(ba[0]);

            // var matrix1 = new BitMatrix(new bool[,] { {true , false}, {true, false}});
            // BitMatrix.Print(matrix1);
            // var matrix2 = new BitMatrix(new bool[,] { {true,true}, {false,true}});
            // BitMatrix.Print(matrix2);
            // var matrix3 = matrix1.MergeVertically(matrix2);
            // BitMatrix.Print(matrix3);
            // Message m = new Message(new []{1,1,1,0});
            // m.CalculateParityBits();
            // BitMatrix.Print(m.ParityBits);
        }

        static void DoubleErrorCorrection(byte[] bits)
        {
            byte b = bits.ToByte();
            var encoded = ECC.Single.Encode(b);
            var c = encoded.ToVector().ToByteArray(true);
            BitMatrix.Print(encoded);
            Console.WriteLine($"{c[0]} {c[1]}");
            File.WriteAllBytes("encoded.bin", c);
            bool error = ECC.Single.CheckCorrectness(encoded);
            Console.WriteLine(error);
            ECC.Single.Correct(encoded);
            var decoded = ECC.Single.Decode(encoded);
            BitMatrix.Print(decoded);
        }
    }
}