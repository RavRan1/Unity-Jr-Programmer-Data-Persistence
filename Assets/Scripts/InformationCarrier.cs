using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationCarrier : MonoBehaviour
{
	public string message1;
	public string message2;
	public static GameObject Instance;
	
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
		{
			Destroy(gameObject);
		} else
		{
			Instance = gameObject;
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
