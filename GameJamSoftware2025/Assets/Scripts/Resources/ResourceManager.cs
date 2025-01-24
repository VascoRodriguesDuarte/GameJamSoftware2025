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

    public void ChainUp()
    {
        gameObject.GetComponent<Movement>().SpeedMultiplier(chainResourceSpeed+(chainCombo/10));
        chainTimer = chainReset + Time.time;
        Debug.Log("Combo:"+chainCombo);
        chainCombo++;
        Invoke("CooldownCheck",3);
    }
    public void BigBoost()
    {
        gameObject.GetComponent<Movement>().SpeedMultiplier(bigBoostSpeed);
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
    }

    public void BoostEnemy()
    {
        gameObject.GetComponent<Movement>().CheckSpeed();
    }

    public void BurstEnemy()
    {
        gameObject.GetComponent<Movement>().CheckBurst();
    }

    public void ExplodeEnemy()
    {
        gameObject.GetComponent<Movement>().CheckSpeed();
    }
}
