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

    [Header("Dashing")]
    public float m_dashChargeLength;
    public float m_maxDashForce;
    public Renderer m_dashIndicatorObj;



    //--- Private Variables ---//
    private Rigidbody m_body;
    private Camera m_cam;
    private InputDefinitions m_inputs;
    private Vector3 m_forceToApply;
    private bool m_isGrounded;
    private Vector3 m_startPos;
    private bool m_isChargingDash;
    private float m_currentDashTimer;
    private float m_dashForceMagnitude;
    private float m_dashChargePercent;



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
        m_isChargingDash = false;
        m_dashChargePercent = 0.0f;
    }

    private void Update()
    {
        if (m_isChargingDash)
        {
            // Increase the dash timer
            m_currentDashTimer += Time.deltaTime;
            m_dashChargePercent = Mathf.Clamp(m_currentDashTimer / m_dashChargeLength, 0.0f, 1.0f);
        }
        else
        {
            // Check to see if the bumper is grounded. This way, we can disable movement controls if it isn't
            var thisPos = this.transform.position;
            m_isGrounded = Physics.Raycast(thisPos, Vector3.down, m_groundCheckLength, m_groundLayers);
        }

        // TEMP: Change the colour to show the charging feedback
        m_dashIndicatorObj.material.color = Color.Lerp(Color.red, Color.green, m_dashChargePercent);
    }

    private void FixedUpdate()
    {
        if (m_isGrounded && !m_isChargingDash)
        {
            // Apply the calculated and stored movement force
            m_body.AddForce(m_forceToApply, ForceMode.Force);
            //m_body.velocity = Vector3.ClampMagnitude(m_body.velocity, m_maxVel);

            // Add the dash force on top if there was one
            var m_dashForceToAdd = m_forceToApply.normalized * m_dashForceMagnitude;
            m_body.AddForce(m_dashForceToAdd, ForceMode.Impulse);
            m_dashForceMagnitude = 0.0f;
        }

        if (m_isChargingDash)
        {
            m_body.velocity = Vector3.zero;
            m_body.angularVelocity = Vector3.zero;
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

    public void Dash(InputAction.CallbackContext _context)
    {
        m_isChargingDash = !_context.canceled;

        // If just letting go of the dash button now, trigger the dash
        // This entails calculating the amount of force to apply in the next fixed update
        if (_context.canceled)
        {
            m_dashForceMagnitude = m_dashChargePercent * m_maxDashForce;
            m_dashChargePercent = 0.0f;
            m_currentDashTimer = 0.0f;

            m_body.velocity = Vector3.zero;
            m_body.angularVelocity = Vector3.zero;
        }
    }
}
