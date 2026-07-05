using UnityEngine;

public sealed class ClickAuraPlane : MonoBehaviour
{
    static readonly int AuraCenterOS = Shader.PropertyToID("_AuraCenterOS");
    static readonly int AuraRadius = Shader.PropertyToID("_AuraRadius");
    static readonly int AuraActive = Shader.PropertyToID("_AuraActive");

    [SerializeField] Camera raycastCamera;
    [SerializeField] Renderer targetRenderer;
    [SerializeField] Collider targetCollider;
    [SerializeField] float expansionSpeed = 2f;
    [SerializeField] LayerMask clickLayers = ~0;

    Material materialInstance;
    Bounds localBounds;
    Vector3 centerLocal;
    float currentRadius;
    float maxRadius;
    bool isPlaying;

    void Awake()
    {
        if (raycastCamera == null)
        {
            raycastCamera = Camera.main;
        }

        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }

        if (targetCollider == null)
        {
            targetCollider = GetComponent<Collider>();
        }

        if (targetRenderer != null)
        {
            materialInstance = targetRenderer.material;
        }

        if (TryGetComponent(out MeshFilter meshFilter) && meshFilter.sharedMesh != null)
        {
            localBounds = meshFilter.sharedMesh.bounds;
        }

        SetAuraActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryStartAura();
        }

        if (!isPlaying)
        {
            return;
        }

        currentRadius += expansionSpeed * Time.deltaTime;
        materialInstance.SetFloat(AuraRadius, currentRadius);

        if (currentRadius >= maxRadius)
        {
            SetAuraActive(false);
        }
    }

    void TryStartAura()
    {
        if (raycastCamera == null || targetCollider == null || materialInstance == null)
        {
            return;
        }

        Ray ray = raycastCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, clickLayers))
        {
            return;
        }

        if (hit.collider != targetCollider)
        {
            return;
        }

        centerLocal = transform.InverseTransformPoint(hit.point);
        currentRadius = 0f;
        maxRadius = CalculateRadiusUntilOutsidePlane(centerLocal);

        materialInstance.SetVector(AuraCenterOS, new Vector4(centerLocal.x, centerLocal.y, centerLocal.z, 0f));
        materialInstance.SetFloat(AuraRadius, currentRadius);
        SetAuraActive(true);
    }

    float CalculateRadiusUntilOutsidePlane(Vector3 localPoint)
    {
        Vector3 min = localBounds.min;
        Vector3 max = localBounds.max;

        float radiusToCornerA = Vector2.Distance(new Vector2(localPoint.x, localPoint.z), new Vector2(min.x, min.z));
        float radiusToCornerB = Vector2.Distance(new Vector2(localPoint.x, localPoint.z), new Vector2(min.x, max.z));
        float radiusToCornerC = Vector2.Distance(new Vector2(localPoint.x, localPoint.z), new Vector2(max.x, min.z));
        float radiusToCornerD = Vector2.Distance(new Vector2(localPoint.x, localPoint.z), new Vector2(max.x, max.z));

        return Mathf.Max(radiusToCornerA, radiusToCornerB, radiusToCornerC, radiusToCornerD);
    }

    void SetAuraActive(bool active)
    {
        isPlaying = active;

        if (materialInstance != null)
        {
            materialInstance.SetFloat(AuraActive, active ? 1f : 0f);
        }
    }
}
