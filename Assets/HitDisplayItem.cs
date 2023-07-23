using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HitDisplayItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text;

    private HitDisplayManager _manager;
    
    private Sequence _sequence;

    public void Init(HitDisplayManager manager)
    {
        _manager = manager;
    }
    
    public void Display(Vector2 position, int damage)
    {
        Text.text = damage.ToString();
        transform.position = position;
        gameObject.SetActive(true);
        _sequence?.Kill();
        _sequence = DOTween.Sequence();
        _sequence.Append(Text.transform.DOLocalMoveY(85f, 1f));
        _sequence.Join(Text.DOFade(0f, 1f));
        _sequence.OnComplete(ReturnToPool);
    }

    private void ReturnToPool()
    {
        Text.alpha = 1f;
        Text.transform.localPosition = Vector3.zero;
        _manager.Return(this);
    }
}
