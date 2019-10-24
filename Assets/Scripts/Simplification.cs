using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using UnityEngine;


public class Simplification : MonoBehaviour
{
    public float division = 5f;
    public GameObject sphere;
    List<Cube> matrix;
    struct Cube
    {
        public Vector3 minPos;
        public Vector3 maxPos;
    }


    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    // Start is called before the first frame update
    void Start()
    {
        OpenMesh();
        createMatrix();
        simplification();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OpenMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        string[] lines = System.IO.File.ReadAllLines(@"Assets/triceratops.off");
        string[] tmpVertex = lines[1].Split(' ');
        int nbvertex = int.Parse(tmpVertex[0]);
        int nbTriangles = int.Parse(tmpVertex[1]);

        vertices = new Vector3[nbvertex];
        triangles = new int[nbTriangles * 3];

        //recupere les points
        for (int i = 0; i < nbvertex; i++)
        {
            string[] coord = lines[2 + i].Split(' ');
            vertices[i] = new Vector3(float.Parse(coord[0], CultureInfo.InvariantCulture),
                                      float.Parse(coord[1], CultureInfo.InvariantCulture),
                                      float.Parse(coord[2], CultureInfo.InvariantCulture));
        }


        //recuperer les indices des triangles
        int triangle = 0;
        for (int i = 0; i < nbTriangles; i++)
        {
            string[] index = lines[2 + i + nbvertex].Split(' ');
            triangles[triangle] = int.Parse(index[1]);
            triangles[triangle + 1] = int.Parse(index[2]);
            triangles[triangle + 2] = int.Parse(index[3]);

            triangle += 3;
        }
    }

    Vector3 PosMax()
    {
        Vector3 posMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        for (int i = 0; i < vertices.Length; i++)
        {
            if (posMax.x < vertices[i].x) posMax.x = vertices[i].x;
            if (posMax.y < vertices[i].y) posMax.y = vertices[i].y;
            if (posMax.z < vertices[i].z) posMax.z = vertices[i].z;
        }
        return posMax;
    }

    Vector3 PosMin()
    {
        Vector3 posMin = new Vector3(0, 0, 0);
        for (int i = 0; i < vertices.Length; i++)
        {
            if (posMin.x > vertices[i].x) posMin.x = vertices[i].x;
            if (posMin.y > vertices[i].y) posMin.y = vertices[i].y;
            if (posMin.z > vertices[i].z) posMin.z = vertices[i].z;
        }
        return posMin;
    }
    void createMatrix()
    {
        Vector3 minpos = PosMin();
        Vector3 maxpos = PosMax();

        float distanceX = maxpos.x - minpos.x;
        float distanceY = maxpos.y - minpos.y;
        float distanceZ = maxpos.z - minpos.z;

        float dvX = distanceX / division;
        Debug.Log(division);
        float dvY = (distanceY / division);
        float dvZ = (distanceZ / division);
        matrix = new List<Cube>();

        for (int k = 0; k <= division; k++)
        {
            for (int j = 0; j <= division; j++)
            {
                for (int i = 0; i <= division; i++)
                {
                    Cube cube;
                    cube.minPos = new Vector3(minpos.x + dvX * i, minpos.y + dvY * j, minpos.z + dvZ * k);
                    cube.maxPos = new Vector3(minpos.x + dvX * (i + 1), minpos.y + dvY * (j + 1), minpos.z + dvZ * (k + 1));
                    //Instantiate(sphere, cube.minPos, Quaternion.identity);
                    //Instantiate(sphere, cube.maxPos, Quaternion.identity);
                    matrix.Add(cube);
                }
            }
        }
    }

    bool InsideTheBox(Cube c, Vector3 pt)
    {
        if ((pt.x > c.minPos.x && pt.x < c.maxPos.x) && (pt.y > c.minPos.y && pt.y < c.maxPos.y) && (pt.z > c.minPos.z && pt.z < c.maxPos.z))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void simplification()
    {
        foreach (Cube c in matrix)
        {
            List<int> listInCube = new List<int>();
            Vector3 somme = Vector3.zero;
            for (int i = 0; i < vertices.Length; i++)
            {

                if (InsideTheBox(c, vertices[i]))
                {
                    listInCube.Add(i);
                    somme += vertices[i];
                }
            }

            //fait la moyenne des points dans ma cellule
            foreach (int t in listInCube)
            {
                vertices[t] = somme / listInCube.Count;
            }
        }
    }


}
