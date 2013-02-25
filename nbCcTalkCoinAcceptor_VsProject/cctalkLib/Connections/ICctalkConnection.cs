using System;
using dk.CctalkLib.Checksumms;
using dk.CctalkLib.Messages;

namespace dk.CctalkLib.Connections
{
    public interface ICctalkConnection:IDisposable
    {
        void Open();
        bool IsOpen();
        CctalkMessage Send(CctalkMessage com, ICctalkChecksum checksumHandler);
        //string Read();
        void Close();
    }
}
