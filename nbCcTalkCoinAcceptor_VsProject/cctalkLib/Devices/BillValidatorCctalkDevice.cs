using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dk.CctalkLib.Devices
{
	public class BillValidatorCctalkDevice : GenericCctalkDevice
	{
		/*
		 *  PER ARDAC ELITE
		 *  
		 *  N.B.: Da verificare cosa effettivamente arriva
		 * 
			Header 159 – Read buffered bill events
			Transmitted data : <none>
			Received data : [ event counter ]
							[ result 1A ] [ result 1B ]
							[ result 2A ] [ result 2B ]
							[ result 3A ] [ result 3B ]
							[ result 4A ] [ result 4B ]
							[ result 5A ] [ result 5B ]
			See the Event Code Table for a list of possible event codes.
			Note that the event counter wraps from 255 to 1. An event counter of zero indicates power-up or
			reset.
		 */

		public new DeviceEventBuffer CmdReadEventBuffer()
		{
			var msg = CreateMessage(159);
			var respond = Connection.Send(msg, _checksumHandler);

			if (respond.DataLength < 11)
				throw new InvalidRespondException(respond);

			var data = respond.Data;
			var events = new[]
                             {
                                 new DeviceEvent(data[1], data[2]),
                                 new DeviceEvent(data[3], data[4]),
                                 new DeviceEvent(data[5], data[6]),
                                 new DeviceEvent(data[7], data[8]),
                                 new DeviceEvent(data[9], data[10]),
                             }
				;


			var ret = new DeviceEventBuffer
			{
				Counter = respond.Data[0],
				Events = events,
			};

			return ret;
		}

		/* da implementare
			Transmitted data : <none>
			Received data : [ opto states 1 ] [ opto states 2 ]
			[ opto states 1 ]
			B0 – front left pos
			B1 – front right pos
			B2 – validation note present ( based on TAOS sensor readings )
			B3 – rear clear ( microswitch non-return hook )
			B4 – rear note present ( head exit sensor )
			B5 – string sensor
			[ opto states 2 ]
			B0 – barcode receiver 1
			B1 – barcode receiver 2
			B2 – barcode receiver 3
		 * 
		 */

		public new CctalkDeviceStatus CmdRequestStatus()
		{
			var msg = CreateMessage(236);

			var respond = Connection.Send(msg, _checksumHandler);
			if (respond.DataLength < 2)
				throw new InvalidRespondException(respond);

			return (CctalkDeviceStatus)respond.Data[0];
		}

		public bool CmdModifyBillOpertingMode(Byte mode)
		{
			bool ret = false;
			Messages.CctalkMessage msg = CreateMessage(153);
			msg.Data = new Byte[] { mode };  //two bits on, for enable the stacker and the escrow simultaneously
			var respond = Connection.Send(msg, _checksumHandler);

			return ret;

		}
	}
}
