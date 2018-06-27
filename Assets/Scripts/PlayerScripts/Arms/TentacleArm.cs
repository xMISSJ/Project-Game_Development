using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleArm : MasterArm {
    public float spacing =0.5f;
    public float scaleDown = 0.8f;
    public int numberOfSegments = 5;
    List<segment> segments = new List<segment>();
    MeshFilter mF;
    MeshRenderer mR;

    class segment
    {
        public float lenght;            //lenght between points
        public Vector3 scale;           //scale of arm crossection
        public Quaternion rotation;     // rotation of arm around the point. NOT THE DIRECTION
        public Vector3 direction;       // the direction of the point. must have a magnitude of 1
    }

    // Use this for initialization
	new void Start () {
        mF = GetComponent<MeshFilter>();
        mF.mesh = new Mesh();
        mR = GetComponent<MeshRenderer>();
        setSegments();
        genMesh();
    }
    public int sNummer = 0;
    float time = 0;
    void setSegments()
    {
        time += Time.deltaTime;
        segments = new List<segment>();
        for (int i = 0; i < numberOfSegments; i++)
        {
            segment s = new segment();
            s.direction = (Quaternion.Euler(Mathf.Cos(time) * i * 2, Mathf.Sin(time) * i*2, 0)* Vector3.forward).normalized;
            s.scale = Vector3.one / (scaleDown * (i+1));
            s.lenght = spacing;
            s.rotation = new Quaternion();

            segments.Add(s);
        }
    }
    
    void genMesh()
    {
        List<Vector3> v = new List<Vector3>();
        v.Add(Vector3.zero);
        //v.Add(Vector3.up);
        //v.Add(Vector3.forward);
        //v.Add(Vector3.down);
        //v.Add(Vector3.back);
        v.Add( Vector3.Scale(Vector3.Cross(Vector3.right, segments[0].direction).normalized, segments[0].scale));      // up
        v.Add( Vector3.Scale(Vector3.Cross(Vector3.down, segments[0].direction).normalized, segments[0].scale));     // forward
        v.Add( Vector3.Scale(Vector3.Cross(Vector3.left, segments[0].direction).normalized, segments[0].scale));     // down
        v.Add( Vector3.Scale(Vector3.Cross(Vector3.up, segments[0].direction).normalized, segments[0].scale));      // backwards
        List<int> triangles = new List<int>();
        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);

        triangles.Add(0);
        triangles.Add(2);
        triangles.Add(3);

        triangles.Add(0);
        triangles.Add(3);
        triangles.Add(4);

        triangles.Add(0);
        triangles.Add(4);
        triangles.Add(1);

       // bool even = true;
        Vector3 point = new Vector3();
        int pointNr = 4;
        Quaternion rot = Quaternion.Euler(0, 0, 0);
        Vector3 lastDirection = Vector3.zero;
        Vector3 direction = Vector3.zero;
        float lastLenth = 0;
        bool first = true;

        foreach (segment s in segments)
        {
            if (first)
            {
                first = false;
                continue;
            }
          
            
            rot = rot * Quaternion.LookRotation(s.direction);
           
           
            direction = rot * s.direction;
            
            lastLenth = s.lenght;
            point += direction * s.lenght;

            //Debug.Log(Vector3.Cross(Vector3.right, direction).magnitude);
            // draai terug op andere axis dan de draai die je wilt maken
            int vMirror = 1;
            int hMirror = 1;
            //may break. see next comment
            Debug.Log(Vector3.Angle(Vector3.forward, direction));
            if (Vector3.Angle(direction - new Vector3(0, direction.y, 0), direction) >= 91)
            {
                vMirror = -1;
  
            }else if(Vector3.Angle(direction - new Vector3(0, direction.y, 0), direction) >= 88)
            {

                direction = (rot *  Quaternion.Euler(0,2,0)) * s.direction;
                vMirror = -1;
           
            }

            Debug.Log(Vector3.Angle(Vector3.forward, direction));
            //create mesh points at right angle with the direction for tentacle thickness
            v.Add(point + Vector3.Scale(Vector3.Cross(Vector3.right, direction ).normalized * vMirror, s.scale));      // 5 point nr
            v.Add(point + Vector3.Scale(Vector3.Cross(Vector3.down, direction).normalized * hMirror, s.scale));     // 6
            v.Add(point + Vector3.Scale(Vector3.Cross(Vector3.left, direction ).normalized * vMirror, s.scale));     // 7
            v.Add(point + Vector3.Scale(Vector3.Cross(Vector3.up, direction ).normalized * hMirror, s.scale));      // 8
           
            // create the triangles between points
            // last segment to new
            triangles.Add(pointNr - 3); // 1
            triangles.Add(pointNr + 1); // 5
            triangles.Add(pointNr - 2); // 2

            triangles.Add(pointNr - 2); // 2
            triangles.Add(pointNr + 2); // 6
            triangles.Add(pointNr - 1); // 3

            triangles.Add(pointNr - 1); // 3
            triangles.Add(pointNr + 3); // 7
            triangles.Add(pointNr);     // 4

            triangles.Add(pointNr);     // 4
            triangles.Add(pointNr + 4); // 8
            triangles.Add(pointNr - 3); // 1

            // new segment to last
            triangles.Add(pointNr + 2); // 6
            triangles.Add(pointNr - 2); // 2
            triangles.Add(pointNr + 1); // 5

            triangles.Add(pointNr + 3); // 7
            triangles.Add(pointNr - 1); // 3
            triangles.Add(pointNr + 2); // 6

            triangles.Add(pointNr + 4); // 8
            triangles.Add(pointNr);     // 4
            triangles.Add(pointNr + 3); // 7

            triangles.Add(pointNr + 1); // 5
            triangles.Add(pointNr - 3); // 1
            triangles.Add(pointNr + 4); // 8


            pointNr += 4;
            lastDirection = direction;
        }
        //closing the mesh at the tip of tentacle
        v.Add(point + (lastDirection* 0.3f));

        triangles.Add(pointNr - 3); // 1
        triangles.Add(pointNr + 1); // 5
        triangles.Add(pointNr - 2); // 2

        triangles.Add(pointNr - 2); // 2
        triangles.Add(pointNr + 1); // 5
        triangles.Add(pointNr - 1); // 3

        triangles.Add(pointNr - 1); // 3
        triangles.Add(pointNr + 1); // 5
        triangles.Add(pointNr - 0); // 4

        triangles.Add(pointNr - 0); // 4
        triangles.Add(pointNr + 1); // 5
        triangles.Add(pointNr - 3); // 1

        mF.sharedMesh.vertices = v.ToArray();
        mF.sharedMesh.triangles = triangles.ToArray();
        mF.sharedMesh.RecalculateNormals();
    }

	// Update is called once per frame
	void Update () {
        Start();
    }
    void OnDrawGizmosSelected()
    {
        Start();
    }

        
}
