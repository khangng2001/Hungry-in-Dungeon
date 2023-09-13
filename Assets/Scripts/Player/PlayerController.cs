using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    //Coin Paper
    private int coinPaper;

    // FADE BLACK
    [SerializeField] private GameObject FadeBlack;

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

    // PLAYER'S COMPONENT
    private PlayerInput playerInput;
    private PlayerController playerController;

    [SerializeField]private float moveSpeed = 5f;
    private Vector3 moveDir = Vector3.zero;
    public static PlayerController instance;
    private bool canMove = true;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerController = GetComponent<PlayerController>();
        SetHealth(maxHealth);
        SetStamina(maxStamina);
        SetStrength(strength);
    }

    void Update()
    {
        LifeController();

        if (canMove)
        {
            Moving();

            FlipXByMouse();

            FlipYByMouse();
        }

        IncreaseStaminaByTime();
    }

    void LifeController()
    {
        if (GetHealth() <= 0)
        {
            animator.Play("Die");
            playerInput.enabled = false;
            //playerController.enabled = false;
            transform.GetChild(1).gameObject.SetActive(false); // LIGHT2D (Tat den cua Ngo Tat To)

            if (!isFadeOut)
            {
                isFadeOut = true;

                // KHONG THE DI CHUYEN NUA
                canMove = false;

                FadeBlack.GetComponent<Animator>().Play("FadeOut");

                StartCoroutine(AfterFadeOut());
            }
        }
    }

    private bool isFadeOut = false;
    IEnumerator AfterFadeOut()
    {
        yield return new WaitForSeconds(2f);

        ResetPlayer();
        FadeBlack.GetComponent<Animator>().Play("FadeIn");

    }

    private void ResetPlayer()
    {
        // SET UP LAI VI TRI SAU KHI PLAY AGAIN
        transform.position = new Vector3(10.91f, -8.92f, 0f); // TUAN SE SET VI TRI NAY
        //DataPersistence.instance.LoadGame();
        //SceneManager.LoadScene(2);

        // SET UP LAI THONG SO
        SetHealth(maxHealth);
        SetStamina(maxStamina);
        SetStrength(strength);

        // INPUT TRO LAI
        playerInput.enabled = true;

        // SETE UP LAI FADE DE CO THE SU DUNG LAI
        isFadeOut = false;

        // MO DEN
        transform.GetChild(1).gameObject.SetActive(true);

        // DI CHUYEN LAI BINH THUONG
        canMove = true;
    }

    void FlipXByMouse()
    {
        Vector2 direction = playerInput.inputMosue - new Vector2(transform.position.x, transform.position.y);

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
        Vector2 direction = playerInput.inputMosue - new Vector2(transform.position.x, transform.position.y);

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
        float horizontal = playerInput.horizontal;
        float vertical = playerInput.vertical;

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

    // ================= HANDLE COINPAPER ======================
    public int GetCointPaper()
    {
        return coinPaper;
    }

    public void IncreaseCoinPaper()
    {
        coinPaper++;
        LoadCoinPaper();
    }

    public void DecreaseCoinPaper()
    {
        coinPaper--;
        LoadCoinPaper();
    }

    private void LoadCoinPaper()
    {
        //update UI
        Debug.Log(coinPaper);
    }

    // ==========================================

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


    // ======================= SAVE AND LOAD DATA =========================
    public void LoadData(GameData data)
    {
        //player's position
        transform.position = new Vector3(data.Position.X, data.Position.Y, data.Position.Z);

        //Coin Paper
        coinPaper = data.CoinPaper;
    }

    public void SaveData(ref GameData data)
    {
        //player's position
        data.Position = new Position
        {
            X = transform.position.x,
            Y = transform.position.y,
            Z = transform.position.z
        };

        //Coin Paper
        data.CoinPaper = coinPaper;
    }
    
    
}
