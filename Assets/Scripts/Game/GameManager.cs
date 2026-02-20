using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Game CurrentActiveGame { get; private set; }

    public bool IsInputEnabled { get; set; }
    public void RegisterGame(Guid gameId, string[] players) {
        CurrentActiveGame = new Game {
            GameId = gameId,
            Players = players,
            CurrentPlayerTurn = players[0],
            GameStartTime = DateTime.Now
        };

        IsInputEnabled = true;

        Debug.Log($"Game registered with ID: {gameId} and players: {string.Join(", ", players)}");
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public class Game {
        public Guid GameId { get; set; }

        public string[] Players { get; set; }

        public string CurrentPlayerTurn { get; set; }

        public DateTime GameStartTime { get; set; }

        public DateTime GameEndTime { get; set; }
    }
}
