using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    public enum ZoneShape { Circle, Box }

    [SerializeField] private ZoneShape shape = ZoneShape.Circle;

    // Circle params
    [SerializeField] private float minRadius = 2f;
    [SerializeField] private float maxRadius = 8f;

    // Box params
    [SerializeField] private Vector2 size = new(10f, 10f); // X and Z

    public Vector3 GetRandomPoint()
    {
        Vector3 local = shape switch
        {
            ZoneShape.Circle => GetCirclePoint(),
            ZoneShape.Box    => GetBoxPoint(),
            _                => Vector3.zero
        };
        return transform.TransformPoint(local);
    }

    private Vector3 GetCirclePoint()
    {
        // Uniform distribution — sqrt avoids clustering at center
        float radius = Mathf.Sqrt(Random.Range(minRadius * minRadius, maxRadius * maxRadius));
        float angle  = Random.Range(0f, Mathf.PI * 2f);
        return new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
    }

    private Vector3 GetBoxPoint()
    {
        float x = Random.Range(-size.x * 0.5f, size.x * 0.5f);
        float z = Random.Range(-size.y * 0.5f, size.y * 0.5f);
        return new Vector3(x, 0f, z);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 0.8f, 0.4f, 0.35f);
        if (shape == ZoneShape.Circle)
        {
            Gizmos.DrawWireSphere(transform.position, maxRadius);
            Gizmos.color = new Color(0.2f, 0.8f, 0.4f, 0.15f);
            Gizmos.DrawWireSphere(transform.position, minRadius);
        }
        else
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(size.x, 0.1f, size.y));
        }
    }
#endif
}
