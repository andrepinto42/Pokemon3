using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameVisualEffectsHandler : MonoBehaviour
{
    public ParticleSystem allyParticles;
    public ParticleSystem enemyParticles;
    public ParticleSystem damageParticles;
    
    [Header("Stats")]
    public float radius = 4f;
    public int numberOfLaps = 2;
    public float speed = 5f;
    
    public Gradient gradientRise;
    public Gradient gradientDecrease;
    public static GameVisualEffectsHandler Singleton;
    void Start()
    {
        if (Singleton == null)
            Singleton = this;
        else    Destroy(this);

        allyParticles.gameObject.SetActive(false);
        enemyParticles.gameObject.SetActive(false);
    }


    public void AllyStartSpinning(SkillBuff skill,MonManager mon)
    {
        StartSpinning(allyParticles,skill.effect,skill.ally,skill.increase,mon.transform.position);
    }

     public void EnemyStartSpinning(SkillBuff skill,MonManager mon)
    {
        StartSpinning(enemyParticles,skill.effect,skill.ally,skill.increase,mon.transform.position);
    }

    private async void StartSpinning(ParticleSystem particles,SkillBuff.Stat stat,bool isAlly,float increase,Vector3 startPosition)
    {
        particles.gameObject.SetActive(true);
        var colorOverlifeTime = particles.colorOverLifetime;
        colorOverlifeTime.color = (increase >= 1) ? gradientRise : gradientDecrease;

        if (isAlly)
        {
            var pm = particles.main;
            pm.gravityModifierMultiplier =1;

            await ParticleSpinnerHandler.StartSpinning(radius,numberOfLaps,speed, particles.transform,startPosition,0f);
        }
        else
        {
            var pm = particles.main;
            pm.gravityModifierMultiplier =0;

            await ParticleSpinnerHandler.StartSpinning(radius,numberOfLaps,speed, particles.transform,startPosition,1f);
        }
        particles.gameObject.SetActive(false);
    }


    public void StartEmittingDamageParticle(MonManager mon,float damage)
    {
        var emission =damageParticles.emission;
        emission.rateOverTime =   (damage/ mon.MonMain.maxHealth) * 60f;
        damageParticles.gameObject.transform.position = mon.GetMonMeshManager().pivotHUD.position;
        damageParticles.Play();
    }


    public void StopEmittingDamageParticle()
    {

        damageParticles.Stop();
    }

   
}
