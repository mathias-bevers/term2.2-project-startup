using Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartupGame : MonoBehaviour
{
    [SerializeField] ControlableEntity[] entities;

    float timer = 0;
    float startupTime = 5;

    [SerializeField] Image[] images;
    [SerializeField] Text[] texts;

    private void Start()
    {
        for(int i = 0; i <entities.Length; i++)
        {
            if (entities[i] == null) continue;
            entities[i].inputModule.enabled = false;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        for(int i =0; i < images.Length; i++)
        {
            images[i].fillAmount = Utils.Map(timer, 0, 2, 1, 0);
        }

        if (timer > 1.5f && timer <= 4.5f)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].text = (3 - ((int)(timer - 1.5f))).ToString();
            }
        }
        if(timer >= 4.5f)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].text = "GO!";
            }
        }

        if (timer > startupTime)
        {
            for (int i = 0; i < entities.Length; i++)
            {
                if (entities[i] == null) continue;
                entities[i].inputModule.enabled = true;
            }
        }
        if (timer > startupTime + 0.5f)
            Destroy(gameObject);
    }
}
