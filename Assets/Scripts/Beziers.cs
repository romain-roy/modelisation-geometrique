using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beziers : MonoBehaviour
{
    public Material material;
    public int nbPoints = 20;
    public List<Vector3> P;

    private LineRenderer lineRenderer;

    void Start()
    {
        gameObject.AddComponent<LineRenderer>();
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.startColor = lineRenderer.endColor = Color.white;
        lineRenderer.startWidth = lineRenderer.endWidth = 0.1f;
        lineRenderer.material = material;

        lineRenderer.positionCount = nbPoints;

        Vector3 a = (P[3] - P[2]).normalized;
        a *= (P[3] - P[2]).magnitude;
        a += P[3];

        Vector3 b = (P[4] - P[5]).normalized;
        b *= (P[4] - P[5]).magnitude;
        b += P[4];

        Vector3 v1 = a - P[3];
        Vector3 v2 = b - P[4];

        Vector3 v2p = new Vector3(-v2.y, v2.x, v2.z);

        float t = (Vector3.Dot(b, v2p) - Vector3.Dot(a, v2p)) / Vector3.Dot(v1, v2p);

        Vector3 intersection = a + t * v1;

        P[3] = intersection;
        P[4] = a;
        P.RemoveAt(5);

        for (int i = 0; i < 20; i++)
        {
            float u = i / (20 - 1f);

            Vector3 position = new Vector3(0f, 0f, 0f);

            for (int j = 0; j < 4; j++)
                position += Bernstein(u, j, 3) * P[j];

            lineRenderer.SetPosition(i, position);
        }

        for (int i = 20; i < 40; i++)
        {
            float u = (i - 20) / (20 - 1f);

            Vector3 position = new Vector3(0f, 0f, 0f);

            for (int j = 3; j < 7; j++)
                position += Bernstein(u, j - 3, 3) * P[j];

            lineRenderer.SetPosition(i, position);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < P.Count - 1; i++)
            Gizmos.DrawLine(P[i], P[i + 1]);
    }

    static int Factorial(int n)
    {
        return n > 1 ? n * Factorial(n - 1) : 1;
    }

    float Bernstein(float u, int i, int n)
    {
        return Factorial(n) / (Factorial(i) * Factorial(n - i)) * Mathf.Pow(u, i) * Mathf.Pow(1f - u, n - i);
    }
}
