using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speed Boost Perk", menuName ="Perks/SpeedBoost",order=0)]
public class SpeedBoostPerk : Perk
{
    public override void OnPerkOnCooldown()
    {
        base.OnPerkOnCooldown();
    }
}
