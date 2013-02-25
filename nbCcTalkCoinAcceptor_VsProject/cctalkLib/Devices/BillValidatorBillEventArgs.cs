using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dk.CctalkLib.Devices
{
	public class BillValidatorBillEventArgs : EventArgs
	{
		/// <summary>
		///  Name of note as was set in CouinAccpetor constructor.
		/// </summary>
		public String NoteName { get; private set; }
		public Decimal NoteValue { get; private set; }

		/// <summary>
		///  Device`s code for note
		/// </summary>
		public Byte NoteCode { get; private set; }

		/// <summary>
		///  Device`s route id for note.
		/// </summary>
		public Byte RoutePath { get; private set; }

		/// <summary>
		///  Creates instance of noteAcceptornoteEventArgs
		/// </summary>
		public BillValidatorBillEventArgs(String noteName, Decimal noteValue, Byte noteCode, Byte routePath)
		{
			NoteName = noteName;
			NoteValue = noteValue;
			NoteCode = noteCode;
			RoutePath = routePath;
		}
	}
}


