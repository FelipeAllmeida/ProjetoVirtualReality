using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartState : MonoBehaviour 
{
    [SerializeField] private float _startTime = 5f;
    private float _timeCounter = 0;

    void Update()
    {
        if (_timeCounter > _startTime)
        {
            SceneManager.LoadScene("Game");
        }
        _timeCounter += Time.deltaTime;
    }

}
