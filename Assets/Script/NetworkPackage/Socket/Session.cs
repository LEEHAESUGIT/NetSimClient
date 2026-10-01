using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Interface;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForSystem;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.Tool;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Socket.Buffer;
using System;
using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Socket
{
	public class Session
	{
		// Local
		private TcpClient _client;
		private NetworkStream _stream;
		private PacketFormatter _formatter;
		private PacketBufferSystem _packetBuffer;

		private int _disconnectedLock = 0; // 1 일때 lock
		internal int SessionID { get; private set; }
		internal DateTime LastHeartBeatTime { get; set; }
		internal long LastHeartBeatTick { get; set; }

		// Token
		private CancellationTokenSource _linkedCts;
		private CancellationTokenSource _sessionCts;
		private CancellationToken _loopToken;

		// Pipe
		private ModulePipe _modulePipe;


		internal Session()
		{
			this._formatter = new PacketFormatter();
			this._packetBuffer = new PacketBufferSystem(4096);
		}
		#region Local

		internal void InitSessionID(int id)
		{
			this.SessionID = id;
		}
		#endregion



		internal void Init(ModulePipe modulePipe, CancellationToken clientToken)
		{
			_disconnectedLock = 0;
			this._sessionCts = new CancellationTokenSource();
			this._linkedCts = CancellationTokenSource.CreateLinkedTokenSource(clientToken, _sessionCts.Token);
			this._loopToken = _linkedCts.Token;

			this._modulePipe = modulePipe;
		}
		internal async Task Start(int port)
		{
			while (!_sessionCts.IsCancellationRequested)
			{
				try
				{
					this._client = new TcpClient();
					await _client.ConnectAsync(IPAddress.Loopback, port);
					this._stream = _client.GetStream();
					break;
				}
				catch (Exception)
				{
					_client?.Close();
					_client = null;
					try
					{
						await Task.Delay(1000, _sessionCts.Token);
					}
					catch (OperationCanceledException)
					{
						return;
					}
				}

			}
			if (_stream != null && !_sessionCts.IsCancellationRequested)
			{

				_ = ReceivedLoopAsync();

				long unixTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				_modulePipe.SystemPacketPipe.OutBoundPipe.TryWrite(
					new ResultPacketContext
					{
						Packet = new C2SHeartBeat(unixTime, 0, 0)
					});

				_modulePipe.SystemPacketPipe.OutBoundPipe.TryWrite(
					new ResultPacketContext
					{
						Packet = new C2S_InitRequest()
					});

			}

		}


		internal void Disconnect()
		{
			if (Interlocked.Exchange(ref _disconnectedLock, 1) == 0)
			{
				_sessionCts.Cancel();
				Clear();
			}
		}


		private void Clear()
		{
			if (this._stream != null && this._client != null)
			{
				this._stream.Close();
				this._client.Close();
			}
			this._stream = null;
			this._client = null;

			this._linkedCts.Dispose();
			this._sessionCts.Dispose();

			this._modulePipe = null;
		}

		internal async Task<bool> Send(ReadOnlyMemory<byte> packetData)
		{
			try
			{
				if (this._disconnectedLock == 1)
					return false;
				await _stream.WriteAsync(packetData);
				return true;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogException(ex);
				return false;
			}
		}
		private async Task ReceivedLoopAsync()
		{
			try
			{
				while (!_loopToken.IsCancellationRequested)
				{
					if (_packetBuffer.Recv.FreeSize == 0)
					{
						_packetBuffer.Recv.TryWritePrepare(1);
						if (_packetBuffer.Recv.FreeSize == 0) break;
					}

					int received = await _stream.ReadAsync(_packetBuffer.Recv.WriteSpace, _loopToken);
					if (received <= 0 || _disconnectedLock == 1)
					{
						//Disconnect();
						break;
					}

					_packetBuffer.Recv.OnWrite(received);

					PacketProcess();
				}
			}
			catch (OperationCanceledException)
			{
				//NetManager.instance.Log($"정상적인 해제 {this.SessionID}");
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogException(ex);
			}
			finally
			{
				//Disconnect();
			}
		}
		private void PacketProcess()
		{
			while (true)
			{
				// 최소 헤더 크기
				if (_packetBuffer.Recv.Datasize < 4) return;

				// 헤더 크기 파싱
				ushort PacketSize = BinaryPrimitives.ReadUInt16LittleEndian(_packetBuffer.Recv.ReadSpace);

				if (PacketSize >= 4096)
				{
					//Disconnect();
					return;
				}

				if (_packetBuffer.Recv.Datasize < PacketSize) return;

				ReadOnlySpan<byte> packetSpan = _packetBuffer.Recv.ReadSpace.Slice(0, PacketSize);

				if (_formatter.TryParse(packetSpan, out IPacket parsePacket))
				{
					BranchPacket(new ReceivePacketContext { Packet = parsePacket });
				}
				else
				{
					Disconnect();
					return;
				}
				_packetBuffer.Recv.OnRead(PacketSize);
			}
		}
		private void BranchPacket(ReceivePacketContext packetContext)
		{
			int packetType = (int)packetContext.Packet.packetID;

			// IngamePacket
			if (0 < packetType && packetType < 100)
			{
				_modulePipe.GamePacketPipe.InBoundPipe.TryWrite(packetContext);
				return;
			}
			// SystemPacket
			if (99 < packetType && packetType < 200)
			{
				_modulePipe.SystemPacketPipe.InBoundPipe.TryWrite(packetContext);
				return;
			}
		}

	}
}