
using UnityEngine;
using UnityEngine.InputSystem;

public class ShotWithAudio : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject bullet;
    public Transform spawnPoint;
    public float shotForce = 1500f;
    public float shotRate = 0.5f;
    public Vector3 rotacionBala = new Vector3(90f, 0f, 0f);

    [Header("Sonido")]
    public AudioClip sonidoDisparo;
    [Range(0f, 1f)]
    public float volumen = 0.7f;

    private float shotRateTime = 0f;

    void Update()
    {
        Debug.Log("ShotWithAudio está funcionando");
        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
            Debug.LogError("PRESIONASTE T EN SHOTWITHAUDIO");
        if (Keyboard.current == null ||
            !Keyboard.current.tKey.wasPressedThisFrame)
            return;

        if (Time.time <= shotRateTime)
            return;

        if (bullet == null || spawnPoint == null)
        {
            Debug.LogError("Asigna Bullet y Spawn Point en ShotWithAudio.");
            return;
        }

        GameObject newBullet = Instantiate(
            bullet, spawnPoint.position, spawnPoint.rotation
        );

        newBullet.transform.Rotate(90f, 180f, 0f, Space.Self);

        Collider bulletCollider = newBullet.GetComponent<Collider>();

        if (bulletCollider != null)
        {
            Collider[] tankColliders = GetComponentsInChildren<Collider>();

            foreach (Collider tankCollider in tankColliders)
                Physics.IgnoreCollision(bulletCollider, tankCollider);
        }

        Rigidbody rb = newBullet.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("BulletTank necesita un Rigidbody.");
            Destroy(newBullet);
            return;
        }

        rb.AddForce(spawnPoint.forward * shotForce);

        if (sonidoDisparo != null)
        {
            Debug.LogError("LLEGÓ A LA PARTE DEL SONIDO");

            AudioSource.PlayClipAtPoint(
                sonidoDisparo,
                Camera.main.transform.position,
                volumen
            );
        }
        else
        {
            Debug.LogError("EL AUDIO NO ESTÁ ASIGNADO");
        }

        shotRateTime = Time.time + shotRate;
        Destroy(newBullet, 2f);
    }
}
