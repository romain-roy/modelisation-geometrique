using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
	public Material material;

	public float radius;
	public int nbParallels;
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
	}

	void Update()
	{
		nbTriangles = (nbMeridians * 6) * (nbParallels + 1) + (nbMeridians * 6);
		nbVertices = (nbMeridians + 1) * (nbParallels + 2) + 2;

		vertices = new Vector3[nbVertices];
		triangles = new int[nbTriangles];

		// Création des vertices

		float x, y, z, phi, teta;

		for (int i = 0; i <= nbMeridians; i++)
			for (int j = 0; j <= nbParallels; j++)
			{
				phi = Mathf.PI * j / nbParallels;
				teta = 2 * Mathf.PI * i / nbMeridians;
				x = radius * Mathf.Sin(phi) * Mathf.Cos(teta);
				y = radius * Mathf.Sin(phi) * Mathf.Sin(teta);
				z = radius * Mathf.Cos(phi);
				vertices[i + ((nbMeridians + 1) * j)] = new Vector3(x, z, y);
			}

		vertices[nbVertices - 2] = new Vector3(0, -radius, 0);
		vertices[nbVertices - 1] = new Vector3(0, radius, 0);

		// Création des triangles

		int nbTrianglesCorps = nbTriangles - (nbMeridians * 6);

		for (int i = 0, k = 0, p = 0; i <= nbTrianglesCorps; i += 6, k++, p++)
		{
			if (p == nbMeridians) { p = 0; k++; }
			triangles[i] = k;
			triangles[i + 1] = k + nbMeridians + 2;
			triangles[i + 2] = k + nbMeridians + 1;
			triangles[i + 3] = k;
			triangles[i + 4] = k + 1;
			triangles[i + 5] = k + nbMeridians + 2;
		}

		for (int i = 0, k = 0; k < nbMeridians; i += 6, k++)
		{
			triangles[i + nbTrianglesCorps] = k;
			triangles[i + nbTrianglesCorps + 1] = k + 1;
			triangles[i + nbTrianglesCorps + 2] = nbVertices - 2;
			triangles[i + nbTrianglesCorps + 3] = k + (nbMeridians + 1) * (nbParallels + 1) + 1;
			triangles[i + nbTrianglesCorps + 4] = k + (nbMeridians + 1) * (nbParallels + 1);
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
