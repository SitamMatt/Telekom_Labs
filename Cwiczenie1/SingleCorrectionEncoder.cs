namespace Cwiczenie1
{
    public class SingleCorrectionEncoder : CodingStream
    {
        public override void StartFlow()
        {
            for (int i = 0; i < Input.Length; i++)
            {
                byte b = (byte)Input.ReadByte();
                var encoded = ECC.Single.Encode(b);
                byte[] bytes = encoded.ToVector().ToByteArray(true);
                Formatter.Write(Output, bytes);    
            }
        }
    }
}