using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSprite : MonoBehaviour
{

    public SpriteRenderer sprite;
    public bool canTakeDamage;
    public int maxLives;
    private int currentLives;

    public int flickerAmnt;
    public float flickerDuration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canTakeDamage = true;
        currentLives = maxLives;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        if(canTakeDamage == true)
        {
            currentLives--;
            StartCoroutine(DamageFlicker());
        }
    }

    IEnumerator DamageFlicker()
    {
        canTakeDamage = false;
        for(int i = 0; i < flickerAmnt; i++)
        {
            sprite.color = new Color(1f,1f,1f,5f);
            yield return new WaitForSeconds(flickerDuration);
            sprite.color = Color.white;
            yield return new WaitForSeconds(flickerDuration);
            canTakeDamage = true;
        }
    }
}