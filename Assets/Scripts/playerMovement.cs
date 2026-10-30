using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class playerMovement : MonoBehaviour
{
    
    
    //checking for ground
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f;
    //movement and jumpforce
    public float movementSpeed = 100f;
    private float currentSpeed;
    public float sprintSpeed = 150f;


    //stamina variable
    public float maxStamina = 100f;
    private float currentStamina;
    public float staminaDrain = 20f;
    public float staminaRegenTime = 5f;
    public float staminaRegen = 50f;

    //health variable
    public float maxHealth = 100f;
    private float currentHealth;
    public float staminaHealth = 50f;



    //UI 

    public Slider staminaBar;
    public Slider healthBar;
    public CanvasGroup pauseMenu;
    private bool isPaused = false;
    

    //Rigidbody
    private Rigidbody rb;
    private Collider playerCollider;

    //key for sprint

    private bool isSprinting;
    private bool isMoving;
     void Start()
    {
        //Intialize Rigidbody, Health and Stamina
        rb = GetComponent<Rigidbody>();
        currentStamina = maxStamina;
        currentSpeed = movementSpeed;
        currentHealth = maxHealth;

        //initialize health UI
        if(healthBar != null )
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
        //intialize stamina UI
        if(staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = currentStamina;
            Debug.Log("staminareset");
        }
        playerCollider = GetComponent<Collider>();
        //initialize pausemenu
        pauseMenu.alpha = 0f;
    }

    void Update()
    {
        //check if the sprint is held and stamina is enough
        isSprinting = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0;
        Debug.Log(isSprinting);

        //UPDATE MOVEMENT
        Movement();

        isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;

        if (isSprinting && isMoving)
        {
            currentSpeed = sprintSpeed;
            drainStamina();
        }
        else
        {
            currentSpeed = movementSpeed;
            regenerateStamina();
        }

        if(staminaBar != null)
        {
            staminaBar.value = currentStamina;
        }

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
        // Check for Space key down and if the player is currently on the ground
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            // Apply an instant upwards physical force
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        // check if escape is pressed, if it is pause or unpause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
            
        }

    }
    //code for the movement
    private void Movement()
    {
        //Debug.Log("Movement Called");
        
        //Get input for movement WASD/Arrow Keys

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // calculate movement direction
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized;
        

        //move player w/Rigidbody

        rb.MovePosition(transform.position + transform.TransformDirection(movement) * currentSpeed * Time.deltaTime);

        
    }

    //drain stamina

    private void drainStamina()
    {
        //currentStamina = currentStamina - staminaDrain * Time.deltaTime;
        currentStamina -= staminaDrain*Time.deltaTime;

        if(currentStamina < 0)
        {
            currentStamina = 0;
            isSprinting = false;
        }
    }

    private void regenerateStamina()
    {
        if(currentStamina < maxStamina)
        {
            currentStamina += staminaRegenTime * Time.deltaTime;

            if(currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }
    }

    //check if player is grounded before jumping again
    private bool IsGrounded()
    {
        // Shoot a raycast straight down from the bottom of the player's collider
        Vector3 rayStart = new Vector3(transform.position.x, playerCollider.bounds.min.y + 0.1f, transform.position.z);
        return Physics.Raycast(rayStart, Vector3.down, groundCheckDistance, groundLayer);
    }
    //if escape is pressed, game will pause
    private void PauseGame()
    {
        pauseMenu.alpha = 1f;          // Makes it visible
        pauseMenu.interactable = true;  // Allows clicking buttons
        pauseMenu.blocksRaycasts = true; ;
        Time.timeScale = 0f;
        Debug.Log("game paused");
        isPaused = true;
    }
    
    private void ResumeGame()
    {
        pauseMenu.alpha = 0f;          
        pauseMenu.interactable = false;  
        pauseMenu.blocksRaycasts = false; ;
        Time.timeScale = 1f;
        Debug.Log("game resumed");
        isPaused = false;
    }
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Prevents health from exceeding max

        Debug.Log("Player healed. Current health: " + currentHealth);
    }
    public void Damage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Prevents health from exceeding max

        Debug.Log("Player Damaged. Current health: " + currentHealth);
    }

}