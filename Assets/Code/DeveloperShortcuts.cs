using System;
using System.Diagnostics;
using Code.Interaction;
using UnityEngine;
using UnityEditor;

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

#if UNITY_EDITOR
    [CustomEditor(typeof(DeveloperShortcuts))]
    public class DeveloperEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Open persistent data path"))
            {
				string path = Application.persistentDataPath.Replace(@"/", @"\");
                Process.Start("explorer.exe", "/select," + path);
			}
        }
    }
#endif
}