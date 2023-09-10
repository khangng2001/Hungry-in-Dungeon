using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy.Boss
{
    public class DradonController : MonoBehaviour
    {
        private EnemyHandle enemyHandle;
        private Animator animator;
        private SpriteRenderer spriteRender;
        private float oldX;
        private RangeDetectAttack rangeDetectAttack;
        private Intro intro;
        private float speed;
    
        [SerializeField] private float timeOfFireRain;

        [SerializeField] private bool fireRain;
        [SerializeField] private GameObject player;
        [SerializeField] private RangeDetect rangeDetect;
        [SerializeField] private RangeHurt rangeHurt;
        [SerializeField] private GameObject RangeFireBall;

        [SerializeField] private GameObject HUDOfBoss;
        [SerializeField] private GameObject healthBar;
        [SerializeField] private TextMeshProUGUI textHealthBar;


        private enum States
        {
            Intro,
            Idle,
            Walk,
            Melee,
            Stomping,
            Hover,
            Launch,
            Fly,
            Landing,
            Firebreath,
            CallFireRain
        }

        [SerializeField] private States changeState;

        private void UpdateState(States newState)
        {
            switch (newState)
            {
                case States.Intro:
                    Intro();
                    break;
                case States.Idle:
                    animator.Play("Idle");
                    break;
                case States.Walk:
                    animator.Play("Walk");
                    MoveToPlayer();
                    break;
                case States.Melee:
                    animator.Play("Attack");
                    break;
                case States.Stomping:
                    animator.Play("Stomping");
                    break;
                case States.Hover:
                    break;
                case States.Launch:
                    break;
                case States.Fly:
                    break;
                case States.Landing:
                    break;
                case States.Firebreath:
                    break;
                case States.CallFireRain:
                    CallFireRain();
                    break;
                default:
                    break;
            }
        }

        private void Awake()
        {
            intro = GetComponent<Intro>();
            enemyHandle = GetComponent<EnemyHandle>();
            animator = GetComponentInChildren<Animator>();
            spriteRender = GetComponentInChildren<SpriteRenderer>();
            rangeDetect = transform.parent.gameObject.GetComponentInChildren<RangeDetect>();
            rangeHurt = GetComponentInChildren<RangeHurt>();
            rangeDetectAttack = GetComponentInChildren<RangeDetectAttack>();
        }

        private void Start()
        {
            changeState = States.Idle;

            oldX = transform.position.x;
            fireRain = true;
            isLaunch = false;
            isFlyDown = false;
            isFlyDownAfter = false;
            isTakeDamage = false;


        }

        private void Update()
        {
            // HANDLE TAKE DAMAGE FROM PLAYER
            HandleTakeDamage();
            //

            UpdateState(changeState);
            FlipX();
        
            // SET UP
            speed = enemyHandle.GetSpeed();
            //

            if (rangeDetect.GetIsDetect())
            {
                // GET PLAYER FROM RANGE DETECT
                player = rangeDetect.GetComponent<RangeDetect>().player;

                if (!intro.GetIsFinish())
                {
                    changeState = States.Intro;
                }
                else
                {
                    if (enemyHandle.GetCurrentHealth() > 0f)
                    {
                        changeState = States.Walk;

                        if (enemyHandle.GetCurrentHealth() < 100f && fireRain)
                        {
                            changeState = States.CallFireRain;
                        }
                        else if (rangeDetectAttack.GetIsDetectAttack())
                        {
                            if (fireRain)
                            {
                                changeState = States.Melee;
                            }
                            else
                            {
                                changeState = States.Stomping;
                            }
                        }
                    }
                    else if (enemyHandle.GetCurrentHealth() <= 0f)
                    {
                        // CHET
                        Destroy(gameObject);
                        //
                    }
                }
            }
            else
            {
                changeState = States.Idle;
            }
        }

        private void FlipX()
        {
            if (oldX < transform.position.x)
            {
                transform.localScale = new Vector2(-1, 1);
            }
            else if (oldX > transform.position.x)
            {
                transform.localScale = new Vector2(1, 1);
            }

            oldX = transform.position.x;
        }

        private void MoveToPlayer()
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }

        private void CallFireRain()
        {
            if (!isLaunch)
            {
                animator.Play("Launch");
                isLaunch = true;
                StartCoroutine(AfterLaunch());
            }
        }

        private bool isLaunch = false;
        IEnumerator AfterLaunch()
        {
            yield return new WaitForSeconds(1f);

            animator.Play("FireRain");

            // CALL FIRE RAIN HERE
            RangeFireBall.SetActive(true);
            //

            if (!isFireRain)
            {
                isFireRain = true;
                StartCoroutine(AfterFireRain());
            }
        }

        private bool isFireRain = false;
        IEnumerator AfterFireRain()
        {
            yield return new WaitForSeconds(timeOfFireRain);

            // STOP FIRE RAIN HERE
            RangeFireBall.SetActive(false);
            //

            animator.Play("Landing");

            if (!isLanding)
            {
                isLanding = true;
                StartCoroutine(AfterLanding());
            }
        }

        private bool isLanding = false;
        IEnumerator AfterLanding()
        {
            yield return new WaitForSeconds(1f);

            fireRain = false;
        }

        private void Intro()
        {
            // KICH HOAT DONG CUA

            //

        

            if (!intro.GetIsSliding())
            {
                if (!isFlyDown)
                {
                    isFlyDown = true;

                    StartCoroutine(FlyDown());
                }
            }
            else
            {
                animator.Play("FireRain");
            }
        }


        private bool isFlyDown = false;
        IEnumerator FlyDown()
        {
            yield return new WaitForSeconds(1f);

            animator.Play("Landing");

            if (!isFlyDownAfter)
            {
                isFlyDownAfter = true;

                StartCoroutine(FlyDownAfter());
            }
        }

        private bool isFlyDownAfter = false;
        IEnumerator FlyDownAfter()
        {
            yield return new WaitForSeconds(1f);

            // LOAD THANH MAU BOSS
            HUDOfBoss.SetActive(true);
            healthBar.GetComponent<Slider>().maxValue = enemyHandle.GetMaxHealth();
            LoadHealth();
            //

            animator.Play("Idle");

            intro.SetIsBacking(true);
        }

        private void LoadHealth()
        {
            textHealthBar.text = enemyHandle.GetCurrentHealth() + "/" + enemyHandle.GetMaxHealth();
            healthBar.GetComponent<Slider>().value = enemyHandle.GetCurrentHealth();
        }

        private void HandleTakeDamage()
        {
            if (rangeHurt.GetIsHurt())
            {
                if (!isTakeDamage)
                {
                    isTakeDamage = true;

                    enemyHandle.TakeDamge(player.GetComponent<PlayerController>().GetStrength());
                    LoadHealth();

                    StartCoroutine(TakeDamage());
                }

            }
        }

        private bool isTakeDamage = false;
        IEnumerator TakeDamage()
        {
            yield return new WaitForSeconds(0.5f);

            isTakeDamage = false;
        }
    }
}
