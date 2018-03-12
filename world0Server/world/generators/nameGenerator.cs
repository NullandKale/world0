using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace world0Server.generators
{
    public class nameGenerator
    {
        public static nameGenerator gen;

        private Random rng;
        private int rngSeed;

        public nameGenerator(int seed)
        {
            if(gen == null)
            {
                gen = this;
            }
            else
            {
                throw new Exception("Singleton Exception @ nameGenerator.cs");
            }

            rngSeed = seed;
            rng = new Random(rngSeed);
        }

        public string generateName(nameType typeOfName)
        {
            return typeOfName.ToString() + rng.Next(99,999).ToString();
        }
    }

    public enum nameType
    {
        village,
        city,
        state,
        street,
        male,
        female,
    }
}
