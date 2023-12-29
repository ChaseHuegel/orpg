using System;
using System.Collections.Generic;

namespace Needlefish.Compiler.Tests.Schema;

public class NsdException : AggregateException
{
    public NsdException(string message, IEnumerable<Exception> exceptions) : base(message, exceptions) { }

    public NsdException(string message, params Exception[] exceptions) : base(message, exceptions) { }
}