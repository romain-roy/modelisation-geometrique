using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylindre : MonoBehaviour
{
	public Material material;

	public int radius;
	public int height;
	public int nbMeridians;

	private Vector3[] vertices;
	private int[] triangles;
	private int nbTriangles;
	private int nbVertices;

	void Start()
	{
		// Création d'un composant MeshFilter qui peut ensuite être visualisé

		gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();

		nbTriangles = nbMeridians * 12;
		nbVertices = nbMeridians * 2 + 4;

		vertices = new Vector3[nbVertices];
		triangles = new int[nbTriangles];

		// Création des vertices

		float x, y;

		for (int i = 0; i <= nbMeridians; i++)
		{
			x = radius * Mathf.Cos(2 * Mathf.PI * i / nbMeridians);
			y = radius * Mathf.Sin(2 * Mathf.PI * i / nbMeridians);
			vertices[i] = new Vector3(x, 0, y);
			vertices[i + nbMeridians + 1] = new Vector3(x, height, y);
		}

		vertices[nbVertices - 2] = new Vector3(0, 0, 0);
		vertices[nbVertices - 1] = new Vector3(0, height, 0);

		// Création des triangles

		int nbTrianglesCorps = nbMeridians * 6;

		for (int i = 0, k = 0; i < nbTrianglesCorps; i += 6, k++)
		{
			triangles[i] = triangles[i + 3] = k;
			triangles[i + 1] = k + nbMeridians + 1;
			triangles[i + 2] = triangles[i + 4] = k + nbMeridians + 2;
			triangles[i + 5] = k + 1;
		}

		int nbTrianglesCouvercle = nbMeridians * 6;

		for (int i = 0, k = 0; i < nbTrianglesCouvercle; i += 6, k++)
		{
			triangles[i + nbTrianglesCorps] = k;
			triangles[i + nbTrianglesCorps + 1] = k + 1;
			triangles[i + nbTrianglesCorps + 2] = nbVertices - 2;
			triangles[i + nbTrianglesCorps + 3] = k + nbMeridians + 3;
			triangles[i + nbTrianglesCorps + 4] = k + nbMeridians + 2;
			triangles[i + nbTrianglesCorps + 5] = nbVertices - 1;
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
