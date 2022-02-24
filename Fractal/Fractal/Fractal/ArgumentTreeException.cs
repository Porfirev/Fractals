using System;

namespace Fractal
{
    /// <summary>
    /// Класс ошибки для Пифагорова Дерева
    /// </summary>
    public class ArgumentTreeException : ArgumentException
    {
        public ArgumentTreeException() { }
        public ArgumentTreeException(string message) : base(message) { }
        public ArgumentTreeException(string message, Exception inner) : base(message, inner) { }
        protected ArgumentTreeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
