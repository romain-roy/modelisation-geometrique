using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maillage : MonoBehaviour
{
	public Material material;
	public string nomFichier;
	public int taille;

	private Vector3[] vertices;
	private int[] triangles;
	private int nbTriangles;
	private int nbVertices;

	void Start()
	{
		// Création d'un composant MeshFilter qui peut ensuite être visualisé

		gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();

		// Lecture du fichier

		string[] reader = System.IO.File.ReadAllLines("Assets/Maillages/" + nomFichier + ".off");

		string[] nombres = reader[1].Split(' ');

		nbVertices = int.Parse(nombres[0]);
		nbTriangles = int.Parse(nombres[1]) * 3;

		vertices = new Vector3[nbVertices];
		triangles = new int[nbTriangles];

		for (int i = 2, v = 0, t = 0; i < reader.Length; i++)
		{
			nombres = reader[i].Split(' ');
			if (i < nbVertices + 2)
				vertices[v++] = new Vector3(float.Parse(nombres[0].Replace('.', ',')), float.Parse(nombres[1].Replace('.', ',')), float.Parse(nombres[2].Replace('.', ',')));
			else
			{
				triangles[t++] = int.Parse(nombres[1]);
				triangles[t++] = int.Parse(nombres[2]);
				triangles[t++] = int.Parse(nombres[3]);
			}
		}

		// Centrer l'objet

		Vector3 sommeVertices = vertices[0];

		for (int i = 1; i < nbVertices; i++)
			sommeVertices += vertices[i];

		Vector3 centreGravite = sommeVertices /= nbVertices;
		gameObject.transform.position = centreGravite;

		// Normaliser sa taille

		float normMax = vertices[0].magnitude;

		for (int i = 1; i < nbVertices; i++)
			if (vertices[0].magnitude > normMax)
				normMax = vertices[0].magnitude;

		normMax /= taille;

		for (int i = 0; i < nbVertices; i++)
			vertices[i] /= normMax;

		// Création et remplissage du Mesh

		Mesh msh = new Mesh();

		msh.vertices = vertices;
		msh.triangles = triangles;

		msh.RecalculateNormals();

		// Remplissage du Mesh et ajout du material

		gameObject.GetComponent<MeshFilter>().mesh = msh;
		gameObject.GetComponent<MeshRenderer>().material = material;
	}
}
