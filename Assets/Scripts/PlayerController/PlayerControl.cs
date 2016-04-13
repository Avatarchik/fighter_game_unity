using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.
    private const int MOVEMENT_RIGHT = 1;
    private const int MOVEMENT_LEFT = 2;

	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public AudioClip[] taunts;				// Array of clips for when the player taunts.
	public float tauntProbability = 50f;	// Chance of a taunt happening.
	public float tauntDelay = 1f;			// Delay for when the taunt should happen.
    
	private int tauntIndex;					// The index of the taunts array indicating the most recent taunt.
	private Transform groundCheck;
    private Transform rightCheck;
	private Animator anim;					// Reference to the player's animator component.
    
    private int movement = 0;
    private bool jumpDown = false;
    private NetworkInterface networkInterface;
    private string session;
    private Vector3 lastPosition;
    public bool multiplayer;

    void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("groundCheck");
        rightCheck = transform.Find("rightCheck");
        lastPosition = transform.position;
		anim = GetComponent<Animator>();
        session = PlayerPrefs.GetString(Login.PREF_LOGIN_SESSION);
        if(multiplayer)
            networkInterface = GameObject.FindGameObjectWithTag("NetworkInterface").GetComponent<NetworkInterface>();
	}


	void Update()
	{
        
	}

    public void OnJumpClick() {
        jumpDown = true;
    }


	void FixedUpdate ()
	{
        

        bool grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        bool rightCollusion = rightCheck.GetComponent<RightCollider>().collision;



        if (CrossPlatformInputManager.GetAxis("Vertical")>0.7 && grounded) {
            jump = true;
            jumpDown = false;
        } else {
            jump = false;
        }

        float h = 0;
        // Cache the horizontal input.

        

        if (CrossPlatformInputManager.GetAxis("Horizontal")<0 && (grounded || !rightCollusion || movement == MOVEMENT_RIGHT)) {
            h = CrossPlatformInputManager.GetAxis("Horizontal");
            movement = MOVEMENT_LEFT;
            
        }

        if (CrossPlatformInputManager.GetAxis("Horizontal") > 0 && (grounded || !rightCollusion || movement == MOVEMENT_LEFT)) {
            h = CrossPlatformInputManager.GetAxis("Horizontal");
            movement = MOVEMENT_RIGHT;
        }

        // The Speed animator parameter is set to the absolute value of the horizontal input.
        anim.SetFloat("Speed", Mathf.Abs(h));

		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
			// ... add a force to the player.
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);

		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

		// If the input is moving the player right and the player is facing left...
		if(h > 0 && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(h < 0 && facingRight)
			// ... flip the player.
			Flip();

		// If the player should jump...
		if(jump)
		{
			// Set the Jump animator trigger parameter.
			anim.SetTrigger("Jump");

			// Play a random jump audio clip.
			//int i = Random.Range(0, jumpClips.Length);
			//AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

			// Add a vertical force to the player.
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
		}
        if (lastPosition != transform.position && networkInterface != null
            && (networkInterface.connectionStatus == NetworkInterface.CONNECTION_SUCESSFULL 
            || networkInterface.connectionStatus == NetworkInterface.ALLREDY_CONNECTED)) {
            SendPosition();
            lastPosition = transform.position;
        }if (lastPosition != transform.position) {
            lastPosition = transform.position;
        }
    }

    private void SendPosition() {
        if (multiplayer) {
            networkInterface.Send(DefaultMessages.GAME_MESSAGE + ";" + session + ";" + DefaultMessages.GAME_POSITION + ";" + transform.position.x + ";" + transform.position.y);
        }
    }

    void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


	public IEnumerator Taunt()
	{
		// Check the random chance of taunting.
		float tauntChance = UnityEngine.Random.Range(0f, 100f);
		if(tauntChance > tauntProbability)
		{
			// Wait for tauntDelay number of seconds.
			yield return new WaitForSeconds(tauntDelay);

			// If there is no clip currently playing.
			if(!GetComponent<AudioSource>().isPlaying)
			{
				// Choose a random, but different taunt.
				tauntIndex = TauntRandom();

				// Play the new taunt.
				GetComponent<AudioSource>().clip = taunts[tauntIndex];
				GetComponent<AudioSource>().Play();
			}
		}
	}


	int TauntRandom()
	{
		// Choose a random index of the taunts array.
		int i = UnityEngine.Random.Range(0, taunts.Length);

		// If it's the same as the previous taunt...
		if(i == tauntIndex)
			// ... try another random taunt.
			return TauntRandom();
		else
			// Otherwise return this index.
			return i;
	}
}
