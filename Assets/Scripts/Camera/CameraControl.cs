using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f; //aprox time to take the camera to move to a specific position                 
    public float m_ScreenEdgeBuffer = 4f;   //number set to make sure the tanks that overflow off the camera screen          
    public float m_MinSize = 6.5f; // minimmum size for the camera                  
    [HideInInspector] public Transform[] m_Targets; //camera targets


    private Camera m_Camera;    //references the camera                        
    private float m_ZoomSpeed;  //references the speed of the orthographic camera zoom                     
    private Vector3 m_MoveVelocity; //references the velocity of the camera                
    private Vector3 m_DesiredPosition;  //changes the camera position towards         


    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
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
        Vector3 averagePos = new Vector3(); //create empty vector3
        int numTargets = 0; 

        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf) //if the tank is not active continues
                continue;

            averagePos += m_Targets[i].position; //change the avgPos to a new position and make that the avgPos
            numTargets++;
        }

        if (numTargets > 0)
            averagePos /= numTargets;

        averagePos.y = transform.position.y;

        m_DesiredPosition = averagePos; //to make sure the avgPos doesnt surpass the DesiredPosition 
    }


    private void Zoom() //finds the camera position to change the zoom too and transitate to it
    {
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
    }


    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

        float size = 0f;

        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);

            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.y));

            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.x) / m_Camera.aspect);
        }
        
        size += m_ScreenEdgeBuffer;

        size = Mathf.Max(size, m_MinSize); //assign the maximum size possible in a range that goes until the minimal size possible "0"

        return size;
    }


    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = m_DesiredPosition;

        m_Camera.orthographicSize = FindRequiredSize();
    }
}
