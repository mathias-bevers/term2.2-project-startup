using Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetDisplayHandler : InstancedSingleton<TargetDisplayHandler>
{
    [SerializeField]
    Image pointPanel;

    [SerializeField]
    Transform circleObject;

    [SerializeField]
    Killer killer;

    Dictionary<Survivor, Image> survivorPoint = new Dictionary<Survivor, Image>();


    private void Update()
    {
        if (killer == null) return;
        if (killer.cameraRig.getCamera == null) return;
        Camera camera = killer.cameraRig.getCamera;
        foreach(Survivor s in survivorPoint.Keys)
        {
            
            Image img = survivorPoint[s];
            if (img == null) continue;
            Transform t = img.transform;
            t.position = camera.WorldToScreenPoint(s.getPosition);
            float distance = Vector3.Distance(s.getPosition, killer.getPosition);
            t.localScale = Vector3.one * Utils.Map(distance, 0, 20, 2, 0.68f);
            if (distance > killer.maxDetectionDistance)
            {
                t.position = new Vector2(100000, 100000);
            }
        }
    }

    public void RegisterSurvivor(Survivor survivor)
    {
        if (survivor == null) return;
        Transform newObject = Instantiate(circleObject, pointPanel.transform);
        Image img = newObject.GetComponent<Image>();
        if (img == null)
        {
            Destroy(newObject.gameObject);
            return;
        }
        survivorPoint.Add(survivor, img); 
    }

    public void DeregisterSurvivor(Survivor survivor)
    {
        if (survivor == null) return;
        Image img = survivorPoint[survivor];
        survivorPoint.Remove(survivor);
        if (img == null) return;
        GameObject o = img.gameObject;
        if (o != null) Destroy(o);
    }
}
