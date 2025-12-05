using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text healthText;
    public TMP_Text scoreText;

    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;
    public Button restartButton;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameOverPanel.SetActive(false);
        UpdateHealth(5);
        UpdateScore(0);
        restartButton.onClick.AddListener(RestartGame);
    }

    public void UpdateHealth(int health)
    {
        healthText.text = $"Health: {health}";
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void ShowGameOver()
    {
        //Hiển thị panel Game Over
        gameOverPanel.SetActive(true);
        finalScoreText.text = $"Final Score: {ScoreManager.Instance.GetScore()}";

        //Mở khóa và hiện lại con trỏ chuột
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
