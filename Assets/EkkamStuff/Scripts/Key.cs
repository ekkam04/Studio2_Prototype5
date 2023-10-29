using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ekkam {
    public class Key : MonoBehaviour
    {
        public enum keyColor { red, blue };
        public keyColor color;

        [SerializeField] Texture2D redKeyTexture;
        [SerializeField] Texture2D blueKeyTexture;

        bool isUsed = false;
        float timer = 0f;

        Player player;

        void Start()
        {
            player = GameObject.Find("Player").GetComponent<Player>();

            foreach (Transform child in transform)
            {
                switch (color)
                {
                    case keyColor.red:
                        child.GetComponent<MeshRenderer>().material.color = Color.red;
                        GetComponent<Item>().itemTexture = redKeyTexture;
                        break;
                    case keyColor.blue:
                        child.GetComponent<MeshRenderer>().material.color = Color.blue;
                        GetComponent<Item>().itemTexture = blueKeyTexture;
                        break;
                    default:
                        break;
                }
            }
        }

        void Update()
        {
            if (isUsed)
            {
                timer += Time.deltaTime;
                if (timer >= 1f)
                {
                    isUsed = false;
                    timer = 0f;
                }

                // spin the key 180
                Quaternion targetRotation = Quaternion.Euler(0f, 180f, 0f);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
            else
            {
                print("not used");
                isUsed = false;
                timer = 0f;
                // spin the key 0
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }

        public void UseKey()
        {
            isUsed = true;
            player.InteractWithDoor();
        }
    }
}
