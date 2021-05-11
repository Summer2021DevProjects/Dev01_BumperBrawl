using UnityEngine;
using UnityEngine.InputSystem;

public class Bumper_Controls : MonoBehaviour
{
    //--- Public Variables ---//
    [Header("Visuals")]
    public Bumper_DashVisuals m_dashVisuals;

    [Header("Movement Controls")]
    public float m_maxVel;
    public float m_moveForce;

    [Header("Grounded Check")]
    public LayerMask m_groundLayers;

    [Header("Dashing")]
    public float m_dashChargeLength;
    public float m_maxDashForce;
    public float m_dashCooldownLength;
    public Renderer m_dashIndicatorObj;



    //--- Private Variables ---//
    private Rigidbody m_body;
    private Camera m_cam;
    private Vector2 m_lastMoveInput;
    private InputActionPhase m_lastDashInput;
    private Vector3 m_nextForceToAdd;
    private float m_dashChargeTime;
    private bool m_isChargingDash;
    private bool m_isGrounded;
    private Vector2 m_baseDragVals;
    private float m_currentDashCooldown;
    private bool m_dashCoolingDown;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_body = GetComponent<Rigidbody>();
        m_cam = Camera.main;
        m_lastMoveInput = Vector2.zero;
        m_lastDashInput = InputActionPhase.Disabled;
        m_nextForceToAdd = Vector3.zero;
        m_dashChargeTime = 0.0f;
        m_isChargingDash = false;
        m_isGrounded = true;
        m_baseDragVals = new Vector2(m_body.drag, m_body.angularDrag);
        m_currentDashCooldown = 0.0f;
        m_dashCoolingDown = false;

        // Disable the dash visuals at the start so the ball just rotates normally with physics
        m_dashVisuals.enabled = false;
    }

    private void Update()
    {
        // Convert the movement to be camera-relative
        Vector3 movementVec3 = new Vector3(m_lastMoveInput.x, 0.0f, m_lastMoveInput.y);
        Vector3 movementRelative = m_cam.transform.TransformDirection(movementVec3.normalized);

        // Negate the y-axis so it doesn't lift off the ground
        movementRelative.y = 0.0f;
        movementRelative.Normalize();

        // Check if the dash is ready again if it is currently on cooldown
        if (m_dashCoolingDown)
        {
            m_currentDashCooldown += Time.deltaTime;

            if (m_currentDashCooldown >= m_dashCooldownLength)
            {
                m_currentDashCooldown = 0.0f;
                m_dashCoolingDown = false;
            }
        }

        // Determine the different effects depending on if dash is being used or not
        switch (m_lastDashInput)
        {
            // Charge the dash
            case InputActionPhase.Started:
            case InputActionPhase.Performed:
            {
                if (!m_dashCoolingDown)
                {
                    m_isChargingDash = true;
                    m_dashChargeTime += Time.deltaTime;

                    m_dashVisuals.enabled = true;

                    // Apply high drag on the ground but not in the air
                    m_body.drag = (m_isGrounded) ? Mathf.Infinity : m_baseDragVals.x;
                    m_body.angularDrag = (m_isGrounded) ? Mathf.Infinity : m_baseDragVals.y;
                }

                break;
            }

            // End dash charging, actually trigger dash force
            case InputActionPhase.Canceled:
            {
                if (m_isGrounded)
                {
                    float dashChargePercent = Mathf.Clamp(m_dashChargeTime / m_dashChargeLength, 0.0f, 1.0f);
                    m_body.AddForce(movementRelative * dashChargePercent * m_maxDashForce, ForceMode.Impulse);
                }

                m_body.drag = m_baseDragVals.x;
                m_body.angularDrag = m_baseDragVals.y;

                m_dashVisuals.enabled = false;
                m_dashCoolingDown = true;

                m_isChargingDash = false;
                m_dashChargeTime = 0.0f;
                m_lastDashInput = InputActionPhase.Waiting; // Need to reset this because the input won't get passed again so it will stay at 'Cancelled'
                break;
            }

            // Standard movement, scaled based on how much the input was pushed (can move slower if desired)
            default:
            {
                if (m_isGrounded)
                    m_nextForceToAdd = m_moveForce * m_lastMoveInput.magnitude * movementRelative;
                break;
            }
        }

        // TEMP: Update the charging indicator so it shows the charge amount
        float chargeT = Mathf.Clamp(m_dashChargeTime / m_dashChargeLength, 0.0f, 1.0f);
        m_dashIndicatorObj.material.color = Color.Lerp(Color.red, Color.green, chargeT);

        // Update the dash visuals so the ball spins independently to match the desired dash direction
        if (m_isChargingDash)
            m_dashVisuals.RotateForDash(movementRelative);
    }

    private void FixedUpdate()
    {
        //  Apply the most recently set force
        m_body.AddForce(m_nextForceToAdd, ForceMode.Force);
        m_nextForceToAdd = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object that was collided with is on one of the ground layers
        if (m_groundLayers == (m_groundLayers | (1 << collision.gameObject.layer)))
            m_isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the object that was collided with is on one of the ground layers
        if (m_groundLayers == (m_groundLayers | (1 << collision.gameObject.layer)))
            m_isGrounded = false;
    }



    //--- Input Callbacks ---//
    public void OnMoveInput(InputAction.CallbackContext _context)
    {
        m_lastMoveInput = _context.ReadValue<Vector2>();
    }

    public void OnDashInput(InputAction.CallbackContext _context)
    {
        m_lastDashInput = _context.phase;
    }

    public void OnResetInput(InputAction.CallbackContext _context)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
