using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plan : MonoBehaviour
{
	public Material mat;

	public int width;
	public int height;
	public int offset;

	private Vector3[] vertices;
	private int[] triangles;
	private int nbTriangles;
	private int n;

	void Start()
	{
		gameObject.AddComponent<MeshFilter>(); // Création d'un composant MeshFilter qui peut ensuite être visualisé
		gameObject.AddComponent<MeshRenderer>();
	}

	void Update()
	{
		nbTriangles = width * height * 6;
		n = (width + 1) * (height + 1);

		vertices = new Vector3[n];
		triangles = new int[nbTriangles];

		// Création des vertices

		for (int i = 0, x = 0, y = 0; i < n; i++)
		{
			if (x > width)
			{
				x = 0;
				y++;
			}
			vertices[i] = new Vector3(x * offset, 0, y * offset);
			x++;
		}

		for (int i = 0, j = 0, k = 0; i < nbTriangles; i += 6, j++, k++)
		{
			if (k == width) { k = 0; j++; }
			triangles[i] = triangles[i + 3] = j;
			triangles[i + 1] = triangles[i + 5] = j + (width + 1) + 1;
			triangles[i + 2] = j + 1;
			triangles[i + 4] = j + (width + 1);
			Mesh msh = new Mesh(); // Création et remplissage du Mesh

			msh.vertices = vertices;
			msh.triangles = triangles;

			gameObject.GetComponent<MeshFilter>().mesh = msh; // Remplissage du Mesh et ajout du material
			gameObject.GetComponent<MeshRenderer>().material = mat;
		}
	}

}
