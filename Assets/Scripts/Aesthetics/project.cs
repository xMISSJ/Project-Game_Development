using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class project : MonoBehaviour
{

    Mesh mesh;                          // for accessing the mesh
    Vector3[] originalVertices;         // keeping the original values of the mesh vertices
    Vector3[] vertices;                 // for modiving the vertices of the mesh
    public LayerMask mask;              // used to select which layer will be projected on
    public float range;                 // the max distance of projection
    List<int>[] triangleIndex;          // used to find the triangles in a mesh based on vertices
    bool projecting = false;            // if projection is currently projecting
    Color32[] col;                      // the color of projection, mostly used to make the projection invisible
    public GameObject fadeObject;       // used for fading the projection
    public bool Projecting              
    {
        get { return projecting; }
        set
        {
            // makes projection invisible if not projecting
            if (value)
            {
                mesh.colors32 = col;
                
            }
            else {
                gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 0);
            }
            projecting = value;
        }
    }   // for setting if projection is currently projecting
   
    // Use this for initialization
    void Start()
    {
        mesh = gameObject.GetComponent<MeshFilter>().mesh;

        col = (Color32[])mesh.colors32.Clone();
       

        vertices = mesh.vertices;
        // store the original vertices, these wil NEVER change
        if (originalVertices == null)
        {
            originalVertices = mesh.vertices;
        }

        triangleIndex = new List<int>[mesh.vertices.Length];
        for (int i = 0; i < triangleIndex.Length; i++)
        {
            triangleIndex[i] = new List<int>();
        }
        //for each vertice add all triangles it belongs to
        for (int j = 0; j < mesh.triangles.Length; j++)
        {

            triangleIndex[mesh.triangles[j]].Add(j - (j % 3));



        }
        Projecting = false;
    }


    // creates a copy of current mesh that can be destroy by fade
    public void fade(float time)
    {
        GameObject f = Instantiate(fadeObject);
        f.transform.position = transform.position;
        f.transform.localScale = transform.localScale;
        f.transform.rotation = transform.rotation;
        f.GetComponent<MeshFilter>().mesh.vertices = (Vector3[])mesh.vertices.Clone();
        f.GetComponent<MeshFilter>().mesh.colors32 = (Color32[])mesh.colors32.Clone();
        f.transform.parent = null;
        f.GetComponent<FadeOut>().FadeTime = time;
        f.SetActive(true);

    }


    // this function projects the mesh on a surface below.
    public void Project(Vector3? position = null, Quaternion? rot = null)
    {
        if(position != null)
        {
            transform.position = (Vector3)position;
        }
        if (rot != null)
        {
            transform.rotation = (Quaternion)rot;
        }
        gameObject.GetComponent<MeshRenderer>().material.color = new Color(1,1,1,1);
        Color32[] c = new Color32[vertices.Length];
        RaycastHit hit;
        // this loop checks for every point in the mesh what lies beneath it.
        //if it finds a surface it moves the point down to the height of that surface
        // each loop check 1 point in the mesh
        for (int i = 0; i < vertices.Length; i++)
        {

            // check for surface below relative to rotation and scale
            if (Physics.Raycast(Vector3.Scale(Quaternion.Inverse(transform.rotation) * originalVertices[i], gameObject.transform.localScale  ) + transform.position, transform.rotation * Vector3.down, out hit, range , mask.value))
            {

                // a surface is detected

                vertices[i] = Quaternion.Inverse(transform.rotation) * (hit.point - transform.position);

                vertices[i].x /= gameObject.transform.localScale.x;
                vertices[i].y /= gameObject.transform.localScale.y;
                vertices[i].z /= gameObject.transform.localScale.z;
                // move point slightly up so that the mesh is not inside surface
                vertices[i] += (Vector3.up * 0.1f);
                // makes mesh at this point visable
                c[i] = new Color32(255, 255, 255, 255);
            }
            else
            { // no surface is detected so the mesh at this point is inviseble
                c[i] = new Color32(0, 0, 0, 0);
            }
        }
        mesh.colors32 = c;

        Vector3[] h = (Vector3[])vertices.Clone();
        // this loop prefents the mesh from letting corners from the surface poke trough the projection
        // each loop checks 1 traingle in the mesh
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            float hi = -(range +1);
            //get the highest of the triangles points
            for (int j = 0; j < 3; j++)
            {
                //   Debug.Log(mesh.triangles[i+j]);
                if (hi < h[mesh.triangles[i + j]].y)
                {
                    // makes sure it only checks points that are visable
                    if (mesh.colors[mesh.triangles[i + j]] != new Color(0, 0, 0, 0))
                    {
                        hi = h[mesh.triangles[i + j]].y;
                    }
                   
                }
            }
            // increases the height of all points to the heigt of the highest point in the triangle
            for (int j = 0; j < 3; j++)
            {
                //checks if point is higher than it already was because a single point can exsist in multiple triangles
                if (hi > vertices[mesh.triangles[i + j]].y)
                { vertices[mesh.triangles[i + j]].y = hi; }

            }

        }

        col = (Color32[])mesh.colors32.Clone();
        // this loop retracts the color from the edge to prevent the transition from visible to invisible to extend to far.
        for (int i = 0; i < mesh.colors32.Length; i++)
        {
            //if a point is invisible
            if (mesh.colors[i] == new Color(0, 0, 0, 0))
            {
                //find all adjacent points and make invisible 
                foreach (int pos in triangleIndex[i])
                {

                    col[mesh.triangles[pos]] = new Color32(0, 0, 0, 0);
                    col[mesh.triangles[pos + 1]] = new Color32(0, 0, 0, 0);
                    col[mesh.triangles[pos + 2]] = new Color32(0, 0, 0, 0);
                }


            }
        }

        mesh.colors32 = col;
        
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }

}


