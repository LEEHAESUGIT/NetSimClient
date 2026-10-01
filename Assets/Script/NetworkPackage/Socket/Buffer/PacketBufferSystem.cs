
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Socket.Buffer
{
	internal class PacketBufferSystem
	{
		internal ReceiveBuffer Recv;

		internal PacketBufferSystem(int bufferSize)
		{
			this.Recv = new ReceiveBuffer(bufferSize);
		}


	}
}
