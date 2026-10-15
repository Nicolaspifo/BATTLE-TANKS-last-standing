
using UnityEngine;
using NetworkingLab.Player;

public class TankSmoke : MonoBehaviour
{
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private float idleEmission = 2f;
    [SerializeField] private float movingEmission = 18f;
    [SerializeField] private float speedThreshold = 0.1f;

    private PlayerController controller;
    private Vector3 lastPosition;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        lastPosition = transform.position;

        if (smoke == null)
            Debug.LogWarning("Asigna el Particle System de humo.");
    }

    private void Update()
    {
        if (smoke == null || controller == null)
            return;

        float speed = (transform.position - lastPosition).magnitude
                      / Mathf.Max(Time.deltaTime, 0.0001f);

        var emission = smoke.emission;
        emission.rateOverTime = speed > speedThreshold
            ? movingEmission
            : idleEmission;

        lastPosition = transform.position;
    }
}
