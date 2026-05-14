using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Exceptions
{
    public class NotFoundExceptionCustome(string message) 
        : Exception(message)
    {
    }
}
