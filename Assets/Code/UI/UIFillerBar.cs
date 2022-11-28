using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIFillerBar : MonoBehaviour
{
    [SerializeField] bool vertical = false;
    [SerializeField] bool displayPercentage;
    [SerializeField] Image backgroundBar;
    [SerializeField] Image foregroundBar;
    [SerializeField] Text displayText;

    [SerializeField] float animStartDelay = 0.5f;
    [SerializeField] float animationTime = 1;

    public float fillAmount { get; private set; }

    float minAmount;
    float maxAmount;
    float amount;
    float lastAmount;
    bool animated = false;


    float delayTimer = 0;
    float animationTimer = 0;

    public void SetFillAmount(float amount, float displayMin, float displayMax, bool animated = false)
    {

        if (animated)
        {
            if (!this.animated)
            {
                lastAmount = this.amount;
                delayTimer = 0;
            }
            animationTimer = animationTime;
            this.animated = true;
        }
        this.amount = amount;
       
        fillAmount = Utils.Map(amount, displayMin, displayMax, 0, 1);
        minAmount = displayMin;
        maxAmount = displayMax;
    }

    void Update()
    {
        fillAmount = Mathf.Clamp01(fillAmount);

        if (foregroundBar == null) return;
        foregroundBar.fillAmount = fillAmount;

        if (displayText == null) return;
        displayText.text = GetDisplayText();


        if(backgroundBar == null) return;

        if (!animated) { backgroundBar.fillAmount = 0; return; }
        delayTimer += Time.deltaTime;
        backgroundBar.fillAmount = Utils.Map(lastAmount, minAmount, maxAmount, 0, 1);
        if (delayTimer < animStartDelay) return;
        animationTimer -= Time.deltaTime;
        backgroundBar.fillAmount = Utils.Map(animationTimer, 0, animationTime, Utils.Map(amount, minAmount, maxAmount, 0, 1), Utils.Map(lastAmount, minAmount, maxAmount, 0, 1));
        if(animationTimer <= 0)
            animated = false;
    }

    string GetDisplayText()
    {
        if (displayPercentage)
            return ((int)Utils.Map(fillAmount, 0, 1, 0, 100)).ToString() + "%";

        string fillerChar = vertical ? "\n" : " ";

        return ((int)amount) + fillerChar + "/" + fillerChar + ((int)maxAmount);
    }
}
