using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe
{
	internal class ModulePipe
	{
		internal SystemPacketPipe SystemPacketPipe = new SystemPacketPipe();
		internal GamePacketPipe GamePacketPipe = new GamePacketPipe();
	}
}
