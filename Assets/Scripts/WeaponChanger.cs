using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    public GameObject currentWeapon;  // El arma actual del jugador
    public Transform weaponSpawnPoint; // El punto de spawn donde aparecerá el AK47

    public void ChangeWeaponToAK47(GameObject ak47Prefab, Transform spawnPoint)
    {
        // Primero, destruye el arma actual si existe
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);  // Destruir el arma anterior
        }

        // Instanciamos el AK47 en la posición y rotación del spawnPoint
        GameObject newWeapon = Instantiate(ak47Prefab, spawnPoint.position, spawnPoint.rotation);

        // Hacemos que el AK47 sea hijo del spawnPoint
        newWeapon.transform.SetParent(spawnPoint);

        // Rotar el arma 180 grados en el eje Y (horizontal)
        newWeapon.transform.Rotate(0, 180f, 0); // Ajuste de rotación

        // Asignamos el AK47 como el arma actual
        currentWeapon = newWeapon;

        // Aseguramos que la referencia a 'Weapon' también se actualice en el Player
        Weapon newWeaponScript = newWeapon.GetComponent<Weapon>();
        Player player = GetComponent<Player>();

        if (player != null)
        {
            player.weapon = newWeaponScript;  // Actualizamos la referencia al arma en el jugador
        }

        // Mostrar en la consola que el arma ha sido cambiada
        Debug.Log("Arma cambiada a AK47");
    }

}
