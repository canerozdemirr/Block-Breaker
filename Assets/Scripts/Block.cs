using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private GameObject blockSparklesVFX;
    [SerializeField] private int timesHit = 0;
    [SerializeField] private Sprite[] hitSprites;

    private Level level;
    private int spriteIndex;

    void Start()
    {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable")
        {
            level.CountBreakableBlocks();
        }
        FindObjectOfType<GameSession>();
    }       

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable")
        {
            HandleHits();
        }
        FindObjectOfType<GameSession>().AdditionToTheScore();
    }

    private void HandleHits()
    {
        timesHit++;
        int maxHits = hitSprites.Length + 1;
        if (timesHit == maxHits)
        {
            DestroyBlocks();
        }
        else
        {
            ShowNextHitSprite();
        }
    }

    private void ShowNextHitSprite()
    {
        spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex] != null)
        {
           GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        
    }

    private void DestroyBlocks()
    {
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
        Destroy(gameObject);
        TriggerSparklesVFX();
        level.BlocksAreDestroyed();
    }

    private void TriggerSparklesVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }


}
