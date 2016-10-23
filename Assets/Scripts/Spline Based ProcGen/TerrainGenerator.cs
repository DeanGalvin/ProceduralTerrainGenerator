using UnityEngine;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour {

	private Mesh mesh = null; //Reference to the mesh we're going to generate
	private Vector3[] points = null; //The points along the top edge of the mesh

	//Mutable lists for the vertices and triangles within the mesh
	private List<Vector3> vertices = new List<Vector3>();
	private List<int> triangles = new List<int>();

	void Start()
	{
		//Get a references to the mesh component and clear it
		MeshFilter filter = GetComponent<MeshFilter>();
		mesh = filter.mesh;
		mesh.Clear ();

		//Generate 4 random points for the top
		points = new Vector3[4];
		for (int i = 0; i < points.Length; i++) {
			points [i] = new Vector3 (0.5f * (float)i, Random.Range (1f, 2f), 0f);
			//AddTerrainPoint(points[i]);
		}

		//Number of points to draw, defines smoothness of curve
		int resolution = 20;
		for (int i = 0; i < resolution; i++) {
			float t = (float)i / (float)(resolution - 1);
			//Get the point on our curve using the 4 generated points
			Vector3 p = CalculateBezierPoint(t, points[0], points[1], points[2], points[3]);
			AddTerrainPoint (p);
		}

		//Assign the vertices and triangles to the mesh
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray ();
	}

	void AddTerrainPoint(Vector3 point)
	{
		//Create a corresponding point along the bottom
		vertices.Add(new Vector3(point.x, 0f, 0f));

		//Add our top point
		vertices.Add(point);

		if (vertices.Count >= 4) {
			//We have completed a new quad, created 2 triangles
			int start = vertices.Count - 4;
			triangles.Add (start + 0);
			triangles.Add (start + 1);
			triangles.Add (start + 2);
			triangles.Add (start + 1);
			triangles.Add (start + 3);
			triangles.Add (start + 2);
		}
	}

	private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float u = 1 - t;
		float tt = t * t;
		float uu = u * u;
		float uuu = uu * u;
		float ttt = tt * t;

		Vector3 p = uuu * p0;
		p += 3 * uu * t * p1;
		p += 3 * u * tt * p2;
		p += ttt * p3;

		return p;
	}

}
