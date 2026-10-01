using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe
{
	internal class SystemPacketPipe
	{
		internal InBoundPipe InBoundPipe { get; } = new InBoundPipe();
		internal OutBoundPipe OutBoundPipe { get; } = new OutBoundPipe();
	}
}

