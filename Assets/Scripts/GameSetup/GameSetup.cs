using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSetup", menuName = "ScriptableObjects/GameSetup", order = 1)]
public class GameSetup : ScriptableObject
{
    public List<GameRound> gameRounds;
    public int upperScoreLimit;
    public int lowerScoreLimit;
}
