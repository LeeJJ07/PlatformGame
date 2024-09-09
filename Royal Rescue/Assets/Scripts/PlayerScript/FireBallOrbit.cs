using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallOrbit : MonoBehaviour
{
    // Start is called before the first frame update
    public LineRenderer lineRenderer;
    public float throwForce = 10f;
    public float gravity = -9.81f;
    public Vector3 ballDir;
    public int resolution = 30; // ������ �ػ�
    public float maxHoldTime = 3f; // �ִ� ���� �ð�

    private float holdTime = 0f;

    void Start()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = resolution;
        }
    }

    void Update()
    {
        if (lineRenderer != null)
        {
            // ���� �ð� ����
            holdTime += Time.deltaTime;
            holdTime = Mathf.Clamp(holdTime, 0, maxHoldTime);

            // ���� ����
            DrawTrajectory();
        }
    }

    private void DrawTrajectory()
    {
        Vector3[] positions = new Vector3[resolution];

        float timeStep = maxHoldTime / resolution;
        Vector3 startPos = transform.position;
        Vector3 startVelocity = (ballDir + Vector3.up * 1.5f) * Mathf.Lerp(5, 10, holdTime / maxHoldTime);

        for (int i = 0; i < resolution; i++)
        {
            float t = i * timeStep;
            Vector3 displacement = startVelocity * t + 0.5f * new Vector3(0, gravity, 0) * t * t;
            positions[i] = startPos + displacement;
        }

        lineRenderer.SetPositions(positions);
    }
}
