using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
	using UnityEditor;
#endif

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
	[SerializeField] private Text nameDisplay;
	[SerializeField] private Text bestScoreDisplay;
	
    private bool m_Started = false;
    private int m_Points;
    private bool m_GameOver = false;
	private string playerName;
	[SerializeField] private int newScore = 0;
	private InformationCarrier infoCarrier;
    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
		
		if (InformationCarrier.Instance != null)
		{
			infoCarrier = InformationCarrier.Instance.GetComponent<InformationCarrier>();
			playerName = infoCarrier.message1;
			nameDisplay.text = "Name: " + playerName;
			
			if (infoCarrier.message2 == "")
			{
				bestScoreDisplay.text = "No Scores Saved.";
			} else
			{
				bestScoreDisplay.text = "Best Score: " + infoCarrier.message2;
			}
		} else
		{
			Debug.Log("Information carrier not found.");
			ExitGame();
		}
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
				SaveScore();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score: {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
	
	private void SaveScore()
	{
		int bestScore = GetBestScore();
		newScore = Max(newScore, m_Points);
		
		if (newScore > bestScore) 
		{
			infoCarrier.message2 =  playerName + " - " + newScore.ToString();
		}
	}
	
	public void ReturnToMenu()
	{
		SaveScore();
		SceneManager.LoadScene(0);
	}
	
	private void ExitGame()
	{
		#if UNITY_EDITOR
			EditorApplication.ExitPlaymode();
		#else
			Application.Quit();
		#endif
	}
	
	private int Max(int a, int b)
	{
		if (a >= b)
		{
			return a;
		}
		
		return b;
	}
	
	private int GetBestScore()
	{
		if (infoCarrier.message2 != "")
		{
			string[] snappedString = infoCarrier.message2.Split(' ');
			return int.Parse(snappedString[snappedString.Length - 1]);
		} else {
			return 0;
		}
	}
}
