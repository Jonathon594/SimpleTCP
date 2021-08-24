using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jonathon594.SimpleTCP.Data
{
    /// <summary>
    /// Contains a list of <see cref="byte"/> for sending accross the network. Employs helper methods for writing and reading specific data types from the <see cref="ByteBuffer"/>.
    /// </summary>
    public class ByteBuffer
    {
        private List<byte> bytes;
        private int position = 0;

        internal ByteBuffer()
        {
            bytes = new List<byte>();
        }

        internal ByteBuffer(byte[] bytes, int length) : this()
        {
            for (int i = 0; i < length; i++)
            {
                this.bytes.Add(bytes[i]);
            }
        }

        /// <summary>
        /// Gets the number of <see cref="byte"/> in the buffer.
        /// </summary>
        /// <returns>An <see cref="int"/> representing how many <see cref="byte"/> are currently in the buffer.</returns>
        public int GetByteCount()
        {
            return bytes.Count();
        }

        /// <summary>
        /// Reads a single <see cref="byte"/> from the buffer and advances the read position forward.
        /// </summary>
        /// <returns>A single <see cref="byte"/> from the buffer. At the current read position.</returns>
        public byte ReadByte()
        {
            if (position >= bytes.Count) return 0;
            byte b = bytes[position];
            position++;
            return b;
        }

        /// <summary>
        /// Gets an <see cref="Array"/> of <see cref="byte"/> from the buffer.
        /// </summary>
        /// <returns>An <see cref="Array"/> of all <see cref="byte"/> in the buffer.</returns>
        public byte[] GetBytes()
        {
            byte[] b = new byte[bytes.Count];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = bytes[i];
            }
            return b;
        }

        /// <summary>
        /// Adds a single <see cref="byte"/> to the buffer.
        /// </summary>
        /// <param name="b">The <see cref="byte"/> to write.</param>
        public void WriteByte(byte b)
        {
            bytes.Add(b);
        }

        /// <summary>
        /// Writes a single <see cref="int"/> as 4 <see cref="byte"/> to the buffer.
        /// </summary>
        /// <param name="i">The <see cref="int"/> to write.</param>
        public void WriteInt(int i)
        {
            byte[] bytes = BitConverter.GetBytes(i);
            this.bytes.AddRange(bytes);
        }

        /// <summary>
        /// Reads an <see cref="int"/> from the buffer.
        /// </summary>
        /// <returns>The next <see cref="int"/> in the buffer.</returns>
        public int ReadInt()
        {
            byte[] bytes = new byte[4];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = ReadByte();
            }
            int value = BitConverter.ToInt32(bytes, 0);
            return value;
        }

        /// <summary>
        /// Writes a single <see cref="string"/> as a number of <see cref="byte"/> to the buffer.
        /// Cannot be longer than 32767 characters.
        /// </summary>
        /// <param name="s">The <see cref="string"/> to write.</param>
        public void WriteString(string s)
        {
            if (s.Length > 32767) throw new FormatException();
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            int length = bytes.Length;
            WriteInt(length);
            this.bytes.AddRange(bytes);
        }

        /// <summary>
        /// Reads a <see cref="string"/> from the buffer.
        /// </summary>
        /// <returns>The next <see cref="string"/> in the buffer.</returns>
        public string ReadString()
        {
            int length = ReadInt();
            byte[] bytes = new byte[length];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = ReadByte();
            }
            string s = Encoding.UTF8.GetString(bytes);
            return s;
        }

        /// <summary>
        /// Writes a single <see cref="float"/> as 4 <see cref="byte"/> to the buffer.
        /// </summary>
        /// <param name="f">The <see cref="float"/> to write.</param>
        public void WriteFloat(float f)
        {
            byte[] bytes = BitConverter.GetBytes(f);
            this.bytes.AddRange(bytes);
        }

        /// <summary>
        /// Reads a <see cref="float"/> from the buffer.
        /// </summary>
        /// <returns>The next <see cref="float"/> in the buffer.</returns>
        public float ReadFloat()
        {
            byte[] bytes = new byte[4];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = ReadByte();
            }
            float value = BitConverter.ToSingle(bytes, 0);
            return value;
        }

        /// <summary>
        /// Writes a single <see cref="double"/> as 8 <see cref="byte"/> to the buffer.
        /// </summary>
        /// <param name="d">The <see cref="double"/> to write.</param>
        public void WriteDouble(double d)
        {
            byte[] bytes = BitConverter.GetBytes(d);
            this.bytes.AddRange(bytes);
        }

        /// <summary>
        /// Reads a <see cref="double"/> from the buffer.
        /// </summary>
        /// <returns>The next <see cref="double"/> in the buffer.</returns>
        public double ReadDouble()
        {
            byte[] bytes = new byte[8];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = ReadByte();
            }
            double value = BitConverter.ToDouble(bytes, 0);
            return value;
        }

        /// <summary>
        /// Writes a single <see cref="long"/> as 8 <see cref="long"/> to the buffer.
        /// </summary>
        /// <param name="l">The <see cref="long"/> to write.</param>
        public void WriteLong(long l)
        {
            byte[] bytes = BitConverter.GetBytes(l);
            this.bytes.AddRange(bytes);
        }

        /// <summary>
        /// Reads a <see cref="long"/> from the buffer.
        /// </summary>
        /// <returns>The next <see cref="long"/> in the buffer.</returns>
        public long ReadLong()
        {
            byte[] bytes = new byte[8];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = ReadByte();
            }
            long value = BitConverter.ToInt64(bytes, 0);
            return value;
        }
    }
}