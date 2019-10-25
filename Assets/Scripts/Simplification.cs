using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simplification : MonoBehaviour
{
    struct Cube
    {
        public Vector3 posMin;
        public Vector3 posMax;
    }

    public Material material;
    public string nomFichier;
    public int taille;
    public bool centrer;
    public bool normaliserTaille;
    public float division;

    private Vector3[] vertices;
    private Vector3[] normals;
    private int[] triangles;
    private int nbTriangles;
    private int nbVertices;
    private List<Cube> matrix;

    void Start()
    {
        // Création d'un composant MeshFilter qui peut ensuite être visualisé

        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        // Lecture du fichier

        string[] reader = System.IO.File.ReadAllLines("Assets/Meshs/" + nomFichier + ".off");

        string[] nombres = reader[1].Split(' ');

        nbVertices = int.Parse(nombres[0]);
        nbTriangles = int.Parse(nombres[1]) * 3;

        vertices = new Vector3[nbVertices];
        normals = new Vector3[nbVertices];
        triangles = new int[nbTriangles];

        for (int i = 2, v = 0, t = 0; i < reader.Length; i++)
        {
            nombres = reader[i].Split(' ');
            if (i < nbVertices + 2)
            {
                vertices[v] = new Vector3(float.Parse(nombres[0].Replace('.', ',')), float.Parse(nombres[1].Replace('.', ',')), float.Parse(nombres[2].Replace('.', ',')));
                v++;
            }
            else
            {
                triangles[t] = int.Parse(nombres[1]);
                triangles[t + 1] = int.Parse(nombres[2]);
                triangles[t + 2] = int.Parse(nombres[3]);

                // Normales

                Vector3 edge1 = vertices[triangles[t + 1]] - vertices[triangles[t]];
                Vector3 edge2 = vertices[triangles[t + 2]] - vertices[triangles[t]];
                Vector3 normal = Vector3.Normalize(Vector3.Cross(edge1, edge2));
                normals[triangles[t]] += normal;
                normals[triangles[t + 1]] += normal;
                normals[triangles[t + 2]] += normal;

                t += 3;
            }
        }

        // Normaliser sa taille

        float ratioTaille = 1.0f;

        if (normaliserTaille)
        {
            float normMax = vertices[0].magnitude;

            for (int i = 1; i < nbVertices; i++)
                if (vertices[i].magnitude > normMax)
                    normMax = vertices[i].magnitude;

            ratioTaille = normMax / taille;

            for (int i = 0; i < nbVertices; i++)
                vertices[i] /= ratioTaille;
        }

        // Centrer l'objet

        if (centrer)
        {
            Vector3 sommeVertices = vertices[0];

            for (int i = 1; i < nbVertices; i++)
                sommeVertices += vertices[i];

            Vector3 centreGravite = sommeVertices /= nbVertices;

            gameObject.transform.position = centreGravite / ratioTaille;
        }

        // Simplification

        if (division > 0)
        {
            Vector3 posMin = vertices[0];
            Vector3 posMax = vertices[0];
            for (int i = 1; i < vertices.Length; i++)
            {
                if (posMin.x > vertices[i].x) posMin.x = vertices[i].x;
                if (posMin.y > vertices[i].y) posMin.y = vertices[i].y;
                if (posMin.z > vertices[i].z) posMin.z = vertices[i].z;
                if (posMax.x < vertices[i].x) posMax.x = vertices[i].x;
                if (posMax.y < vertices[i].y) posMax.y = vertices[i].y;
                if (posMax.z < vertices[i].z) posMax.z = vertices[i].z;
            }
            CreateMatrix(posMin, posMax);
            Simplify();
        }

        // Création et remplissage du Mesh

        Mesh msh = new Mesh();

        msh.vertices = vertices;
        msh.triangles = triangles;
        msh.normals = normals;

        // Remplissage du Mesh et ajout du material

        gameObject.GetComponent<MeshFilter>().mesh = msh;
        gameObject.GetComponent<MeshRenderer>().material = material;
    }

    void CreateMatrix(Vector3 posMin, Vector3 posMax)
    {
        float distanceX = posMax.x - posMin.x;
        float distanceY = posMax.y - posMin.y;
        float distanceZ = posMax.z - posMin.z;

        float dvX = distanceX / division;
        float dvY = (distanceY / division);
        float dvZ = (distanceZ / division);
        matrix = new List<Cube>();

        for (int k = 0; k <= division; k++)
            for (int j = 0; j <= division; j++)
                for (int i = 0; i <= division; i++)
                {
                    Cube cube;
                    cube.posMin = new Vector3(posMin.x + dvX * i, posMin.y + dvY * j, posMin.z + dvZ * k);
                    cube.posMax = new Vector3(posMin.x + dvX * (i + 1), posMin.y + dvY * (j + 1), posMin.z + dvZ * (k + 1));
                    matrix.Add(cube);
                }
    }

    bool InsideTheBox(Cube c, Vector3 pt)
    {
        return (pt.x > c.posMin.x && pt.x < c.posMax.x) && (pt.y > c.posMin.y && pt.y < c.posMax.y) && (pt.z > c.posMin.z && pt.z < c.posMax.z);
    }
    void Simplify()
    {
        foreach (Cube c in matrix)
        {
            List<int> listInCube = new List<int>();
            Vector3 somme = Vector3.zero;
            for (int i = 0; i < vertices.Length; i++)
                if (InsideTheBox(c, vertices[i]))
                {
                    listInCube.Add(i);
                    somme += vertices[i];
                }

            foreach (int t in listInCube)
                vertices[t] = somme / listInCube.Count;
        }
    }
}
