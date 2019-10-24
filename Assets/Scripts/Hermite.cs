using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hermite : MonoBehaviour
{
    public Material material;
    public Vector3 P0, P1, V0, V1;
    public int nbPoints = 20;

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
            float F1 = 2f * u * u * u - 3f * u * u + 1f;
            float F2 = -2f * u * u * u + 3f * u * u;
            float F3 = u * u * u - 2f * u * u + u;
            float F4 = u * u * u - u * u;
            Vector3 position = F1 * P0 + F2 * P1 + F3 * V0 + F4 * V1;
            lineRenderer.SetPosition(i, position);
        }
    }
}