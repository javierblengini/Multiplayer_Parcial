using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    // Diccionario para almacenar los puntajes (ID del Jugador -> Sus Puntos)
    public Dictionary<ulong, int> playerScores = new Dictionary<ulong, int>();

    // Tiempo restante de la partida (Sincronizado)
    public NetworkVariable<float> timeRemaining = new NetworkVariable<float>(120f); // 2 minutos [cite: 40]
    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (!IsServer || gameEnded) return;

        // Manejo del temporizador en el servidor
        if (timeRemaining.Value > 0)
        {
            timeRemaining.Value -= Time.deltaTime;
        }
        else
        {
            timeRemaining.Value = 0;
            EndGame();
        }
    }

    public void AddScoreServer(ulong clientId, int points)
    {
        if (!IsServer) return;

        if (!playerScores.ContainsKey(clientId))
        {
            playerScores[clientId] = 0;
        }

        playerScores[clientId] += points;

        // Aquí mandarías a actualizar la UI de los clientes mediante un ClientRpc
        UpdateUIClientRpc(clientId, playerScores[clientId]);
    }

    [ClientRpc]
    private void UpdateUIClientRpc(ulong clientId, int newScore)
    {
        [cite_start]// Lógica para actualizar los textos de la pantalla (ej. "Jugador X: Puntos") [cite: 74]
        Debug.Log($"Jugador {clientId} ahora tiene {newScore} puntos.");
    }

    private void EndGame()
    {
        gameEnded = true;
        [cite_start]// Lógica del Servidor para avisar a todos que muestren la pantalla de Ganador [cite: 22, 64]
        ShowEndScreenClientRpc();
    }

    [ClientRpc]
    private void ShowEndScreenClientRpc()
    {
        [cite_start]// Activar el panel de Fin de Partida en las pantallas de todos [cite: 22, 63]
        Time.timeScale = 0f; // Pausa visual opcional
    }
}