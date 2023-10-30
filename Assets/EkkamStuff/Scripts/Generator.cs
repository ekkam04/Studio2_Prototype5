using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ekkam {
    public class Generator : MonoBehaviour
    {
        GeneratorFixing generatorFixing;
        public enum generatorColor { yellow, green };
        public generatorColor color;
        public bool isFixed = false;

        public GameObject[] objectsToEnableOnFix;

        Player player;

        void Start()
        {
            player = FindObjectOfType<Player>();
            generatorFixing = FindObjectOfType<GeneratorFixing>();
            foreach (GameObject obj in objectsToEnableOnFix)
            {
                obj.SetActive(false);
            }
        }

        public void StartFixing()
        {
            generatorFixing.StartFixing(color);
            player.rb.velocity = Vector3.zero;
        }

        public void Fix()
        {
            isFixed = true;
            foreach (GameObject obj in objectsToEnableOnFix)
            {
                obj.SetActive(true);
            }
            player.audioSource.PlayOneShot(player.doorUnlockSound);
        }
    }
}
