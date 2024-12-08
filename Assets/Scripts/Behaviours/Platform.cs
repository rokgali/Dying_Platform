using UnityEngine;

public class Platform : MonoBehaviour, IPickable
{
    public Vector3 Position { get; private set; }
    private bool IsPickedUp { get; set; }
    private Color OriginalColor { get; set; }
    private bool IsHighlighted { get; set; } = false;
    public MeshRenderer MeshRenderer { get; private set; }
    [field:SerializeField] public float PickupTimeMilliseconds { get; private set; }
    [field: SerializeField] public float Weight { get; private set; }

    private void Start()
    {
        this.Position = transform.position;
        PickupTimeMilliseconds = 1000;

        MeshRenderer = GetComponent<MeshRenderer>();
        // MeshRenderer.sharedMaterial = new Material(MeshRenderer.sharedMaterial);
        OriginalColor = MeshRenderer.sharedMaterial.color;
    }

    public void Highlight(bool highlight)
    {
        if(MeshRenderer == null) return;

        if(highlight)
        {
            MeshRenderer.material.SetColor("_Color", Color.red);
            IsHighlighted = true;
        }
        else
        {
            MeshRenderer.material.SetColor("_Color", OriginalColor);
            IsHighlighted = false;
        }
    }

    public void Move(Vector3 carryPosition, float elapsedCarryTimeMilliseconds, float maxElapsedCarryTimeMilliseconds)
    {
        IsPickedUp = true;

        Position = carryPosition;
        transform.position = carryPosition;

        if(elapsedCarryTimeMilliseconds >= maxElapsedCarryTimeMilliseconds)
        {
            IsPickedUp = false;

            Destroy(gameObject);
        }
    }

    public void Launch(Vector3 direction, float launchPower)
    {
        if (IsPickedUp)
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();

            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }

            rb.AddForce(direction * launchPower, ForceMode.Impulse);
            IsPickedUp = false;
        }
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public MeshRenderer GetMeshRenderer()
    {
        return MeshRenderer;
    }

    public Vector3 GetPosition()
    {
        return Position;
    }

    public bool GetIsHighlighted()
    {
        return IsHighlighted;
    }

    public float GetPickableObjectPickupTimeMilliseconds()
    {
        return PickupTimeMilliseconds;
    }

    public float GetPickupTimeMilliseconds()
    {
        return 1000;
    }

    public float GetWeight()
    {
        return Weight;
    }

    public float GetMaxCarryTimeMilliseconds(float playerStrength)
    {
        return 3000;
    }
}
