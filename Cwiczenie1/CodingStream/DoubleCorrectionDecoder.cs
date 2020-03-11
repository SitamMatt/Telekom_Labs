using System;

namespace Cwiczenie1
{
    public class DoubleCorrectionDecoder : CodingStream
    {
        public override void StartFlow()
        {
            if (Input.Length % 2 != 0)
            {
                throw new Exception();
            }
            byte[] buffer = new byte[2];
            for (int i = 0; i < Input.Length/2; i++)
            {
                Input.Read(buffer);
                BitMatrix vector = new BitMatrix(buffer);
                if (!ECC.Double.CheckCorrectness(vector))
                {
                    ECC.Double.Correct(vector);
                }

                var decoded = ECC.Double.Decode(vector);
                byte[] bytes = decoded.ToVector().ToByteArray();
                Formatter.Write(Output, bytes);
            }
        }
    }
}