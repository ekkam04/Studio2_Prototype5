using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ekkam {
public class Door : MonoBehaviour
    {
        public enum doorColor { red, blue, yellow, green };
        public doorColor color;

        [SerializeField] GameObject frontLock;
        [SerializeField] GameObject backLock;

        Animator anim;
        bool isOpen = false;

        void Start()
        {
            anim = GetComponent<Animator>();
            switch (color)
            {
                case doorColor.red:
                    foreach (Transform child in frontLock.transform)
                    {
                        child.GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                    foreach (Transform child in backLock.transform)
                    {
                        child.GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                    break;
                case doorColor.blue:
                    foreach (Transform child in frontLock.transform)
                    {
                        child.GetComponent<MeshRenderer>().material.color = Color.blue;
                    }
                    foreach (Transform child in backLock.transform)
                    {
                        child.GetComponent<MeshRenderer>().material.color = Color.blue;
                    }
                    break;
                case doorColor.yellow:
                    foreach (Transform child in frontLock.transform)
                    {
                        child.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    }
                    foreach (Transform child in backLock.transform)
                    {
                        child.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    }
                    break;
                case doorColor.green:
                    foreach (Transform child in frontLock.transform)
                    {
                        child.GetComponent<MeshRenderer>().material.color = Color.green;
                    }
                    foreach (Transform child in backLock.transform)
                    {
                        child.GetComponent<MeshRenderer>().material.color = Color.green;
                    }
                    break;
                default:
                    break;
            }
        }

        public void Open()
        {
            if (!isOpen)
            {
                anim.SetBool("open", true);
                isOpen = true;
                GetComponent<BoxCollider>().enabled = false;
            }
        }

        private void OnTriggerEnter(Collider other) {
            
        }
    }
}
