using Coffee.UIExtensions;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;

public class UIMole : MonoBehaviour, IPointerClickHandler
{
    private float _countdownBeforeShowUp;
    private Tween _twShowUpDelay;
    private Tween _twHiding;
    private bool _isShowing;
    
    private Sequence _seq;
    private Sequence _seqWhack;
    
    [SerializeField] private RectTransform _rt;
    
    private float _min = 400f;
    private float _max = 600f;

    [SerializeField] private UIMoleSettingSO _data;
    private float _delayBeforeShowUp;
    private float _delayAfterShowUp;
    private float _showUpDuration;

    [SerializeField] private TextMeshProUGUI _imgWhack;
    [SerializeField] private AudioClip _audioHit;
    [SerializeField] private AudioClip _audioImpact;
    [SerializeField] private UIParticle _uiParticle;
    
    public System.Action<float> onHit;
    
    public void Init(UIMoleSettingSO data)
    {
        _data = data;
        _delayBeforeShowUp = _data._delayBeforeShowUp.Get();
        _delayAfterShowUp = _data._delayAfterShowUp.Get();
        _showUpDuration = _data._showUpDuration.Get();
        _countdownBeforeShowUp = _data._delayBeforeShowUp.Get();
        
        _seq = DOTween.Sequence();
        _seq.SetAutoKill(false);

        _seq.Insert(0f, _rt.DOAnchorPosY(_max, _showUpDuration).From(new Vector2(0, _min)).SetEase(Ease.OutBack));
        _seq.Pause();

        _seqWhack = DOTween.Sequence();
        _seqWhack.SetAutoKill(false);
        _seqWhack.Insert(0f, _imgWhack.DOFade(1f, 0.25f));
        _seqWhack.Insert(0f, _imgWhack.rectTransform.DOScale(1f, 0.5f).From(1.5f));
        _seqWhack.Insert(0.65f, _imgWhack.DOFade(0f, 0.25f));
        _seqWhack.Insert(0.65f, _imgWhack.rectTransform.DOScale(0f, 0.25f));
        _seqWhack.Complete();
    }

    private void Update()
    {
        if(GameController._state == GameController.GameState.End)
            return;
        
        _countdownBeforeShowUp -= Time.deltaTime;
        if(_countdownBeforeShowUp > 0)
            return;

        if (!_isShowing)
        {
            _seq.Restart();
            _isShowing = true;

            _twShowUpDelay = DOVirtual.DelayedCall(_delayAfterShowUp, null).OnComplete(AfterDelayShowUp);
        }
    }

    private void AfterDelayShowUp()
    {
        _seq.SmoothRewind();
        _twHiding = DOVirtual.DelayedCall(_seq.Duration(), () =>
        {
            _isShowing = false;
            _countdownBeforeShowUp = _delayBeforeShowUp;
        });
    }

    private void Hit()
    {
        float percentage = Mathf.InverseLerp(_min, _max, _rt.anchoredPosition.y);
        _seq?.Rewind();
        _seq?.Pause();
        
        KillAllTween();
        
        _uiParticle.RefreshParticles();
        _uiParticle.Play();

        _imgWhack.rectTransform.position = _rt.position;
        _imgWhack.rectTransform.localRotation = quaternion.Euler(0f, 0f, Random.Range(0, 65f));
        _seqWhack.Restart();
        
        AudioManager.Instance.PlayOneShot(_audioHit);
        AudioManager.Instance.PlayOneShot(_audioImpact);

        _delayBeforeShowUp = _data._delayBeforeShowUp.Get();
        _delayAfterShowUp = _data._delayAfterShowUp.Get();
        _showUpDuration = _data._showUpDuration.Get();
        _countdownBeforeShowUp = _data._delayBeforeShowUp.Get();
        
        _seq = DOTween.Sequence();
        _seq.SetAutoKill(false);

        _seq.Insert(0f, _rt.DOAnchorPosY(_max, _showUpDuration).From(new Vector2(0, _min)).SetEase(Ease.OutBack));
        _seq.Pause();
        
        _isShowing = false;
        onHit?.Invoke(percentage);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Hit();
    }

    public void GameEnd()
    {
        KillAllTween();
    }

    private void KillAllTween()
    {
        _twShowUpDelay?.Kill();
        _twHiding?.Kill();
        _seq?.Kill();
    }
}
