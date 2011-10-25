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
            bool changed = !EqualityComparer<T>.Default.Equals(field, value);

            if (changed)
            {
                field = value;
                base.NotifyOfPropertyChange(propertyExpression);
            }

            return changed;
        }
    }
}