using UnityEngine;

public class PaperObject : MonoBehaviour
{
    [SerializeField] private GameObject ui;

    private bool check;

    private void Awake()
    {
        ui.SetActive(false);
    }

    private void Update()
    {
        if (check)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerController.instance.IncreaseCoinPaper();
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ui.SetActive(true);
            check = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ui.SetActive(false);
            check = false;
        }
    }
}
