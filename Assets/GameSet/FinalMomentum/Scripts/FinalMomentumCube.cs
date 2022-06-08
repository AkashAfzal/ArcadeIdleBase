using TMPro;
using UnityEngine;


namespace GameAssets.GameSet.FinalMomentum.Scripts
{


	public class FinalMomentumCube : MonoBehaviour
	{

		[SerializeField] GameObject[] boxes;

		private Color       ColorToApply;
		bool                IsTriggered;
		static readonly int kColor = Shader.PropertyToID("_BaseColor");

		public void SetColor(Color colorToApply)
		{
			ColorToApply = colorToApply;
		}

		public void SetText(double numberX, string text)
		{
			transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = $"{numberX}{text}";
			transform.gameObject.name                                         = $"Cube {numberX}";
		}

		public void ShowColor()
		{
			SetMeshMaterialColorProperty();
		}


		private void SetMeshMaterialColorProperty()
		{
			MaterialPropertyBlock prop           = new MaterialPropertyBlock();
			var                   myMeshRenderer = transform.GetComponent<Renderer>();
			prop.SetColor(kColor, ColorToApply);
			myMeshRenderer.SetPropertyBlock(prop);
		}


		void OnTriggerEnter(Collider other)
		{
			if (IsTriggered) return;
			IsTriggered = true;
//			SoundManager.Instance.PlayOneShot(SoundManager.Instance.multiplierClip,1);
		}

	}

}