using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SlimeMovement : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D bc;

    SpriteRenderer asr;
    System.Random rand = new System.Random();

    [SerializeField] int minJumpDist;
    [SerializeField] int maxJumpDist;
    [SerializeField] int maxJumpHeight;
    [SerializeField] int minJumpHeight;
    [SerializeField] int jumpFrequency;

    [SerializeField] private GameObject arrowPrefab; // Reference to the arrow prefab
    private GameObject arrowInstance; // Instance of the arrow for this slime

    private bool isGrounded = true;
    private float decisionTimer = 1f;
    private float timer = 0f;
    private int failedJumps = 0;
    private bool isLeader = false;
    public static SlimeMovement furthestSlime = null;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();

        // Instantiate the arrow and make it a child of this slime
        if (arrowPrefab != null)
        {
            arrowInstance = Instantiate(arrowPrefab, transform);
            arrowInstance.transform.localPosition = new Vector3(0, 15f, 0); // Position the arrow above the slime
            asr = arrowInstance.GetComponent<SpriteRenderer>();
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (bc.IsTouchingLayers(LayerMask.GetMask("Goal")))
        {
            Debug.Log("You win!");
        }
        // Check if the slime is on the ground
        isGrounded = bc.IsTouchingLayers(LayerMask.GetMask("Ground"));

        // Check if the slime should try to jump
        if (isGrounded)
        {
            if (timer < decisionTimer)
            {
                timer += Time.deltaTime;
            }
            else
            {
                JumpDecision();
                timer = 0f;
            }
        }

        // Check if this slime is the furthest
        if (furthestSlime == null || transform.position.x > furthestSlime.transform.position.x)
        {
            furthestSlime = this;
            isLeader = true;
        }
        else
        {
            isLeader = false;
        }

        asr.color = (furthestSlime == this) ? new Color32(255, 0, 0, 255) : new Color32(0, 0, 0, 255);

    }
    void JumpDecision()
    {
        // Generate values to determine if slime jumps
        float jumpProbability = jumpFrequency / 10f;
        float randomValue = Random.Range(0f, 1f);

        // Check if the random value is less than the jump probability
        if ((randomValue < jumpProbability && isGrounded) || failedJumps >= 8)
        {
            rb.velocity = new Vector2(rand.Next(minJumpDist, maxJumpDist), rand.Next(minJumpHeight, maxJumpHeight));
            failedJumps = 0;
        }
        else
        {
            failedJumps += 1;
        }
    }
}
