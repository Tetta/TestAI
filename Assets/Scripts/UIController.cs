using UnityEngine;
using UnityEngine.UI;
//t
public class UIController : MonoBehaviour
{
    public Text scoreText;
    public void SetScore(int v) { if (scoreText) scoreText.text = "Score: " + v; }
}
