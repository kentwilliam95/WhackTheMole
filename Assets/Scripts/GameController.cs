using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        Intro,
        Begin,
        End,
    }

    private const string KEYHIGHSCORE = "Highscore";

    private UIMoleSettingSO _selectedData;
    private float _countdown;
    private float _score;
    public static GameState _state = GameState.Intro;
    
    [Header("Data")] 
    [SerializeField] private UIMoleSettingSO[] _data;
    

    [SerializeField] private AudioClip _audioCorrect;
    [SerializeField] private AudioClip _audioWinGame;

    [Header("UI")] [SerializeField] private UIGameController _uiGameController;

    private void Start()
    {
        _state = GameState.Intro;
        _uiGameController.onGameStart = OnGameStart;
        _uiGameController.Init(0f, 0f);
    }

    private void OnGameStart(UIGameController.Settings _data)
    {
        _state = GameState.Begin;
        _selectedData = _data._data;
        _countdown = _selectedData._gameDuration;
        _uiGameController.onMoleHit = Mole_OnHit;

        var arr = _data._rt.GetComponentsInChildren<UIMole>();
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i].onHit = Mole_OnHit;
            arr[i].Init(_data._data);
        }
        
        _data._rt.gameObject.SetActive(true);
    }

    private void Mole_OnHit(float percentage)
    {
        if (percentage > _selectedData._successHitAccuracy * 0.01f)
        {
            _score += _selectedData._successHitPoint;
            _uiGameController.UpdateTextScore(_score);
            AudioManager.Instance.PlayOneShot(_audioCorrect);
        }
    }

    private void Update()
    {
        if (_state != GameState.Begin)
            return;

        _countdown -= Time.deltaTime;
        _uiGameController.UpdateTextTimer(Mathf.Round(_countdown));
        if (_countdown <= 0)
        {
            GameEnd();
        }
    }

    private void GameEnd()
    {
        AudioManager.Instance.PlayOneShot(_audioWinGame, 1.5f);
        _uiGameController.GameEnd(_score);
        _state = GameState.End;
    }
}