using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.Genetic_Algorithms
{

    public class GeneticObject : IComparable<GeneticObject>
    {
        public int FitnessValue { get; private set; } = 0;
        int[] _values;
        public GeneticObject(params int[] objectValues)
        {
            _values = objectValues;
        }
        public int[] GetValues()
        {
            return _values;
        }
        public void SetFitnessValue(GeneticObject PerfectObject)
        {
            if (PerfectObject._values.Count() == _values.Count())
            {
                for (int i = 0; i < PerfectObject._values.Count(); i++)
                {
                    if (_values[i] == PerfectObject._values[i])
                    {
                        FitnessValue++;
                    }
                }
            }
        }
        public bool IsNull()
        {
            return _values.Any(x => x == 0);
        }
        public bool IsIdentical(GeneticObject other)
        {
            bool tempBool = true;
            for (int i = 0; i < _values.Count(); i++)
            {
                if (_values[i] != other._values[i])
                {
                    tempBool = false;
                }
            }
            return tempBool;
        }
        public int CompareTo(GeneticObject other)
        {
            return FitnessValue - other.FitnessValue;
        }
    }
}
