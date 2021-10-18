using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControlUI : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button repeatBtn;
    [SerializeField] private Transform overlay;

    private void Start()
    {
        GameManager.Instance.IsGameOver += Instance_IsGameOver;

        repeatBtn.onClick.AddListener(() => 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        });

        playBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.StartGame();
            playBtn.gameObject.SetActive(false);
            overlay.gameObject.SetActive(false);
        });

        repeatBtn.gameObject.SetActive(false);
    }

    private void Instance_IsGameOver()
    {
        repeatBtn.gameObject.SetActive(true);
        overlay.gameObject.SetActive(true);
    }
}
