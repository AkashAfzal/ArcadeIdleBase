using System;
using UnityEngine;


namespace GameDevUtils.StackSystem.UpgradeSystem
{


	[Serializable]
	public class Upgrades
	{

		public int upgradeCapacity;
		public int upgradePrice;

	}

	[CreateAssetMenu(fileName = "UpgradesData", menuName = "GameDevUtils/StackSystem/UpgradesData")]
	public class UpgradesData : ScriptableObject
	{

		public Upgrades[] upgrades;

	}


}