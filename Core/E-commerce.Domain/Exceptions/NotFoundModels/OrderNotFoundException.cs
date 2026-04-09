using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Domain.Exceptions.NotFoundModels
{
    public class OrderNotFoundException(string msg) : NotFoundExceptionCustome(msg)
    {
    }
}
