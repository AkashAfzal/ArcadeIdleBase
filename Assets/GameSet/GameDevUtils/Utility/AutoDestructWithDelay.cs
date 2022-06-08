using System.Collections;
using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.Utility
{
	public class AutoDestructWithDelay : MonoBehaviour
	{
		// If true, deactivate the object instead of destroying it
		public bool  onlyDeactivate;
		public float lifeTime;

		void OnEnable()
		{
			StartCoroutine(nameof(DestructAfterLifeTime));
		}

		IEnumerator DestructAfterLifeTime()
		{

			yield return new WaitForSeconds(lifeTime);
			if (onlyDeactivate)
				this.gameObject.SetActive(false);
			else
				Destroy(this.gameObject);
		}

	}
}
