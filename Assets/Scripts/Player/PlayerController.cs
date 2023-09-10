using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // HEALTH PLAYER
    [SerializeField] private float maxHealth = 0f;
    private float currentHealth = 0f;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private TextMeshProUGUI textHealthBar;

    // STAMINA PLAYER
    [SerializeField] private float maxStamina = 0f;
    private float currentStamina = 0f;
    [SerializeField] private GameObject staminaBar;
    [SerializeField] private TextMeshProUGUI textStaminaBar;
    
    // STRENGTH PLAYER
    [SerializeField] private float strength = 0f;

    // BLOOD PARTICLE
    [SerializeField] private GameObject bloodObject;


    private float countTimeIncreaseStamina = 0f;

    [SerializeField] private Animator animator;

    private PlayerInput input;
    private PlayerController controller;

    [SerializeField]private float moveSpeed = 5f;
    private Vector3 moveDir = Vector3.zero;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        input = GetComponent<PlayerInput>();
        controller = GetComponent<PlayerController>();

        SetHealth(maxHealth);
        SetStamina(maxStamina);
        SetStrength(strength);
    }

    void Start()
    {
        
    }

    void Update()
    {
        LifeController();

        Moving();

        FlipXByMouse();

        FlipYByMouse();

        IncreaseStaminaByTime();
    }

    void LifeController()
    {
        if (GetHealth() <= 0)
        {
            animator.Play("Die");
            input.enabled = false;
            controller.enabled = false;
        }
    }

    void FlipXByMouse()
    {
        Vector2 direction = input.inputMosue - new Vector2(transform.position.x, transform.position.y);

        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (direction.x > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void FlipYByMouse()
    {
        Vector2 direction = input.inputMosue - new Vector2(transform.position.x, transform.position.y);

        if (direction.y < 0)
        {
            animator.Play("DownRight");
        }
        else if (direction.y > 0)
        {
            animator.Play("UpRight");
        }
    }

    private void Moving()
    {
        float horizontal = input.horizontal;
        float vertical = input.vertical;

        moveDir.Set(horizontal, vertical, 0f);
        moveDir.Normalize();

        transform.Translate( moveDir * (moveSpeed * Time.deltaTime));
    }

    public void BloodOut()
    {
        Instantiate(bloodObject, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttackRange"))
        {
            DecreaseHealth(collision.gameObject.GetComponentInParent<EnemyHandle>().GetStrength());
            BloodOut();
        }
    }

    // ================= HANDLE HEALTH ======================
    public void SetHealth(float newHealth)
    {
        maxHealth = newHealth;
        currentHealth = maxHealth;
        healthBar.GetComponent<Slider>().maxValue = maxHealth;
        LoadHealth();
    }

    public void LoadHealth()
    {
        textHealthBar.text = currentHealth + "/" + maxHealth;
        healthBar.GetComponent<Slider>().value = currentHealth;
    }

    public void IncreaseHealth(float newHealth)
    {
        if (currentHealth <= maxHealth)
        {
            currentHealth += newHealth;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }

        LoadHealth();
    }

    public void DecreaseHealth(float lostHealth)
    {
        if (currentHealth > 0)
        {
            currentHealth -= lostHealth;

            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
        }

        LoadHealth();
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    // ==========================================

    // ======= HANDLE STAMINA ==============
    public void SetStamina(float newStamina)
    {
        maxStamina = newStamina;
        currentStamina = maxStamina;
        staminaBar.GetComponent<Slider>().maxValue = maxStamina;
        LoadStamina();
    }

    public void LoadStamina()
    {
        textStaminaBar.text = currentStamina + "/" + maxStamina;
        staminaBar.GetComponent<Slider>().value = currentStamina;
    }

    public void IncreaseStamina(float newStamina)
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += newStamina;

            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }

        LoadStamina();
    }

    public void DecreaseStamina(float lostStamina)
    {
        if (currentStamina >= 0)
        {
            currentStamina -= lostStamina;

            if (currentStamina < 0)
            {
                currentStamina = 0;
            }
        }

        LoadStamina();
    }

    public float GetStamina()
    {
        return currentStamina;
    }

    private void IncreaseStaminaByTime()
    {
        if (currentStamina < maxStamina)
        {
            countTimeIncreaseStamina += Time.deltaTime;

            if (countTimeIncreaseStamina > 0.5f)
            {
                IncreaseStamina(1);

                countTimeIncreaseStamina = 0;
            }
        }
    }
    // ===========================================

    // ======= HANDLE STRENGTH ==============
    public void SetStrength(float newStrength)
    {
        strength = newStrength;
    }

    public void IncreaseStength(float newStrength)
    {
        strength += newStrength;
    }

    public void DecreaseStength(float lostStrength)
    {
        if (strength >= 0)
        {
            strength -= lostStrength;

            if (strength < 0)
            {
                strength = 0;
            }
        }
    }

    public float GetStrength()
    {
        return strength;
    }
    // ===========================================

    // ======= HANDLE SPEED ==============
    public float GetSpeed()
    {
        return moveSpeed;
    }
    // ===========================================
}
