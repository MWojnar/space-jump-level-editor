using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace spaceJumpLevelEditor
{
	public class Settings
	{
		public float PlayerGravity { get; set; }
		public int PlayerMaxJumpHoldFrames { get; set; }
		public float PlayerJumpSpeed { get; set; }
		public float PlayerHorSpeed { get; set; }
		public float PlayerHorSpeedNoBoard { get; set; }
		public int PlayerInvincibilityFrames { get; set; }
		public int PlayerStunFrames { get; set; }
		public Phase[] Phases { get; set; }
		public float FinalPhaseIntensityFactor { get; set; }
		public float CandyInitVelocity { get; set; }
		public float CandyTermVelocity { get; set; }
		public float CandyGravity { get; set; }
		public float TPSpeed { get; set; }
		public float BoardSpeed { get; set; }
		public int BoardHangTime { get; set; }
		public float BoardGravity { get; set; }
	}
}
