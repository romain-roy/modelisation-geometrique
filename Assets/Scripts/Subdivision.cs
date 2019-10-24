using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subdivision : MonoBehaviour
{
    [Range(0, 5)]
    public int nbChaikin;
    public List<Vector3> pointsChaikin;
    public List<Vector3> pointsBase;

    void Start()
    {
        // Crée un carré

        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 2; j++)
                if (i % 2 == 0) pointsBase.Add(new Vector3(i, j, 0));
                else pointsBase.Add(new Vector3(i, 1 - j, 0));

        pointsChaikin = pointsBase;

        Debug.Log("Appuyez sur Espace pour actualiser le nombre de Chakin");
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            pointsChaikin = pointsBase;
            for (int i = 0; i < nbChaikin; i++)
                pointsChaikin = Chaikin(pointsChaikin);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        for (int k = 0; k < pointsChaikin.Count; k++)
            Gizmos.DrawLine(5 * pointsChaikin[k], 5 * pointsChaikin[(k + 1) % pointsChaikin.Count]);
        Gizmos.color = Color.cyan;
        for (int k = 0; k < pointsBase.Count; k++)
            Gizmos.DrawLine(5 * pointsBase[k], 5 * pointsBase[(k + 1) % pointsBase.Count]);
    }

    List<Vector3> Chaikin(List<Vector3> pointsChaikin)
    {
        List<Vector3> newPoints = new List<Vector3>();
        for (int i = 0; i < pointsChaikin.Count; i++)
        {
            newPoints.Add(0.75f * pointsChaikin[i % pointsChaikin.Count] + 0.25f * pointsChaikin[(i + 1) % pointsChaikin.Count]);
            newPoints.Add(0.25f * pointsChaikin[i % pointsChaikin.Count] + 0.75f * pointsChaikin[(i + 1) % pointsChaikin.Count]);
        }
        return newPoints;
    }
}
