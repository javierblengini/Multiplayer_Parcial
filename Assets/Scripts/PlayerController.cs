using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = 5f;

    // Variable en red para saber si este jugador lleva un objeto
    // Se sincroniza automáticamente de Servidor a Clientes
    public NetworkVariable<bool> hasObject = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private void Update()
    {
        // Si no es el jugador local, no hacemos nada con el teclado/joystick
        if (!IsOwner) return;

        Move();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }
}