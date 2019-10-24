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
    }

    void Update()
    {
        lineRenderer.positionCount = nbPoints;

        for (int i = 0; i < nbPoints; i++)
        {
            float u = i / (nbPoints - 1f);

            Vector3 position = new Vector3(0f, 0f, 0f);

            for (int j = 0; j < P.Count; j++)
                position += Bernstein(u, j, P.Count - 1) * P[j];

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
