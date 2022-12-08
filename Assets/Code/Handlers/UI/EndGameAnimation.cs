using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameAnimation : MonoBehaviour
{
    [SerializeField] Canvas[] canvasses;

    [SerializeField] Image[] images;
    [SerializeField] Text[] texts;

    bool survivorsWin;
    float timer = 0;

    void Start()
    {
        for(int i = 0; i < canvasses.Length; i++)
        {
            canvasses[i].enabled = false;
        }
    }

    public void SetWin(bool survivorsWin)
    {
        this.survivorsWin = survivorsWin;
        gameObject.SetActive(true);
    }

    void Update()
    {
        for (int i = 0; i < canvasses.Length; i++)
        {
            canvasses[i].enabled = true;
        }
        timer += Time.deltaTime;
        for(int i = 0; i < images.Length; i++)
        {
            images[i].fillAmount = Mathf.Clamp01(Utils.Map(timer, 0, 2, 0, 1));
        }
        if(timer >= 2)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].text = (survivorsWin ? "The survivors Won!" : "The killer Won!");

            }
        }

        if(timer >= 5)
        {
            Destroy(gameObject);
        }
    }
}
