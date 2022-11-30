using Code;
using UnityEngine;

public class Perk : ScriptableObject
{
    public string perkName = "Perk";
    public string perkDescription = "Perk_Description";
    public Sprite perkImage;
    public int activateCount = -1;
    public float cooldown = 10;
    public bool startOnCooldown = true;

    //public InputSettings inputSettings;
    public InputSettingsEnum callbackOnInput;

    float timer = 0;

    bool switchedOffOfCooldown = false;

    bool onCooldown = false;

    Entity entity;

    public void Start()
    {
        OnPerkStart();
    }

    public void RegisterEntity(Entity entity)
    {
        this.entity = entity;
    }

    

    public void InternalUpdate()
    {
        if (timer <= cooldown)
            timer += Time.deltaTime;
        if (startOnCooldown)
            timer = cooldown;
        if(timer >= cooldown && !switchedOffOfCooldown)
        {
            PerkOffCooldown();
            switchedOffOfCooldown = true;
        }
        OnInputReceive();
        OnPerkUpdate();
    }

    private void OnInputReceive()
    {
        //if (inputSettings == null) return;
        //if (!inputSettings.IsDown(callbackOnInput)) return;
        PerkOnCooldown();
    }

    private void PerkOffCooldown()
    {
        onCooldown = false;
        OnPerkOffCooldown();
    }

    private void PerkOnCooldown()
    {
        timer = 0;
        onCooldown = true;
        OnPerkOnCooldown();
    }

    public virtual void OnPerkStart() { }
    public virtual void OnPerkUpdate() { }
    public virtual void OnPerkEnd() { }
    public virtual void OnPerkOffCooldown() { }
    public virtual void OnPerkOnCooldown() { }
}
