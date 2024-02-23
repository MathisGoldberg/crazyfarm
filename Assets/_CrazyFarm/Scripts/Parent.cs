using CrazyFarm;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

namespace SheepFold
{
    public class Parent : Family, IPointerClickHandler
    {
        [SerializeField] private int minBabies;
        [SerializeField] private int maxBabies;

		private List<Baby> currentBabies;
		private int requiredBabies;

		private void Start()
		{
			currentBabies = new List<Baby>();
			requiredBabies = Random.Range(minBabies, maxBabies);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			/*if (collision.gameObject.TryGetComponent<Baby>(out Baby lBaby))
				lBaby.OnDropped += Dropped_CheckRightBaby;*/
			if (collision.gameObject.TryGetComponent<Baby>(out Baby lBaby))
				Dropped_CheckRightBaby(lBaby);
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.gameObject.TryGetComponent<Baby>(out Baby lBaby))
				lBaby.OnDropped -= Dropped_CheckRightBaby;
		}

		private void Dropped_CheckRightBaby(Baby baby)
		{
			if (baby.type == type)
			{
				Debug.Log("It's the right baby");
				currentBabies.Add(baby);

				if (currentBabies.Count == requiredBabies)
					Despawn();
			}
			else
			{
				Debug.Log("It's the wrong baby");
				RejectBaby(baby);
			}
		}

		private void RejectBaby(Baby baby)
		{
			baby.gameObject.transform.position = Enclosure.Instance.Bounds.RandomPoint();
		}

		private void Despawn()
		{
			for (int i = currentBabies.Count - 1; i >= 0; i--)
				Destroy(currentBabies[i].gameObject);

			Destroy(this);
		}

        public void OnPointerClick(PointerEventData eventData)
        {
			//PARENT ANIMAL SOUND
			transform.DOPunchScale(-Vector3.one * 0.2f, 1f)
				.OnComplete(AllChildCry);
        }

		private void AllChildCry()
		{
			foreach (Baby lBaby in FindObjectsOfType<Baby>())
			{
				if (lBaby.type == type)
					lBaby.Cry();
			}
        }
    }
}
