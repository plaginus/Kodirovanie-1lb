using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabaKod1
{
    public struct PixelYUV
    {
        public int Y { get; set; }
        public int U { get; set; }
        public int V { get; set; }

        public byte Aligning(int x)
        {
            return x > 255 ? (byte)255 : (byte)x;
        }

        public byte ToR()
        {
            double result = Y + 1.13983 * (V - 128);

            byte outResult = result > 255 ? (byte)255 : (byte)result;
            outResult = result < 0 ? (byte)0 : (byte)outResult;

            return outResult;
        }

        public byte ToG()
        {
            double result = Y - 0.39465 * (U - 128) - 0.58060 * (V - 128);

            byte outResult = result > 255 ? (byte)255 : (byte)result;
            outResult = result < 0 ? (byte)0 : (byte)outResult;

            return outResult;
        }

        public byte ToB()
        {
            double result = Y + 2.03211 * (U - 128);

            byte outResult = result > 255 ? (byte)255 : (byte)result;
            outResult = result < 0 ? (byte)0 : (byte)outResult;

            return outResult;
        }
    }
}
