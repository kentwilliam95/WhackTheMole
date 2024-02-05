using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MovingCloudScript : MonoBehaviour
{
    [System.Serializable]
    public struct Data
    {
        public RectTransform _rt;
        public bool toRight;
    }

    [SerializeField] private Data[] _data;
    [SerializeField] private RectTransform _leftPoint;

    [FormerlySerializedAs("_RightPoint")] [SerializeField]
    private RectTransform _rightPoint;

    private Sequence[] _sequences;

    private void OnDestroy()
    {
        for (int i = 0; i < _sequences.Length; i++)
            _sequences[i]?.Kill();
    }

    private void Start()
    {
        _sequences = new Sequence[_data.Length];
        for (int i = 0; i < _data.Length; i++)
        {
            Data d = _data[i];
            _sequences[i] = DOTween.Sequence();
            _sequences[i].SetLoops(-1);
            _sequences[i].SetAutoKill(false);
            
            if (d.toRight)
            {
                _sequences[i].Insert(0f, d._rt.DOAnchorPos(new Vector2(_rightPoint.anchoredPosition.x, d._rt.anchoredPosition.y), 50f + Random.Range(5f, 10f)).SetEase(Ease.Linear));
                _sequences[i].Insert(_sequences[i].Duration(), d._rt.DOAnchorPos(new Vector2(_leftPoint.anchoredPosition.x, d._rt.anchoredPosition.y), 50f + Random.Range(5f, 10f)).SetEase(Ease.Linear));
            }
            else
            {
                _sequences[i].Insert(0f, d._rt.DOAnchorPos(new Vector2(_leftPoint.anchoredPosition.x, d._rt.anchoredPosition.y), 50f+ Random.Range(5f, 10f)).SetEase(Ease.Linear));
                _sequences[i].Insert(_sequences[i].Duration(), d._rt.DOAnchorPos(new Vector2(_rightPoint.anchoredPosition.x, d._rt.anchoredPosition.y), 35f+ Random.Range(5f, 10f)).SetEase(Ease.Linear));
            }
        }
    }
}