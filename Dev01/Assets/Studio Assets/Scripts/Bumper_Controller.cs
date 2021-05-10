using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Assertions;

public class Bumper_Controller : MonoBehaviour
{
    //--- Public Variables ---//
    [Header("Movement Controls")]
    public float m_maxVel;
    public float m_moveForce;

    [Header("Grounded Check")]
    public float m_groundCheckLength;
    public LayerMask m_groundLayers;



    //--- Private Variables ---//
    private Rigidbody m_body;
    private Camera m_cam;
    private InputDefinitions m_inputs;
    private Vector3 m_forceToApply;
    private bool m_isGrounded;
    private Vector3 m_startPos;



    //--- Unity Lifecyle ---//
    private void Awake()
    {
        m_body = GetComponent<Rigidbody>();
        m_cam = Camera.main;
        Assert.IsNotNull(m_body);
        Assert.IsNotNull(m_cam);

        m_inputs = new InputDefinitions();
        m_inputs.PlayerControls.Move.performed += context => Move(context);

        m_forceToApply = Vector3.zero;
        m_isGrounded = true;
        m_startPos = this.transform.position;
    }

    private void Update()
    {
        // Check to see if the bumper is grounded. This way, we can disable movement controls if it isn't
        var thisPos = this.transform.position;
        m_isGrounded = Physics.Raycast(thisPos, Vector3.down, m_groundCheckLength, m_groundLayers);
    }

    private void FixedUpdate()
    {
        // Apply the calculated and stored force
        if (m_isGrounded)
        {
            m_body.AddForce(m_forceToApply, ForceMode.Force);
            m_body.velocity = Vector3.ClampMagnitude(m_body.velocity, m_maxVel);
        }
    }



    //--- Input Callbacks ---//
    public void Move(InputAction.CallbackContext _context)
    {
        // Read the input value and convert it to 3D so it matches the world
        var inputVec = _context.ReadValue<Vector2>();
        Vector3 inputAsVec3 = new Vector3(inputVec.x, 0.0f, inputVec.y);
        inputAsVec3.Normalize();

        // Transform the vector so it is relative to the camera
        // This means that pressing up moves towards the top of the screen
        Vector3 inputCameraRelative = m_cam.transform.TransformDirection(inputAsVec3);

        // Negate the y-axis on the transformed movement so it doesn't lift off the ground
        inputCameraRelative.y = 0.0f;
        inputCameraRelative.Normalize();

        // Calculate the scaled force based on the orginal value of the input
        // This way, if the thumbstick is only half pressed, the force to apply will be half of the max
        Vector3 forceToAdd = m_moveForce * inputVec.magnitude * inputCameraRelative;

        // Store the force so that it is applied in FixedUpdate only
        m_forceToApply = forceToAdd;
    }

    public void Reset(InputAction.CallbackContext _context)
    {
        this.transform.position = m_startPos;
        this.transform.rotation = Quaternion.identity;
        m_body.velocity = Vector3.zero;
        m_body.angularVelocity = Vector3.zero;
    }
}
