﻿using VL.Core;

namespace VL.ImGui.Monadic
{
    /// <summary>
    /// Part of infrastructure to support connecting <typeparamref name="T"/> to <see cref="Channel{T}"/>
    /// </summary>
    public class ChannelFactory<T> : IMonadicFactory<T, Channel<T>>
    {
        public static readonly ChannelFactory<T> Default = new ChannelFactory<T>();

        public IMonadBuilder<T, Channel<T>> GetMonadBuilder(bool isConstant)
        {
            return new ChannelBuilder<T>();
        }
    }
}
