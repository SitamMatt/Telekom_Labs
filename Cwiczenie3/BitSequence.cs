using System;
using System.Text;

namespace Telekom.BitCollections
{
    //todo change to struct
    public class BitSequence
    {
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        protected bool Equals(BitSequence other)
        {
            if (Length != other.Length)
                return false;
            for (int i = 0; i < Length; i++)
            {
                if (other[i] != this[i])
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BitSequence) obj);
        }

        public byte[] rawData;
        public int ByteCapacity { get; private set; }
        public int Length { get; private set; }
        public int BytesLength => Length % 8 == 0 ? Length / 8 : Length / 8 + 1;

        public BitSequence(int count)
        {
            int bytesRequired = count / 8;
            if (count % 8 != 0)
                bytesRequired++;
            ByteCapacity = bytesRequired;
            rawData = new byte[ByteCapacity];
            Length = 0;
        }

        //todo decide whatever array should be copied (mutable) or assigned by reference (immutable) 
        public BitSequence(byte[] array, in int length)
        {
            rawData = array;
            ByteCapacity = rawData.Length;
            Length = length;
        }

        public void PushLength(int length)
        {
            Length += length;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Length);
            for (int i = 0; i < Length; i++)
            {
                sb.Append(this[i] ? '1' : '0');
            }

            return sb.ToString();
        }

        public (byte[], int) GetBytes()
        {
            int padLen = 8* BytesLength - Length;
            var arr = rawData.AsSpan(0, BytesLength).ToArray();
            return (arr, padLen);
        }

        public void Reverse()
        {
            int len = Length / 2;
            int offset = Length - 1;
            bool temp;
            for (int i = 0; i < len; i++)
            {
                temp = this[i];
                this[i] = this[offset - i];
                this[offset - i] = temp;
            }
        }

        public void Push(BitSequence seq)
        {
            // todo try to improve push process
            for (int i = 0; i < seq.Length; i++)
            {
                Push(seq[i]);
            }
        }

        public void Push()
        {
            var b = this[Length];
            Length++;
        }

        public void Push(bool value)
        {
            this[Length++] = value;
        }

        public BitSequence DeepCopy()
        {
            BitSequence seq = (BitSequence) MemberwiseClone();
            seq.rawData = new byte[ByteCapacity]; 
            rawData.CopyTo(seq.rawData, 0);
            return seq;
        }
        
        // todo include Length range validation
        public bool this[int index]
        {
            get
            {
                int byteIndex = index / 8;
                byte bitIndex = (byte) (index % 8);
                if (byteIndex >= ByteCapacity)
                    throw new ArgumentOutOfRangeException();
                byte mask = (byte)(1 << (7 - bitIndex));
                byte bit = (byte) (rawData[byteIndex] & mask);
                return bit != 0;
            }
            set
            {
                if (value == this[index])
                    return;
                int byteIndex = index / 8;
                byte bitIndex = (byte) (index % 8);
                if (byteIndex >= ByteCapacity)
                    throw new ArgumentOutOfRangeException();
                byte mask = (byte)(1 << (7 - bitIndex));
                if (value)
                    rawData[byteIndex] |= mask;
                else
                    rawData[byteIndex] &= (byte) (~mask);
            }
        }
    }
}