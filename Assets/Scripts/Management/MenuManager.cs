using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
	using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
	[SerializeField] private Text nameText;
	
	private InformationCarrier infoCarrier;
	private string playerName;
	private bool iCReady;
	
    // Start is called before the first frame update
    void Start()
    {
		playerName = "";
		iCReady = false;
    }

    // Update is called once per frame
    void Update()
    {
		if (!iCReady)
		{
			if (InformationCarrier.Instance != null )
			{
				infoCarrier = InformationCarrier.Instance.GetComponent<InformationCarrier>();
				DontDestroyOnLoad(infoCarrier.gameObject);
				iCReady = true;
				SaveGame();
			} else
			{
				Debug.Log("Information carrier not found.");
				ExitGame();
			}
		}
    }
	
	public void StartGame()
	{
		if (nameText != null)
		{
			playerName = nameText.text;
			infoCarrier.message1 = playerName;
			infoCarrier.message2 = GetBestScore();
			SceneManager.LoadScene(1);
		}
	}
	
	public void OpenControls()
	{
		infoCarrier.message1 = "Controls";
		infoCarrier.message2 = "";
		SceneManager.LoadScene(2);
	}
	
	public void ExitGame()
	{
		#if UNITY_EDITOR
			EditorApplication.ExitPlaymode();
		#else
			Application.Quit();
		#endif
	}
	
	private void SaveGame()
	{
		if (infoCarrier.message2 != "")
		{
			string bestScorePlayer = infoCarrier.message2;
			File.WriteAllText(Application.persistentDataPath + "/bestScore.json", bestScorePlayer);
		}
	}
	
	private string GetBestScore()
	{
		string path = Application.persistentDataPath + "/bestScore.json";
		if (File.Exists(path))
		{
			return File.ReadAllText(path);
		}
		
		return "";
	}
}
