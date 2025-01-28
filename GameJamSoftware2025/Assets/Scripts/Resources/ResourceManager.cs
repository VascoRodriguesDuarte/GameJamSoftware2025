using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private float chainReset;
    [SerializeField] private float chainResourceSpeed;
    [SerializeField] private float smallBoostSpeed;
    [SerializeField] private float bigBoostSpeed;
    [SerializeField] private float gaugeRefill;

    private float chainTimer = 0.0f;
    private float chainCombo = 0f;

    [SerializeField] private ScoreManager scoreManager;

    [SerializeField] private int boostEnemyScore = 250;
    [SerializeField] private int burstEnemyScore = 500;
    [SerializeField] private int explodeEnemyScore = 1000;

    [SerializeField] private int bigResourceScore = 300;
    [SerializeField] private int gaugeResourceScore = 150;
    [SerializeField] private int chainResourceScore = 25;
    [SerializeField] private int chainExtraResourceScore = 25;

    public void ChainUp()
    {
        gameObject.GetComponent<Movement>().SpeedMultiplier(chainResourceSpeed+(chainCombo/10));
        chainTimer = chainReset + Time.time;
        Debug.Log("Combo:"+chainCombo);
        chainCombo++;
        scoreManager.UpdateScore(chainResourceScore + (chainExtraResourceScore * ((int)chainCombo-1)));
        Invoke("CooldownCheck",3);
    }
    public void BigBoost()
    {
        gameObject.GetComponent<Movement>().SpeedMultiplier(bigBoostSpeed);
        scoreManager.UpdateScore(bigResourceScore);
    }
    public void SmallBoost()
    {
        gameObject.GetComponent<Movement>().SpeedMultiplier(smallBoostSpeed);
    }
    private void CooldownCheck()
    {
        if(Time.time >= chainTimer)
        {
            chainCombo =0;
        }
    }
    public void GaugeResource()
    {
        gameObject.GetComponentInChildren<Gauge>().IncreaseGauge(gaugeRefill);
        scoreManager.UpdateScore(gaugeResourceScore);
    }

    public void BoostEnemy()
    {
        gameObject.GetComponent<Movement>().CheckSpeed();
        scoreManager.UpdateScore(boostEnemyScore);
    }

    public void BurstEnemy()
    {
        gameObject.GetComponent<Movement>().CheckBurst();
        scoreManager.UpdateScore(burstEnemyScore);
    }

    public void ExplodeEnemy()
    {
        gameObject.GetComponent<Movement>().CheckSpeed();
        scoreManager.UpdateScore(explodeEnemyScore);
    }
}
