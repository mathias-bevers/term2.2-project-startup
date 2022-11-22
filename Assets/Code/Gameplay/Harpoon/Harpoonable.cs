using System.Collections.Generic;
using UnityEngine;

public enum HarpoonType
{
    Player,
    PullTowards,
    PullAway,
    Break
}

public class Harpoonable : MonoBehaviour
{
    [SerializeField] HarpoonType _harpoonType = HarpoonType.Break;
    [SerializeField] Vector3 _hitOffset;
    public Vector3 hitOffset { get => _hitOffset;  }
    public HarpoonType harpoonType { get => _harpoonType; }

    public float canSurviveHarpoonTime = 5;

    [SerializeField] List<MonoBehaviour> disableOnHit;
    [SerializeField] List<MonoBehaviour> enableOnHit;

    public void OnHit(HarpoonInfo info)
    {
        for(int i = 0; i < disableOnHit.Count; i++)
            disableOnHit[i].enabled = false;

        for (int i = 0; i < enableOnHit.Count; i++)
        {
            enableOnHit[i].enabled = true;
            if (enableOnHit[i] is HarpoonInfoRelay)
                ((HarpoonInfoRelay)enableOnHit[i]).info = info;
            
        }
    }

    public void OnBreak()
    {
        for (int i = 0; i < disableOnHit.Count; i++)
            disableOnHit[i].enabled = true;

        for (int i = 0; i < enableOnHit.Count; i++)
        {
            enableOnHit[i].enabled = false;
            if (enableOnHit[i] is HarpoonInfoRelay)
                ((HarpoonInfoRelay)enableOnHit[i]).info = null;
        }
    }
}
