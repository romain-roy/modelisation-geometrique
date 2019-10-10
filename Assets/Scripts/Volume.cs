using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour
{
    [System.Serializable]
    public class Sphere
    {
        public Vector3 position;
        public float rayon;

        public Sphere(Vector3 position, float rayon)
        {
            this.position = position;
            this.rayon = rayon;
        }
    }

    public Transform cube;
    public float tailleCube = 1.0f;
    public bool intersection;
    public List<Sphere> spheres;

    private bool hasIntersection;

    void Start()
    {
        float minX = spheres[0].position.x - spheres[0].rayon;
        float minY = spheres[0].position.y - spheres[0].rayon;
        float minZ = spheres[0].position.z - spheres[0].rayon;
        float maxX = spheres[0].position.x + spheres[0].rayon;
        float maxY = spheres[0].position.y + spheres[0].rayon;
        float maxZ = spheres[0].position.z + spheres[0].rayon;

        for (int i = 1; i < spheres.Count; i++)
        {
            if (spheres[i].position.x - spheres[i].rayon < minX) minX = spheres[i].position.x - spheres[i].rayon;
            if (spheres[i].position.y - spheres[i].rayon < minY) minY = spheres[i].position.y - spheres[i].rayon;
            if (spheres[i].position.z - spheres[i].rayon < minZ) minZ = spheres[i].position.z - spheres[i].rayon;
            if (spheres[i].position.x + spheres[i].rayon > maxX) maxX = spheres[i].position.x + spheres[i].rayon;
            if (spheres[i].position.y + spheres[i].rayon > maxY) maxY = spheres[i].position.y + spheres[i].rayon;
            if (spheres[i].position.z + spheres[i].rayon > maxZ) maxZ = spheres[i].position.z + spheres[i].rayon;
        }

        Vector3 boiteEnglobante = new Vector3(maxX - minX, maxY - minY, maxZ - minZ);

        Debug.Log("Nombre de cubes\nX : " + minX + " à " + maxX + ", Y : " + minY + " à " + maxY + ", Z : " + minZ + " à " + maxZ);

        for (float x = -boiteEnglobante.x; x < boiteEnglobante.x; x++)
        {
            for (float y = -boiteEnglobante.y; y < boiteEnglobante.y; y++)
            {
                for (float z = -boiteEnglobante.z; z < boiteEnglobante.z; z++)
                {
                    if (intersection)
                    {
                        hasIntersection = true;
                        foreach (Sphere s in spheres)
                        {
                            hasIntersection &= (x - s.position.x) * (x - s.position.x) + (y - s.position.y) * (y - s.position.y) + (z - s.position.z) * (z - s.position.z) - s.rayon * s.rayon < 0.0f;
                        }
                        if (hasIntersection)
                            {
                                Transform newCube = Instantiate(cube, new Vector3(x * tailleCube, y * tailleCube, z * tailleCube), Quaternion.identity);
                                newCube.localScale = new Vector3(tailleCube, tailleCube, tailleCube);
                            }
                    }
                    else
                    {
                        foreach (Sphere s in spheres)
                        {
                            if ((x - s.position.x) * (x - s.position.x) + (y - s.position.y) * (y - s.position.y) + (z - s.position.z) * (z - s.position.z) - s.rayon * s.rayon < 0.0f)
                            {
                                Transform newCube = Instantiate(cube, new Vector3(x * tailleCube, y * tailleCube, z * tailleCube), Quaternion.identity);
                                newCube.localScale = new Vector3(tailleCube, tailleCube, tailleCube);
                            }
                        }
                    }
                }
            }
        }

    }
}
