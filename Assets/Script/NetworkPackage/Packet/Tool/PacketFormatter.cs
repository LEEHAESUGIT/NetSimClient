using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Interface;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForIngame;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForSystem;
using System;
using System.Buffers.Binary;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.Tool
{

	internal class PacketFormatter
	{
		private FrozenDictionary<EPacketID, Func<PacketReader, IPacket>> _parseMap;
		internal PacketFormatter() 
		{
			_parseMap = InitParseMap();
		}


		// 송신하기 위한 데이터 패킷을 byte배열형 으로 변환
		internal ReadOnlyMemory<byte> Format(IPacket packet)
		{
			PacketWriter _packetWriter = new PacketWriter();
			_packetWriter.Clear();

			_packetWriter.Write((ushort)0); // size 자료형 ushort(2byte)
			_packetWriter.Write((int)0);    // packetID 자료형 int(4byte)

			packet.Serialize(_packetWriter);

			ushort totalSize = (ushort)_packetWriter.GetMutableSpan().Length;
			Span<byte> headerSpan = _packetWriter.GetMutableSpan().Slice(0, 6);

			BinaryPrimitives.WriteUInt16LittleEndian(headerSpan.Slice(0, 2), totalSize);
			BinaryPrimitives.WriteInt32LittleEndian(headerSpan.Slice(2, 4), (int)packet.packetID);

			return _packetWriter.writterMemory;

		}

		// 수신한 바이트배열 형식의 패킷을 패킷형태로 변환
		//internal bool TryParse(ReadOnlySpan<byte> packetData, out IPacket packet)
		//{
		//	PacketReader _packetReader = new PacketReader();
		//	packet = null;
		//	try
		//	{
		//		//PacketReader reader = new(packetData);
		//		_packetReader.Clear();

		//		_packetReader.Init(packetData);

		//		PacketHeader header = PacketHeader.Deserialize(_packetReader);

		//		switch (header.packetID)
		//		{
		//			case EPacketID.S_HeartBeat:
		//				packet = S2CHeartBeat.Deserialize(_packetReader);
		//				return true;
		//			case EPacketID.S_Init:
		//				packet = S2C_InitResponse.Deserialize(_packetReader);
		//				return true;

		//			default:
		//				return false;
		//		}
		//	}
		//	catch (Exception)
		//	{
		//		//Console.WriteLine(ex.Message);
		//		return false;
		//	}
		//}

		internal bool TryParse(ReadOnlySpan<byte> packetData, out IPacket packet)
		{
			PacketReader _packetReader = new PacketReader();
			packet = null;
			try
			{
				_packetReader.Clear();
				_packetReader.Init(packetData);
				PacketHeader header = PacketHeader.Deserialize(_packetReader);

				if(_parseMap.TryGetValue(header.packetID , out Func<PacketReader , IPacket> func ))
				{
					packet = func.Invoke(_packetReader);
					return true;
				}
				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}
		private FrozenDictionary<EPacketID , Func<PacketReader ,IPacket>> InitParseMap()
		{
			var tempInitParseMap = new Dictionary<EPacketID, Func<PacketReader, IPacket>>();
			// TODO: [Perf Optimization]
			// 현재 Func<PacketReader, IPacket> 반환 시 struct -> interface 캐스팅으로 인한 Boxing(GC Alloc) 발생 중.
			// 추후 Action<Session, PacketReader> 기반의 In-place 파싱 및 핸들러 직접 호출 구조로 전환하여 제로 알로케이션 달성할 것.
			tempInitParseMap.Add(EPacketID.S_Spawn , (_packetReader) => S2C_SpawnResponse.Deserialize(_packetReader));
			

			tempInitParseMap.Add(EPacketID.S_HeartBeat , (_packetReader) => S2CHeartBeat.Deserialize(_packetReader));
			tempInitParseMap.Add(EPacketID.S_Init , (_packetReader) => S2C_InitResponse.Deserialize(_packetReader));
			


			return tempInitParseMap.ToFrozenDictionary();
		}


	}
}