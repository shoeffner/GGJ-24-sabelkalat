using System;
using System.Collections.Generic;

[Serializable]
public struct GameRound
{
    public List<Viewer> viewers;
    public float correctAnswersScore;
    public float wrongAnswersScore;
}
