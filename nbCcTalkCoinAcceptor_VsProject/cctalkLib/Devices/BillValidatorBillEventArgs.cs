using System;

namespace dk.CctalkLib.Devices
{
	public class BillValidatorBillEventArgs : EventArgs
	{
		/// <summary>
		///  Name of note as was set in CoinAccpetor constructor.
		/// </summary>
		public string NoteName { get; }
		public decimal NoteValue { get; }

		/// <summary>
		///  Device`s code for note
		/// </summary>
		public byte NoteCode { get; }

		/// <summary>
		///  Device`s route id for note.
		/// </summary>
		public byte RoutePath { get; }

		/// <summary>
		///  Creates instance of noteAcceptornoteEventArgs
		/// </summary>
		public BillValidatorBillEventArgs(string noteName, decimal noteValue, byte noteCode, byte routePath)
		{
			NoteName = noteName;
			NoteValue = noteValue;
			NoteCode = noteCode;
			RoutePath = routePath;
		}
	}
}


