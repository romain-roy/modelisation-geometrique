using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plan : MonoBehaviour
{
	public Material material;

	public int width;
	public int height;
	public int size;

	private Vector3[] vertices;
	private int[] triangles;
	private int nbTriangles;
	private int nbVertices;

	void Start()
	{
		// Création d'un composant MeshFilter qui peut ensuite être visualisé

		gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();
        
		nbTriangles = width * height * 6;
		nbVertices = (width + 1) * (height + 1);

		vertices = new Vector3[nbVertices];
		triangles = new int[nbTriangles];

		// Création des vertices

		for (int i = 0, x = 0, y = 0; i < nbVertices; i++)
		{
			if (x > width) { x = 0; y++; }
			vertices[i] = new Vector3(x * size, 0, y * size);
			x++;
		}

		// Création des triangles

		for (int i = 0, j = 0, k = 0; i < nbTriangles; i += 6, j++, k++)
		{
			if (k == width) { k = 0; j++; }
			triangles[i] = triangles[i + 3] = j;
			triangles[i + 1] = triangles[i + 5] = j + (width + 1) + 1;
			triangles[i + 2] = j + 1;
			triangles[i + 4] = j + (width + 1);
		}

		// Création et remplissage du Mesh

		Mesh msh = new Mesh();

		msh.vertices = vertices;
		msh.triangles = triangles;

		// Remplissage du Mesh et ajout du material

		gameObject.GetComponent<MeshFilter>().mesh = msh;
		gameObject.GetComponent<MeshRenderer>().material = material;
	}
}
