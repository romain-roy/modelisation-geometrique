using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour
{
    public Transform cube;
    public int nombreCubes;
    public float rayonSphere;

    void Start()
    {
        float offset = nombreCubes / 2.0f;
        for (float x = -offset; x < nombreCubes; x++)
        {
            for (float y = -offset; y < nombreCubes; y++)
            {
                for (float z = -offset; z < nombreCubes; z++)
                {
                    if (x * x + y * y + z * z - rayonSphere * rayonSphere < 0)
                        Instantiate(cube, new Vector3(x, y, z), Quaternion.identity);
                }
            }
        }

    }
}
