using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CritterBehaviour : MonoBehaviour
{
    [Header("COMPONENTS")]
    public GameObject critter;
    public Rigidbody2D rb;
    public Material flash;

    [Header("INFORMATION")]
    public int cMode = 1;
    public int cPersonality = 0;

    [Header("Lighting")]
    public Light2D cLight;
    public float wantedIntensity;
    public float wantedRadius;
    public float lightSpeed;

    [Header("FREEZE")]
    public float freezeDuration;
    public bool flashed = false;

    [Header("MOVEMENT")]
    public bool inAction = false;
    public bool inversed = false;
    public float xRandom;
    public float yRandom;
    public float speedRandom;
    public float actionTimeRandom;
    public float breakRandom;

    public void Awake()
    {
        cLight = critter.GetComponentInChildren<Light2D>();

        //Deciding personality
        cPersonality = Random.Range(0, 6);
    }

    public void Update()
    {   
        //Lighting
        if (critter.tag == "LitCritter" && cMode != 0)
        {
            cMode = 0;
            BrightGlow();
            if (!flashed)
            {
                flashed = true;
                StartCoroutine(DoFreeze());
            }
        }

        if (cLight.intensity < wantedIntensity && Mathf.Abs(cLight.intensity - wantedIntensity) > 0.02)
        {
            cLight.intensity += lightSpeed * Time.deltaTime;
        }

        if (cLight.intensity > wantedIntensity && Mathf.Abs(cLight.intensity - wantedIntensity) > 0.02)
        {
            cLight.intensity -= lightSpeed * Time.deltaTime;
        }

        if (cLight.pointLightOuterRadius < wantedRadius && Mathf.Abs(cLight.pointLightOuterRadius - wantedRadius) > 0.02)
        {
            cLight.pointLightOuterRadius += lightSpeed * 20 * Time.deltaTime;
        }

        if (cLight.pointLightOuterRadius > wantedRadius && Mathf.Abs(cLight.pointLightOuterRadius - wantedRadius) > 0.02)
        {
            cLight.pointLightOuterRadius -= lightSpeed * 20 * Time.deltaTime;
        }

        //Starting movement
        if(!inAction)
        {
            inAction = true;
            StartCoroutine(Movement());
        }

        //Actually performing the movement
        if(cMode == 1)
        {
            rb.AddForce(new Vector2(xRandom, yRandom) * (speedRandom * 5) * Time.deltaTime);
        } 
        else
        if (cMode == 0)
        {
            rb.AddForce(new Vector2(xRandom, yRandom) * (speedRandom) * Time.deltaTime);
        }
    }

    public void BrightGlow()
    {
        wantedIntensity = 0.5f;
        wantedRadius = 10f;
    }

    IEnumerator DoFreeze()
    {
        SpriteRenderer critterRenderer = GetComponent<SpriteRenderer>();
        Material flashMaterial = flash;
        Material originalMaterial = this.GetComponent<SpriteRenderer>().material;

        critterRenderer.material = flashMaterial;

        yield return new WaitForSecondsRealtime(freezeDuration);

        critterRenderer.material = originalMaterial;

        yield return new WaitForSecondsRealtime(freezeDuration);

        critterRenderer.material = flashMaterial;

        yield return new WaitForSecondsRealtime(freezeDuration);

        critterRenderer.material = originalMaterial;
    }

    IEnumerator Movement()
    {
        if(cPersonality == 0)
        {
            Personality0();
        }
        else

        if (cPersonality == 1)
        {
            Personality1();
        }
        else

        if (cPersonality == 2)
        {
            Personality2();
        }
        else

        if (cPersonality == 3)
        {
            Personality3();
        }
        else

        if (cPersonality == 4)
        {
            Personality4();
        }
        else

        if (cPersonality == 5)
        {
            Personality5();
        }

        yield return new WaitForSecondsRealtime(actionTimeRandom);
        
        xRandom = 0;
        yRandom = 0;
        speedRandom = 0;
        actionTimeRandom = 0;

        yield return new WaitForSecondsRealtime(breakRandom);

        breakRandom = 0;

        inAction = false;
    }

    private void Personality0() //slow
    {
        xRandom = Random.Range(-1f, 1f);
        yRandom = Random.Range(-1f, 1f);
        speedRandom = Random.Range(20f, 35f);
        actionTimeRandom = Random.Range(2f, 7f);
        breakRandom = Random.Range(0f, 2f);
    }
    private void Personality1() //determined
    {
        xRandom = Random.Range(-1f, 1f);
        yRandom = Random.Range(-1f, 1f);
        speedRandom = Random.Range(40f, 50f);
        actionTimeRandom = Random.Range(8f, 15f);
        breakRandom = Random.Range(5f, 7f);
    }
    private void Personality2() //chaotic
    {
        xRandom = Random.Range(-1f, 1f);
        yRandom = Random.Range(-1f, 1f);
        speedRandom = Random.Range(80f, 100f);
        actionTimeRandom = Random.Range(0.1f, 1.5f);
        breakRandom = Random.Range(0.1f, 1.5f);
    }
    private void Personality3() //lazy
    {
        xRandom = Random.Range(-1f, 1f);
        yRandom = Random.Range(-1f, 1f);
        speedRandom = Random.Range(20f, 35f);
        actionTimeRandom = Random.Range(2f, 5f);
        breakRandom = Random.Range(10f, 15f);
    }
    private void Personality4() //sprinter
    {
        xRandom = Random.Range(-1f, 1f);
        yRandom = Random.Range(-1f, 1f);
        speedRandom = Random.Range(100f, 120f);
        actionTimeRandom = Random.Range(1f, 3f);
        breakRandom = Random.Range(8f, 12f);
    }
    private void Personality5() //varied
    {
        xRandom = Random.Range(-1f, 1f);
        yRandom = Random.Range(-1f, 1f);
        speedRandom = Random.Range(10f, 70f);
        actionTimeRandom = Random.Range(1f, 8f);
        breakRandom = Random.Range(2f, 12f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!inversed)
        {
            inversed = true;
            xRandom *= -1;
            yRandom *= -1;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (inversed)
        {
            inversed = false;
        }
    }
}
