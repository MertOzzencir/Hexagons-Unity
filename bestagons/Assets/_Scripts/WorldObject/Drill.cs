using System.Collections;
using UnityEngine;

public class Drill : MonoBehaviour
{
    [SerializeField] private float drillTimer = 2f;

    public ExtractorBase CurrentBase;
    private ResourceHexTile currentTile;
    private float timer;
    Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void InitilizeDrill(ResourceHexTile current, ExtractorBase currentBase)
    {
        currentTile = current;
        rb.isKinematic = true;
        this.CurrentBase = currentBase;
        transform.parent = currentBase.DrillPlacement.transform;
        transform.position = currentBase.DrillPlacement.transform.position;
    }
    public void Update()
    {
        timer += Time.deltaTime;
        if (timer > drillTimer)
        {
            currentTile.Dig(out Materials currentDiggingMaterial);
            CurrentBase.BaseFeeder.StoreMaterial(currentDiggingMaterial);
            timer = 0f;
        }
    }

    void OnDisable()
    {
        timer = 0;
    }
}
public enum DrillType { Normal, Crusher, Heated }
