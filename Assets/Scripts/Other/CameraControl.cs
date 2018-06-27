using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;
    public float m_ScreenEdgeBuffer = 4f;
    public float m_MinSize = 6.5f;
    /*[HideInInspector]*/
    private Transform[] m_Targets;
    private int targetCount; //Keeps track of how many targets have been added to the targets array

    private Camera m_Camera;
    private float m_ZoomSpeed;
    private Vector3 m_MoveVelocity;
    private Vector3 m_DesiredPosition;

    /*
     * NOT OUR CODE
     * Code was written fully by unity and implemented into the game By Stijn Kroon
     * Code is from the unity tutorial "Tanks" (https://unity3d.com/learn/tutorials/projects/tanks-tutorial/scene-setup?playlist=20081)
     */
     
    private void Awake()
    {
        m_Targets = new Transform[5];
        m_Camera = Camera.main;
    }
    
    private void FixedUpdate()
    {
        Move();
        Zoom();
    }


    private void Move()
    {
        FindAveragePosition();

        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }


    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < targetCount; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            averagePos += m_Targets[i].position;
            numTargets++;
        }

        if (numTargets > 0)
            averagePos /= numTargets;

        averagePos.y = transform.position.y;

        m_DesiredPosition = averagePos;
    }


    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
    }


    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

        float size = 0f;

        for (int i = 0; i < targetCount; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);

            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
        }

        size += m_ScreenEdgeBuffer;

        size = Mathf.Max(size, m_MinSize);

        return size;
    }

    public void addPlayer(GameObject currentPlayer)
    {
        m_Targets[targetCount] = currentPlayer.GetComponent<Transform>();
        targetCount++;
    }

    public void removePlayer(string playerName)
    {

        for (int i = 0; i < targetCount; i++)
        {
            if (m_Targets[i] != null)
            {
                if (m_Targets[i].name == playerName && i < m_Targets.Length - 1) { m_Targets[i] = m_Targets[i + 1]; m_Targets[i + 1] = null; }
            }
            if (m_Targets[i] == null && i < m_Targets.Length - 1) { m_Targets[i] = m_Targets[i + 1]; m_Targets[i + 1] = null; }
        }

        targetCount -= 1; // lengte aanpassen van array ^  m_Targets.Length - targetCountDifference;
    }

    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = m_DesiredPosition;

        m_Camera.orthographicSize = FindRequiredSize();
    }
}
