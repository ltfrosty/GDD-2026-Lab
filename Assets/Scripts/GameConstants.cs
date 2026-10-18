using UnityEngine;

[CreateAssetMenu(fileName = "GameConstants", menuName = "ScriptableObjects/GameConstants", order = 1)]
public class GameConstants : ScriptableObject
{
    // Game settings
    public int targetFrameRate = 40;

    // lives
    public int maxLives;

    // Mario
    public int speed;
    public int maxSpeed;
    public int upSpeed;
    public int deathImpulse;
    public float bounceOffGoombaForce;
    public Vector3 marioStartingPosition;
    public Vector3 marioStartPosition = new Vector3(-10.0f, -4.69f, 0.0f);
    public float gameOverDelay = 1.2f;

    // Goomba
    public float goombaPatrolTime = 5.0f;
    public float goombaMaxOffset = 2.0f;
    public int goombaScoreValue = 1;
    public float goombaDestroyDelay = 0.5f;

    // Powerup Box
    public float boxBounceDisplacement = 0.3f;
    public float boxMaxBounceTime = 1f;

    // Starman
    public float powerupMoveForce = 3f;
    public float starmanBounceForce = 8f;
}