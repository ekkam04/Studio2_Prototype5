using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ekkam {
    public class GeneratorFixing : MonoBehaviour
    {
        public List<Color> wireColors;
        private List<Color> availableColors;

        public List<Wire> leftWires;
        public List<Wire> rightWires;

        private List<int> availableLeftWireIndex;
        private List<int> availableRightWireIndex;

        void Start()
        {
            availableColors = new List<Color>(wireColors);
            availableLeftWireIndex = new List<int>();
            availableRightWireIndex = new List<int>();

            for (int i = 0; i < leftWires.Count; i++)
            {
                availableLeftWireIndex.Add(i);
            }

            for (int i = 0; i < rightWires.Count; i++)
            {
                availableRightWireIndex.Add(i);
            }

            while (availableColors.Count > 0 && availableLeftWireIndex.Count > 0 && availableRightWireIndex.Count > 0)
            {
                Color pickedColor = availableColors[Random.Range(0, availableColors.Count)];
                int pickedLeftWireIndex = availableLeftWireIndex[Random.Range(0, availableLeftWireIndex.Count)];
                int pickedRightWireIndex = availableRightWireIndex[Random.Range(0, availableRightWireIndex.Count)];

                leftWires[pickedLeftWireIndex].SetColor(pickedColor);
                rightWires[pickedRightWireIndex].SetColor(pickedColor);

                availableColors.Remove(pickedColor);
                availableLeftWireIndex.Remove(pickedLeftWireIndex);
                availableRightWireIndex.Remove(pickedRightWireIndex);
            }
        }

        void Update()
        {
            
        }
    }
}
