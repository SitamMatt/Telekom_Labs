using System;
using System.Collections.Generic;
using System.Text;

namespace Cwiczenie3
{
    public class HuffmanMessenger
    {
        Uri uri;
        public HuffmanMessenger(string address, int port)
        {
            UriBuilder builder = new UriBuilder();
            builder.Host = address;
            builder.Port = port;
            uri = builder.Uri;
        }

        public void Send(string message)
        {

        }

        // onRetrieve
        //Listen
    }
}
