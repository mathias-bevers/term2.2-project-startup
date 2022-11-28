using System;
using Code.Interaction;
using UnityEngine;

namespace Code.Editor
{
    public class DeveloperShortcuts : MonoBehaviour
    {
        private Survivor survivor;
        private InteractionHandler interactionHandler;

        private void Awake()
        {
            survivor = FindObjectOfType<Survivor>();
            interactionHandler = survivor.GetComponent<InteractionHandler>();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.F1))
            {
                interactionHandler.inventory.Drop<Key>(survivor.transform.position);
            }
        }
    }
}