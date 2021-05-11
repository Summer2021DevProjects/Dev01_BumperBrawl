using UnityEngine;

public class Bumper_VisualsCharacter : MonoBehaviour
{
    //--- Private Variables ---//
    private Animator m_animator;
    private Transform m_parentBumper;
    private Vector3 m_parentOffset;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_animator = GetComponent<Animator>();
        m_parentBumper = this.transform.parent;
        m_parentOffset = this.transform.localPosition;

        // Separate from the parent so it rotates separately
        this.transform.parent = null;
    }

    private void LateUpdate()
    {
        // Always move to match the parent position
        this.transform.position = m_parentBumper.position + m_parentOffset;
    }



    //--- Methods ---//
    public void UpdateOrientation(Vector3 _movementDir)
    {
        // Billboard-style rotation to always be looking in the direction of movement
        if (_movementDir != Vector3.zero)
            this.transform.rotation = Quaternion.LookRotation(_movementDir, Vector3.up);
    }

    public void UpdateAnimation(float _moveSpeed, bool _isChargingDash)
    {
        // Update the animator so the character moves to match the actual bumper movement
        m_animator.SetFloat("MovementSpeed", _moveSpeed);
        m_animator.SetBool("IsDashing", _isChargingDash);
    }
}
