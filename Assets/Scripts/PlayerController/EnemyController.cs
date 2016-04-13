using UnityEngine;
using System.Collections;
using System;

public class EnemyController : MonoBehaviour {

    public AudioClip[] jumpClips;            // Amount of force added when the player jumps.
    public AudioClip[] taunts;              // Array of clips for when the player taunts.
    public float tauntProbability = 50f;    // Chance of a taunt happening.
    public float tauntDelay = 1f;           // Delay for when the taunt should happen.
    private Transform groundCheck;
    public const int FACE_RIGHT = 1;
    public const int FACE_LEFT = 2;
    private Vector3 lastPosition;
    private int faceDirection;
    private int tauntIndex;    
    private Animator anim;                  // Reference to the player's animator component.
    private bool grounded = true;
    
    private bool jump;
    private Vector3 nextPosition;

    public void SetPosition(Vector2 vector2) {
        nextPosition = new Vector3(vector2.x, vector2.y, transform.position.z);
    }

    void Awake() {
        anim = GetComponent<Animator>();
        lastPosition = transform.position;
        nextPosition = transform.position;
        jump = true;
        groundCheck = transform.Find("groundCheck");
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
    }

    void FixedUpdate() {
        transform.position = nextPosition;
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        if (lastPosition.x > transform.position.x) {
            Flip(FACE_LEFT);
        } else {
            Flip(FACE_RIGHT);
        }
        if (!grounded && jump) {
            anim.SetTrigger("Jump");
            jump = false;
        }else if(grounded && !jump) {
            jump = true;
        }
        float speedX = Math.Abs(transform.position.x - lastPosition.x);
        anim.SetFloat("Speed", speedX * 10);
        lastPosition = transform.position;
    }

    void Flip(int direction) {
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        if (direction == FACE_LEFT) {
            if (theScale.x > 0) {
                theScale.x *= -1;
            }
        } else {
            if (theScale.x < 0) {
                theScale.x *= -1;
            }
        }
        transform.localScale = theScale;
    }


    public IEnumerator Taunt() {
        // Check the random chance of taunting.
        float tauntChance = UnityEngine.Random.Range(0f, 100f);
        if (tauntChance > tauntProbability) {
            // Wait for tauntDelay number of seconds.
            yield return new WaitForSeconds(tauntDelay);

            // If there is no clip currently playing.
            if (!GetComponent<AudioSource>().isPlaying) {
                // Choose a random, but different taunt.
                tauntIndex = TauntRandom();

                // Play the new taunt.
                GetComponent<AudioSource>().clip = taunts[tauntIndex];
                GetComponent<AudioSource>().Play();
            }
        }
    }


    int TauntRandom() {
        // Choose a random index of the taunts array.
        int i = UnityEngine.Random.Range(0, taunts.Length);

        // If it's the same as the previous taunt...
        if (i == tauntIndex)
            // ... try another random taunt.
            return TauntRandom();
        else
            // Otherwise return this index.
            return i;
    }
}
