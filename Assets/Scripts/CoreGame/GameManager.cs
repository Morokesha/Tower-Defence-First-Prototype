using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Statics
    public static SettingsData SETTINGS_DATA;

    [Header("GAME SETTINGS")]
    [SerializeField] private int money;
    [SerializeField] private int lives;
    [SerializeField] private int enemiesCount;
    [SerializeField] private int waveIndex;
    [SerializeField] private int waveInLevel;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        SETTINGS_DATA = new SettingsData(money, waveIndex, enemiesCount, lives, waveInLevel);
    }
    #endregion
    #region Ивенты
    public event Action<int> OnMoneyChanged;
    public event Action OnLivesChanged;
    public event Action OnWaveChanged;
    public event Action IsGameOver;
    #endregion
    #region Сериализованные поля и переменные
    [SerializeField] private Transform enemyPf;
    [SerializeField] private List<EnemyTypeSO> enemyList; 

    [Header ("Передвижение врагов")]

    [SerializeField] private Transform startPoint;
    [SerializeField] private List<Transform> waypointsList;
    // Создание врагов
    // текущий счетчик врагов в одной волне
    [SerializeField] private int currentEnemyCount = 0;
    // кол-во средних врагов в одной волне
    private int mediumEnemyCount = 0;
    // кол-во сложных врагов в одной волне
    private int hardEnemyCount = 0;
    private Dictionary<EnemyDifficulty, EnemyTypeSO> enemyDictionary = new Dictionary<EnemyDifficulty, EnemyTypeSO>();
    #endregion
    #region Start and DieOnEnemy
    private void Start()
    {
        //Добавление врагов в словарь
        foreach (var enemy in enemyList)
        {
            enemyDictionary.Add(enemy.enemyDifficulty, enemy);
        }
    }
    private void Enemy_OnEnemyDied(int coins)
    {
        AddMoney(coins);
        currentEnemyCount--;

        if (currentEnemyCount == 0)
        {
            EndWave();
        }
    }
    #endregion
    #region LevelManager
    public void StartGame()
    {
        StartWave();
    }

    private void StopGame()
    {
        StopAllCoroutines();

        IsGameOver?.Invoke();
    }

    private void StartWave()
    {
        StartCoroutine(SpawnEnemyCO());
    }

    private void EndWave()
    {
        if (SETTINGS_DATA.Lives == 0)
        {
            return;
        }
        if (SETTINGS_DATA.WaveIndex < SETTINGS_DATA.WaveInLevel)
        {
            SETTINGS_DATA.WaveIndex++;

            OnWaveChanged?.Invoke();

            mediumEnemyCount = 0;
            hardEnemyCount = 0;
            int nextEnemyCount = SETTINGS_DATA.WaveIndex / 2;
            SETTINGS_DATA.EnemiesCount += nextEnemyCount;

            StartWave();
            return;
        }

        StopGame();
    }
    #endregion
    #region SpawnEnemy
    private IEnumerator SpawnEnemyCO()
    {
        yield return new WaitForSeconds(3f);

        List<EnemyTypeSO> enemyListInWave = MakeEnemyList();
        currentEnemyCount = enemyListInWave.Count;

        foreach (var enemyType in enemyListInWave)
        {
            Enemy enemy = Enemy.Create(enemyType, enemyPf, waypointsList, startPoint.position);
            enemy.OnEnemyDied += Enemy_OnEnemyDied;

            yield return new WaitForSeconds(1f);
        }
    }

    private List<EnemyTypeSO> MakeEnemyList()
    {
        //готовый лист врагов
        List<EnemyTypeSO> resultEnemyList = new List<EnemyTypeSO>();
        //общий счетчик врагов 
        int enemiesCount = SETTINGS_DATA.EnemiesCount;

        for (int i = 1; i <=SETTINGS_DATA.WaveIndex; i++)
        {
            if (i % 3 ==0)
            {
                mediumEnemyCount += 2;
            }

            if (i % 5 == 0)
            {
                hardEnemyCount++;
            }
        }

        // добавляем в общий список врагов для одной волны по их типу
        AddEnemyToList(resultEnemyList, EnemyDifficulty.Hard, hardEnemyCount);
        AddEnemyToList(resultEnemyList, EnemyDifficulty.Medium, mediumEnemyCount);
        AddEnemyToList(resultEnemyList, EnemyDifficulty.Easy, enemiesCount - (mediumEnemyCount + hardEnemyCount));

        return resultEnemyList;
    }

    private void AddEnemyToList(List<EnemyTypeSO> list, EnemyDifficulty enemyDifficulty, int count)
    {
        while (count > 0)
        {
            EnemyTypeSO enemy;
            enemyDictionary.TryGetValue(enemyDifficulty, out enemy);

            if (enemy != null)
            {
                list.Add(enemy);
            }

            count--;
        }
    }

    #endregion  
    #region Ивенты добавления/отнимания
    public void HitByEnemy()
    {
        SETTINGS_DATA.Lives--;
        OnLivesChanged?.Invoke();

        if (SETTINGS_DATA.Lives == 0)
        {
            StopGame();
        }
    }

    public void TakeMoney(int amount)
    {
        // currentMoney служит для отрисовки юай
        int currentMoney = Mathf.Clamp(SETTINGS_DATA.Money - amount, 0, 9999);

        // обновляем текущие деньги
        SETTINGS_DATA.Money -= amount;

        OnMoneyChanged?.Invoke(currentMoney);
    }

    public void AddMoney(int amount)
    {
        // currentMoney служит для отрисовки юай
        int currentMoney = Mathf.Clamp(SETTINGS_DATA.Money + amount, 0, 9999);

        // обновляем текущие деньги
        SETTINGS_DATA.Money += amount;

        OnMoneyChanged?.Invoke(currentMoney);
    }
    #endregion
}
