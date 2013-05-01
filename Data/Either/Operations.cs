﻿using System;
using Sobbs.Functional;

namespace Sobbs.Data.Either
{
    public static class Operations
    {
        public static void DoEither<TLeft, TRight>(this IEither<TLeft, TRight> either, Action
           <TLeft> actLeft, Action<TRight> actRight)
        {
            either.Either(actLeft.ToFunc(), actRight.ToFunc());
        }
    }
}
