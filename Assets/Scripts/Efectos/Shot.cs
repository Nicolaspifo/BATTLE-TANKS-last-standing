
using UnityEngine;
using UnityEngine.InputSystem;

public class Shot : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject bullet;
    public Transform SpawnPoint;

    public float shotForce = 1500f;
    public float shotRate = 0.5f;

    [Header("Tecla de disparo")]
    public Key teclaDisparo = Key.T;

    [Header("Orientación visual de la bala")]
    public Vector3 rotacionBala = new Vector3(90f, 180f, 0f);

    private float shotRateTime = 0f;

    void Update()
    {
        // Comprobar si se presionó la tecla asignada
        if (Keyboard.current == null ||
            !Keyboard.current[teclaDisparo].wasPressedThisFrame)
        {
            return;
        }

        // Controlar la velocidad de disparo
        if (Time.time < shotRateTime)
        {
            return;
        }

        // Comprobar que estén asignados la bala y el punto de salida
        if (bullet == null || SpawnPoint == null)
        {
            Debug.LogError(
                "Falta asignar Bullet o Spawn Point en Shot."
            );
            return;
        }

        // Crear la bala
        GameObject newBullet = Instantiate(
            bullet,
            SpawnPoint.position,
            SpawnPoint.rotation
        );

        // Corregir la orientación visual de la bala
        newBullet.transform.Rotate(
            rotacionBala,
            Space.Self
        );

        // Evitar que la bala choque inmediatamente con su propio tanque
        Collider[] bulletColliders =
            newBullet.GetComponentsInChildren<Collider>();

        Collider[] tankColliders =
            GetComponentsInChildren<Collider>();

        foreach (Collider bulletCollider in bulletColliders)
        {
            foreach (Collider tankCollider in tankColliders)
            {
                if (bulletCollider != null && tankCollider != null)
                {
                    Physics.IgnoreCollision(
                        bulletCollider,
                        tankCollider
                    );
                }
            }
        }

        // Aplicar fuerza a la bala
        Rigidbody rb = newBullet.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError(
                "El prefab de la bala necesita un Rigidbody."
            );

            Destroy(newBullet);
            return;
        }

        rb.AddForce(
            SpawnPoint.forward * shotForce,
            ForceMode.Force
        );

        // Esperar antes de permitir otro disparo
        shotRateTime = Time.time + shotRate;

        // Destruir la bala después de dos segundos
        Destroy(newBullet, 2f);
    }
}
