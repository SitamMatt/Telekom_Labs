namespace Cwiczenie1
{
    public class Message
    {
        public bool[] Bits { get; private set; }
        
        public BitMatrix BitsM { get; private set; }

        public BitMatrix H { get; private set; } = new BitMatrix(new bool[,]
        {
            {true, true, false,true},
            {true, false, true, true},
            {false, true, true, true}
        });
        
        public BitMatrix ParityBits { get; private set; }

        public Message(int[] bits)
        {
            Bits = new bool[bits.Length];
            for (int i = 0; i < bits.Length; i++)
            {
                Bits[i] = bits[i] != 0;
            }
            BitsM = new BitMatrix(Bits);
            // BitMatrix.Print(BitsM);
        }

        public void CalculateParityBits()
        {
            this.ParityBits = H * BitsM;
        }
    }
}