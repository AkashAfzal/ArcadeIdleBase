using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.Utility
{


	[RequireComponent(typeof(Renderer))]
	public class ChangeMaterial : MonoBehaviour
	{

		public                Color color;
		[Range(-1, 3)] public int   renderingMode = 0;
		Renderer                    ObjectRenderer;

		void OnEnable()
		{
			SetMeshMaterialColorProperty();
		}

		void OnValidate()
		{
			SetMeshMaterialColorProperty();
		}


		void SetMeshMaterialColorProperty(int materialIndex = 0)
		{
			MaterialPropertyBlock prop = new MaterialPropertyBlock();
			ObjectRenderer = transform.GetComponent<Renderer>();
			if (renderingMode != -1)
			{
				prop.SetFloat("_Mode", renderingMode);
				color.a = 1;
			}

			prop.SetColor("_BaseColor",     color);
			prop.SetColor("_EmissionColor", color);
			ObjectRenderer.SetPropertyBlock(prop, materialIndex);
		}

	}


}