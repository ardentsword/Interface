using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ManusInterface
{
    class  KeyBindNotFoundException: Exception
    {
            public KeyBindNotFoundException()
    {

    }

    public KeyBindNotFoundException(string message)
        : base(message)
    {
        Debug.Write("Could not find binding for "+message);
    }

    public KeyBindNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
            // Constructor needed for serialization 
    // when exception propagates from a remoting server to the client.
    protected KeyBindNotFoundException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) {}
    }
}
