namespace GameAssets.GameSet.GameDevUtils.StateMachine
{


	public class Transition : ITransition
	{

		public string toState   { get; }
		public bool   condition { get; set; }
		public float  exitTime  { get; }

		public Transition(string to, float exitTime = 0)
		{
			this.toState  = to;
			this.exitTime = exitTime;
		}

	}


}