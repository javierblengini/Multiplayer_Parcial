using Unity.Netcode;
using UnityEngine;

public class CollectibleItem : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Solo el servidor procesa las colisiones de mecánicas de juego
        if (!IsServer) return;

        // Verificamos si el que lo tocó es un jugador
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            // Si el jugador no tiene un objeto, se lo lleva
            if (!player.hasObject.Value)
            {
                player.hasObject.Value = true;

                // Desaparece el objeto de la red de forma segura
                GetComponent<NetworkObject>().Despawn();
                Destroy(gameObject);
            }
        }
    }
}