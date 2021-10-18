[System.Serializable]
public class SettingsData
{
    // текущие деньги
    private int money;
    // индекс волны врагов
    private int waveIndex;
    // кол-во врагов в одной волне
    private int enemiesCount;
    // жизни игрока
    private int lives;
    // кол-во волн в одном уровне
    private int waveInLevel;

    // дефолтный конструктор
    public SettingsData()
    {
        Money = 10;
        WaveIndex = 1;
        EnemiesCount = 3;
        Lives = 10;
        WaveInLevel = 20;
    }

    public SettingsData(int money, int waveIndex, int enemiesCount, int lives, int waveInLevel)
    {
        Money = money;
        WaveIndex = waveIndex;
        EnemiesCount = enemiesCount;
        Lives = lives;
        WaveInLevel = waveInLevel;
    }

    public int Money
    {
        get { return money; }
        set { money = value < 0 ? 0 : value; }
    }

    public int WaveIndex
    {
        get { return waveIndex; }
        set
        {
            if (isMoreThanZero(value))
            {
                waveIndex = value;
            }
        }
    }

    public int WaveInLevel
    {
        get { return waveInLevel; }
        set
        {
            if (isMoreThanZero(value))
            {
                waveInLevel = value;
            }
        }
    }

    public int EnemiesCount
    {
        get { return enemiesCount; }
        set { enemiesCount = value < 0 ? 0 : value; }
    }

    public int Lives
    {
        get { return lives; }
        set { lives = value < 0 ? 0 : value; }
    }

    private bool isMoreThanZero(int value)
    {
        return value > 0;
    }
}
