
using UnityEngine;

public class BulletImpact : MonoBehaviour
{
    public GameObject ExplotionPrefab;

    private bool impacted = false;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("LA BALA HA CHOCADO CON: " + collision.gameObject.name);
        if (impacted) return;
        impacted = true;

        // Obtener el punto exacto del impacto.
        Vector3 impactPoint = transform.position;

        if (collision.contactCount > 0)
        {
            impactPoint = collision.GetContact(0).point;
            Debug.Log("Punto de impacto: " + impactPoint);
        }

        // Crear la explosión.
        if (ExplotionPrefab != null)
        {
            Instantiate(
                ExplotionPrefab,
                impactPoint,
                Quaternion.identity
            );
        }
        else
        {
            Debug.LogWarning("Falta asignar el prefab Explosión.");
        }

        // Destruir la bala.
        Destroy(gameObject);
    }
}
