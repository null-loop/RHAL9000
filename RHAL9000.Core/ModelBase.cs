using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Caliburn.Micro;

namespace RHAL9000.Core
{
    public class ModelBase : PropertyChangedBase
    {
        protected bool SetField<T>(ref T field, T value, Expression<Func<T>> propertyExpression)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                NotifyOfPropertyChange(propertyExpression);
            }

            return !EqualityComparer<T>.Default.Equals(field, value);
        }
    }
}