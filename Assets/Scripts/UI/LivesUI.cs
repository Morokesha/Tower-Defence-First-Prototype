using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    [SerializeField] private Transform livesPf;
    private Transform[] livesContainer;

    private void Start()
    {
        int initialiizeLives = GameManager.SETTINGS_DATA.Lives;
        livesContainer = new Transform[initialiizeLives];
        GameManager.Instance.OnLivesChanged += Instance_OnLivesChanged;

        for (int i = 0; i < initialiizeLives; i++)
        {
            Transform livesTransform = Instantiate(livesPf, transform);
            livesTransform.gameObject.SetActive(true);

            float offset = 50 * i;
            livesTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(250f + offset + 35f,-26f);

            livesContainer[i] = livesTransform;

        }
    }

    private void Instance_OnLivesChanged()
    {
        int currentLives = GameManager.SETTINGS_DATA.Lives;
        UpdateUILives(currentLives);
    }

    private void UpdateUILives(int lives)
    {
        livesContainer[lives].gameObject.SetActive(false);
    }
}
