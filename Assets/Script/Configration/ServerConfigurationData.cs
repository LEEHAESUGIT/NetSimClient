using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETSIM_Ver2.Assets.Script.Configration
{
	public struct ServerConfigurationData
	{
		public const int MaxConnectedSessionCount = 8;
		public const int MaxPacketSize = 4096;

		public const int SessionMin = 0;
		public const int SessionMax = 8;

		public const int TickRate = 64;
		public const int InterpolationTickUnit = 10;
	}
}
