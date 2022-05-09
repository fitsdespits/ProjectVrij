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
    public GameObject player;

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

    [Header("PERFORMANCE")]
    public float sleepDistance;
    public float fadeDistance;
    public bool fading = false;
    public float fadeCooldown;

    public void Awake()
    {
        cLight = critter.GetComponentInChildren<Light2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        //Deciding personality
        cPersonality = Random.Range(0, 7);
    }

    public void Update()
    {
        if (cMode != 0)
        {
            //Lighting
            if (critter.tag == "LitCritter" && cMode != 2)
            {
                cMode = 2;
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
            if (!inAction)
            {
                inAction = true;
                StartCoroutine(Movement());
            }

            //Actually performing the movement
            if (cMode == 1)
            {
                rb.AddForce(new Vector2(xRandom, yRandom) * (speedRandom * 7) * Time.deltaTime);
            }
            else
            if (cMode == 2)
            {
                rb.AddForce(new Vector2(xRandom, yRandom) * (speedRandom) * Time.deltaTime);
            }

            //Disable movement if critter is not within range of the player
            if (Vector3.Distance(critter.transform.position, player.transform.position) > sleepDistance)
            {
                cMode = 0;
            }

            //If player gets to close to critter, they fade for clarity
            if(Vector3.Distance(critter.transform.position, player.transform.position) < fadeDistance && !fading)
            {
                fading = true;
                StartCoroutine(Fade());
            } 
            else
            if (!fading)
            {
                fading = true;
                StartCoroutine(Unfade());
            }

        } 
        else
        if(Vector3.Distance(critter.transform.position, player.transform.position) < sleepDistance)
        {
            if (critter.tag == "LitCritter")
            {
                cMode = 2;
            }
            else
            {
                cMode = 1;
            }
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

    IEnumerator Fade()
    {
        SpriteRenderer critterRenderer = GetComponent<SpriteRenderer>();

        if (critterRenderer.color.a > 0.5f)
        {
            critterRenderer.color = new Color(critterRenderer.color.r, critterRenderer.color.g, critterRenderer.color.b, critterRenderer.color.a - 0.01f);
        }
        yield return new WaitForSecondsRealtime(fadeCooldown * Vector3.Distance(critter.transform.position, player.transform.position));
        fading = false;
    }
    IEnumerator Unfade()
    {
        SpriteRenderer critterRenderer = GetComponent<SpriteRenderer>();

        if (critterRenderer.color.a < 1)
        {
            critterRenderer.color = new Color(critterRenderer.color.r, critterRenderer.color.g, critterRenderer.color.b, critterRenderer.color.a + 0.01f);
        }
        yield return new WaitForSecondsRealtime(fadeCooldown * Vector3.Distance(critter.transform.position, player.transform.position));
        fading = false;
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
        else
        
        if (cPersonality == 6)
        {
            Personality6();
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
    private void Personality6() //cant stop wont stop
    {
        xRandom = Random.Range(-1f, 1f);
        yRandom = Random.Range(-1f, 1f);
        speedRandom = 100f;
        actionTimeRandom = Random.Range(1.5f, 3f);
        breakRandom = 0;
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
