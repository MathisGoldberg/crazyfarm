using UnityEngine;

namespace SheepFold
{
    public class InactiveFeedback : MonoBehaviour
    {
        [SerializeField] private float inactiveDurationForShow = 8f;
        [SerializeField] private GameObject animObject;

        private float durationCounter = 0f;

        private void Update()
        {
            if (Input.GetMouseButton(0))
                durationCounter = 0f;
            else
                durationCounter += Time.deltaTime;

            if (animObject != null)
                animObject.SetActive(durationCounter >= inactiveDurationForShow);
        }
    }
}
