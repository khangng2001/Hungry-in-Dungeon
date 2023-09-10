using UnityEngine;

public class ChangeToNewScene : MonoBehaviour
{
    [Header("Scene's Name To Go")]
    [SerializeField] private string goToSceneName;

    private Collider2D hit = null;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float radiusCheck = 0;
    private void CheckPlayer()
    {
        hit = Physics2D.OverlapCircle(transform.position, radiusCheck, playerMask);
    }

    private void Update()
    {
        CheckPlayer();
        if (hit != null)
        {
            //Player reaches the door
            GameManager.instance.ChangeScene(goToSceneName);
        }
    }
}
