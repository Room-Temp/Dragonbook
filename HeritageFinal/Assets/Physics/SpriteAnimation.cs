using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    Name: SpriteAnimation
    Type: Action Script
    Purpose: This script handles all forms of animation within the game. 
             This will make it easier to integrate than Unity's default animation feature.
*/

public class SpriteAnimation : MonoBehaviour {

    public enum spriteIndex {defaultSprite, upMovement, rightMovement, downMovement, leftMovement, attacking, casting, channelling, item, death, revive};

    public Sprite defaultSprite;
    public Sprite[] upMovementFrames;
    public Sprite[] rightMovementFrames;
    public Sprite[] downMovementFrames;
    public Sprite[] leftMovementFrames;
    public Sprite[] attackAnimation;
    public Sprite[] castingAnimation;
    public Sprite[] channellingAnimation;
    public Sprite[] itemGetAnimation;
    public Sprite[] deathAnimation;
    public Sprite[] reviveAnimation;
    
    public Sprite[] currentSprites;

    private IEnumerator animate;
    private bool coroutineRunning;
   

    void Start () {
        coroutineRunning = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite;
	}

    public void anim(spriteIndex sprites)
    {
        switch (sprites)
        {
            case spriteIndex.upMovement:
                currentSprites = upMovementFrames;
                break;
            case spriteIndex.rightMovement:
                currentSprites = rightMovementFrames;
                break;
            case spriteIndex.downMovement:
                currentSprites = downMovementFrames;
                break;
            case spriteIndex.leftMovement:
                currentSprites = leftMovementFrames;
                break;
            case spriteIndex.attacking:
                currentSprites = attackAnimation;
                break;
            case spriteIndex.casting:
                currentSprites = castingAnimation;
                break;
            case spriteIndex.channelling:
                currentSprites = channellingAnimation;
                break;
            case spriteIndex.item:
                currentSprites = itemGetAnimation;
                break;
            case spriteIndex.death:
                currentSprites = deathAnimation;
                break;
            case spriteIndex.revive:
                currentSprites = reviveAnimation;
                break;
        }
        startAnimation();
    }

    public void startAnimation() //Function for forcibly starting animation
    {
        if (coroutineRunning) StopCoroutine(animate);
        coroutineRunning = true;
        animate = _animate();
        StartCoroutine(animate);
    }

    public void startAnimation(Sprite[] sprites) //Cutscene animations can be passed through this overload function
    {
        currentSprites = sprites;
        startAnimation();
    }

    public void stopAnimation() //Stop non-movement animation
    {
        coroutineRunning = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = currentSprites[0];
    }

    public void stopAnimation(int prevDir, int prevCardDir) //Stop animation for movement
    {
        coroutineRunning = false;
            switch (prevDir)
            {
                case Direction.UP:
                    defaultSprite = upMovementFrames[0];
                    break;
                case Direction.UP_RIGHT:
                    if (prevCardDir == Direction.RIGHT) defaultSprite = rightMovementFrames[0];
                    else defaultSprite = upMovementFrames[0];
                    break;
                case Direction.RIGHT:
                    defaultSprite = rightMovementFrames[0];
                    break;
                case Direction.DOWN_RIGHT:
                    if (prevCardDir == Direction.RIGHT) defaultSprite = rightMovementFrames[0];
                    else defaultSprite = downMovementFrames[0];
                    break;
                case Direction.DOWN:
                    defaultSprite = downMovementFrames[0];
                    break;
                case Direction.DOWN_LEFT:
                    if (prevCardDir == Direction.LEFT) defaultSprite = leftMovementFrames[0];
                    else defaultSprite = downMovementFrames[0];
                    break;
                case Direction.LEFT:
                    defaultSprite = leftMovementFrames[0];
                    break;
                case Direction.UP_LEFT:
                    if (prevCardDir == Direction.LEFT) defaultSprite = leftMovementFrames[0];
                    else defaultSprite = upMovementFrames[0];
                    break;
            }
            gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite;       
    }

    private IEnumerator _animate()
    {
        int spritePos = 0;
        while (coroutineRunning)
        {
            if (!coroutineRunning) break;
            spritePos++;
            if (spritePos == currentSprites.Length)
            {
                spritePos = 0;
            }
            gameObject.GetComponent<SpriteRenderer>().sprite = currentSprites[spritePos];
            for (int i = 0; i <= gameObject.GetComponent<Character>().animationSpeed; i++)
            {
                //if (!coroutineRunning) break;
                yield return new WaitForEndOfFrame();
            }            
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
