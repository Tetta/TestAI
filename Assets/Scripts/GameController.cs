using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public UIController ui;
    public Transform player;
    public GameObject coinPrefab;
    int score;
    float t, interval = 1.2f;
    Vector2 range = new Vector2(7,4);

    void Update()
    {
        t += Time.deltaTime;
        if (t >= interval) { t = 0f; Spawn(); }
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene("MainMenu");
    }

    void Spawn()
    {
        if (!coinPrefab) return;
        Vector3 p = new Vector3(Random.Range(-range.x, range.x), Random.Range(-range.y, range.y), 0);
        Instantiate(coinPrefab, p, Quaternion.identity);
    }

    public void Add(int v) { score += v; if (ui) ui.SetScore(score); }
}
