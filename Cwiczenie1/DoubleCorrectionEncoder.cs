namespace Cwiczenie1
{
    public class DoubleCorrectionEncoder : CodingStream
    {
        public override void StartFlow()
        {
            for (int i = 0; i < Input.Length; i++)
            {
                byte b = (byte)Input.ReadByte();
                var encoded = ECC.Double.Encode(b);
                byte[] bytes = encoded.ToVector().ToByteArray();
                Formatter.Write(Output, bytes);    
            }
        }
    }
}