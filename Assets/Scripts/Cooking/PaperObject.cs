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
                Debug.Log("Receive 1 paper");   //player.instance.addPaper()
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
        else
        {
            ui.SetActive(false);
            check = false;
        }
    }
}
