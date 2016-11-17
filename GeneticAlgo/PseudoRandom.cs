using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgo
{
    public class PseudoRandom
    {
        private int[] next100000 = { 77396, 41714, 1360, 38831, 41304, 60889, 28196, 40109, 9373, 89521 };

        private int next100000Pointer = 0;

        public int Next100000()
        {
            if (!IsPseudorandom)
                return random.Next(100000);

            if (next100000Pointer == next100000.Length)
                next100000Pointer = 0;

            var result = next100000[next100000Pointer];
            next100000Pointer++;
            return result;
        }

        private enum nextState
        {
            First,
            Middle,
            Last
        }
        private nextState nextStatePointer = PseudoRandom.nextState.First;

        private Random random = new Random();

        public bool IsPseudorandom { get; private set; }

        public int Next(int limit)
        {
            if (!IsPseudorandom)
                return random.Next(limit);

            var result = 0;
            switch (nextStatePointer)
            {
                case nextState.First:
                    result = 0;
                    nextStatePointer = nextState.Middle;
                    break;
                case nextState.Middle:
                    result = limit / 2;
                    nextStatePointer = nextState.Last;
                    break;
                case nextState.Last:
                    result = limit > 0 ? limit - 1 : 0;
                    nextStatePointer = nextState.First;
                    break;
            }
            return result;
        }

        public void Reset()
        {
            next100000Pointer = 0;
            nextStatePointer = nextState.First;
        }

        public PseudoRandom(bool isPseudoRandom)
        {
            IsPseudorandom = isPseudoRandom;
        }
    }
}
