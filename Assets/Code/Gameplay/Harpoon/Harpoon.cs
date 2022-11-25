using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Harpoon : MonoBehaviour
{
    public MovementHarpoon shotFrom;
    [SerializeField] Transform shootPoint;

    LineRenderer line;
    [SerializeField] float speed = 10;
    [SerializeField] float _harpoonStrength = 3;

    public float harpoonStrength { get => _harpoonStrength; }
    public Transform getShootPoint { get => shootPoint; }

    [SerializeField] LayerMask mask;

    bool hitTarget = false;

    Harpoonable hitMe = null;
    Vector3 offsetHit => shotFrom.harpoonable.hitOffset;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        mask &= ~(1 << shotFrom.transform.gameObject.layer);
        line.SetPositions(new Vector3[2] { shotFrom.transform.position + offsetHit, shootPoint.position });
        if (hitTarget) 
        {
            shootPoint.position = hitMe.transform.position + hitMe.hitOffset;
            return; 
        }
        shootPoint.localPosition += new Vector3(0, 0, speed * Time.deltaTime);


        RaycastHit hit;

        if (Physics.Linecast(shotFrom.transform.position + offsetHit, shootPoint.position, out hit, mask, QueryTriggerInteraction.Ignore))
        {
            Harpoonable hp = hit.collider.GetComponent<Harpoonable>();
            if (hp == null) BreakHarpoon();
            else HitHarpoonable(hp);
        }

        if (Vector3.Distance(shootPoint.position, shotFrom.transform.position) > 140)
            BreakHarpoon();
    }

    void HitHarpoonable(Harpoonable hp)
    {
        hitTarget = true;
        hitMe = hp;
        hitMe.OnHit(shotFrom.OnHarpoonCallback(hp));

    }

    public void BreakHarpoon()
    {
        shotFrom.OnHarpoonCallback(null);
        if (hitMe != null)
            hitMe.OnBreak();
        hitMe = null;
        hitTarget = false;
        Destroy(gameObject);
    }
}
