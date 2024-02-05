using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIGameController : MonoBehaviour
{
    [System.Serializable]
    public struct Settings
    {
        public UIMoleSettingSO _data;
        public RectTransform _rt;
    }

    [SerializeField] private UIMole _mole;
    [SerializeField] private TextMeshProUGUI _textTimer;
    [SerializeField] private TextMeshProUGUI _textScore;

    [Header("Game End")] [SerializeField] private CanvasGroup _cgGameEnd;
    [SerializeField] private TextMeshProUGUI _textScoreGameEnd;
    [SerializeField] private TextMeshProUGUI _textTitleGameEnd;
    [SerializeField] private Button _buttonGameEnd;

    [Header("Game Start")] [SerializeField]
    private CanvasGroup _cgGameStart;

    [Header("Setting")] private UIMoleSettingSO _selectedSettingSo;
    private Settings _selectedSetting;

    public UIToggle<UIMoleSettingSO>[] _toggles;
    [FormerlySerializedAs("_Settings")] public Settings[] _settings;

    public System.Action<float> onMoleHit;
    public System.Action<Settings> onGameStart;

    private void Toggle_OnValueChanged(UIMoleSettingSO selectedSetting)
    {
        for (int i = 0; i < _settings.Length; i++)
        {
            if (_settings[i]._data == selectedSetting)
            {
                _selectedSetting = _settings[i];
                break;
            }
        }
    }

    public UIGameController Init(float scoreValue, float timerValue)
    {
        for (int i = 0; i < _toggles.Length; i++)
            _toggles[i].onValueChanged = Toggle_OnValueChanged;

        _cgGameEnd.alpha = 0;
        _cgGameStart.alpha = 1;
        SetCanvasGroupEnable(_cgGameStart, true);

        UpdateTextScore(scoreValue);
        UpdateTextTimer(timerValue);
        return this;
    }

    public void GameStart()
    {
        _cgGameStart.DOFade(0f, 0.25f);
        SetCanvasGroupEnable(_cgGameStart, false);
        onGameStart?.Invoke(_selectedSetting);
    }

    public UIGameController UpdateTextTimer(float value)
    {
        _textTimer.text = value.ToString();
        return this;
    }

    public UIGameController UpdateTextScore(float value)
    {
        _textScore.text = value.ToString();
        _textScore.transform.DOScale(Vector3.one, 0.25f).From(Vector3.one * 1.5f);
        return this;
    }

    public void GameEnd(float score)
    {
        _mole.GameEnd();
        SetCanvasGroupEnable(_cgGameEnd, true);

        _textScoreGameEnd.text = $"Score : {score.ToString()}";

        var seq = DOTween.Sequence();
        seq.Insert(0f, _cgGameEnd.DOFade(1f, 0.5f));
        seq.Insert(0.25f, _textTitleGameEnd.rectTransform.DOScale(1f, 0.25f).From(0));
        seq.Insert(0.375f, _textScoreGameEnd.rectTransform.DOScale(1f, 0.25f).From(0));
        seq.Insert(0.5f, _buttonGameEnd.transform.DOScale(1f, 0.25f).From(0));
    }

    private void SetCanvasGroupEnable(CanvasGroup cg, bool isEnable)
    {
        cg.interactable = isEnable;
        cg.blocksRaycasts = isEnable;
    }

    public void ButtonReset_OnClick()
    {
        SceneManager.LoadScene("Game");
    }
}