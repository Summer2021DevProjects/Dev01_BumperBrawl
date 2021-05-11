using UnityEngine;

public class Bumper_VisualsDash : MonoBehaviour
{
    //--- Public Variables ---//
    public float m_dashRotSpeed;



    //--- Private Variables ---//
    private Transform m_parentBumper;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_parentBumper = transform.parent;
    }

    private void OnEnable()
    {
        // Detach from the parent so we can follow it but also rotate independently
        transform.parent = null;
    }

    private void OnDisable()
    {
        // Re-attach to the parent so we can continue to follow it
        transform.parent = m_parentBumper;
    }

    private void LateUpdate()
    {
        // Move to the parent bumper's new position
        // Do this in LateUpdate() so we know it has already moved
        this.transform.position = m_parentBumper.position;
    }



    //--- Methods ---//
    public void RotateForDash(Vector3 _movementDir)
    {
        // We want to rotate so that it looks like we are rolling in the correct direction
        // Therefore, the movement direction is the 'forward' vector. World up is the 'up' vector
        // So, the vector the rotation axis is the 'right' vector that matches the other two
        if (_movementDir != Vector3.zero)
        {
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, _movementDir);
            this.transform.Rotate(rotationAxis, Time.deltaTime * m_dashRotSpeed, Space.World);
        }
    }
}
