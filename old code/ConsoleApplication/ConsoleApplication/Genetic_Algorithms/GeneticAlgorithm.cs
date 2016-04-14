using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.Genetic_Algorithms
{
    public class GeneticAlgoritm
    {
        Random r = new Random();
        int _numberOfIterations;
        List<GeneticObject> currentObjects = new List<GeneticObject>();
        public int Count
        {
            get { return currentObjects.Count; }
        }
        int _maxValue;
        public int NumberOfGeneticValues
        {
            get; private set;
        }
        public GeneticObject PerfectObject { get; set; }
        private int _numberOfObjectsToRemove;
        public GeneticAlgoritm(int numberOfGeneticValues, int numberOfIterations, int maxValue, GeneticObject perfectObject)
        {
            _maxValue = maxValue;
            PerfectObject = perfectObject;
            _numberOfIterations = numberOfIterations;
            NumberOfGeneticValues = numberOfGeneticValues;
        }
        public List<GeneticObject> StartAlgorithm()
        {
            int tempInt = 0;
            if (!PerfectObject.IsNull())
            {
                do
                {
                    BreedObjects();
                    SortObjects();
                    RemoveWeakObjects();
                    tempInt++;
                }
                while (EndAlgorithm() && tempInt < _numberOfIterations);
            }
            return currentObjects;
        }
        public void AddRange(params GeneticObject[] gObjects)
        {
            foreach(GeneticObject gObject in gObjects)
            {
                currentObjects.Add(gObject);
            }
            _numberOfObjectsToRemove += gObjects.Count();
        }
        public void ClearObjects()
        {
            currentObjects.Clear();
            _numberOfObjectsToRemove = 0;
        }
        private bool EndAlgorithm()
        {
            bool temporaryBool = true;
            foreach (GeneticObject gO in currentObjects)
            {
                if (gO.IsIdentical(PerfectObject))
                {
                    temporaryBool = false;
                }
            }
            return temporaryBool;
        }
        private void BreedObjects()
        {
            _numberOfObjectsToRemove = 0;
            int tempInt = currentObjects.Count;
            for (int i = 0; i < tempInt / 2; i++)
            {
                Breed(currentObjects[i], currentObjects[currentObjects.Count - 1 - i]);
            }
            foreach (GeneticObject gObject in currentObjects)
            {
                gObject.SetFitnessValue(PerfectObject);
            }
        }
        private void Breed(GeneticObject object1, GeneticObject object2)
        {
            int[] tempValues1, tempValues2, newValues = new int[NumberOfGeneticValues];
            for (int index = 0; index < 2; index++)
            {
                tempValues1 = object1.GetValues();
                tempValues2 = object2.GetValues();
                for (int j = 0; j < NumberOfGeneticValues; j++)
                {
                    switch (r.Next(9))
                    {
                        case 0: newValues[j] = r.Next(1,_maxValue + 1); break;
                        case 1: case 2: case 3:
                        case 4: newValues[j] = tempValues1[j]; break;
                        case 5: case 6: case 7:
                        case 8: newValues[j] = tempValues2[j]; break;
                    }
                }
                currentObjects.Add(new GeneticObject(newValues));
                _numberOfObjectsToRemove++;
            }
        }
        public void AddObject(GeneticObject gObject)
        {
            if (gObject.GetValues().Count() == NumberOfGeneticValues)
            {
                currentObjects.Add(gObject);
            }
        }
        private void RemoveWeakObjects()
        {
            for (int i = 0; i < _numberOfObjectsToRemove; i++)
            {
                currentObjects.RemoveAt(currentObjects.Count - 1);
            }
        }
        private void SortObjects()
        {
            currentObjects.Sort();
        }
    }
}
