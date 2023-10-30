using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Ekkam {
    public class GeneratorFixing : MonoBehaviour
    {
        public GameObject generatorFixingUI;
        public List<Color> wireColors;
        private List<Color> availableColors;

        public List<Wire> leftWires;
        public List<Wire> rightWires;

        private List<int> availableLeftWireIndex;
        private List<int> availableRightWireIndex;

        public Generator.generatorColor fixingColor;

        public Wire currentDraggedWire;
        public Wire currentHoveredWire;

        public bool generatorFixed = false;

        Player player;

        void Start()
        {
            player = FindObjectOfType<Player>();

            ResetGenerator();

            generatorFixingUI.SetActive(false);
        }

        public async void ResetGenerator()
        {
            await Task.Delay(500);
            // clear all wires
            foreach (Wire wire in leftWires)
            {
                wire.SetColor(Color.white);
            }
            foreach (Wire wire in rightWires)
            {
                wire.SetColor(Color.white);
            }

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

            // reset all wires
            foreach (Wire wire in leftWires)
            {
                wire.isSuccessful = false;
            }
            foreach (Wire wire in rightWires)
            {
                wire.isSuccessful = false;
            }

            generatorFixed = false;

            // reset line renderer for all wires
            foreach (Wire wire in leftWires)
            {
                wire.lineRenderer.SetPosition(0, wire.transform.position);
                wire.lineRenderer.SetPosition(1, wire.transform.position);
            }

            print("Generator reset!");
        }

        void Update()
        {
            if (generatorFixed) return;
            int SuccessfulWires = 0;
            for (int i = 0; i < leftWires.Count; i++)
            {
                if (rightWires[i].isSuccessful)
                {
                    SuccessfulWires++;
                }
            }

            if (SuccessfulWires == leftWires.Count && !generatorFixed)
            {
                FinishFixing();
                generatorFixed = true;
            }
        }

        public void StartFixing(Generator.generatorColor color)
        {
            fixingColor = color;
            player.allowMovement = false;
            generatorFixingUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void FinishFixing()
        {
            print("Generator fixed!");
            player.allowMovement = true;
            generatorFixingUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // find generator with this color and fix it
            Generator[] generators = FindObjectsOfType<Generator>();
            foreach (Generator generator in generators)
            {
                if (generator.color == fixingColor)
                {
                    generator.Fix();
                }
            }
            // find all doors with this color and open them
            Door[] doors = FindObjectsOfType<Door>();
            foreach (Door door in doors)
            {
                switch (door.GetComponent<Door>().color)
                {
                    case Door.doorColor.yellow:
                        if (fixingColor == Generator.generatorColor.yellow)
                        {
                            door.Open();
                        }
                        break;
                    case Door.doorColor.green:
                        if (fixingColor == Generator.generatorColor.green)
                        {
                            door.Open();
                        }
                        break;
                }
            }
        }

        public void EndFixing()
        {
            print("Generator fixing ended!");
            player.allowMovement = true;
            generatorFixingUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ResetGenerator();
        }
    }
}
