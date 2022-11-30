using System;
using Code.Interaction;
using UnityEngine;

namespace Code.Editor
{
    public class DeveloperShortcuts : MonoBehaviour
    {
        [SerializeField] private GameObject endingPanel;

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

            if (Input.GetKeyUp(KeyCode.F2))
            {
                //TODO: move this to the survivor, or game handler etc.
                endingPanel.SetActive(true);
                Animation animation = endingPanel.GetComponent<Animation>();
                animation["EndingScreen"].wrapMode = WrapMode.Once;
                animation.Play();
            }
        }
    }
}