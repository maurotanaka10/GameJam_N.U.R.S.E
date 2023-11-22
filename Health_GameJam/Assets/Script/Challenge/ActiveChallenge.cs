using System;

public class ActiveChallenge
{
    public EChallenges ChallengeType { get; private set; }
    public float ChallengeTimer { get; private set; }

    public event Action<EChallenges> OnChallengeTimeout;

    public ActiveChallenge(EChallenges challengeType, float challengeTimer)
    {
        ChallengeType = challengeType;
        ChallengeTimer = challengeTimer;
    }

    public void UpdateTimer(float deltaTime)
    {
        ChallengeTimer -= deltaTime;

        if (ChallengeTimer <= 0)
        {
            OnChallengeTimeout?.Invoke(ChallengeType);
        }
    }
}
