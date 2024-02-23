using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CrazyFarm
{
    public delegate void BabyEventHandler(Baby baby);
    public class Baby : Family, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField, MinMaxSlider(0f, 2f)] private Vector2 minMaxStepDistance;
        [SerializeField, MinMaxSlider(0f, 2f)] private Vector2 minMaxDurationBtwSteps;
        [SerializeField] private float stepDuration = 0.5f;

        private Vector2 startDragPosition;

        public event BabyEventHandler OnDropped; 

        public float RandomStepDistance => Random.Range(minMaxStepDistance.x, minMaxStepDistance.y);
        public float RandomDurationBtwSteps => Random.Range(minMaxDurationBtwSteps.x, minMaxDurationBtwSteps.y);

        private void Start()
        {
            StartStep();
        }

        private void StartStep()
        {
            Vector3 lEndPosition = transform.position +
                Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward) * Vector3.right * RandomStepDistance;
            lEndPosition = Enclosure.Instance.Bounds.ClosestPoint(lEndPosition);

            //KILL WHEN DRAGGED, RESTART WHEN DROPPED
            DOTween.Sequence(this)
                .Append(transform.DOMove(lEndPosition, stepDuration))
                .AppendInterval(RandomDurationBtwSteps)
                .AppendCallback(StartStep);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            startDragPosition = eventData.position;
            DOTween.Kill(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnDropped?.Invoke(this);
        }

        public void CancelDrop()
        {
            //Call this function when it can't be dropped and you have to put it back to its original position 
            if (startDragPosition != null)
            {
                transform.DOMove(startDragPosition, 0.5f).OnComplete(() =>
                {
                   StartStep();
                });
            }
        }

        public void Cry()
        {
            //BABY ANIMAL SOUND
            transform.DOPunchScale(-Vector3.one * 0.2f, 1f);
        }
    }
}
