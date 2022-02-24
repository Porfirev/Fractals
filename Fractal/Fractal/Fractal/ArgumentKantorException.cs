using System;

namespace Fractal
{
    /// <summary>
    /// Класс ошибки для Множества Кантора
    /// </summary>
    public class ArgumentKantorException : ArgumentException
    {
        public ArgumentKantorException() { }
        public ArgumentKantorException(string message) : base(message) { }
        public ArgumentKantorException(string message, Exception inner) : base(message, inner) { }
        protected ArgumentKantorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
