using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dk.CctalkLib.Devices
{
	public enum BillValidatorErrors
	{

		///<summary>Null event ( no error ) </summary>
		NoError = 0,
		///<summary>Reject note </summary>
		BillReturned = 1,				// Bill returned from escrow Status
		InvalidBillValidation = 2,		// Invalid bill ( due to validation fail ) Reject
		InvalidBillTransport = 3,		// Invalid bill ( due to transport problem ) Reject
		InhibitedBillSerial = 4,		// Inhibited bill ( on serial ) Status
		InhibitedBillDip = 5,			// Inhibited bill ( on DIP switches ) Status
		BillJammedTransport = 6,		// Bill jammed in transport ( unsafe mode ) Fatal Error
		BillJammedStacker = 7,			// Bill jammed in stacker Fatal Error
		BillFraudAttempt = 8,			// Bill pulled backwards Fraud Attempt
		BillTemperFraudAttempt = 9,		// Bill tamper Fraud Attempt
		StackerOk = 10,					// Stacker OK Status
		StackerRemoved = 11,			// Stacker removed Status
		StackerInserted = 12,			// Stacker inserted Status
		StackerFaulty = 13,				// Stacker faulty Fatal Error
		StackerFull = 14,				// Stacker full Status
		StackerJammed = 15,				// Stacker jammed Fatal Error
		BillJammed = 16,				// Bill jammed in transport ( safe mode ) Fatal Error
		OptoFraud = 17,					// Opto fraud detected Fraud Attempt
		StringFraud = 18,				// String fraud detected Fraud Attempt
		AntiStringFaulty = 19,			// Anti-string mechanism faulty Fatal Error
		BarcodeDetected = 20,			// Barcode detected Status
		UnknownBill = 21,				// Unknown bill type stacked Status
		UnspecifiedAlarmCode = 255
	}
}
