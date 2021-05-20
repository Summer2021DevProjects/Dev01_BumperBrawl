using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bumper_AIControls : MonoBehaviour
{
    public float m_safetyBias;
    public float m_arenaRadius;
    public float m_outOfBoundsDangerScore;
    public Vector3 m_arenaCenter;

    public float m_aggressionBias;
    public float m_chargeRadius;
    public float m_attackRadius;

    private List<Bumper_Controls> m_otherBumpers;
    private Bumper_Controls m_controls;
    private bool m_isCharging;

    private void Awake()
    {
        m_otherBumpers = new List<Bumper_Controls>();
        var controlSystems = FindObjectsOfType<Bumper_Controls>();
        foreach(var control in controlSystems)
        {
            if (control.gameObject == this.gameObject)
                m_controls = control;
            else
                m_otherBumpers.Add(control);
        }
        m_isCharging = false;

    }

    private void Update()
    {
        float safetyScore = EvaluateSafetyScore();

        Vector3 closestEnemyPos = Vector3.zero;
        bool enemyInDashRange = false;
        float aggressionScore = EvaluateAggressionScore(out closestEnemyPos, out enemyInDashRange);

        Vector3 newMovementTarget;
        if (safetyScore >= aggressionScore)
        {
            newMovementTarget = m_arenaCenter;
            ToggleDash(false);
        }
        else
        {
            newMovementTarget = closestEnemyPos;
            ToggleDash(enemyInDashRange);
        }

        
        float movementForce = Mathf.Clamp(Mathf.Max(safetyScore, aggressionScore), 0.0f, 1.0f);

        MoveTowardsTarget(newMovementTarget, movementForce);
    }

    public float EvaluateAggressionScore(out Vector3 _targetPos, out bool _targetInDashRange)
    {
        // Find the closest bumper 
        Transform closestTarget = null;
        float closestDist = Mathf.Infinity;
        Vector3 vecToTarget = Vector3.zero;
        float distToTarget = Mathf.Infinity;
        foreach (var bumper in m_otherBumpers)
        {
            if (bumper == null)
            {
                m_otherBumpers.Remove(bumper);
                continue;
            }
            Vector3 newVec = bumper.transform.position - this.transform.position;
            distToTarget = vecToTarget.sqrMagnitude;

            if (distToTarget < closestDist && // Closer than the previous target
                distToTarget <= m_attackRadius &&  // Actually in range
                CalcPositionDanger(bumper.transform.position) < m_outOfBoundsDangerScore) // Not currently in the process of falling and dying (if they are, we can just ignore them)
            {
                distToTarget = closestDist;
                closestTarget = bumper.transform;
                vecToTarget = newVec;
            }
        }

        // If there is a target in range, consider its danger for whether or not we should attack it
        // If they are in the center of the map, attacking is less useful since they are quite safe
        if (closestTarget != null)
        {
            _targetPos = closestTarget.transform.position;
            _targetInDashRange = Mathf.Sqrt(distToTarget) <= m_chargeRadius;
            return CalcPositionDanger(_targetPos) + m_aggressionBias;
        }

        _targetPos = Vector3.zero;
        _targetInDashRange = false;
        return 0.0f;
    }

    public float EvaluateSafetyScore()
    {
        return CalcPositionDanger(this.transform.position) + m_safetyBias;
    }

    private float CalcPositionDanger(Vector3 _position)
    {
        // Determine how far the position is from the center of the map
        Vector3 positionOffset = _position - m_arenaCenter;
        float distFromCenter = positionOffset.magnitude;

        // Calculate the danger rating. 1.0 (and above) is basically at the edges of the map, whereas 0.0 is directly on top of the center
        float dangerRating = distFromCenter / m_arenaRadius;
        return dangerRating;
    }

    public void MoveTowardsTarget(Vector3 _targetPosition, float _speedModifier)
    {
        // Convert the positions to screen space so we can determine what 'input' would be needed to actually make the movement
        // This is because the control system relies on the input to control it
        var targetScreenPos = Camera.main.WorldToScreenPoint(_targetPosition);
        var thisScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 screenMovementVec3 = targetScreenPos - thisScreenPos;
        Vector2 screenMovementVec2 = new Vector2(screenMovementVec3.x, screenMovementVec3.y);

        // Normalize the vector so it has a length of 1 and can also go into the negatives
        screenMovementVec2.Normalize();
        screenMovementVec2 *= _speedModifier;

        // Pass the 'input' to the controls to actually move the bumper around
        m_controls.SetLastMoveInput(screenMovementVec2);
    }

    public void ToggleDash(bool _shouldBeDashing)
    {
        // If the dash has already been charged up fully, we should actually cancel the input to trigger the dash
        if (m_controls.CalcChargePercent() >= 1.0f)
            _shouldBeDashing = false;

        if (_shouldBeDashing) // Should be dashing this frame
        {
            if (m_isCharging)
            {
                // Was already dashing, and so this is just holding the button
                m_controls.SetLastDashInput(InputActionPhase.Performed);
            }
            else
            {
                // Was not already dashing, and so this is now the beginning of the action
                m_controls.SetLastDashInput(InputActionPhase.Started);
            }
            m_isCharging = true;
        }
        else // Should not be dashing this frame
        {
            if (m_isCharging)
            {
                // Was already dashing, and so this is the button being let go of
                m_controls.SetLastDashInput(InputActionPhase.Canceled);
            }
            else
            {
                // Was not already dashing, and so that hasn't changed
                m_controls.SetLastDashInput(InputActionPhase.Waiting);
            }
            m_isCharging = false;
        }
    }
}
