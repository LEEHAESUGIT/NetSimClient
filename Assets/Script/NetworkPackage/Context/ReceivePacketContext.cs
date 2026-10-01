
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Context
{
	internal struct ReceivePacketContext
	{
		//internal int SessionID;
		internal IPacket Packet;
	}
}
