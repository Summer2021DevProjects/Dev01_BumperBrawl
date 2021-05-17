using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper_DashFX : MonoBehaviour
{
    [Header("Main Ring")]
    public ParticleSystem m_mainRing_System;
    public Vector2 m_mainRing_EmissionRateRange;
    public Vector2 m_mainRing_ScaleRange;

    [Header("Wind")]
    public GameObject m_wind_Systems;

    [Header("Colour Ring")]
    public ParticleSystem m_colourRing_System;

    private Transform m_parentBumper;
    private Vector3 m_parentOffset;

    private void Awake()
    {
        // Init the private variables
        m_parentBumper = m_wind_Systems.transform.parent;
        m_parentOffset = m_wind_Systems.transform.localPosition;

        // Separate from the parent so it rotates separately
        m_wind_Systems.transform.parent = null;
    }

    private void LateUpdate()
    {
        // Always move to match the parent position
        m_wind_Systems.transform.position = m_parentBumper.position + m_parentOffset;
    }

    public void UpdateEffect(float _chargePercentage)
    {
        UpdateWind(_chargePercentage);
        UpdateMainRing(_chargePercentage);
        UpdateColourRing(_chargePercentage);
    }

    private void UpdateMainRing(float _mainRingT)
    {
        m_mainRing_System.gameObject.SetActive(_mainRingT > 0.0f);

        // Scale the number of particles in the ring up over time to make it glow more
        var emission = m_mainRing_System.emission;
        emission.rateOverTimeMultiplier = Mathf.Lerp(m_mainRing_EmissionRateRange.x, m_mainRing_EmissionRateRange.y, _mainRingT);

        // Scale the ring itself over time to make it grow from inside the bumper to outside
        var transform = m_mainRing_System.gameObject.transform;
        transform.localScale = Vector3.one * Mathf.Lerp(m_mainRing_ScaleRange.x, m_mainRing_ScaleRange.y, _mainRingT);
    }

    private void UpdateWind(float _chargePercentage)
    {
        // Turn on the wind effects anytime the dash is being charged
        m_wind_Systems.SetActive(_chargePercentage > 0.0f);
    }

    private void UpdateColourRing(float _chargePercentage)
    {
        m_colourRing_System.gameObject.SetActive(_chargePercentage >= 1.0f);
    }

    public void SetRingColour(Color _color)
    {
        var ringMain = m_colourRing_System.main;
        float alpha = ringMain.startColor.color.a;
        Color colorWithAlpha = new Color(_color.r, _color.g, _color.b, alpha);
        ringMain.startColor = colorWithAlpha;
    }

    public void UpdateMoveDirection(Vector3 _newDirection)
    {
        m_wind_Systems.transform.forward = _newDirection;
    }
}
