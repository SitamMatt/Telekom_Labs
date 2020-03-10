using System;

namespace Cwiczenie1
{
    public class SingleCorrectionDecoder : CodingStream
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
                BitMatrix vector = new BitMatrix(buffer, 12);
                if (!ECC.Single.CheckCorrectness(vector))
                {
                    ECC.Single.Correct(vector);
                }

                var decoded = ECC.Single.Decode(vector);
                byte[] bytes = decoded.ToVector().ToByteArray();
                Formatter.Write(Output, bytes);
            }
        }
    }
}