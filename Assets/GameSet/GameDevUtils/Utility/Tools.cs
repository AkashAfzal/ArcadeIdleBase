using System.Collections.Generic;
using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.Utility
{


	public class Tools : MonoBehaviour
	{

		/// <summary>
		/// Set Material Block Property Color for Renderer with rendering mode 
		/// </summary>
		/// <param name="color"></param>
		/// <param name="renderer"></param>
		/// <param name="renderingMode"></param>
		/// <param name="materialIndex"></param>
		public static void SetMeshMaterialColorProperty(Color color, Renderer renderer, int renderingMode = -1, int materialIndex = 0)
		{
			MaterialPropertyBlock prop = new MaterialPropertyBlock();
			if (renderingMode != -1)
			{
				prop.SetFloat("_Mode", renderingMode);
				color.a = 1;
			}

			prop.SetColor("_BaseColor", color);
			renderer.SetPropertyBlock(prop, materialIndex);
		}

		/// <summary>
		/// Set Material Block Property Color for Skinned Mesh Renderer 
		/// </summary>
		/// <param name="color"></param>
		/// <param name="renderer"></param>
		public static void SetMeshMaterialColorProperty(Color color, SkinnedMeshRenderer renderer)
		{
			MaterialPropertyBlock prop = new MaterialPropertyBlock();
			prop.SetColor("_Color", color);
			renderer.SetPropertyBlock(prop);
		}

		/// <summary>
		/// Return position From Screen to World Space
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
		{
			return camera.ScreenToWorldPoint(position);
		}

		/// <summary>
		/// Cast ray from screen point to world and return hit point
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static Vector3 ScreenPointToRayOutPoint(Camera camera, Vector3 position)
		{
			Ray ray = camera.ScreenPointToRay(position);
			return Physics.Raycast(ray, out RaycastHit hit) ? hit.point : Vector3.zero;
		}

		/// <summary>
		/// Cast ray from screen point to world and return hit game object
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static GameObject ScreenPointToRayOutGameObject(Camera camera, Vector3 position)
		{
			Ray ray = camera.ScreenPointToRay(position);
			return Physics.Raycast(ray, out RaycastHit hit) ? hit.transform.gameObject : null;
		}


		/// <summary>
		/// Cast ray from screen point to world with max distance and return hit game object
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="position"></param>
		/// <param name="maxDistance"></param>
		/// <returns></returns>
		public static GameObject ScreenPointToRayOutGameObject(Camera camera, Vector3 position, float maxDistance)
		{
			Ray ray = camera.ScreenPointToRay(position);
			return Physics.Raycast(ray, out RaycastHit hit) ? hit.transform.gameObject : null;
		}

		/// <summary>
		/// Cast ray for specific layer from screen point to world of infinity distance and return hit game object
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="position"></param>
		/// <param name="layerMask"></param>
		/// <returns></returns>
		public static GameObject ScreenPointToRayOutGameObject(Camera camera, Vector3 position, LayerMask layerMask)
		{
			Ray ray = camera.ScreenPointToRay(position);
			return Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask) ? hit.transform.gameObject : null;
		}


		/// <summary>
		/// Return Rotation Towards Direction Position form this point to Other for 2D view
		/// </summary>
		/// <param name="form"></param>
		/// <param name="to"></param>
		/// <param name="xRotation"></param>
		/// <param name="zRotation"></param>
		/// <returns></returns>
		public static Quaternion RotationYToDirection(Vector3 form, Vector3 to, float xRotation = 0, float zRotation = 0)
		{
			Vector3 direction = (to - form).normalized;
			float   angle     = Mathf.Atan2(-direction.z, direction.x) * Mathf.Rad2Deg;
			return Quaternion.Euler(xRotation, angle, zRotation);
		}

		/// <summary>
		/// Return Random Position Inside Sphere around center by Y position is 0
		/// </summary>
		/// <param name="center"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		public static Vector3 GetRandomPositionInsideUnitSphere(Vector3 center, float radius)
		{
			Vector3 randomPoint = new Vector3(Random.insideUnitSphere.x * radius, 0, Random.insideUnitSphere.z * radius);
			;
			return (center + randomPoint);
		}


		/// <summary>
		/// Return Random Point On Circle around y axis and rotation towards center or opposite center
		/// You can Access this method like that
		/// Vector3    pos ;
		/// Quaternion rot;
		/// (pos, rot) = Tools.RandomCircle(transform.position, radius, false);
		/// </summary>
		/// <param name="center"></param>
		/// <param name="radius"></param>
		/// <param name="rotationToCenter"></param>
		/// <returns></returns>
		public static (Vector3, Quaternion) RandomCircle(Vector3 center, float radius, bool rotationToCenter = false)
		{
			float   ang = Random.value * 360;
			Vector3 pos;
			pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
			pos.y = center.y;
			pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
			Quaternion rot = rotationToCenter ? Quaternion.FromToRotation(Vector3.forward, center - pos) : Quaternion.FromToRotation(Vector3.forward, pos - center);
			return (pos, rot);
		}

		/// <summary>
		/// Return position from world space to UI Space
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="parentCanvas"></param>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public static Vector3 WorldToUISpace(Camera camera, Canvas parentCanvas, Vector3 worldPos)
		{
			//Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
			Vector3 screenPos = camera.WorldToScreenPoint(worldPos);

			//Convert the screen point to ui rectangle local point
			RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out Vector2 movePos);
			//Convert the local point to world point
			return parentCanvas.transform.TransformPoint(movePos);
		}


		/// <summary>
		/// Get Point in Circle for Angle 360
		/// </summary>
		/// <param name="center"></param>
		/// <param name="num"></param>
		/// <param name="angle"></param>
		/// <returns></returns>
		public Vector3 GetPointOnCircle(Vector3 center, int num, float angle)
		{
			float   newAngle = angle                                                    * num;
			Vector3 offset   = new Vector3(Mathf.Sin(newAngle), 0, Mathf.Cos(newAngle)) * 1.5f;
			return (center + offset);
		}


		/// <summary>
		/// Divide Circle in Equal number of parts and Return Lists of Positions And Rotations Towards positions 
		/// </summary>
		/// <param name="numOfParts"></param>
		/// <param name="center"></param>
		/// <param name="radius"></param>
		/// <param name="rotationToCenter"></param>
		/// <returns></returns>
		public static (List<Vector3>, List<Quaternion>) CreateCirclePointsAroundCenter(int numOfParts, Vector3 center, float radius, bool rotationToCenter = false)
		{
			var positions = new List<Vector3>();
			var rotations = new List<Quaternion>();
			for (int i = 0; i < numOfParts; i++)
			{
				float      radians    = 2 * Mathf.PI / numOfParts * i;
				float      vertcial   = Mathf.Sin(radians);
				float      horizontal = Mathf.Cos(radians);
				Vector3    spawnDir   = new Vector3(horizontal, 0, vertcial);
				Vector3    spawnPos   = center + spawnDir * radius;
				Quaternion rot        = rotationToCenter ? Quaternion.FromToRotation(Vector3.forward, center - spawnPos) : Quaternion.FromToRotation(Vector3.forward, spawnPos - center);
				rotations.Add(rot);
				positions.Add(spawnPos);
			}

			return (positions, rotations);
		}


		/// <summary>
		/// Return Point in front of object for given distance
		/// </summary>
		/// <param name="thisTransform"></param>
		/// <param name="distance"></param>
		/// <returns></returns>
		public static Vector3 SomePositionFrontOfTransform(Transform thisTransform, float distance)
		{
			// local coordinate rotation around the Y axis to the given angle
			Quaternion direction = Quaternion.AngleAxis(thisTransform.rotation.y, Vector3.up);
			// add the desired distance to the direction
			Vector3 addDistanceToDirection = direction * thisTransform.forward * distance;
			return thisTransform.position + addDistanceToDirection;
		}


		/// <summary>
		/// Return Point within radius around center
		/// </summary>
		/// <param name="radius"></param>
		/// <param name="centerPosition"></param>
		/// <param name="newLocation"></param>
		/// <returns></returns>
		public static Vector3 PositionWithinRadius(float radius, Vector3 centerPosition, Vector3 newLocation)
		{
			float distance = Vector3.Distance(newLocation, centerPosition); //distance from ~green object~ to *black circle*
			if (distance > radius)                                          //If the distance is less than the radius, it is already within the circle.
			{
				Vector3 fromOriginToObject = newLocation - centerPosition; //~GreenPosition~ - *BlackCenter*
				fromOriginToObject *= radius / distance;                   //Multiply by radius //Divide by Distance
				newLocation        =  centerPosition + fromOriginToObject; //*BlackCenter* + all that Math
			}

			return newLocation;
		}


		/// <summary>
		/// Check Target position Within Radius
		/// </summary>
		/// <param name="radius"></param>
		/// <param name="centerPosition"></param>
		/// <param name="newLocation"></param>
		/// <returns></returns>
		public static bool IsTargetPositionWithinRadius(float radius, Vector3 centerPosition, Vector3 newLocation)
		{
			float distance = Vector3.Distance(newLocation, centerPosition);
			return !(distance > radius);
		}

		/// <summary>
		/// Change Start Color Of Particle
		/// </summary>
		/// <param name="ps"></param>
		/// <param name="color"></param>
		public static void ChangeParticleStartColor(ParticleSystem ps, Color color)
		{
			ParticleSystem.MainModule main = ps.main;
			main.startColor = color;
		}

		/// <summary>
		/// Return a vector2 in canvas space from world space position
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="worldPoint"></param>
		/// <param name="canvasRect"></param>
		/// <returns></returns>
		public static Vector2 WordPointToCanvasPoint(Camera camera, Vector3 worldPoint, RectTransform canvasRect)
		{
			Vector2 viewportPosition = camera.WorldToViewportPoint(worldPoint);
			Vector2 screenPosition   = new Vector2(((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)), ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
			return screenPosition;
		}

	}


}